using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp_Curd.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.MapPost("/employees", (Employee employee) =>
{
    /**
     * - we can do required validation here but with the help of asp.net w can take advanatnge of the c# feature which is using 
     *   System.ComponentModel.DataAnnotations; we can specify how these things can be validated.
     * **/
    EmployeesRepository.AddEmployee(employee);
    return "Employee is added successfully.";
}).WithParameterValidation();

app.Run();

/**
 * - What kind of problem model binding resolve ? If we go to any endpoint , where we are tyring to extract value from HTTP Context object 
 *   for example,
 *   
 *       var id = context.Request.RouteValues["id"];
 *       
 *       - Here we are taking the id from the RouteValues inside the request object, it takes lot of effort  reading employee information from the body,
 *         we are doing lot of maunal work that can be implemented by the framework. The framework is there to eliminate this kind of work for us. The
 *         data extraction from one data structure to another data structure this type of work should be done by the asp.net core framework. This is 
 *         done by model binding.
 *       
 *       - model binding : Extarct data from http request to .NET objects parameters in endpoint handlers.
 *       
 *       - What are the sources of the data in the HTTP request, 
 *         1. RouteValues from HTTP Request
 *         2. QueryStrings
 *         3. Header
 *         4. Body
 *         
 *         we have 4 different source of data used to extract data.
 *         
 *       - How does asp.net core implement the model binding process ? 
 *        - It first look at the parameter and the goes to the HTTP Context Request object and try to find the parameter inside the HTTP Context Request object.
 *          We are binding from the HTTP request object to the parameter this is how data flows, data from from HTTPRequest object ot the endpoint handler but
 *          because how asp.net work here it actually first look at the parameter and then looks for the source. We are binding the aparamter to the HTTP Request
 *          object.
 *          
 *            endpoints.MapGet("/employees/{id:int}", (int id) => {
 *             
 *             });
 *             
 *       - When the model binding happens ? 
 *         
 *         - Routing is the process of map an HTTP request to a particular hanlder and then record thta endpoint handler information in the HTTP request object
 *           after that we execute the selected endpoint, so routing first selectes a endpoint then asp.net core tries to execute the endpoint but between 
 *           these two model binding happens before the ednpoint sis executed. Which makes sense beacuse the model binding responsible for the extraction of data
 *           from the HTTP request object , and pass the data into the endpoint handlers as parameter, so this is when model binding happens.
 *           
 *                    Routing ------------------ Model Binding -----------------> Execute Endpoint
 *                    
 * - Binding to Route values :   
 * 
 *       endpoints.MapGet("/employees/{id:int}", ([FromRoute]int id) => {
 *       
 *       });
 *       
 *   - [FromRoute] int id : This explicitly tells ASP.NET,Bind this parameter ONLY from the route.   
 *     
 *     - When [FromRoute] actually needed ? When the paramter name does not match the route paramter name.
 *     
 *       for example ,
 *                    app.MapGet("/employees/{id:int}", ([FromRoute(Name = "id")] int employeeId) => {
 *                    
 *                    });
 *                    
 * - Binding to Query String :
 * 
 *   - Query string binding means ASP.NET Core automatically reads values from the URL after ? and binds them to your method parameters.
 *   
 *                                                     HTTP Request
 *                                                           ↓
 *                                                Routing selects endpoint
 *                                                           ↓
 *                                           Model Binding reads query string
 *                                                           ↓
 *                                                  Converts to C# types
 *                                                           ↓
 *                                                   Endpoint executes  
 *                                                   
 *     
 *    1. endpoints.MapGet("/employees", (string name) => {
 *     
 *       });
 *     
 *    2. enpoints.MapGet("/employees",(string? position, int? minSalary ) => {
 *                                  
 *       });
 *     
 *    3. enpoints.MapGet("/employees",([FromQuery] string name) => {
 *     
 *       });
 *       
 *    4. enpoints.NapGet("/employees/{id:int}", (int id, string? name) => {
 *                  
 *       });   
 *     
 * -  The RouteValues takes priority over query string. 
 * 
 * - Binding to HTTP Headers : Binding to HTTP Headers is different from binding to RouteValues and binding to QueryString here we need to use explicitly binding.
 *    
 *   for example. 
 *   
 *          endpoints.MapGet("/employees",([FromHeader] int id) => {
 *            
 *          });
 *          
 * - Use AsParamter to group parameters :
 * 
 *   In ASP.NET Core Minimal APIs, [AsParameters] lets you group multiple bound parameters into a single object instead of writing many 
 *   parameters in your endpoint method.
 *   
 *   It makes your endpoint:
 *   
 *   1. Cleaner
 *   2. More readable
 *   3. Easier to scale
 *   
 *   enpoints.MapGet("/employees/{id:int}",(int id, [FromQuery] string name, [FromHeader] string position)=> {
 *     
 *              var employee = EmployeesRepository.GetEmployeeById(id);
 *              employee.Name =  name;
 *              employee.Position =  position;
 *              return employee;
 *   });
 *   
 * - Bind arrays  to query strings or headers :   ASP.NET Core automatically binds repeated query parameters into arrays.
 *                                                
 *                                                1. From query string : app.MapGet("/employees", (int[] ids) => 
 *                                                                       {
 *                                                                          return ids;
 *                                                                       });
 *                                                                       
 *                                                2. From Header : app.MapGet("/test", ([FromHeader(Name = "X-Ids")] int[] ids) =>
 *                                                                 {
 *                                                                    return ids;
 *                                                                 });
 *                                                                      

 * - Bind to HTTP Body : Body binding means ASP.NET Core reads the request body (usually JSON), deserializes it, and binds it to a C# object. Binding to the 
 *                       body usually always bind a complex type to the body most commonly used with POST, PUT, PATCH. 
 *                       
 *                       ASP.NET Core automatically:  1. Reads request body
 *                                                    2. Uses System.Text.Json
 *                                                    3. Converts JSON → Employee
 *                                                    4. Injects it into parameter
 *                                                    
 *                                                    we dont need to use HttpContext.
 *                                                    
 *                                                    
 *                       ednpoints.MapPost("employee",(Employee employee) => {
 *                            
 *                            if(employee is null || employee.Id <=0)
 *                            {
 *                              return "Employee data is not valid";
 *                            }
 *                            
 *                            EmployeesRepository.AddEmployee(employee);
 *                            return "Employeea added successfully";
 *                       
 *                       });                             
 *                       
 *                       for minimal api the infromation must be JSON cannot be anything else.
 *                       
 * - Custom binding with BuildAsync method :   It allows you to completely control how a parameter is created from the HttpContext.
 *   - BindAsync is a custom model binding hook for Minimal APIs. 
 *   - If your type defines: public static ValueTask<T?> BindAsync(HttpContext context); ASP.NET Core will automatically use it to create that parameter.
 *   
 *   - Why Use Custom Binding? 
 *     - Use it when: 
 *       
 *       1. You need to combine Route + Query + Header
 *       2. You need complex parsing logic
 *       3. You want validation before endpoint runs
 *       4. You want to avoid [FromQuery], [FromHeader], etc.
 *       5. You want reusable binding logic
 *                    
 * - Binding source priorities :  When ASP.NET Core tries to bind a parameter, it checks multiple sources:
 *                                
 *                                1. Route values
 *                                2. Query string
 *                                3. Headers
 *                                4. Body
 *                                5. Services (Dependency Injection)
 *                                
 *                                - If multiple sources contain the same name, ASP.NET must decide: Which source wins?
 *                                  That’s where binding source priority comes in.
 *                                  
 *                                  - Default Binding Priority (Minimal APIs)
 *                                  
 *                                    - For simple types (int, string, bool, etc.):
 *                                       1️. Route
 *                                       2️. Query
 *                                       3️. Header
 *                                     
 *                                    - For complex types (classes):
 *                                       1. Body
 *                                       2. Custom BindAsync (if exists)
 *                                      
 *                                      
 * - Model Validation : Model Validation ensures that incoming request data is, It works together with model binding.
 * 
 *                      - Correct
 *                      - Complete
 *                      - Expected format
 *                      - Rejected if invalid
 *                      
 *   -  What Happens Internally? 
 *   
 *                      HTTP Request
 *                           ↓
 *                      Model Binding
 *                           ↓
 *                    Model Validation     
 *                           ↓
 *                Endpoint Executes (if valid)
 *                
 *      - If validation fails → ASP.NET returns 400 Bad Request (in controllers automatically). 
 *      
 *      - Common Validation Attributes
 *      
 *                  |      Attribute   |       Purpose          |
                    | ---------------- | ---------------------- |
                    | `[Required]`     | Field must not be null |
                    | `[StringLength]` | Limit string length    |
                    | `[MaxLength]`    | Max size               |
                    | `[Range]`        | Numeric range          |
                    | `[EmailAddress]` | Valid email            |
                    | `[Phone]`        | Valid phone            |
                    | `[Compare]`      | Compare two fields     |

 * 
 * - With mininmal api data annotation validation with minimal api, it is not part of this technology, with MVC and Razer pages the model validation is triggered 
 *   automatically , we provide data annotations to our model, but case of minimal api we have to add anuget package.
 *          
 * - Custom model validation with validation attribute :   If we have more comlicated logic that requires us to use multiple properties at the same time
 *   then we might run into issues because data annotation is not enough to do that.
 *   
 *   for example : if the position is manager the salaty has to be higher than 100000, this is a logic that needs a combination of both the position property
 *                 and salary property , so how do we do this type of validation ? We can still use the data annotation but the built-in attributes are not
 *                 enough, we need to create a custom validation attribute, So how do we do that ? 
 *                 
 *                 step-1 : create a class 
 *                 step-2 : it must be derived from the ValidationAttribute class
 **/

public class GetEmployeeParameter
{
    [FromRoute]
    public int Id { get; set; }

    [FromQuery]
    public string? Name { get; set; }

    [FromHeader(Name = "X-Client-Version")]
    public string? Position { get; set; }
}
// Custom binding with BuildAsync method
class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public static ValueTask<Person?> BindAsync(HttpContext context)
    {
        var idStr = context.Request.Query["id"];
        var nameStr = context.Request.Headers["name"];

        if (int.TryParse(idStr, out var id))
        {
            return new ValueTask<Person?>(new Person() { Id = id, Name = nameStr });
        }

        return new ValueTask<Person?>(Task.FromResult<Person?>(null));
    }
}


using Microsoft.AspNetCore.Http.HttpResults;
using WebApp_Curd.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp_Curd.Result;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

var app = builder.Build();

// below code helps us get standard errors and status on incorrect request.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();

}
app.UseStatusCodePages();

app.MapGet("/", HtmlResult () =>
{
    string html = "<h2> Welcome to our API </h2> Our api is ued to learn ASP.NET CORE"; // pure text
    return new HtmlResult(html);
});

app.MapGet("/employees", () =>
{
    var employee = EmployeesRepository.GetEmployees();
    return employee; // return an object 
});

//app.MapPost("/employees", (Employee emp) =>
//{
//    if (emp.Id <= 0)
//        return TypedResults.BadRequest("Invalid Employee Id"); // Bad request 400

//    EmployeesRepository.AddEmployee(emp);
//    return TypedResults.Created($"/employees/{emp.Id}", emp);
//});


app.MapPost("/employees",
    Results<ProblemHttpResult, Created<Employee>>
    (Employee emp) =>
    {
        if (emp.Id <= 0)
        {
            return TypedResults.Problem(
                title: "Invalid Employee Id",
                detail: "Employee Id must be greater than zero.",
                statusCode: 400
            );
        }

        EmployeesRepository.AddEmployee(emp);
        return TypedResults.Created($"/employees/{emp.Id}", emp);
    });

app.Run();

/**
 * - So far we have been returning strings and objects directly from our minimal api endpont handlers. 
 * - In minimal api endpoint handlers also returns another type which is IResult , we don't have to explicitly put the IResult , but we can 
 *   actually return the IResult.
 *   
 *   IResult is very simple interface that has only one method defined in it , which basically returns a task and that means return nothing, and
 *   it has an execute function in it an dthe onlye paramter it take is HttpContext. SO if we create a class that implements  the IResult interface 
 *   we have to implement ExecuteAsync method , that means that we can do any thing to the HTTP context object and that means we prepare the response
 *   inside this particular method that when we want to customize the respons. It most cases we dont neeed to create our own class that implements this 
 *   interface asp.net core provides us with lost helper functions to help us to return IResult and those helper functions is mainly to provide a status
 *   code.
 *   
 *   - For get http method don't have to because if this successful bu defualt the statu soce is http 200, but if we want ot expilicitly specify http 200
 *     what we can do is we can use 
 *     
 *     Result.Ok(emp); -> It is going to spcify the status code and also it's going to serialize the object into json and put it into http body, so that's 
 *                        the implementation of that execute async interface. 
 *     
 *   -  We can also use TypedResults : it also tells us the type , due tot this asp.net core knows what is actually in the body.
 *   
 *   - In order to return status code we can take advantage of the IResult return type in the endpoint handler and in order to return Irsult we can either use
 *     TypedResult or Result.
 *   
 *   - We have discuessed that whenever possible it's recommeded to use TypedResult but 
 *   
 *   
 *      app.MapPost("/employees", (Employee emp) => // here we will get swiggly line error
 *      {
 *         if (emp.Id <= 0)
 *         return TypedResults.BadRequest("Invalid Employee Id"); // Bad request 400  // this 
 *
 *         EmployeesRepository.AddEmployee(emp);
 *         return TypedResults.Created($"/employees/{emp.Id}", emp); // this are not of same type and this cause an error.
 *      });
 *   
 *     - the purpose of post request is to crete resource therefore the return type when it is successful it is not TypedResult.Ok,it is not http 200 ok,
 *       it should be created , so created is staus 201. 
 *       
 * - What Is Problem Details?
 * 
 *   - It is a standard JSON format for returning error from APIs. Instaed of returning random error message like "Invalid employeeId" we can send staructured
 *     respone.  
 *             for example ,
 *                           {
 *                              "type": "https://example.com/errors/invalid-id",
 *                              "title": "Invalid Employee Id",
 *                             "status": 400,
 *                              "detail": "Employee Id must be greater than 0",
 *                              "instance": "/employees"
 *                            }
 *             - this makes api 
 *                1. Stardard
 *                2. Structured
 *                3. Easier to consume by the frontend/mobile. 
 *                
 *     
 *  
 * - Standardize API results : RFC standard  - https://www.rfc-editor.org/rfc/rfc7807.html
 * 
 *   - We know that inside the pipeline there is exception handler middleware, we can  add that middleware to our pipeline  and we hav o do that is specific 
 *     manner, we can't do that in noraml way.
 *     
 * - Customize results by implementing IResult : Till now we know the result class provide all those status code, but what if we need to return a HTML. How
 *     do we do it ? we get HTML from it ? We can create a HTML result class that implements the IResult interface and then by implementing the interface we can
 *     prepare any response that we want to create.
 **/




using WebApp_Curd.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.MapGet("/", () =>
{
    return " Welcome to the portal"; // pure text
});

app.MapGet("/employees", () =>
{
    var employee = EmployeesRepository.GetEmployees();
    return employee; // return an object 
});

app.MapPost("/employees", (Employee emp) =>
{
    if (emp.Id <= 0)
        return Results.BadRequest("Invalid Employee Id"); // Bad request 400

    EmployeesRepository.AddEmployee(emp);
    return Results.Created($"/employees/{emp.Id}", emp);
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
 **/


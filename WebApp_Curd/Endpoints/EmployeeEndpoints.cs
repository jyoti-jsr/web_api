using WebApp_Curd.Models;
using WebApp_Curd.Result;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp_Curd.Endpoints
{
    public static class EmployeeEndpoints
    {
        public static void MapEmployeeEndpoints(this WebApplication app)
        {
            app.MapGet("/", HtmlResult () =>
            {
                string html = "<h2> Welcome to our API </h2> Our api is ued to learn ASP.NET CORE"; // pure text
                return new HtmlResult(html);
            });

            app.MapGet("/employees", () =>
            {
                var employee = EmployeesRepository.GetEmployees();
                return TypedResults.Ok(employee); // return an object 
            });

            app.MapGet("/employees/{id:int}", (int id) =>
            {
                var employee = EmployeesRepository.GetEmployeeById(id);


                return employee is not null ? TypedResults.Ok(employee) : Results.ValidationProblem(new Dictionary<string, string[]>
                {
                      {"id", new [] {$"Employee with id {id} doesn't exists."} }
                }, statusCode: 404);

            });

            app.MapPost("/employees", (Employee employee) =>
            {

                if (employee is null || employee.Id < 0)
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        {"id", new [] {"Employee is not provided or is not valid"} }
                    });
                }

                EmployeesRepository.AddEmployee(employee);
                return TypedResults.Created($"/employee/{employee.Id}", employee);

            });

            app.MapPut("/employees/{id:int}", (int id, Employee employee) =>
            {

                if (id != employee.Id)
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                       {"id", new [] {"Employee id is not matching any records"} }
                    });
                }


                return EmployeesRepository.UpdateEmployee(employee) ? TypedResults.NoContent() : Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    {"id", new [] {"Employee does'nt exists"} }
                });
            });

            app.MapDelete("/employees/{id:int}", (int id) =>
            {

                if (id < 0)
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                       {"id", new [] {"Employee is not provided or is not valid"} }
                    });
                }

                return EmployeesRepository.DeleteEmployeeById(id) ? TypedResults.Ok(id) : Results.ValidationProblem(new Dictionary<string, string[]>
                {
                     {"id", new [] {"Employee with the id {id} doesn't exits"} }
                });

            });
        }
    }
}

using System.Text.Json;
using WebApp_Curd.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{

    endpoints.MapGet("/", async (HttpContext context) =>
    {
        await context.Response.WriteAsync("Welcome to homepage");
    });

    endpoints.MapGet("/employees", async (HttpContext context) =>
    {
        var employees = EmployeesRepository.GetEmployees();
        string employeesJson = JsonSerializer.Serialize(employees);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(employeesJson);
    });

    endpoints.MapPost("/employees", async (HttpContext context) =>
    {
        var employee = await context.Request.ReadFromJsonAsync<Employee>();

        if (employee == null)
        {
            context.Response.StatusCode = 400;
            return;
        }

        EmployeesRepository.AddEmployee(employee);

        context.Response.StatusCode = 201;
        await context.Response.WriteAsJsonAsync("New employee created");
    });

    endpoints.MapPut("/employees/{id}", async (HttpContext context) =>
    {
        //var employeeId = context.Request.RouteValues["id"];
        var employee = await context.Request.ReadFromJsonAsync<Employee>();

        EmployeesRepository.UpdateEmployee(employee);

    });

    endpoints.MapDelete("/employees/{id}", (HttpContext context) =>
    {
        var employeeId = context.Request.RouteValues["id"];
        EmployeesRepository.DeleteEmployeeById(Convert.ToInt32(employeeId));

    });

});

app.Run();

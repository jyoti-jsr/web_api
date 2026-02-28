

using Microsoft.AspNetCore.Http.HttpResults;
using WebApp_Curd.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp_Curd.Result;
using WebApp_Curd.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

var app = builder.Build();

// below code helps us get standard errors and status on incorrect request.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();

}
app.UseStatusCodePages();

app.MapEmployeeEndpoints();


app.Run();

/**
 * - Organize minimal api endpoints : code organization and dependency injection
 * 
 *   - In our program.cs files we have many endpoinst and they may grow and make our code messay  so it can be impossible to maintain code properly with every
 *     thing within the same file, so there must be a way to organize the code. One of the way is to use extension method.
 *     
 *   - Program.cs files is where we configure the application and kestrel server, it should not include any logic whether it's framnework logic or 
 *     any logic.
 **/



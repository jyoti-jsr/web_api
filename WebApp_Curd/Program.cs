
using Microsoft.AspNetCore.Mvc;
using WebApp_Curd.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.MapGet("/", () =>
{
    return " Welcome to the portal";
});

// using query string

//app.MapPost("/registration", (Registration? res) =>
//{
//    if (res is null)
//        return Results.BadRequest("Invalid query parameters.");

//    return Results.Ok($"User: {res.Email} {res.Password} {res.ConfirmPassword}");
//}).WithParameterValidation();

// using body

app.MapPost("/registration", ([FromBody]Registration res) =>
{
    if (res is null)
        return Results.BadRequest("Invalid query parameters.");

    return Results.Ok($"User: {res.Email} {res.Password} {res.ConfirmPassword}");
}).WithParameterValidation();



app.Run();

/**
 * - Email is required.
 * - Valid email address.
 * - Password is required.
 * - Confirm password is required.
 * - Password must be at least 6 character long.
 * - Password do not match.
 * 
 * - Create two version of endpoints hendlers.
 * 
 *   1. Registration info from query string.
 *   2. Registration info comes from body.
 * 
 * **/


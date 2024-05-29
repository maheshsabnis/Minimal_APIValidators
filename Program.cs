using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Minimal_APIValidators.CustomFilters;
using Minimal_APIValidators.Models;
using Minimal_APIValidators.Validators;

// For JSON Seriaization Policy
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IValidator<ProductInfo>, ProductInfoValidator>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});

var app = builder.Build();
// Read the HTTP request body data
app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/product", (ProductInfo product) => {

    var validator = new ProductInfoValidator();

    var validationresult = validator.Validate(product);

    if (validationresult.IsValid)
    {
        return Results.Ok("Data is Valid");
    }
    return Results.BadRequest(validationresult.Errors.Select(e=>e.ErrorMessage).ToList());
});

// Endpoint with the ModelValidationFilter Applied on it
app.MapPost("/api/filter/product", (ProductInfo product) => {
    return Results.Ok("Data is Valid");

}).AddEndpointFilter<ModelValidationFilter<ProductInfo>>();

app.Run();

 
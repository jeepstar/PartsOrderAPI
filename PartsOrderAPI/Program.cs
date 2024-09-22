using Microsoft.AspNetCore.Mvc;
using PartsOrderAPI.Middleware;
using PartsOrderAPI.Services;
using PartsOrderAPI.Validation;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new CustomStringConverter());
        options.JsonSerializerOptions.Converters.Add(new CustomDecimalConverter());
        options.JsonSerializerOptions.Converters.Add(new CustomIntConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPartService, PartService>();
builder.Services.AddScoped<IOrderService, OrderService>(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parts Inventory API V1");
    });
}


app.UseHttpsRedirection();

// custom error handling middleware
app.UseMiddleware<CustomErrorHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

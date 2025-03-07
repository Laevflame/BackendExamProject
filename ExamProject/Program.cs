using MediatR;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ExamProject.Entities;
using ExamProject.Services;
using Serilog;
using ExamProject.Behavior;
using ExamProject.Validation;
using ExamProject.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration
        .MinimumLevel.Information()
        .WriteTo.File($"logs/Log-{DateTime.UtcNow:yyyyMMdd}.txt", rollingInterval: RollingInterval.Day);
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkSqlServer();
builder.Services.AddDbContextFactory<BackendExamProjectContext>(options =>
{
    var conString = configuration.GetConnectionString("SQLServerDb");
    options.UseSqlServer(conString);
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<BackendExamProjectContext>(); // Ensure DbContext is registered as scoped

builder.Services.AddScoped<TicketsServices>();
builder.Services.AddScoped<BookedTicketsServices>();
builder.Services.AddScoped<RevokeTicketServices>();
builder.Services.AddScoped<EditTicketServices>();
builder.Services.AddScoped<BookingTicketService>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<BookingListModelCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditTicketCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RevokeTicketValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetBookedTicketValidator>();


// Register the EditTicketRequestValidator

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
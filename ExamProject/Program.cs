
using Microsoft.EntityFrameworkCore;
using ExamProject.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ExamProject.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => {
    loggerConfiguration.WriteTo.File($"logs/log-{DateTime.UtcNow:yyyyMMdd}.txt", rollingInterval: RollingInterval.Day);
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkSqlServer();
builder.Services.AddDbContextPool<BackendExamProjectContext>(options =>
{
    var conString = configuration.GetConnectionString("SQLServerDb");
    options.UseSqlServer(conString);
});

builder.Services.AddTransient<TicketsServices>();
builder.Services.AddTransient<BookedTicketsServices>();
builder.Services.AddTransient<RevokeTicketServices>();
builder.Services.AddTransient<EditTicketServices>();
builder.Services.AddTransient<BookingTicketService>();
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

using AnalyticsService.Models;
using AnalyticsService.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddTransient<PatientInterface, PatientRepository>();
builder.Services.AddTransient<DoctorInterface, DoctorRepository>();
builder.Services.AddTransient<OperationInterface, OperationRepository>();
builder.Services.AddTransient<OperationHistoryInterface, OperationHistoryRepository>();

builder.Services.AddTransient<TaskManager>();

builder.Services.AddDbContext<AnalyticsServiceContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("AnalyticsService")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

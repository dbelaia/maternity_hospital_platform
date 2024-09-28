using Microsoft.EntityFrameworkCore;
using MaternityMicroservices.Models;
using MaternityMicroservices.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddTransient<IPatientRepository, PatientRepository>();

builder.Services.AddDbContext<MaternityAppointmentServiceContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("MaternityAppointmentService")));

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
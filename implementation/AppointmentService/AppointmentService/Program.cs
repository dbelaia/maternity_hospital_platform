using AppointmentService.Models;
using AppointmentService.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<PatientInterface, PatientRepository>();
builder.Services.AddTransient<DoctorInterface, DoctorRepository>();
builder.Services.AddTransient<AppointmentStatusInterface, AppointmentStatusRepository>();
builder.Services.AddTransient<AppointmentInterface, AppointmentRepository>();
builder.Services.AddTransient<AppointmentHistoryInterface, AppointmentHistoryRepository>();

builder.Services.AddTransient<TaskManager>();

builder.Services.AddDbContext<AppointmentServiceContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppointmentService")));

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

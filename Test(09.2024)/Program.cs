using Microsoft.EntityFrameworkCore;
using Task;
using Task.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["MsSql:ConnectionString"]; // Connection string to DB
// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MsSqlDbContext>(options => options.UseSqlServer(connectionString))       // Connection to DB 
    .AddTransient<IRentRepositories, RentRepositoies>();                                               // DI for entity


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
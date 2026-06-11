using Microsoft.EntityFrameworkCore;
using SIV.Infrastructure.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();

var connectionString = builder.Configuration.GetConnectionString("SIVConnection");


builder.Services.AddDbContext<SIVDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("SIV.Infrastructure")));

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

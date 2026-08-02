using SIV.Application;
using SIV.Presentation.WebApi;
using SIV.Presentation.WebApi.Hubs;
using SIV.Presentation.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PermitirFrontend");

app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapControllers();

// Mapeas el Hub de SignalR para la consola FIDS
app.MapHub<VuelosHub>("/hubs/vuelos");

app.Run();
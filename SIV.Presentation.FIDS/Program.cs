using SIV.Presentation.FIDS;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFidsPresentationServices(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Tablero/General");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tablero}/{action=Index}/{id?}");

app.Run();

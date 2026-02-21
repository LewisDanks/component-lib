using Papercut.Web.Infrastructure.ClientContext;
using Papercut.Web.Pages;
using Vite.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add vite services
builder.Services.AddViteServices();
// Add services to the container.
builder.Services.AddClientContext();
builder.Services.AddScoped<LoginClientContextBuilder>();
builder.Services.AddScoped<DashboardClientContextBuilder>();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.ConfigureFilter(
        new Microsoft.AspNetCore.Mvc.ServiceFilterAttribute(typeof(ClientContextPageFilter))
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    // Websocket support for HMR
    app.UseWebSockets();
    app.UseViteDevelopmentServer();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

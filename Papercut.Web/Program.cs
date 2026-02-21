using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Papercut.Web.Infrastructure.Auth;
using Papercut.Web.Infrastructure.ClientContext;
using Papercut.Web.Pages;
using Vite.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add vite services
builder.Services.AddViteServices();

builder.Services.Configure<AuthSeedOptions>(builder.Configuration.GetSection("AuthSeed"));
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-PAPERCUT-CSRF";
});

builder.Services.AddDbContext<AuthDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Auth") ?? "Data Source=papercut.auth.db";
    options.UseSqlite(connectionString);
});

builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = true;
    })
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddDefaultTokenProviders();

builder.Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services.AddAuthorization();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/Login";
});

// Add services to the container.
builder.Services.AddClientContext();
builder.Services.AddScoped<LoginClientContextBuilder>();
builder.Services.AddScoped<DashboardClientContextBuilder>();
builder.Services.AddScoped<SettingsClientContextBuilder>();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

await using (var scope = app.Services.CreateAsyncScope())
{
    await AuthSeeder.SeedAsync(scope.ServiceProvider);
}

app.Run();

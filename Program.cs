using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using rgproj.Data;
using rgproj.Data.Seeders;
using rgproj.Models;
using rgproj.Services;
using QuestPDF;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;




QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);



var connectionString = builder.Configuration.GetConnectionString("ConnectionString");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<AppUser, IdentityRole>(
    options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
    }
    )
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<AdminSeeder>();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpClient<IAiReportService, ReportService>()
    .ConfigureHttpClient(client =>
    {
        client.Timeout = TimeSpan.FromMinutes(5);
    })
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(30),
        KeepAlivePingPolicy = HttpKeepAlivePingPolicy.WithActiveRequests,
        EnableMultipleHttp2Connections = true,
        ConnectTimeout = TimeSpan.FromMinutes(30),
        PooledConnectionIdleTimeout = TimeSpan.FromMinutes(30),
        MaxConnectionsPerServer = 10
    });

builder.Services.AddScoped<IReportFormatter, ReportFormatter>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleSeeder = new RoleSeeder(scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>());
    await roleSeeder.SeedRolesAsync();

    var adminSeeder = scope.ServiceProvider.GetRequiredService<AdminSeeder>();
    await adminSeeder.SeedAdminAsync();
}

app.Use(async (context, next) =>
{
    context.Request.Headers["ngrok-skip-browser-warning"] = "true";
    await next();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using KeyBoardSleepType.models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Npgsql;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var conn = "Host=localhost;Port=5433;Database=SleepType;User ID=postgres;Password=ss";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(conn));


builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(400);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();


builder.Services.AddSingleton<InputModel>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.MapRazorPages();

app.Run();

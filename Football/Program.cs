using Football.Application.DTOs;
using Football.Application.Interfaces.Aggregation;
using Football.Application.Interfaces.AI;
using Football.Application.Interfaces.Facades;
using Football.Application.Interfaces.Final;
using Football.Application.Interfaces.Math;
using Football.Application.Interfaces.Providers;
using Football.Application.Services.Aggregation;
using Football.Application.Services.AI;
using Football.Application.Services.Facades;
using Football.Application.Services.Final;
using Football.Application.Services.Math;
using Football.Application.Services.Providers;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// =======================
// MVC
// =======================
builder.Services.AddControllersWithViews();

// =======================
// CACHE (Rate limit üçün)
// =======================
builder.Services.AddMemoryCache();

// =======================
// CONFIG BINDINGS
// =======================
builder.Services.Configure<OpenAiOptions>(
    builder.Configuration.GetSection("OpenAI")
);

// =======================
// HTTP CLIENTS
// =======================

// -------- API-Football --------
builder.Services.AddHttpClient<IApiFootballService, ApiFootballService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiFootball:BaseUrl"]!
    );

    client.DefaultRequestHeaders.Add(
        "x-apisports-key",
        builder.Configuration["ApiFootball:ApiKey"]!
    );
});

// -------- SportMonks --------
builder.Services.AddHttpClient<ISportMonksService, SportMonksService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["SportMonks:BaseUrl"]!
    );
});

// -------- Football-Data.org --------
builder.Services.AddHttpClient<IFootballDataService, FootballDataService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["FootballData:BaseUrl"]!
    );

    client.DefaultRequestHeaders.Add(
        "X-Auth-Token",
        builder.Configuration["FootballData:ApiKey"]!
    );
});

// -------- OpenAI --------
builder.Services.AddHttpClient<IOpenAiService, OpenAiService>(client =>
{
    client.BaseAddress = new Uri("https://api.openai.com");

    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue(
            "Bearer",
            builder.Configuration["OpenAI:ApiKey"]
        );
});

// =======================
// FACADES
// =======================
builder.Services.AddScoped<IFootballPredictionFacade, FootballPredictionFacade>();

// =======================
// ENGINES
// =======================
builder.Services.AddScoped<IProviderAggregator, ProviderAggregator>();
builder.Services.AddScoped<IMathScoringEngine, MathScoringEngine>();
builder.Services.AddScoped<IAiPredictionEngine, AiPredictionEngine>();
builder.Services.AddScoped<IFinalDecisionEngine, FinalDecisionEngine>();

// =======================
// BUILD
// =======================
var app = builder.Build();

// =======================
// MIDDLEWARE
// =======================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// =======================
// ROUTING
// =======================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();

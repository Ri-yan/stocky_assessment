
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stocky.Api.Services;
using Stocky.Api.Repositories;
using Stocky.Api.Background;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// In-memory repositories / services
builder.Services.AddSingleton<IRewardRepository, InMemoryRewardRepository>();
builder.Services.AddSingleton<ILedgerRepository, InMemoryLedgerRepository>();
builder.Services.AddSingleton<IHoldingRepository, InMemoryHoldingRepository>();
builder.Services.AddSingleton<IPriceService, RandomPriceService>();
builder.Services.AddSingleton<IIdempotencyService, InMemoryIdempotencyService>();

// Business service
builder.Services.AddScoped<IRewardService, RewardService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<IStatsService, StatsService>();

// Background price updater (simulates hourly price refresh)
builder.Services.AddHostedService<PriceBackgroundService>();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stocky API V1");
        c.RoutePrefix = string.Empty; // so Swagger UI shows at http://localhost:<port>/
    });
//}


app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();

using System;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RiotSharp;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {
        builder.Services.AddLogging();
        builder.Services.AddOptions();
        builder.Services.Configure<JsonSerializerOptions>(options =>
        {
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.PropertyNameCaseInsensitive = false;
        });

        string? apiKey = Environment.GetEnvironmentVariable("RIOTGAMES_API_KEY", EnvironmentVariableTarget.Process);
        Console.WriteLine(apiKey);
        builder.Services.AddSingleton(RiotApi.GetDevelopmentInstance(apiKey));
        builder.Services.AddSingleton(provider => provider.GetRequiredService<RiotApi>().DataDragon);
    })
    .Build();

await host.RunAsync();

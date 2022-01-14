using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RiotSharp;
using RiotSharp.Endpoints.ChampionMasteryEndpoint;
using RiotSharp.Misc;

namespace HextechCheck.Functions;

public class ChampionMasteries
{
    private readonly ILogger _logger;
    private readonly RiotApi _riotApi;

    public ChampionMasteries(ILoggerFactory loggerFactory, RiotApi riotApi)
    {
        _logger = loggerFactory.CreateLogger<ChampionMasteries>();
        _riotApi = riotApi;
    }

    [Function("ChampionMasteries")]
    public async Task<IEnumerable<ChampionMastery>?> Run(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "get",
            Route = "ChampionMasteries/{region}/{name}")]
        HttpRequestData req,
        string region,
        string name,
        FunctionContext executionContext)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(region))
        {
            return null;
        }

        // Fetch summoner by name and region from route.
        name = Regex.Replace(name, @"\s+", "");
        var regionEnum = Enum.Parse<Region>(region, true);
        var summoner = await _riotApi.Summoner.GetSummonerByNameAsync(regionEnum, name);
        var championMasteries = await _riotApi.ChampionMastery
            .GetChampionMasteriesAsync(regionEnum, summoner.Id);

        _logger.LogTrace($"Fetched champion masteries for {name} in {region}");
        return championMasteries;
    }
}

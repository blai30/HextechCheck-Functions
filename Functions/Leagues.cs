using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RiotSharp;
using RiotSharp.Endpoints.LeagueEndpoint;
using RiotSharp.Misc;

namespace HextechCheck.Functions;

public class Leagues
{
    private readonly ILogger _logger;
    private readonly RiotApi _riotApi;

    public Leagues(ILoggerFactory loggerFactory, RiotApi riotApi)
    {
        _logger = loggerFactory.CreateLogger<Leagues>();
        _riotApi = riotApi;
    }

    [Function("Leagues")]
    public async Task<List<LeagueEntry>?> Run(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "get",
            Route = "Leagues/{region}/{summonerId}")]
        HttpRequestData req,
        string region,
        string summonerId,
        FunctionContext executionContext)
    {
        if (string.IsNullOrEmpty(summonerId) || string.IsNullOrEmpty(region))
        {
            return null;
        }

        // Fetch leagues by summoner Id and region from route.
        var regionEnum = Enum.Parse<Region>(region, true);
        var leagues = await _riotApi.League.GetLeagueEntriesBySummonerAsync(regionEnum, summonerId);

        _logger.LogTrace($"Fetched {leagues.Count} leagues for summoner {summonerId} in {region}");
        return leagues;
    }
}

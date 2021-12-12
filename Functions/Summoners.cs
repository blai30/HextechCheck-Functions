using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RiotSharp;
using RiotSharp.Endpoints.SummonerEndpoint;
using RiotSharp.Misc;

namespace HextechCheck.Functions;

public class Summoners
{
    private readonly ILogger _logger;
    private readonly RiotApi _riotApi;

    public Summoners(ILoggerFactory loggerFactory, RiotApi riotApi)
    {
        _logger = loggerFactory.CreateLogger<Summoners>();
        _riotApi = riotApi;
    }

    [Function("Summoners")]
    public async Task<Summoner?> Run(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "get",
            Route = "Summoners/{region}/{name}")]
        HttpRequestData req,
        string region,
        string name,
        FunctionContext executionContext)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(region))
        {
            return null;
        }

        name = Regex.Replace(name, @"\s+", "");
        var regionEnum = Enum.Parse<Region>(region, true);
        var summoner = await _riotApi.Summoner.GetSummonerByNameAsync(regionEnum, name);
        return summoner;
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RiotSharp;
using RiotSharp.Endpoints.Interfaces.Static;
using RiotSharp.Endpoints.StaticDataEndpoint.Champion;

namespace HextechCheck.Functions;

public class Champions
{
    private readonly ILogger _logger;
    private readonly RiotApi _riotApi;
    private readonly IDataDragonEndpoints _dataDragon;

    public Champions(ILoggerFactory loggerFactory, RiotApi riotApi, IDataDragonEndpoints dataDragon)
    {
        _logger = loggerFactory.CreateLogger<Champions>();
        _riotApi = riotApi;
        _dataDragon = dataDragon;
    }

    [Function("Champions")]
    public async Task<Dictionary<int, ChampionStatic>> Run(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "get")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var versions = await _riotApi.DataDragon.Versions.GetAllAsync();
        string? latestVersion = versions.FirstOrDefault();
        var championList = await _dataDragon.Champions.GetAllAsync(latestVersion, fullData: false);

        var champions = new Dictionary<int, ChampionStatic>();
        foreach (var champion in championList.Champions.Values)
        {
            champions[champion.Id] = champion;
        }

        return champions;
    }
}

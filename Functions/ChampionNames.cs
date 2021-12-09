using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RiotSharp;
using RiotSharp.Endpoints.Interfaces.Static;

namespace HextechCheck.Functions;

public class ChampionNames
{
    private readonly ILogger _logger;
    private readonly RiotApi _riotApi;
    private readonly IDataDragonEndpoints _dataDragon;

    public ChampionNames(ILoggerFactory loggerFactory, RiotApi riotApi, IDataDragonEndpoints dataDragon)
    {
        _logger = loggerFactory.CreateLogger<ChampionNames>();
        _riotApi = riotApi;
        _dataDragon = dataDragon;
    }

    [Function("ChampionNames")]
    public async Task<Dictionary<string, string>?> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        var versions = await _riotApi.DataDragon.Versions.GetAllAsync();
        string? latestVersion = versions.FirstOrDefault();
        var champions = await _dataDragon.Champions.GetAllAsync(latestVersion);

        var championNames = new Dictionary<string, string>();
        foreach (var champion in champions.Champions.Values)
        {
            championNames[champion.Id.ToString()] = champion.Name;
        }

        return championNames;
    }
}

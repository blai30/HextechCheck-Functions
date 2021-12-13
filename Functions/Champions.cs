using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RiotSharp.Endpoints.Interfaces.Static;
using RiotSharp.Endpoints.StaticDataEndpoint;
using RiotSharp.Endpoints.StaticDataEndpoint.Champion;

namespace HextechCheck.Functions;

public class Champions
{
    private readonly ILogger _logger;
    private readonly IDataDragonEndpoints _dataDragon;

    public Champions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Champions>();
        _dataDragon = DataDragonEndpoints.GetInstance();
    }

    [Function("Champions")]
    public async Task<Dictionary<int, ChampionStatic>> Run(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "get",
            Route = "Champions")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var versions = await _dataDragon.Versions.GetAllAsync();
        string? latestVersion = versions.FirstOrDefault();
        var championList = await _dataDragon.Champions.GetAllAsync(latestVersion, fullData: false);
        var champions = championList.Champions.Values.ToDictionary(champion => champion.Id);
        return champions;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using HextechCheck.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RiotSharp;
using RiotSharp.Endpoints.Interfaces.Static;
using RiotSharp.Endpoints.StaticDataEndpoint;
using RiotSharp.Misc;

namespace HextechCheck.Functions;

public class ChampionMasteries
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly RiotApi _riotApi;
    private readonly IDataDragonEndpoints _dataDragon;

    public ChampionMasteries(ILoggerFactory loggerFactory, IMapper mapper, RiotApi riotApi)
    {
        _logger = loggerFactory.CreateLogger<ChampionMasteries>();
        _mapper = mapper;
        _riotApi = riotApi;
        _dataDragon = DataDragonEndpoints.GetInstance();
    }

    [Function("ChampionMasteries")]
    public async Task<IEnumerable<ChampionMasteryDto>?> Run(
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

        name = Regex.Replace(name, @"\s+", "");
        var regionEnum = Enum.Parse<Region>(region, true);
        var summoner = await _riotApi.Summoner.GetSummonerByNameAsync(regionEnum, name);
        var championMasteries = await _riotApi.ChampionMastery
            .GetChampionMasteriesAsync(regionEnum, summoner.Id);

        var versions = await _dataDragon.Versions.GetAllAsync();
        string? latestVersion = versions.FirstOrDefault();
        var championList = await _dataDragon.Champions.GetAllAsync(latestVersion, fullData: false);
        var champions = championList.Champions.Values.ToDictionary(champion => champion.Id);

        var dto = _mapper.Map<IEnumerable<ChampionMasteryDto>?>(championMasteries);

        return dto;
    }
}

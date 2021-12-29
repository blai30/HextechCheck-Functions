using System;
using System.Text.Json.Serialization;
using RiotSharp.Endpoints.StaticDataEndpoint.Champion;
using RiotSharp.Endpoints.SummonerEndpoint;

namespace HextechCheck.Models;

public class ChampionMasteryDto
{
    /// <summary>
    /// Champion for this entry.
    /// </summary>
    [JsonPropertyName("champion")]
    public ChampionStatic Champion { get; set; }

    /// <summary>
    /// Champion level for specified player and champion combination.
    /// </summary>
    [JsonPropertyName("championLevel")]
    public int ChampionLevel { get; set; }

    /// <summary>
    /// Total number of champion points for this player and champion combination -
    /// they are used to determine championLevel.
    /// </summary>
    [JsonPropertyName("championPoints")]
    public int ChampionPoints { get; set; }

    /// <summary>
    /// Number of points earned since current level has been achieved.
    /// Zero if player reached maximum champion level for this champion.
    /// </summary>
    [JsonPropertyName("championPointsSinceLastLevel")]
    public long ChampionPointsSinceLastLevel { get; set; }

    /// <summary>
    /// Number of points needed to achieve next level.
    /// Zero if player reached maximum champion level for this champion.
    /// </summary>
    [JsonPropertyName("championPointsUntilNextLevel")]
    public long ChampionPointsUntilNextLevel { get; set; }

    /// <summary>
    /// Is chest granted for this champion or not in current season.
    /// </summary>
    [JsonPropertyName("chestGranted")]
    public bool ChestGranted { get; set; }

    /// <summary>
    /// Last time this champion was played by this player.
    /// </summary>
    [JsonPropertyName("lastPlayTime")]
    public DateTime LastPlayTime { get; set; }

    /// <summary>
    /// Summoner for this entry.
    /// </summary>
    [JsonPropertyName("summoner")]
    public Summoner Summoner { get; set; }
}

using AutoMapper.Configuration;
using HextechCheck.Models;
using RiotSharp.Endpoints.ChampionMasteryEndpoint;

namespace HextechCheck.Maps;

public class MappingProfile : MapperConfigurationExpression
{
    public MappingProfile()
    {
        CreateMap<ChampionMastery, ChampionMasteryDto>()
            .ForPath(
                e => e.Champion.Id,
                opt => opt
                    .MapFrom(e => e.ChampionId))
            .ForPath(
                e => e.Summoner.Id,
                opt => opt
                    .MapFrom(e => e.SummonerId))
            .ReverseMap();
    }
}

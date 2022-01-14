using AutoMapper;
using HextechCheck.Models;
using RiotSharp.Endpoints.ChampionMasteryEndpoint;

namespace HextechCheck.Maps;

public class MappingProfile : MapperConfigurationExpression
{
    public MappingProfile()
    {
        CreateMap<ChampionMastery, ChampionMasteryDto>();
    }
}

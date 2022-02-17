using System;
using AutoMapper;
using Tournament.DAL.Entities;
using Tournament.Common.Models;


namespace Tournament.API.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PersonEntity, PersonListModel>();
            CreateMap<PersonEntity, PersonDetailModel>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.TeamName))
                .ForMember(dest => dest.TeamLogoURL, opt => opt.MapFrom(src => src.Team.LogoURL));
            CreateMap<PersonDetailModel, PersonEntity>();
            CreateMap<PersonListModel, PersonEntity>();

            CreateMap<PlaceEntity, PlaceListModel>();
            CreateMap<PlaceEntity, PlaceDetailModel>();
            CreateMap<PlaceDetailModel, PlaceEntity>();
            CreateMap<PlaceListModel, PlaceEntity>();

            CreateMap<MatchEntity, MatchListModel>()
                .ForMember(dest => dest.Team1Name, opt => opt.MapFrom(src => src.Team1.TeamName))
                .ForMember(dest => dest.Team2Name, opt => opt.MapFrom(src => src.Team2.TeamName));
            CreateMap<MatchEntity, MatchDetailModel>()
                .ForMember(dest => dest.Team1Name, opt => opt.MapFrom(src => src.Team1.TeamName))
                .ForMember(dest => dest.Team1LogoURL, opt => opt.MapFrom(src => src.Team1.LogoURL))
                .ForMember(dest => dest.Team2Name, opt => opt.MapFrom(src => src.Team2.TeamName))
                .ForMember(dest => dest.Team2LogoURL, opt => opt.MapFrom(src => src.Team2.LogoURL))
                .ForMember(dest => dest.PlaceName, opt => opt.MapFrom(src => src.Place.Name));
            CreateMap<MatchListModel, MatchEntity>();
            CreateMap<MatchDetailModel, MatchEntity>();

            CreateMap<TeamEntity, TeamListModel>();
            CreateMap<TeamEntity, TeamDetailModel>();
            CreateMap<TeamDetailModel, TeamEntity>();
            CreateMap<TeamListModel, TeamEntity>();
        }
    }
}

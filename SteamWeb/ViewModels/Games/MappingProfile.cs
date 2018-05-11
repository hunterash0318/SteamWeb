using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace SteamWeb.ViewModels.Games
{
    using SteamWeb.Models;
    using SteamWeb.ViewModels.Users;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, Buy>().ReverseMap();
            CreateMap<Game, GameDetail>().ReverseMap();
            CreateMap<Game, Add>().ReverseMap();
            CreateMap<Game, Edit>().ReverseMap();
            CreateMap<Game, GameItem>().ReverseMap();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace SteamWeb.Infrastructure
{
    using SteamWeb.Models;
    using SteamWeb.ViewModels.Users;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDetail>().ReverseMap();
            CreateMap<User, EditUser>().ReverseMap();
            CreateMap<User, AddFunds>().ReverseMap();
            CreateMap<User, AddUser>().ReverseMap();
        }
    }
}
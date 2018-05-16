using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SteamWeb.Infrastructure;
using SteamWeb.Infrastructure.Authentication;

namespace SteamWeb.ViewComponents.NavBar
{
    public class NavBarViewComponent : ViewComponent
    {
        private readonly ICurrentUserContext _context;
        public NavBarViewComponent(ICurrentUserContext context)
        {
            _context = context;
        }
    }
}

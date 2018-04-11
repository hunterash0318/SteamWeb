using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamWeb.Infrastructure.Authentication
{
    // My own implementation that stores the same information as Microsoft.AspNetCore.Authentication.AuthenticationProperties class.
    // It exists so that my custom "IAuthenticationManager" interface doesn't need
    // to depend on the Microsoft AuthenticationProperties class.
    public class CookieOptions
    {
        // I don't care about readonly or making this immutable, currently.
        // It's just a simple DTO.
        public DateTime ExpirationDateTime { get; set; }
        public bool IsPersistent { get; set; }
        public bool AllowRefresh { get; set; }

    }
}
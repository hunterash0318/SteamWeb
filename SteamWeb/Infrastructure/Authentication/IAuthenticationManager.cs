using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace SteamWeb.Infrastructure.Authentication
{
    public interface IAuthenticationManager
    {
        Task SignInAsync(IEnumerable<UserInfo> userInformations);
        Task SignInAsync(IEnumerable<UserInfo> userInformations, CookieOptions options);
        Task SignOutAsync();
        Task SignOutAsync(CookieOptions options);
    }
}

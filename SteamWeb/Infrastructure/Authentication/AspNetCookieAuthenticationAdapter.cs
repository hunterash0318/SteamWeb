using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SteamWeb.Infrastructure.Authentication
{
    public class AspNetCookieAuthenticationAdapter : IAuthenticationManager
    {
        private readonly IHttpContextAccessor _accessor;
        public AspNetCookieAuthenticationAdapter(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Task SignInAsync(IEnumerable<UserInfo> userInformations)
        {
            return _accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, CreatePrincipalFromUserInfos(userInformations));
        }
        public Task SignInAsync(IEnumerable<UserInfo> userInformations, CookieOptions options)
        {
            return _accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, CreatePrincipalFromUserInfos(userInformations), new AuthenticationProperties
            {
                ExpiresUtc = options.ExpirationDateTime,
                IsPersistent = options.IsPersistent,
                AllowRefresh = options.AllowRefresh
            });
        }

        private ClaimsPrincipal CreatePrincipalFromUserInfos(IEnumerable<UserInfo> userInformations)
        {
            var claims = userInformations.Select(info => new Claim(info.InfoType, info.Value));
            var userIdentity = new ClaimsIdentity(claims, "WebsiteLogin");
            return new ClaimsPrincipal(userIdentity);
        }

        public Task SignOutAsync()
        {
            return _accessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public Task SignOutAsync(CookieOptions options)
        {
            return _accessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                ExpiresUtc = options.ExpirationDateTime,
                IsPersistent = options.IsPersistent,
                AllowRefresh = options.AllowRefresh
            });
        }

        public void RefreshCookie(params UserInfo[] infos)
        {
            var Identity = _accessor.HttpContext.User.Identity as ClaimsIdentity;
            foreach (var info in infos)
            {
                var MaybeClaim = Identity.FindFirst(info.InfoType);
                if (MaybeClaim == null)
                {
                    throw new KeyNotFoundException($"Key '{info.InfoType}' not found in current Claims");
                }

                Identity.RemoveClaim(Identity.FindFirst(info.InfoType));
                Identity.AddClaim(new Claim(info.InfoType, info.Value));
            }

            _accessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(Identity));
            return;
        }
    }
}
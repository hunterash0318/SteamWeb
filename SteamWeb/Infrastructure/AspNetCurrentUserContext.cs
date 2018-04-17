using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SteamWeb.Infrastructure.Authentication;

namespace SteamWeb.Infrastructure
{
    //TODO: Make this a sealed class?
    public class AspNetCurrentUserContext : ICurrentUserContext
    {
        //private readonly IHttpContextAccessor _accessor;
        private readonly Func<ClaimsPrincipal> _provider;
        public AspNetCurrentUserContext(Func<ClaimsPrincipal> userProvider)
        {
            _provider = userProvider;
        }

        public bool IsAuthenticated => _provider().Identity.IsAuthenticated;

        public string UserName
        {
            get
            {
                var principle = _provider();
                var value = _provider().Claims.SingleOrDefault(x => x.Type == UserInfo.UserNameInfoType)?.Value;
                if (value == null)
                {
                    // We throw our OWN "KeyNotFoundException" here instead of just calling "Single" above because the error message that "Single" provides when it
                    // doesn't find anything isn't very helpful in diagnosing what went wrong.
                    throw new KeyNotFoundException($"The cookie did not contain a value for the key \"{UserInfo.UserNameInfoType}\".");
                }
                return value;
            }
        }

        public int UserId
        {
            get
            {
                var rawValue = _provider().Claims.SingleOrDefault(x => x.Type == UserInfo.UserIdInfoType)?.Value;
                if (rawValue == null)
                {
                    throw new KeyNotFoundException($"The cookie did not contain a value for the key \"{UserInfo.UserIdInfoType}\".");
                }
                if (!Int32.TryParse(rawValue, out int result))
                {
                    throw new FormatException($"The cookie value for the User Id was in a bad format!");
                }
                return result;
            }
        }

        public bool UserType
        {
            get
            {
                var rawValue = _provider().Claims.SingleOrDefault(x => x.Type == UserInfo.UserTypeInfoType)?.Value;
                if (rawValue == null)
                {
                    throw new KeyNotFoundException($"The cookie did not contain a value for the key \"{UserInfo.UserTypeInfoType}\".");
                }
                if (!Boolean.TryParse(rawValue, out bool value))
                {
                    throw new FormatException($"The cookie value for the user type value was in a bad format!");
                }
                return value;
            }
        }
    }
}
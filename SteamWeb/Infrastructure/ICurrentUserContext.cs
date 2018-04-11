using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using SteamWeb.Models;

namespace SteamWeb.Infrastructure
{
    public interface ICurrentUserContext
    {
        string UserName { get; }
        int UserId { get; }
        DateTime WorkingDate { get; }
        bool UserType { get; }
        bool IsAuthenticated { get; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SteamWeb.Infrastructure.Authentication
{
    public abstract class UserInfo
    {
        // Use the built-in value for user names - this will allow us to retrieve the UserName
        // by calling ClaimsPrincipal.Identity.Name (I think)
        public const string UserNameInfoType = ClaimTypes.Name;
        public const string UserIdInfoType = "usrId";
        public const string WorkingDateInfoType = "wrkDt";
        public const string UserTypeInfoType = "usrTp";

        // Switch out for my equivalents
        /*
        public const string AgencyIdInfoType = "agyId";
        public const string WorkingDateInfoType = "wrkDt";
        public const string UserTypeInfoType = "usrTp";
        */

        public readonly string InfoType;
        public string Value { get; protected set; }

        public UserInfo(string infoType)
        {
            InfoType = infoType;
            //Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public class UserIdInfo : UserInfo
    {
        public UserIdInfo(int userId)
            : base(UserIdInfoType)
        {
            Value = userId.ToString();
        }
    }
    public class UserNameInfo : UserInfo
    {
        public UserNameInfo(string username)
            : base(UserNameInfoType)
        {
            Value = username;
        }
    }

    public class UserTypeInfo : UserInfo
    {
        public UserTypeInfo(bool type)
            : base(UserTypeInfoType)
        {
            Value = type.ToString();
        }
    }
}

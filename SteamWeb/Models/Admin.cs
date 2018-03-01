using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamWeb.Models
{
    public class Admin
    {
        public virtual int Id { get; protected set; }

        public virtual string Username { get; set; }

        public virtual bool AddPermissions { get; set; }

        public virtual bool EditPermissions { get; set; }

        public virtual bool DeletePermissions { get; set; }

        public virtual bool UserPermissions { get; set; }
    }
}

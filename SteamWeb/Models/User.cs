using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamWeb.Models
{
    public class User
    {
        public virtual int Id { get; protected set; }

        public virtual string Username { get; set; }

        public virtual string Bio { get; set; }

        public virtual decimal Wallet { get; set; }

        public virtual string Location { get; set; }

        public virtual string Password { get; set; }

        public virtual IEnumerable<Game> GamesOwned { get; set; }

        public virtual IEnumerable<User> Friends { get; set; }
    }
}

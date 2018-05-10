using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamWeb.Models
{
    public class Gift
    {
        public virtual int Id { get; protected set; }

        public virtual int ReceiverId { get; set; }

        public virtual int SenderId { get; set; }

        public virtual int GameId { get; set; }

        public virtual bool Returned { get; set; }

        public virtual string Message { get; set; }
    }
}

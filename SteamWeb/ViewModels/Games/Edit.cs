using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamWeb.ViewModels.Games
{
    public class Edit
    {
        public virtual int  Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string Developer { get; set; }

        public virtual string Description { get; set; }

        public virtual string Genre { get; set; }

        public virtual decimal Price { get; set; }

        public virtual DateTime ReleaseDate { get; set; }
    }
}

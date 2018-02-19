using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamWeb.Models
{
    public class Game
    {
        public virtual int Id { get; protected set; }

        public virtual string Title { get; set; }

        public virtual string Developer { get; set; }

        public virtual string Description { get; set; }

        public virtual string Genre { get; set; }

        public virtual decimal Price { get; set; }

        public virtual DateTime ReleaseDate { get; set; }

        /*
        public void PrintAll()
        {
            Console.WriteLine("Title: {0}", Title);
            Console.WriteLine("Developer: {0}", Developer);
            Console.WriteLine("Description: {0}", Description);
            Console.WriteLine("Genre: {0}", Genre);
            Console.WriteLine("Price: ${0}", Price);
            Console.WriteLine("Release Date: {0}", ReleaseDate);
        }
        */

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"Title: {Title}");
            result.AppendLine($"Developer: {Developer}");
            result.AppendLine($"Description: {Description}");
            result.AppendLine($"Genre: {Genre}");
            result.AppendLine($"Price: $ {Price}");
            result.AppendLine($"Release Date: {ReleaseDate}");
            result.AppendLine($"Id Number: {Id}");

            return result.ToString();
        }
    }
}

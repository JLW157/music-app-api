using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Models.DbModels
{
    public class Genre : DbItem
    {
        public Genres TypeOfGenre { get; set; }
    }

    public enum Genres
    {
        Rock,
        Pop,
        HipHop,
        Rap,
    }
}

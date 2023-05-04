using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Models.DbModels
{
    public class Audio : DbItem
    {
        public string Name { get; set; } = null!;
        public string AudioUrl { get; set; } = null!;
        public string PosterUrl { get; set; } = null!;
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
        public ICollection<User> Artists { get; set; } = null!;
    }
}

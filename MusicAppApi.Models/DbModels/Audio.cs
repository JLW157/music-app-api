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
        public string Name { get; set; }
        public string AudioUrl { get; set; }
        public string? PosterUrl { get; set; }
        public Guid GenreId { get; set; }
        public virtual Genre Genre { get; set; }
        public int PlayedCount { get; set; }

        public virtual ICollection<Set> Sets { get; set; }
        public virtual ICollection<User> Artists { get; set; }
    }
}
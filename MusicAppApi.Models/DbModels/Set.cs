using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Models.DbModels
{
    public class Set : DbItem
    {
        public string Name { get; set; }
        public string? PosterUrl { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Audio> Audios { get; set; }
    }
}

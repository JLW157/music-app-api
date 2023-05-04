using MusicAppApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Models.DTO_s
{
    public class AudioResponse : DbItem
    {
        public string Name { get; set; } = null!;
        public string AudioUrl { get; set; } = null!;
        public string PosterUrl { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public List<string> Artists { get; set; } = null!;
    }
}

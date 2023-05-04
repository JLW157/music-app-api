using MusicAppApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Models.DTO_s.Artist
{
    public class ArtistsResponse
    {
        public string Name { get; set; } = null!;

        public List<string> AudioUrls { get; set; } = null!;

    }
}

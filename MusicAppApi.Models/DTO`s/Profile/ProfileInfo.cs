using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Models.DTO_s.Profile
{
    public class ProfileInfo
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IEnumerable<AudioResponse> Tracks { get; set; } = Enumerable.Empty<AudioResponse>();
    }
}

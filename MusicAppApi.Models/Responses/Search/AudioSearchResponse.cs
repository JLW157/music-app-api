using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Models.DTO_s;

namespace MusicAppApi.Models.Responses.Search
{
    public class AudioSearchResponse : AudioResponse
    {
        public string ItemRelativeUrl { get; set; }
        public string ItemAbsoluteUrl { get; set; }
    }
}

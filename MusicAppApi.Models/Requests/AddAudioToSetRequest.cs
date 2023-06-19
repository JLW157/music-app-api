using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Models.Requests
{
    public class AddAudioToSetRequest
    {
        public string AudioId { get; set; }
        public string SetId { get; set; }
    }
}

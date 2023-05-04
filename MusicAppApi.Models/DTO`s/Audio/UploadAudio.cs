using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace MusicAppApi.Models.DTO_s.Audio
{
    public class UploadAudio
    {
        public IFormFile File { get; set; }
    }
}

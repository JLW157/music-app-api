using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MusicAppApi.Models.Requests
{
    public class CreateSetRequest
    {
        public string Name { get; set; }
        public IFormFile? Poster { get; set; }
    }

}

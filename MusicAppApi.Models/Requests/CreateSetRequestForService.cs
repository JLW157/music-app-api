using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MusicAppApi.Models.Requests
{
    public class CreateSetRequestForService
    {
        public string Name { get; set; }
        public IFormFile Poster { get; set; }
        public required Guid UserId { get; set; }
    }
}

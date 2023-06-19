using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1;

namespace MusicAppApi.Models.Requests
{
    public class CreateSetRequestForDb
    {
        public string Name { get; set; }
        public string PosterUrl { get; set; }
        public required Guid UserId { get; set; }
    }
}

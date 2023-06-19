using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1;

namespace MusicAppApi.Models.Responses.Search
{
    public class UserSearchResponse
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string UserImageUrl { get; set; }

        public string ProfileRelativeUrl { get; set; }

        public string ProfileAbsoluteUrl { get; set; }
    }
}

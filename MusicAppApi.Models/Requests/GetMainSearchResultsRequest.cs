using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Models.Enums;
using Org.BouncyCastle.Asn1.Crmf;

namespace MusicAppApi.Models.Requests
{
    public class GetMainSearchResultsRequest
    {
        public string Term { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 10;
        public SearchRequestType SearchResultType { get; set; }
    }
}

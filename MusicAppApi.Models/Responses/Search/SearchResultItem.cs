using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Models.Enums;

namespace MusicAppApi.Models.Responses.Search
{
    public abstract class SearchResultItem
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
    }
}

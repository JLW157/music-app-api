using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Models.Enums;

namespace MusicAppApi.Models.Responses.Search
{
    public class SearchItem : SearchResultItem
    {
        public SearchItem()
        {
            
        }

        public SearchResultItemType ItemType { get; set; }
        public int Score { get; set; }
    }
}

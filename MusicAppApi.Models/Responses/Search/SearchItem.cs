using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Models.Enums;
using Newtonsoft.Json;

namespace MusicAppApi.Models.Responses.Search
{
    public class SearchItem
    {
        public SearchItem()
        {
            
        }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("itemRelativeUrl")]
        public string ItemRelativeUrl { get; set; }
        [JsonProperty("itemAbsoluteUrl")]
        public string ItemAbsoluteUrl { get; set; }

        [JsonProperty("itemType")]
        public SearchResultItemType ItemType { get; set; }
        [JsonProperty("score")]
        public int Score { get; set; }
    }
}

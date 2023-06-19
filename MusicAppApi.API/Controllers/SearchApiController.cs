using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAppApi.Core;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s;
using MusicAppApi.Models.Requests;
using MusicAppApi.Models.Responses.Search;
using Nest;
using System.Linq;
using Microsoft.OpenApi.Services;
using MusicAppApi.Core.interfaces.Utils;
using MusicAppApi.Models.Enums;
using static System.Net.WebRequestMethods;
using System.Drawing.Printing;
using Elastic.Clients.Elasticsearch;
using MailKit.Search;

namespace MusicAppApi.API.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchApiController : ControllerBase
    {
        private readonly MusicAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPagingUtils _pagingUtils;
        private readonly IConfiguration _configuration;
        public SearchApiController(MusicAppDbContext context, IMapper mapper, IPagingUtils pagingUtils, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _pagingUtils = pagingUtils;
            _configuration = configuration;
        }

        [HttpGet("audios")]
        public async Task<ActionResult<IEnumerable<SearchItem>>> GetAudios([FromQuery] string searchTerm)
        {
            var res = await SearchAutocompleteAsync(searchTerm);

            return Ok(res);
        }

        [HttpPost("GetMainSearchResults")]
        public async Task<ActionResult<SearchResultsContainer>> GetMainSearchResults(GetMainSearchResultsRequest searchRequest)
        {
            if (string.IsNullOrEmpty(searchRequest.Term))
            {
                // later i`ll add handling for this case
                return BadRequest("SearchTerm can`t be empty");
            }

            var result = new SearchResultsContainer();

            result.PageSize = searchRequest.PageSize;

            var searchResults = await GetMainSearchResultsSearchAsync(searchRequest);

            result.TotalResultsCount += searchResults.Count();

            searchResults = searchResults.OrderByDescending(s => s.Score)
                .Skip((searchRequest.CurrentPage - 1) * searchRequest.PageSize)
                .Take(searchRequest.PageSize)
                .ToList();

            result.SearchResultItems = searchResults;

            // and now pagination part
            result.CurrentPage = searchRequest.CurrentPage;
            result.TotalPages = _pagingUtils.GetPagerTotalPages(searchRequest.PageSize, result.TotalResultsCount);

            return Ok(result);
        }

        public class SearchResultsContainer
        {
            public string SearchTerm { get; set; }
            public int TotalResultsCount { get; set; }
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }

            public SearchResultsContainer()
            {
                SearchResultItems = new List<SearchGetMainSearchResultItem>();
            }

            public IEnumerable<SearchGetMainSearchResultItem> SearchResultItems { get; set; }
        }

        #region PrivateMethods

        private int CalculateAudioScore(Audio audio, string searchTerm, Guid? genreId)
        {
            int score = 0;

            if (audio.Name.ToLower().Contains(searchTerm))
                score += 10;

            if (genreId != null)
            {
                if (audio.GenreId == genreId)
                    score += 10;
            }


            return score;
        }

        private int CalculateArtistScore(User user, string searchTerm)
        {
            int score = 0;

            if (user.UserName.ToLower().Contains(searchTerm))
                score += 25;

            return score;
        }

        private async Task<IEnumerable<SearchItem>> SearchAutocompleteAsync(string searchTerm, Guid? genreId = null)
        {
            var frontUrl = _configuration.GetSection("front-url").Value;

            var audios = _context.Audios
                .Where(x => x.Name.ToLower().Contains(searchTerm))
                .Include(x => x.Genre)
                .Include(x => x.Artists);

            // artist search
            var artists = _context.Users
                .Where(x => x.UserName.ToLower().Contains(searchTerm));

            var searchResultsAudios = await audios
                .Select(a => new SearchItem
                {
                    ItemType = SearchResultItemType.Audio,
                    Score = (a.Name.ToLower().Contains(searchTerm) ? 10 : 0)
                            + (genreId != null && a.GenreId == genreId ? 10 : 0),
                    ImageUrl = a.PosterUrl,
                    Name = a.Name,
                    ItemRelativeUrl = $"/{a.Artists.First().UserName}/{a.Name}",
                    ItemAbsoluteUrl = $"{frontUrl}/{a.Artists.First().UserName}/{a.Name}"
                })
                .ToListAsync();

            var searchResultsArtists = await artists
                .Select(u => new SearchItem
                {
                    ItemType = SearchResultItemType.Artist,
                    Score = (u.UserName.ToLower().Contains(searchTerm) ? 25 : 0),
                    // todo: change this in the future (add user pics)
                    ImageUrl = "https://audiostorageweb123.blob.core.windows.net/user-images/defaultUserImg.png",
                    Name = u.UserName,
                    ItemRelativeUrl = $"/{u.UserName}",
                    ItemAbsoluteUrl = $"{frontUrl}/{u.UserName}"
                })
                .ToListAsync();

            var searchResults = searchResultsAudios
                .Union(searchResultsArtists);

            return searchResults;
        }

        private async Task<IEnumerable<SearchGetMainSearchResultItem>> GetMainSearchResultsSearchAsync(
            GetMainSearchResultsRequest searchResultsRequest)
        {
            var frontUrl = _configuration.GetSection("front-url").Value;
            List<SearchGetMainSearchResultItem> searchItems = new List<SearchGetMainSearchResultItem>();

            if (searchResultsRequest.SearchResultType == SearchRequestType.All || searchResultsRequest.SearchResultType == SearchRequestType.Tracks)
            {
                var audios = _context.Audios
                    .Where(x => x.Name.ToLower().Contains(searchResultsRequest.Term))
                    .Include(x => x.Genre)
                    .Include(x => x.Artists);

                searchItems.AddRange(await audios
                    .Select(a => new SearchGetMainSearchResultItem
                    {
                        ItemType = SearchResultItemType.Audio,
                        Score = (a.Name.ToLower().Contains(searchResultsRequest.Term) ? 10 : 0),
                        AudioItem = new AudioSearchResponse()
                        {
                            Id = a.Id,
                            AudioUrl = a.AudioUrl,
                            PosterUrl = a.PosterUrl,
                            Name = a.Name,
                            ItemRelativeUrl = $"/{a.Artists.First().UserName}/{a.Name}",
                            ItemAbsoluteUrl = $"{frontUrl}/{a.Artists.First().UserName}/{a.Name}",
                            PlayedCount = a.PlayedCount
                        },

                    }).ToListAsync());
            }

            if (searchResultsRequest.SearchResultType == SearchRequestType.All || searchResultsRequest.SearchResultType == SearchRequestType.Users)
            {
                // artist search
                var artists = _context.Users
                    .Where(x => x.UserName.ToLower().Contains(searchResultsRequest.Term));


                searchItems.AddRange(await artists
                    .Select(u => new SearchGetMainSearchResultItem()
                    {
                        ItemType = SearchResultItemType.Artist,
                        Score = (u.UserName.ToLower().Contains(searchResultsRequest.Term) ? 20 : 0),
                        UserItem = new UserSearchResponse()
                        {
                            Id = u.Id,
                            // todo: change this in the future (add user pics)
                            UserImageUrl = "https://audiostorageweb123.blob.core.windows.net/user-images/defaultUserImg.png",
                            Username = u.UserName,
                            ProfileRelativeUrl = $"/{u.UserName}",
                            ProfileAbsoluteUrl = $"{frontUrl}/{u.UserName}"
                        }
                    }).ToListAsync());
            }

            return searchItems;
        }

        public class SearchGetMainSearchResultItem
        {
            public float Score { get; set; }
            public SearchResultItemType ItemType { get; set; }
            public AudioSearchResponse AudioItem { get; set; }
            public UserSearchResponse UserItem { get; set; }

        }

        #endregion
    }
}

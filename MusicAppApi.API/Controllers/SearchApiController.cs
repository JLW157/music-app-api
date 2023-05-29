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

namespace MusicAppApi.API.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchApiController : ControllerBase
    {
        private readonly MusicAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPagingUtils _pagingUtils;
        public SearchApiController(MusicAppDbContext context, IMapper mapper, IPagingUtils pagingUtils)
        {
            _context = context;
            _mapper = mapper;
            _pagingUtils = pagingUtils;
        }

        [HttpGet("audios")]
        public async Task<ActionResult<IEnumerable<AudioResponse>>> GetAudios([FromQuery] string searchTerm)
        {
            var res = await _context.Audios
                .Where(x => x.Name.ToLower().Trim().Contains(searchTerm.ToLower().Trim()))
                .Include(x => x.Artists)
                .Include(x => x.Genre)
                .ToListAsync();


            var mappedRes = _mapper.Map<List<Audio>, List<AudioResponse>>(res);

            return Ok(mappedRes);
        }

        [HttpPost("GetMainSearchResults")]
        public async Task<ActionResult<SearchResultsContainer>> GetMainSearchResults(GetMainSearchResultsRequest searchRequest)
        {
            if (string.IsNullOrEmpty(searchRequest.Term))
            {
                // later i`ll add handling for this case
                return BadRequest("SearchTerm can`t be empty");
            }

            Guid? genreId = null;
            if (!string.IsNullOrEmpty(searchRequest.GenreId))
            {
                genreId = Guid.Parse(searchRequest.GenreId);
            }

            var result = new SearchResultsContainer();

            result.PageSize = searchRequest.PageSize;

            // audios search
            var audios = _context.Audios
                .Where(x => x.Name.ToLower().Contains(searchRequest.Term))
                .Include(x => x.Genre)
                .Include(x => x.Artists);

            // artist search
            var artists = _context.Users
                .Where(x => x.UserName.ToLower().Contains(searchRequest.Term.ToLower()));

            result.TotalResultsCount += await artists.CountAsync();
            result.TotalResultsCount += await audios.CountAsync();
            var searchResultsAudios = await audios
                .Select(a => new SearchItem()
                {
                    ItemType = SearchResultItemType.Audio,
                    Score = (a.Name.ToLower().Contains(searchRequest.Term) ? 10 : 0)
                            + (genreId != null && a.GenreId == genreId ? 10 : 0),
                    ImageUrl = a.PosterUrl,
                    Name = a.Name
                })
                .ToListAsync();

            var searchResultsArtists = await artists
                .Select(u => new SearchItem()
                {
                    ItemType = SearchResultItemType.Artist,
                    Score = (u.UserName.ToLower().Contains(searchRequest.Term) ? 25 : 0),
                    // todo: change this in the future (add user pics)
                    ImageUrl = "https://audiostorageweb123.blob.core.windows.net/user-images/defaultUserImg.png;",
                    Name = u.UserName
                })
                .ToListAsync();

            var searchResults = searchResultsAudios
                .Union(searchResultsArtists)
                .OrderByDescending(sr => sr.Score)
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
                SearchResultItems = new List<SearchItem>();
            }

            public IEnumerable<SearchItem> SearchResultItems { get; set; }
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

        #endregion
    }
}

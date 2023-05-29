using AutoMapper;
using Elastic.Clients.Elasticsearch.Core.UpdateByQueryRethrottle;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using MusicAppApi.Core;
using MusicAppApi.Core.Constants;
using MusicAppApi.Core.interfaces.Services;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s.Genres;

namespace MusicAppApi.API.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly MusicAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICachingService _cacheProvider;

        public GenresController(MusicAppDbContext musicDbContext,
            IMapper mapper, ICachingService cacheProvider)
        {
            _context = musicDbContext;
            _mapper = mapper;
            _cacheProvider = cacheProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreResponse>>> GetGenres()
        {
            var genres =
                _cacheProvider.GetFromCache<IEnumerable<GenreResponse>>(AppConstants.CacheConstants.GenreCacheKey);

            if (!genres.IsNullOrEmpty())
                return Ok(genres);

            var mappedGenres = _mapper.Map<IEnumerable<Genre>, IEnumerable<GenreResponse>>(await _context.Genres.ToListAsync());

            if (mappedGenres.IsNullOrEmpty())
                return StatusCode(500, "Genres not found");

            var res = _cacheProvider.SetCache<IEnumerable<GenreResponse>?>(
                AppConstants.CacheConstants.GenreCacheKey,
                mappedGenres, new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(60))
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10)));

            return Ok(mappedGenres);
        }
    }
}

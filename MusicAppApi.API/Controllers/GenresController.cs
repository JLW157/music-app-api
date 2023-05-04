using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAppApi.Core;
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

        public GenresController(MusicAppDbContext musicDbContext,
            IMapper mapper)
        {
            _context = musicDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<GenreResponse>> GetGenres()
        {
            return _mapper.Map<IEnumerable<Genre>, IEnumerable<GenreResponse>>(await _context.Genres.ToListAsync());
        }
    }
}

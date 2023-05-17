using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAppApi.Core;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s;
using Nest;

namespace MusicAppApi.API.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchApiController : ControllerBase
    {
        private readonly MusicAppDbContext _context;
        private readonly IMapper _mapper;
        public SearchApiController(MusicAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("audios")]
        public async Task<ActionResult<IEnumerable<AudioResponse>>> GetAudios([FromQuery]string searchTerm)
        {
            var res = await _context.Audios
                .Where(x => x.Name.ToLower().Trim().Contains(searchTerm.ToLower().Trim()))
                .Include(x => x.Artists)
                .Include(x => x.Genre)
                .ToListAsync();


            var mappedRes = _mapper.Map<List<Audio>, List<AudioResponse>>(res);
            
            return Ok(mappedRes);
        }
    }
}

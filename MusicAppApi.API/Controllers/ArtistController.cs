using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using MusicAppApi.Core;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s.Artist;

namespace MusicAppApi.API.Controllers
{
    [Route("api/artists")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly MusicAppDbContext _context;
        private readonly IMapper _mapper;

        public ArtistController(MusicAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //[HttpPost]
        //public async Task<ActionResult> CreateArtist(ArtistCreationDTO artistCreationDTO)
        //{
        //    if (artistCreationDTO.AudioUrls != null)
        //    {
        //        // find urls find audios by url in db
        //    }

        //    // just add artist to db

        //    var artistToAdd = new Artist
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = artistCreationDTO.Name,
        //    };


        //    await _context.AddAsync(artistToAdd);

        //    return Ok(_mapper.Map<Artist, ArtistsResponse>(artistToAdd));
        //}
    }
}

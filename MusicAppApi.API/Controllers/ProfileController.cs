using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAppApi.Core;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s;
using MusicAppApi.Models.DTO_s.Profile;

namespace MusicAppApi.API.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly MusicAppDbContext _context;
        private readonly IMapper _mapper;
        public ProfileController(MusicAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ProfileInfo>> GetProfileInfo(string username)
        {
            ProfileInfo profileInfo = new ProfileInfo();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Trim() == username.ToLower().Trim());

            if (user == null)
            {
                return BadRequest("Profile not found");
            }

            profileInfo.Username = user.UserName;
            profileInfo.Email = user.Email;

            var audios = await _context.Audios
                .Include(x => x.Artists)
                .Include(x => x.Genre)
                .Where(a => a.Artists.Any(a => a.Id == user.Id))
                .ToListAsync();

            var mappedAudios = _mapper.Map<List<Audio>, List<AudioResponse>>(audios);
            profileInfo.Tracks = mappedAudios;

            return Ok(profileInfo);
        }
    }
}

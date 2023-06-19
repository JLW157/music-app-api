using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicAppApi.API.Extensions;
using MusicAppApi.Core.interfaces.Services;
using MusicAppApi.Core.Services;
using MusicAppApi.Models.DTO_s.Sets;
using MusicAppApi.Models.Requests;
using MusicAppApi.Models.Responses;

namespace MusicAppApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SetsController : ControllerBase
    {
        private readonly ISetsService _setsService;
        private readonly IAudioService _audioService;

        public SetsController(ISetsService setsService, IAudioService audioService)
        {
            _setsService = setsService;
            _audioService = audioService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<CreateSetResponse>> CreateSet([FromForm] CreateSetRequest createSetRequest)
        {
            var userId = Guid.Parse(HttpContext.GetUserIdFromClaim());

            var createUserRequestForService = new CreateSetRequestForService()
            { Name = createSetRequest.Name, Poster = createSetRequest.Poster, UserId = userId };

            var resposne = await _setsService.CreateSet(createUserRequestForService);

            if (!resposne.IsSuccess)
            {
                return BadRequest(resposne.Message);
            }

            return Ok(resposne);
        }

        [HttpGet]
        public async Task<ActionResult<SetDto>> GetUserSets()
        {
            var userId = Guid.Parse(HttpContext.GetUserIdFromClaim());

            var sets = await _setsService.GetUserSets(userId);

            return Ok(sets);
        }

        [HttpGet("getSetsByUsername")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SetDto>>> GetSetsByUsername([FromQuery] string username)
        {
            var userSets = await _setsService.GetSetsByUsername(username);

            return Ok(userSets);
        }

        [HttpPost("addAudio")]
        public async Task<ActionResult> AddAudioToSet(AddAudioToSetRequest addAudioToSetRequest)
        {
            var userId = Guid.Parse(HttpContext.GetUserIdFromClaim());

            var audio = await _setsService.AddAudioToSet(addAudioToSetRequest);

            return Ok(audio);
        }

        [HttpDelete("removeAudio")]
        public async Task<ActionResult> RemoveAudioFromSet(RemoveAudioFromSetRequest removeAudioFromSetRequest)
        {
            var userId = Guid.Parse(HttpContext.GetUserIdFromClaim());

            var audio = await _setsService.RemoveAudioFromSet(removeAudioFromSetRequest);

            return Ok(audio);
        }
    }
}

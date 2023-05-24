using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAppApi.Core;
using MusicAppApi.Core.Constants;
using MusicAppApi.Models.DbModels;
using MusicAppApi.Models.DTO_s;
using MusicAppApi.Models.DTO_s.Audio;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MusicAppApi.Core.Interfaces;
using MusicAppApi.Models.Enums;
using MusicAppApi.Core.interfaces;

namespace MusicAppApi.API.Controllers
{
    [Route("api/audio")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly IAudioService _audioService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAzureBlobService _azureBlobService;
        private readonly IGenreService _genreService;

        public AudioController(
            IMapper mapper, 
            IAzureBlobService blobService, 
            IAudioService audioService, 
            IUserService userService, 
            IGenreService genreService)
        {
            this._mapper = mapper;
            _azureBlobService = blobService;
            _audioService = audioService;
            _userService = userService;
            _genreService = genreService;
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IEnumerable<AudioResponse>> GetAudios()
        {
            var audios = await _audioService.GetAllAudios();

            return audios;
        }

        [HttpGet("audioByName")]
        public async Task<ActionResult<AudioResponse>> GetAudioByName([FromQuery]string name)
        {
            var audio = await _audioService.GetAudioByName(name);

            if (audio != null)
            {
                return Ok(audio);
            }

            return BadRequest("Audio not found");
        }

        [HttpGet("userAudiosByUsername")]
        public async Task<ActionResult> GetUserAudiosByUsername([FromQuery]string username)
        {
            var audios = await _audioService.GetAudiosByName(username);
            
            return Ok(audios);
        }

        [HttpGet("userAudios")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<AudioResponse>>> GetUserAudios()
        {
            var artistIdClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.ClaimNames.Id);
            if (artistIdClaim == null)
            {
                return BadRequest("Invalid user");
            }

            var artist = await _userService.GetUserById(Guid.Parse(artistIdClaim.Value));
            if (artist == null)
            {
                return BadRequest("Invalid user");
            }

            var userAudios = await _audioService.GetUserAudios(artist.Id);
            
            return Ok(userAudios);
        }


        [HttpPost]
        public async Task<ActionResult> CreateAudio(AudioCreationDTO audioCreationDTO)
        {
            // Check genre exists
            var genreExisting = await _genreService.GetGenreById(Guid.Parse(audioCreationDTO.Genre));

            if (genreExisting == null)
            {
                return BadRequest("Incorrect genre");
            }

            // Check correctness of artists

            var artists = await _userService.GetUsersBySpecificCase(audioCreationDTO.Artists);

            if ((artists == null) || (artists.Count != audioCreationDTO.Artists.Count))
            {
                return BadRequest("Bad artists request");
            }

            var audioToAdd = new Audio
            {
                Id = Guid.NewGuid(),
                Artists = artists,
                Genre = genreExisting,
                Name = audioCreationDTO.Name,
                AudioUrl = audioCreationDTO.AudioUrl
            };

            await _audioService.CreateAudio(audioToAdd);

            return Ok(_mapper.Map<Audio, AudioResponse>(audioToAdd));
        }

        [HttpPost("upload")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Upload([FromForm] UploadAudio uploadAudio)
        {
            try
            {
                var trackUploadResult = await _azureBlobService.UploadBlobAsync(uploadAudio.Track, uploadAudio.Title, AppConstants.AzureBlob.AudioBlobContainerName);

                if (trackUploadResult.UploadStatus == AzureUploadStatuses.NameAlreadyExists)
                {
                    return BadRequest("Track with that name already exists");
                }
                if (trackUploadResult.UploadStatus == AzureUploadStatuses.Notuploaded)
                {
                    return BadRequest("Track not uploaded. Something went wrong");
                }

                var artistIdClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.ClaimNames.Id);
                if (artistIdClaim == null)
                {
                    return BadRequest("Invalid user");
                }

                var artist = await _userService.GetUserById(Guid.Parse(artistIdClaim.Value));
                if (artist == null)
                {
                    return BadRequest("Invalid user");
                }

                var audio = new Audio();

                audio.Artists = new List<User>() { artist };
                audio.AudioUrl = trackUploadResult.BlobUrl;
                audio.Name = uploadAudio.Title;
                audio.GenreId = Guid.Parse(uploadAudio.Genre);

                if (uploadAudio.Image != null)
                {
                    var imageUploadResult = await _azureBlobService.UploadBlobAsync(uploadAudio.Image, uploadAudio.Image.FileName, AppConstants.AzureBlob.ImageBlobContainerName);

                    if (imageUploadResult.UploadStatus == AzureUploadStatuses.Notuploaded)
                        return BadRequest("Image not uploaded. Something went wrong");

                    audio.PosterUrl = imageUploadResult.BlobUrl;
                }

                await _audioService.CreateAudio(audio);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return Ok();
        }

        //[HttpPost("createTest")]
        //public async Task<AudioResponse> CreateAudioTest(AudioCreationDTO audioCreationDTO)
        //{
        //    var genre = await _context.Genres.FirstOrDefaultAsync(x => x.TypeOfGenre == Genres.Pop);

        //    var artist = await _context.Artists.FirstOrDefaultAsync(x => x.Name == "Amigo");

        //    await _context.AddAsync(genre);
        //    await _context.AddAsync(artist);

        //    var audio = new Track()
        //    {
        //        Id = Guid.NewGuid(),
        //        Genre = genre,
        //        Artists = new List<Artist> { artist },
        //        Name = audioCreationDTO.Name,
        //        AudioUrl = audioCreationDTO.AudioUrl,
        //        PosterUrl = audioCreationDTO.PosterUrl
        //    };

        //    await _context.AddAsync(audio);

        //    await _context.SaveChangesAsync();

        //    return _mapper.Map<Track, AudioResponse>(audio);
        //}

        [HttpGet("popular")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<AudioResponse>> GetPopularAudios()
        {
            var audios = await _audioService.GetAudiosWithLimit(20);

            return audios;
        }

        [HttpGet("test")]
        public string ForTest()
        {
            return "Success";
        }
    }
}

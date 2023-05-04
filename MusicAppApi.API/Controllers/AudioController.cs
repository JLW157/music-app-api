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

namespace MusicAppApi.API.Controllers
{

    [Route("api/audio")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly MusicAppDbContext context;
        private readonly IMapper mapper;

        public AudioController(MusicAppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IEnumerable<AudioResponse>> GetAudios()
        {
            var audios = await context.Audios.Include(x => x.Artists)
                .Include(x => x.Genre)
                .ToListAsync();

            return mapper.Map<List<Audio>, List<AudioResponse>>(audios);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAudio(AudioCreationDTO audioCreationDTO)
        {
            // Check genre exists
            var genreExisting = await context.Genres.FirstOrDefaultAsync(x => x.Id == Guid.Parse(audioCreationDTO.Genre));

            if (genreExisting == null)
            {
                return BadRequest("Incorrect genre");
            }

            // Check correctness of artists

            var artists = await context.Users.Where(a => audioCreationDTO.Artists.Contains(a.Id.ToString())).ToListAsync();

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

            await context.Audios.AddAsync(audioToAdd);

            return Ok(mapper.Map<Audio, AudioResponse>(audioToAdd));
        }

        [HttpPost("upload")]
        //[Authorize(Roles = $"{AuthConstants.UserRoles.Artist}, {AuthConstants.UserRoles.Artist}")]
        public async Task<IActionResult> Upload([FromForm]UploadAudio uploadAudio)
        {
            try
            {
                //string path = Path.Combine((Directory.GetCurrentDirectory(), ))
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
        //    var genre = await context.Genres.FirstOrDefaultAsync(x => x.TypeOfGenre == Genres.Pop);

        //    var artist = await context.Artists.FirstOrDefaultAsync(x => x.Name == "Amigo");

        //    await context.AddAsync(genre);
        //    await context.AddAsync(artist);

        //    var audio = new File()
        //    {
        //        Id = Guid.NewGuid(),
        //        Genre = genre,
        //        Artists = new List<Artist> { artist },
        //        Name = audioCreationDTO.Name,
        //        AudioUrl = audioCreationDTO.AudioUrl,
        //        PosterUrl = audioCreationDTO.PosterUrl
        //    };

        //    await context.AddAsync(audio);

        //    await context.SaveChangesAsync();

        //    return mapper.Map<File, AudioResponse>(audio);
        //}

        [HttpGet("popular")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<AudioResponse>> GetPopularAudios()
        {
            var audios = await context.Audios.Include(x => x.Genre).Include(x => x.Artists).Take(20).ToListAsync();

            return mapper.Map<List<Audio>, List<AudioResponse>>(audios);
        }

        [HttpGet("test")]
        public string ForTest()
        {
            return "Success";
        }

        //// use this to seedData 
        //[HttpPost("seedMockData")]
        //public async Task<ActionResult> SeedData()
        //{
        //    var genre = await context.Genres.FirstOrDefaultAsync(x => x.TypeOfGenre == Genres.Pop);

        //    var artist = await context.Artists.FirstOrDefaultAsync(x => x.Name == "Amigo");

        //    if (genre == null)
        //    {
        //        genre = new Genre { Id = Guid.NewGuid(), TypeOfGenre = Genres.Pop };

        //        await context.AddAsync(genre);
        //    }

        //    if (artist == null)
        //    {
        //        artist = new Artist { Id = Guid.NewGuid(), Name = "Amigo" };
        //        await context.AddAsync(artist);
        //    }

        //    context.Audios.RemoveRange(await context.Audios.ToListAsync());

        //    var audios = new List<File>
        //    {
        //        new File()
        //        {
        //            Id = Guid.NewGuid(),
        //            Artists = new List<Artist>{artist},
        //            Genre=  genre,
        //            AudioUrl = "https://mp3store.cc/download.php?file=eyJpZCI6IjM1NTQyNiIsInVybCI6InBsYXlib2ktY2FydGktbGFtZS1uaWdnYXoiLCJ0eXBlIjoibG9hZCIsImJsb2NrIjoiMCJ9",
        //            PosterUrl = "https://mp3store.cc/uploads/cover/artist/250x250/79030867413adea98893ff6fb5dfdd28.jpg",
        //            Name = "Lame Niggaz"
        //        },
        //        new File
        //        {
        //            Name = "Forever",
        //            AudioUrl = "https://cdnstore.xyz/download.php?zc_tid=0&id=76041&url=drake-forever&title=Rm9yZXZlcg==&artist=RHJha2U=&method=load",
        //            PosterUrl = "https://upload.wikimedia.org/wikipedia/en/0/01/Drake-Forever.jpg",
        //            Genre = genre,
        //            Artists = new List<Artist> { artist },
        //            Id = Guid.NewGuid(),
        //        },
        //        new File
        //        {
        //            Name = "positions",
        //            AudioUrl = "https://soundloud.net/index.php?do=download&id=11286",
        //            PosterUrl = "https://upload.wikimedia.org/wikipedia/en/5/5d/Ariana_Grande_-_positions.png",
        //            Genre = genre,
        //            Artists = new List<Artist> { artist},
        //            Id = Guid.NewGuid()
        //        },
        //        new File
        //        {
        //            Name = "dotthatshit!",
        //            AudioUrl = "https://mp3store.cc/download.php?file=eyJpZCI6IjkxMTAyIiwidXJsIjoicGxheWJvaS1jYXJ0aS1kb3RoYXRzaGl0IiwidHlwZSI6ImxvYWQiLCJibG9jayI6IjAifQ==",
        //            PosterUrl = "https://mp3store.cc/uploads/cover/artist/250x250/79030867413adea98893ff6fb5dfdd28.jpg",
        //            Genre = genre,
        //            Artists = new List<Artist> { artist},
        //            Id = Guid.NewGuid(),
        //        },
        //        new File
        //        {
        //            Name = "Slay3r",
        //            AudioUrl = "https://mp3store.cc/download.php?file=eyJpZCI6IjM5MjIyMyIsInVybCI6InBsYXlib2ktY2FydGktc2xheTNyIiwidHlwZSI6ImxvYWQiLCJibG9jayI6IjAifQ==",
        //            PosterUrl = "https://mp3store.cc/uploads/cover/artist/250x250/79030867413adea98893ff6fb5dfdd28.jpg",
        //            Genre = genre,
        //            Artists = new List<Artist> { artist},
        //            Id = Guid.NewGuid(),
        //        },
        //    };
        //    context.AddRange(audios);
        //    await context.SaveChangesAsync();

        //    return Ok(mapper.Map<IEnumerable<File>, IEnumerable<AudioResponse>>(audios));
        //}
    }
}

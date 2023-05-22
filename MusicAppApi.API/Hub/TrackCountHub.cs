using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MusicAppApi.Core;
using MusicAppApi.Models.DbModels;

namespace MusicAppApi.API.Hub
{
    public class TrackCountHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly MusicAppDbContext _context;


        public TrackCountHub(MusicAppDbContext context)
        {
            _context = context;
        }

        public async Task IncrementTrackCount(string audioId)
        {
            var guid = Guid.Parse(audioId);
            var audio = await _context.Audios.FirstOrDefaultAsync(x => x.Id == guid);

            if (audio != null)
            {
                // Audio found, increment the track count
                audio.PlayedCount++;
                await _context.SaveChangesAsync();

                // Notify connected clients about the updated count
                await Clients.All.SendAsync("UpdateTrackCount", audio.PlayedCount, audioId);
            }
            else
            {
                // Audio not found
                // Handle the situation accordingly (e.g., logging, error handling)
            }
        }
    }
}

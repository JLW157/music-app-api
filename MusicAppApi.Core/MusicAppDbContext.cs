using Google;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicAppApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Core
{
    public class MusicAppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public MusicAppDbContext(DbContextOptions<MusicAppDbContext> opts) : base(opts)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Set>()
                .HasIndex(e => new { e.Name, e.UserId })
                .IsUnique();
            
            base.OnModelCreating(builder);
        }

        public  DbSet<Genre> Genres { get; set; }
        public DbSet<Audio> Audios { get; set; }
        public DbSet<Set> Sets { get; set; }
    }
}

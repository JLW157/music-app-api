using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Models.DTO_s.Genres
{
    public class GenreResponse
    {
        public Guid Id { get; set; }
        public string Genre { get; set; } = null!;
    }
}
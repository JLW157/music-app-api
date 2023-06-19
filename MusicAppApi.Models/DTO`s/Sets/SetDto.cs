using MusicAppApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Models.DTO_s.Sets
{
    public class SetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public string PosterUrl { get; set; }
        
        public DateTime CreatedDate { get; set; }

        public string User { get; set; }
        
        public List<AudioResponse> Audios { get; set; }
    }
}

using MusicAppApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Models.DTO_s
{
    public class AudioCreationDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string AudioUrl { get; set; } = null!;
        // Here will be imageFile, but now for testng UrlToImage
        [Required]
        public string PosterUrl { get; set; }
        //[Required]
        // Id for genere
        public string? Genre { get; set; }
        //[Required]
        public List<string>? Artists { get; set; }
    }
}

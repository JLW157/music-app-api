using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Models.DTO_s.Sets;

namespace MusicAppApi.Models.Responses
{
    public class CreateSetResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public SetDto? Set { get; set; }
    }
}

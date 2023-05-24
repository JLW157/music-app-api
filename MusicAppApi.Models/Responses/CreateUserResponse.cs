using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Models.DbModels;

namespace MusicAppApi.Models.Responses
{
    public class CreateUserResponse
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public User? User { get; set; }
    }
}

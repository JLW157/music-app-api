using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Models.DbModels
{
    public class User : IdentityUser<Guid>
    {
        public AuthType AuthType { get; set; }
        public ICollection<Audio> Audios { get; set; }
    }

    public enum AuthType
    {
        Google,
        Normal
    }
}

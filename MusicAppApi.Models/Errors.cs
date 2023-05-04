using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Models
{
    public class ValidationError
    {
        public string Name { get; set; } = null!;
        public IEnumerable<string> Errors { get; set; } = null!;
    }
}

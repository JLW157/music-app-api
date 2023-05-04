using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Core.Interfaces
{
    public interface IJWTGenerator
    {
        string GenerateToken(string userId);
    }
}

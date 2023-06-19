using Microsoft.AspNetCore.Http;
using MusicAppApi.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.API.Extensions
{
    public static class HttpExtensions
    {
        public static string GetUserIdFromClaim(this HttpContext httpContext)
         => httpContext.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.ClaimNames.Id).Value;
    }
}

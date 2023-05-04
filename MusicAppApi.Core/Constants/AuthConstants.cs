using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MusicAppApi.Core.Constants
{
    public class AuthConstants
    {
        public static class UserRoles
        {
            public const string Admin = "Admin";
            public const string User = "User";
            public const string Artist = "Artist";
        }

        public static class ClaimNames
        {
            public const string Id = "Id";
        }

        public static class Policies
        {
            public const string RequireAdmin = "RequiredAdmin";
            public const string RequireArtistAndAdmin = "RequireArtistAndAdmin";
        }
    }
}

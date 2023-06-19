using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Tests.Models.User
{
    public class TestUser : MusicAppApi.Models.DbModels.User
    {
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            MusicAppApi.Models.DbModels.User otherUser = (MusicAppApi.Models.DbModels.User)obj;

            // Compare all relevant properties for equality
            return Id.Equals(otherUser.Id) &&
                   AuthType == otherUser.AuthType &&
                   UserName == otherUser.UserName &&
                   Email == otherUser.Email &&
                   PhoneNumber == otherUser.PhoneNumber &&
                   EmailConfirmed == otherUser.EmailConfirmed &&
                   base.Equals(obj); // Compare inherited properties
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Models.DbModels;

namespace MusicAppApi.Core.interfaces.Services
{
    public interface IRoleService
    {
        Task<bool> CheckRoleExists(string roleName);
        Task<bool> CreateRole(Role role);
    }
}

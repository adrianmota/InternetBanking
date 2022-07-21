using InternetBanking.Core.Application.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IRoleService
    {
        List<RoleViewModel> GetAllRoles();
    }
}

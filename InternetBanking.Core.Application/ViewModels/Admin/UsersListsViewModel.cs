using InternetBanking.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Admin
{
    public class UsersListsViewModel
    {
        public List<UserViewModel> Clients { get; set; }
        public List<UserViewModel> Admins { get; set; }
        public SaveUserViewModel NewClient { get; set; }
        public SaveUserViewModel NewAdmin { get; set; }
    }
}

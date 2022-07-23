using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Admin;
using InternetBanking.Core.Application.ViewModels.Role;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Entities;
using StockApp.Core.Application.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UserService(IAccountService accountService, IRoleService roleService, IMapper mapper)
        {
            _accountService = accountService;
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel login)
        {
            AuthenticationRequest request = _mapper.Map<AuthenticationRequest>(login);
            AuthenticationResponse response = await _accountService.AuthenticateAsync(request);
            return response;
        }

        public async Task<RegisterResponse> Add(SaveUserViewModel saveViewModel)
        {
            RegisterRequest request = _mapper.Map<RegisterRequest>(saveViewModel);
            RegisterResponse response = await _accountService.RegisterAdminUserAsync(request);
            return response;
        }

        public async Task<RegisterResponse> Update(SaveUserViewModel saveViewModel)
        {
            RegisterRequest request = _mapper.Map<RegisterRequest>(saveViewModel);
            RegisterResponse response = await _accountService.UpdateUserAsync(request);
            return response;
        }

        public async Task<List<UserViewModel>> GetAllViewModel()
        {
            List<UserViewModel> viewModelList = await _accountService.GetAllUsers();
            return viewModelList;
        }

        public async Task<UsersListsViewModel> GetAllForAdministrateViewModel()
        {
            UsersListsViewModel usersLists = new();
            usersLists.Clients = new();
            usersLists.Admins = new();
            List<UserViewModel> viewModelList = await _accountService.GetAllUsers();

            foreach(UserViewModel user in viewModelList)
            {
                if(user.Role=="Admin")
                    usersLists.Admins.Add(user);
                else if(user.Role=="Client")
                    usersLists.Clients.Add(user);
            }

            usersLists.NewAdmin = new();
            usersLists.NewClient = new();

            return usersLists;
        }

        public async Task<SaveUserViewModel> GetByIdSaveViewModel(string id)
        {
            var roles = _roleService.GetAllRoles();
            SaveUserViewModel user = await _accountService.GetByIdSaveViewModel(id);

            foreach (var role in roles)
            {
                if (role.Name == user.Role)
                {
                    user.Type = role.Id;
                }
            }

            return user;
        }

        public List<RoleViewModel> GetAllRoles()
        {
            List<RoleViewModel> roles = _roleService.GetAllRoles();
            return roles;
        }

        public async Task SetUserStatus(string id)
        {
            await _accountService.SetUserStatusAsync(id);
        }

        public async Task LogOut()
        {
            await _accountService.SignOutAsync();
        }
    }
}
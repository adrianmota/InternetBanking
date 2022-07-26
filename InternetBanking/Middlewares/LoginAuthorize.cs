using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using WebApp.InternetBanking.Controllers;

namespace WebApp.InternetBanking.Middlewares
{
    public class LoginAuthorize : IAsyncActionFilter
    {
        private readonly ValidateUserSession _userSession;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _user;

        public LoginAuthorize(ValidateUserSession userSession, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _userSession = userSession;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_userSession.HasUser())
            {
                var controller = (UserController)context.Controller;

                if (_user.Roles[0] == "Client")
                {
                    context.Result = controller.RedirectToAction("Index", "Home");
                }

                if (_user.Roles[0] == "Admin")
                {
                    context.Result = controller.RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                await next();
            }
        }
    }
}
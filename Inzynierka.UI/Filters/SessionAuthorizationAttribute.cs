using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inzynierka.UI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SessionAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _userIdParamName;

        public SessionAuthorizationAttribute(string userIdParamName = "userId")
        {
            _userIdParamName = userIdParamName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userIdFromSession = context.HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdFromSession))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var routeData = context.RouteData.Values;
            if (routeData.TryGetValue(_userIdParamName, out var userIdFromRoute))
            {
                if (userIdFromRoute == null || userIdFromRoute.ToString() != userIdFromSession)
                {
                    context.Result = new ForbidResult(); 
                }
            }
        }
    }
}

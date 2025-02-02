using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inzynierka.UI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SessionAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _userIdParamName;

        public SessionAuthorizationAttribute(string userIdParamName = "UserId")
        {
            _userIdParamName = userIdParamName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Session.IsAvailable)
            {
                Console.WriteLine("Session is not available.");
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }

            var userIdFromSession = context.HttpContext.Session.GetString("UserId");
            Console.WriteLine($"Session UserId: {userIdFromSession}");

            if (string.IsNullOrEmpty(userIdFromSession))
            {
                Console.WriteLine("Unauthorized: No UserId in session.");
                context.Result = new UnauthorizedResult();
                return;
            }

            var routeData = context.RouteData.Values;
            if (!routeData.TryGetValue(_userIdParamName, out var userIdFromRoute))
            {
                Console.WriteLine("No UserId in route. Authorization passed.");
                return;
            }

            if (userIdFromRoute == null || !int.TryParse(userIdFromRoute.ToString(), out var routeUserId))
            {
                Console.WriteLine("Invalid UserId in route.");
                context.Result = new BadRequestResult();
                return;
            }

            if (routeUserId.ToString() != userIdFromSession)
            {
                Console.WriteLine("Forbidden: UserId mismatch.");
                context.Result = new ForbidResult();
            }
        }
    }
}

using System.Security.Claims;

namespace RunWebAppTutorial
{
    public static class ClaimsPrincipalExtention
    {
        public static string GetUserById(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}

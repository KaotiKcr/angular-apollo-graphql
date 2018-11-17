using System.Security.Claims;

namespace Server
{
    public class GraphQLUserContext
    {
        public ClaimsPrincipal User { get; set; }
    }
}
namespace GraphQLServer.Types
{
    using GraphQLServer.Repositories;

    public class UserCreatedEvent : UserType
    {
        public UserCreatedEvent(ILinkRepository linkRepository)
            : base(linkRepository)
        {
        }
    }
}

namespace GraphQLServer.Types
{
    using GraphQLServer.Repositories;

    public class VoteCreatedEvent : VoteType
    {
        public VoteCreatedEvent(IUserRepository userRepository, ILinkRepository linkRepository)
            : base(linkRepository, userRepository)
        {
        }
    }
}

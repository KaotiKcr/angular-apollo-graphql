namespace GraphQLServer.Types
{
    using GraphQLServer.Repositories;

    public class LinkCreatedEvent : LinkType
    {
        public LinkCreatedEvent(IUserRepository userRepository, IVoteRepository voteRepository)
            : base(userRepository, voteRepository)
        {
        }
    }
}

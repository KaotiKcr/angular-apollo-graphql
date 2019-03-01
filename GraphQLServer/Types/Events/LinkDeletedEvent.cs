namespace GraphQLServer.Types
{
    using GraphQLServer.Repositories;

    public class LinkDeletedEvent : LinkType
    {
        public LinkDeletedEvent(IUserRepository userRepository, IVoteRepository voteRepository)
            : base(userRepository, voteRepository)
        {
        }
    }
}

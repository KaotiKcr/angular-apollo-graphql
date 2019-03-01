namespace GraphQLServer.Schemas
{
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using GraphQL.Resolvers;
    using GraphQL.Types;
    using GraphQLServer.Models;
    using GraphQLServer.Repositories;
    using GraphQLServer.Types;

    /// <summary>
    /// All subscriptions defined in the schema used to be notified of changes in data.
    /// </summary>
    /// <example>
    /// The is an example subscription to be notified when a human is created.
    /// <c>
    /// subscription whenHumanCreated {
    ///   humanCreated(homePlanets: ["Earth"])
    ///   {
    ///     id
    ///     name
    ///     dateOfBirth
    ///     homePlanet
    ///     appearsIn
    ///   }
    /// }
    /// </c>
    /// </example>
    public class SubscriptionObject : ObjectGraphType<object>
    {
        public SubscriptionObject(
            ILinkRepository linkRepository,
            IUserRepository userRepository,
            IVoteRepository voteRepository)
        {
            this.Name = "Subscription";
            this.Description = "The subscription type, represents all updates can be pushed to the client in real time over web sockets.";

            this.AddField(
                new EventStreamFieldType()
                {
                    Name = "linkCreated",
                    Description = "Subscribe to link created events.",                 
                    Type = typeof(LinkCreatedEvent),
                    Resolver = new FuncFieldResolver<Link>(context => context.Source as Link),
                    Subscriber = new EventStreamResolver<Link>(context => linkRepository.WhenLinkCreated),
                });

            this.AddField(
                new EventStreamFieldType()
                {
                    Name = "linkDeleted",
                    Description = "Subscribe to link deleted events.",
                    Type = typeof(LinkDeletedEvent),
                    Resolver = new FuncFieldResolver<Link>(context => context.Source as Link),
                    Subscriber = new EventStreamResolver<Link>(context => linkRepository.WhenLinkDeleted),
                });

            this.AddField(
                new EventStreamFieldType()
                {
                    Name = "userCreated",
                    Description = "Subscribe to user created events.",
                    Type = typeof(UserCreatedEvent),
                    Resolver = new FuncFieldResolver<User>(context => context.Source as User),
                    Subscriber = new EventStreamResolver<User>(context => userRepository.WhenUserCreated),
                });

            this.AddField(
                new EventStreamFieldType()
                {
                    Name = "voteCreated",
                    Description = "Subscribe to vote created events.",
                    Type = typeof(VoteCreatedEvent),
                    Resolver = new FuncFieldResolver<Vote>(context => context.Source as Vote),
                    Subscriber = new EventStreamResolver<Vote>(context => voteRepository.WhenVoteCreated),
                });
        }
    }    
}

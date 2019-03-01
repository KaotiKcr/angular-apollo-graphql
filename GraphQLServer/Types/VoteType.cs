namespace GraphQLServer.Types
{
    using System.Collections.Generic;
    using GraphQL.Types;
    using GraphQLServer.Models;
    using GraphQLServer.Repositories;

    public class VoteType : ObjectGraphType<Vote>
    {
        public VoteType(ILinkRepository linkRepository, IUserRepository userRepository )
        {
            this.Name = "Vote";
            this.Description = "A vote into my test app.";

            Field(d => d.Id).Description("The id of the vote.");
            
            FieldAsync<UserType, User>(
               "user",
               resolve: context => userRepository.GetUser(context.Source.UserId, context.CancellationToken)
               );

            FieldAsync<LinkType, Link>(
               "link",
               resolve: context => linkRepository.GetLink(context.Source.LinkId, context.CancellationToken)
               );
        }
    }
}

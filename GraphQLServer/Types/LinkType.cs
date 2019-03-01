namespace GraphQLServer.Types
{
    using System.Collections.Generic;
    using GraphQL.Types;
    using GraphQLServer.Models;
    using GraphQLServer.Repositories;

    public class LinkType : ObjectGraphType<Link>
    {
        public LinkType(
            IUserRepository userRepository,
            IVoteRepository voteRepository)
        {
            this.Name = "Link";
            this.Description = "A link into my test app.";
            
            Field(d => d.Id).Description("The id of the link.");
            Field(d => d.CreatedAt).Description("The created date of the link.");
            Field(d => d.UpdatedAt).Description("The updated date of the link.");

            Field(d => d.Description).Description("The description of the link.");
            Field(d => d.Url).Description("The url of the link.");

            FieldAsync<UserType, User>(
                "postedBy",
                resolve: context => userRepository.GetUser(context.Source.UserId, context.CancellationToken)
                );
            FieldAsync<ListGraphType<VoteType>, List<Vote>>(
               "votes",
               resolve: context => voteRepository.GetVotesByLinkId(context.Source.Id, context.CancellationToken)
               );
        }
    }
}

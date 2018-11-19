using GraphQL.Types;
using Links.Models;
using Links.Services;

namespace Links.Schema.Types
{
    public class LinkType : ObjectGraphType<Link>
    {
        public LinkType(IUserService users, IVoteService votes)
        {
            Field(d => d.Id).Description("The id of the link.");
            Field(d => d.CreatedAt).Description("The created date of the link.");
            Field(d => d.UpdatedAt).Description("The updated date of the link.");

            Field(d => d.Description).Description("The description of the link.");
            Field(d => d.Url).Description("The url of the link.");
            Field<UserType>(
                "postedBy",
                resolve: context => users.GetUserByIdAsync(context.Source.UserId)
                );
            Field<ListGraphType<VoteType>>(
               "votes",
               resolve: context => votes.GetVotesByLinkIdAsync(context.Source.Id)
               );
        }
    }
}

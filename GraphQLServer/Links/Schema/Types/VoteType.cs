using GraphQL.Types;
using Links.Models;
using Links.Services;

namespace Links.Schema.Types
{
    public class VoteType : ObjectGraphType<Vote>
    {
        public VoteType(IUserService users, ILinkService links)
        {
            Field(d => d.Id).Description("The id of the vote.");            
            Field<UserType>(
                "user",
                resolve: context => users.GetUserByIdAsync(context.Source.UserId)
                );
            Field<LinkType>(
                "link",
                resolve: context => links.GetLinkByIdAsync(context.Source.LinkId)
                );
        }
    }
}

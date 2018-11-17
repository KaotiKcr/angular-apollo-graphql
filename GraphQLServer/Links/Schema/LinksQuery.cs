using GraphQL.Types;
using Links.Schema.Types;
using Links.Services;

namespace Links.Schema
{
    public class LinksQuery : ObjectGraphType
    {
        public LinksQuery(ILinkService links, IUserService users)
        {
            Name = "Query";
            Field<ListGraphType<LinkType>>(
                "links",
                resolve: context => links.GetLinksAsync() 
                );
            Field<LinkType>(
               "link",
               arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "id of the link" }
                ),
               resolve: context => links.GetLinkById(context.GetArgument<int>("id"))
               );
            Field<ListGraphType<LinkType>>(
               "users",
               resolve: context => users.GetUsersAsync()
               );
            Field<LinkType>(
               "user",
               arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "id of the link" }
                ),
               resolve: context => users.GetUserByIdAsync(context.GetArgument<int>("id"))
               );
        }
    }
}

using GraphQL.Types;
using Links.Models;
using Links.Services;
using System.Collections.Generic;

namespace Links.Schema.Types
{
    public class UserType : ObjectGraphType<User>
    {
        public UserType(ILinkService links)
        {
            Field(d => d.Id).Description("The id of the user.");
            Field(d => d.CreatedAt).Description("The created date of the user.");
            Field(d => d.UpdatedAt).Description("The updated date of the user.");

            Field(d => d.Name).Description("The name of the user.");
            Field(d => d.Email).Description("The email of the user.");

            Field<ListGraphType<LinkType>>(
               "links",
               resolve: context => links.GetLinksByUserIdAsync(context.Source.Id)
               );

        }
    }
}

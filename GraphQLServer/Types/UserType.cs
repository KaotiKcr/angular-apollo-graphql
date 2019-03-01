namespace GraphQLServer.Types
{
    using System.Collections.Generic;
    using GraphQL.Types;
    using GraphQLServer.Models;
    using GraphQLServer.Repositories;

    public class UserType : ObjectGraphType<User>
    {
        public UserType(ILinkRepository linkRepository)
        {
            this.Name = "User";
            this.Description = "A user into my test app.";

            Field(d => d.Id).Description("The id of the user.");
            Field(d => d.CreatedAt).Description("The created date of the user.");
            Field(d => d.UpdatedAt).Description("The updated date of the user.");

            Field(d => d.Name).Description("The name of the user.");
            Field(d => d.Email).Description("The email of the user.");
            Field(d => d.Password).Description("The password of the user.");

            FieldAsync<ListGraphType<LinkType>, List<Link>>(
               "links",
               resolve: context => linkRepository.GetLinksByUserId(context.Source.Id, context.CancellationToken)
               );
        }
    }
}

namespace GraphQLServer.Types
{
    using System.Collections.Generic;
    using GraphQL.Types;
    using GraphQLServer.Models;
    using GraphQLServer.Repositories;

    public class SigninUserPayloadType : ObjectGraphType<SigninUserPayload>
    {
        public SigninUserPayloadType(IUserRepository userRepository)
        {
            this.Name = "SigninUserPayload";
            this.Description = "SigninUserPayload information.";

            Field(d => d.Token).Description("The Token of login.");
            FieldAsync<UserType, User>(
                "user",
                resolve: context => userRepository.GetUser(context.Source.Id, context.CancellationToken)
                );
        }
    }
}

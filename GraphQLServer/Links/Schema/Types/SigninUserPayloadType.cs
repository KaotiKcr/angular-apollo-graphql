using GraphQL.Types;
using Links.Models;
using Links.Services;

namespace Links.Schema.Types
{
    public class SigninUserPayloadType : ObjectGraphType<SigninUserPayload>
    {
        public SigninUserPayloadType(IUserService users)
        {
            Name = "SigninUserPayload";
            Field<UserType>(
                "user",
                resolve: context => users.GetUserByIdAsync(context.Source.Id)
                );

            Field(h => h.Token, nullable: true).Description("The token of the user.");
        }
    }
}

using GraphQL.Types;
using Links.Models;

namespace Links.Schema.Types
{
    public class SignupUserPayloadType : ObjectGraphType<SignupUserPayload>
    {
        public SignupUserPayloadType()
        {
            Name = "SignupUserPayload";

            Field(h => h.Id).Description("The id of the user.");
            Field(h => h.Token, nullable: true).Description("The token of the user.");
        }
    }
}

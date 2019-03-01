namespace GraphQLServer.Types
{
    using GraphQL.Types;
    using GraphQLServer.Models;

    public class SigninUserInputObject : InputObjectGraphType<SigninUser>
    {
        public SigninUserInputObject()
        {
            this.Name = "SigninUserInput";
            this.Description = "A SignUser data for my app.";

            this.Field(x => x.Email)
                .Description("The Email of the user.");
            this.Field(x => x.Password)
                .Description("The Password of the user.");
        }
    }
}

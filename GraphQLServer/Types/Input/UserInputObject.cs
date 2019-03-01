namespace GraphQLServer.Types
{
    using GraphQL.Types;
    using GraphQLServer.Models;

    public class UserInputObject : InputObjectGraphType<User>
    {
        public UserInputObject()
        {
            this.Name = "UserInput";
            this.Description = "A user data for my app.";

            this.Field(x => x.Name)
                .Description("The Name of the user.");
            this.Field(x => x.Email)
                .Description("The Email of the user.");
            this.Field(x => x.Password)
                .Description("The Password of the user.");
        }
    }
}

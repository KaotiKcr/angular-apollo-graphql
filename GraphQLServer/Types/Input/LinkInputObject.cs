namespace GraphQLServer.Types
{
    using GraphQL.Types;
    using GraphQLServer.Models;

    public class LinkInputObject : InputObjectGraphType<Link>
    {
        public LinkInputObject()
        {
            this.Name = "LinkInput";
            this.Description = "A link data for my app.";

            this.Field(x => x.Description)
                .Description("The Description of the link.");
            this.Field(x => x.Url)
                .Description("The Url of the link.");
            this.Field(x => x.UserId)
                .Description("The UserId of the link.");
        }
    }
}

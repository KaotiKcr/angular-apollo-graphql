namespace GraphQLServer.Schemas
{
    using GraphQL.Types;
    using GraphQLServer.Models;
    using GraphQLServer.Repositories;
    using GraphQLServer.Types;

    public class MutationObject : ObjectGraphType<object>
    {
        public MutationObject(
            ILinkRepository linkRepository,
            IUserRepository userRepository,
            IVoteRepository voteRepository)
        {
            this.Name = "Mutation";
            this.Description = "The mutation type, represents all updates we can make to our data.";

            this.FieldAsync<LinkType, Link>(
                "createLink",
                "Create a new link.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<LinkInputObject>>()
                    {
                        Name = "link",
                        Description = "The link you want to create.",
                    }),
                resolve: context =>
                {
                    var link = context.GetArgument<Link>("link");
                    return linkRepository.AddLink(link);
                });

            this.FieldAsync<LinkType, Link>(
                "deleteLink",
                "Delete an existent link.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>>()
                    {
                        Name = "id",
                        Description = "The id of the link to delete.",
                    }),
                resolve: context => linkRepository.DeleteLink(context.GetArgument<int>("id")));

            this.FieldAsync<UserType, User>(
                "createUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputObject>> { Name = "user" }
                ),
                resolve: context =>
                {
                    var user = context.GetArgument<User>("user");
                    return userRepository.AddUser(user);
                });

            this.FieldAsync<SigninUserPayloadType, SigninUserPayload>(
                "signinUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<SigninUserInputObject>> { Name = "signinUser" }
                ),
                resolve: context =>
                {
                    var signinUser = context.GetArgument<SigninUser>("signinUser");
                    return userRepository.SigninUser(signinUser);
                });

            this.FieldAsync<VoteType, Vote>(
                "createVote",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "userId" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "linkId" }
                ),
                resolve: context =>
                {
                    int userId = context.GetArgument<int>("userId");
                    int linkId = context.GetArgument<int>("linkId");
                    return voteRepository.AddVote(userId, linkId, context.CancellationToken);
                });
        }
    }
}

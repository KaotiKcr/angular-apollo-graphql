using GraphQL.Types;
using Links.Models;
using Links.Schema.InputTypes;
using Links.Schema.Types;
using Links.Services;

namespace Links.Schema
{
    public class LinksMutation : ObjectGraphType
    {
        public LinksMutation(ILinkService links, IUserService users)
        {
            Name = "Mutation";

            Field<SignupUserPayloadType>(
                "signupUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "password" }
                ),
                resolve: context => users.SignupUser(context.GetArgument<string>("email"), context.GetArgument<string>("password"))
                );

            Field<LinkType>(
                "createLink",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<LinkInputType>> { Name = "link" }
                ),
                resolve: context => links.CreateLinkAsync(context.GetArgument<Link>("link"))
                );

            Field<LinkType>(
                "deleteLink",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }
                ),
                resolve: context => links.DeleteLinkAsync(context.GetArgument<int>("id"))
                );


            Field<UserType>(
                "createUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context => users.CreateUserAsync(context.GetArgument<User>("user"))
                );
        }
    }
}

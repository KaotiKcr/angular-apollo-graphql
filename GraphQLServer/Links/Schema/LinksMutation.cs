using GraphQL.Types;
using Links.Models;
using Links.Schema.InputTypes;
using Links.Schema.Types;
using Links.Services;

namespace Links.Schema
{
    public class LinksMutation : ObjectGraphType
    {
        public LinksMutation(ILinkService links, IUserService users, IVoteService votes)
        {
            Name = "Mutation";

           
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

            Field<SigninUserPayloadType>(
               "signinUser",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
               ),
               resolve: context => users.SignupUser(context.GetArgument<User>("user"))
               );
            Field<VoteType>(
                "createVote",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "userId" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "linkId" }
                ),
                resolve: context => votes.CreateVoteAsync(context.GetArgument<int>("userId"), context.GetArgument<int>("linkId"))
                );

        }
    }
}

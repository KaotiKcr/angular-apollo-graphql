namespace GraphQLServer.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.AspNetCore;
    using GraphQL.Builders;
    using GraphQL.Types;
    using GraphQL.Types.Relay.DataObjects;
    using GraphQLServer.Models;
    using GraphQLServer.Repositories;
    using GraphQLServer.Types;

  
    public class QueryObject : ObjectGraphType<object>
    {
        private const int MaxPageSize = 25;

        public QueryObject(
            ILinkRepository linkRepository,
            IUserRepository userRepository)
        {
            this.Name = "Query";
            this.Description = "The query type, represents all of the entry points into our object graph.";

            this.Connection<LinkType>()
                .Name("links")
                .Argument<StringGraphType, string>(
                    name: "searchText",
                    description: "The searchText of the search criteria.",
                    defaultValue: default
                    )
                .Description("Gets pages of links.")
                // Enable the last and before arguments to do paging in reverse.
                .Bidirectional()
                // Set the maximum size of a page, use .ReturnAll() to set no maximum size.
                .PageSize(MaxPageSize)
                .ResolveAsync(context => ResolveConnectionLinks(linkRepository, context));

            this.FieldAsync<LinkType, Link>(
                "link",
                "Get a link by its unique identifier.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>>
                    {
                        Name = "id",
                        Description = "The id of the link.",
                    }),
                resolve: context =>
                    linkRepository.GetLink(
                        context.GetArgument<int>("id"),
                        context.CancellationToken));

            this.Connection<UserType>()
                .Name("users")
                .Description("Gets pages of users.")
                // Enable the last and before arguments to do paging in reverse.
                .Bidirectional()
                // Set the maximum size of a page, use .ReturnAll() to set no maximum size.
                .PageSize(MaxPageSize)
                .ResolveAsync(context => ResolveConnectionUsers(userRepository, context));

            this.FieldAsync<UserType, User>(
                "user",
                "Get a user by its unique identifier.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>>
                    {
                        Name = "id",
                        Description = "The id of the user.",
                    }),
                resolve: context =>
                    userRepository.GetUser(
                        context.GetArgument<int>("id"),
                        context.CancellationToken));
        }

        private async static Task<object> ResolveConnectionLinks(
            ILinkRepository linkRepository,
            ResolveConnectionContext<object> context)
        {
            var first = context.First;
            var afterCursor = Cursor.FromCursor<int?>(context.After);
            var last = context.Last;
            var beforeCursor = Cursor.FromCursor<int?>(context.Before);
            var cancellationToken = context.CancellationToken;
            var searchText = context.GetArgument<string>("searchText");

            var getLinksTask = GetLinks(linkRepository, searchText, first, afterCursor, last, beforeCursor, cancellationToken);
            var getHasNextPageTask = GetHasNextPageLinks(linkRepository, searchText, first, afterCursor, cancellationToken);
            var getHasPreviousPageTask = GetHasPreviousPageLinks(linkRepository, searchText, last, beforeCursor, cancellationToken);
            var totalCountTask = linkRepository.GetTotalCount(searchText, cancellationToken);

            await Task.WhenAll(getLinksTask, getHasNextPageTask, getHasPreviousPageTask, totalCountTask);
            var links = getLinksTask.Result;
            var hasNextPage = getHasNextPageTask.Result;
            var hasPreviousPage = getHasPreviousPageTask.Result;
            var totalCount = totalCountTask.Result;
            var (firstCursor, lastCursor) = Cursor.GetFirstAndLastCursor(links, x => x.Id);

            return new Connection<Link>()
            {
                Edges = links
                    .Select(x =>
                        new Edge<Link>()
                        {
                            Cursor = Cursor.ToCursor(x.Id),
                            Node = x
                        })
                    .ToList(),
                PageInfo = new PageInfo()
                {
                    HasNextPage = hasNextPage,
                    HasPreviousPage = hasPreviousPage,
                    StartCursor = firstCursor,
                    EndCursor = lastCursor,
                },
                TotalCount = totalCount,
            };
        }

        private static Task<List<Link>> GetLinks(
            ILinkRepository linkRepository,
            string searchText,
            int? first,
            int? afterCursor,
            int? last,
            int? beforeCursor,
            CancellationToken cancellationToken)
        {
            Task<List<Link>> getLinksTask;
            if (first.HasValue)
            {
                getLinksTask = linkRepository.GetLinks(searchText, first, afterCursor, cancellationToken);
            }
            else
            {
                getLinksTask = linkRepository.GetLinksReverse(searchText, last, beforeCursor, cancellationToken);
            }

            return getLinksTask;
        }

        private static async Task<bool> GetHasNextPageLinks(
            ILinkRepository linkRepository,
            string searchText,
            int? first,
            int? afterCursor,
            CancellationToken cancellationToken)
        {
            if (first.HasValue)
            {
                return await linkRepository.GetHasNextPage(searchText, first, afterCursor, cancellationToken);
            }
            else
            {
                return false;
            }
        }

        private static async Task<bool> GetHasPreviousPageLinks(
            ILinkRepository linkRepository,
            string searchText,
            int? last,
            int? beforeCursor,
            CancellationToken cancellationToken)
        {
            if (last.HasValue)
            {
                return await linkRepository.GetHasPreviousPage(searchText, last, beforeCursor, cancellationToken);
            }
            else
            {
                return false;
            }
        }


        private async static Task<object> ResolveConnectionUsers(
            IUserRepository userRepository,
            ResolveConnectionContext<object> context)
        {
            var first = context.First;
            var afterCursor = Cursor.FromCursor<int?>(context.After);
            var last = context.Last;
            var beforeCursor = Cursor.FromCursor<int?>(context.Before);
            var cancellationToken = context.CancellationToken;

            var getUsersTask = GetUsers(userRepository, first, afterCursor, last, beforeCursor, cancellationToken);
            var getHasNextPageTask = GetHasNextPageUsers(userRepository, first, afterCursor, cancellationToken);
            var getHasPreviousPageTask = GetHasPreviousPageUsers(userRepository, last, beforeCursor, cancellationToken);
            var totalCountTask = userRepository.GetTotalCount(cancellationToken);

            await Task.WhenAll(getUsersTask, getHasNextPageTask, getHasPreviousPageTask, totalCountTask);
            var users = getUsersTask.Result;
            var hasNextPage = getHasNextPageTask.Result;
            var hasPreviousPage = getHasPreviousPageTask.Result;
            var totalCount = totalCountTask.Result;
            var (firstCursor, lastCursor) = Cursor.GetFirstAndLastCursor(users, x => x.Id);

            return new Connection<User>()
            {
                Edges = users
                    .Select(x =>
                        new Edge<User>()
                        {
                            Cursor = Cursor.ToCursor(x.Id),
                            Node = x
                        })
                    .ToList(),
                PageInfo = new PageInfo()
                {
                    HasNextPage = hasNextPage,
                    HasPreviousPage = hasPreviousPage,
                    StartCursor = firstCursor,
                    EndCursor = lastCursor,
                },
                TotalCount = totalCount,
            };
        }

        private static Task<List<User>> GetUsers(
            IUserRepository userRepository,
            int? first,
            int? afterCursor,
            int? last,
            int? beforeCursor,
            CancellationToken cancellationToken)
        {
            Task<List<User>> getUsersTask;
            if (first.HasValue)
            {
                getUsersTask = userRepository.GetUsers(first, afterCursor, cancellationToken);
            }
            else
            {
                getUsersTask = userRepository.GetUsersReverse(last, beforeCursor, cancellationToken);
            }

            return getUsersTask;
        }

        private static async Task<bool> GetHasNextPageUsers(
            IUserRepository userRepository,
            int? first,
            int? afterCursor,
            CancellationToken cancellationToken)
        {
            if (first.HasValue)
            {
                return await userRepository.GetHasNextPage(first, afterCursor, cancellationToken);
            }
            else
            {
                return false;
            }
        }

        private static async Task<bool> GetHasPreviousPageUsers(
            IUserRepository userRepository,
            int? last,
            int? beforeCursor,
            CancellationToken cancellationToken)
        {
            if (last.HasValue)
            {
                return await userRepository.GetHasPreviousPage(last, beforeCursor, cancellationToken);
            }
            else
            {
                return false;
            }
        }
    }
}

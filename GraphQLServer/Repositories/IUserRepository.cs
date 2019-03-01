namespace GraphQLServer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using GraphQLServer.Models;

    public interface IUserRepository
    {
        IObservable<User> WhenUserCreated { get; }

        Task<User> GetUser(
            int id, 
            CancellationToken cancellationToken);

        Task<List<User>> GetUsers(
            int? first,
            DateTime? createdAfter,
            CancellationToken cancellationToken);

        Task<List<User>> GetUsersReverse(
            int? last,
            DateTime? createdBefore,
            CancellationToken cancellationToken);

        Task<bool> GetHasNextPage(
            int? first,
            DateTime? createdAfter,
            CancellationToken cancellationToken);

        Task<bool> GetHasPreviousPage(
            int? last,
            DateTime? createdBefore,
            CancellationToken cancellationToken);

        Task<int> GetTotalCount(
            CancellationToken cancellationToken);

        Task<User> AddUser(
            User user);

        Task<SigninUserPayload> SigninUser(
            SigninUser signinUser);
    }
}

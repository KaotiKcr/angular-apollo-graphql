namespace GraphQLServer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;
    using GraphQLServer.Models;

    public class UserRepository : IUserRepository
    {
        public UserRepository()
        {
            this.whenUserCreated = new Subject<User>();
        }

        private readonly Subject<User> whenUserCreated;
        public IObservable<User> WhenUserCreated => this.whenUserCreated.AsObservable();

        public Task<User> GetUser(
            int id, 
            CancellationToken cancellationToken) =>
            Task.FromResult(Database.Users.FirstOrDefault(x => x.Id == id));

        public Task<User> GetUserByEmail(string email)
        {
            return Task.FromResult(Database.Users.SingleOrDefault(u => Equals(u.Email, email)));
        }

        public Task<List<User>> GetUsers(
            int? first,
            DateTime? createdAfter,
            CancellationToken cancellationToken) =>
            Task.FromResult(Database
                .Users
                .If(createdAfter.HasValue, x => x.Where(y => y.CreatedAt > createdAfter.Value))
                .If(first.HasValue, x => x.Take(first.Value))
                .ToList());

        public Task<List<User>> GetUsersReverse(
            int? last,
            DateTime? createdBefore,
            CancellationToken cancellationToken) =>
            Task.FromResult(Database
                .Users
                .If(createdBefore.HasValue, x => x.Where(y => y.CreatedAt < createdBefore.Value))
                .If(last.HasValue, x => x.TakeLast(last.Value))
                .ToList());

        public Task<bool> GetHasNextPage(
            int? first,
            DateTime? createdAfter,
            CancellationToken cancellationToken) =>
            Task.FromResult(Database
                .Users
                .If(createdAfter.HasValue, x => x.Where(y => y.CreatedAt > createdAfter.Value))
                .Skip(first.Value)
                .Any());

        public Task<bool> GetHasPreviousPage(
            int? last,
            DateTime? createdBefore,
            CancellationToken cancellationToken) =>
            Task.FromResult(Database
                .Users
                .If(createdBefore.HasValue, x => x.Where(y => y.CreatedAt < createdBefore.Value))
                .SkipLast(last.Value)
                .Any());

        public Task<User> AddUser(
            User user)
        {
            user.Id = Database.Users.Max(u => u.Id) + 1;
            Database.Users.Add(user);
            this.whenUserCreated.OnNext(user);
            return Task.FromResult(user);
        }

        public Task<SigninUserPayload> SigninUser(
            SigninUser signinUser)
        {
            User validUser = GetUserByEmail(signinUser.Email).Result;
            if (validUser != null && signinUser.Password == validUser.Password)
            {
                return Task.FromResult(new SigninUserPayload{
                    Id = validUser.Id,
                    Token= "kaotik"
                });
            }
            return Task.FromResult<SigninUserPayload>(null);
        }

        public Task<int> GetTotalCount(CancellationToken cancellationToken) =>
            Task.FromResult(Database
                .Users
                .Count());       
    }
}

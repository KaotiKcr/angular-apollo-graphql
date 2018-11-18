using Links.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Links.Services
{
    public class UserService : IUserService
    {
        private IList<User> _users;
        public UserService()
        {
            _users = new List<User>();
            _users.Add(new User(1, "Ivan Castillo", "ivan.castillo@triquimas.cr", "pass", new int[] { 3 }));
            _users.Add(new User(2, "Mateo Castillo", "mateo.castillo@triquimas.cr", "pass", new int[] { 1, 2 }));

        }

        public Task<User> CreateUserAsync(User user)
        {
            User newUser = new User(_users.Max(u => u.Id) + 1, user.Name, user.Email, user.Password, null);
            _users.Add(newUser);
            return Task.FromResult(newUser);
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            return Task.FromResult(_users.Single(u => Equals(u.Id, id)));
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return Task.FromResult(_users.Single(u => Equals(u.Email, email)));
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            return Task.FromResult(_users.AsEnumerable());
        }

        public Task<SignupUserPayload> SignupUser(string email, string password)
        {
            User user = GetUserByEmailAsync(email).Result;
            if (user.Password == password)
            {
                return Task.FromResult(new SignupUserPayload(user.Id, "kaotik"));
            }
            return null;
        }

    }

    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<SignupUserPayload> SignupUser(string email, string password);
    }
}

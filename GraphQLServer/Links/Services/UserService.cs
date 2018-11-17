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

        public Task<User> GetUserByIdAsync(int id)
        {
            return Task.FromResult(_users.Single(u => Equals(u.Id, id)));
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            return Task.FromResult(_users.AsEnumerable());
        }
    }

    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetUsersAsync();
    }
}

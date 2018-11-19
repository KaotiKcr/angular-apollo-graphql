using Links.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Links.Services
{
    public class LinkService : ILinkService
    {
        private IList<Link> _links;

        public LinkService()
        {
            _links = new List<Link>();
            _links.Add(new Link(1, "Google", "https://www.google.com/", 2, new int[] { 1, 4 }));
            _links.Add(new Link(2, "Facebook", "https://www.facebook.com/", 2, new int[] { 2 }));
            _links.Add(new Link(3, "Triquimas", "https://www.triquimas.cr/", 1, new int[] { 3, 5 }));
        }

        public Task<Link> CreateLinkAsync(Link link)
        {
            Link newLink = new Link((_links.Count > 0)? _links.Max(u => u.Id) + 1 : 1, link.Description, link.Url, link.UserId, null);
            _links.Add(newLink);
            return Task.FromResult(newLink);
        }

        public Task<Link> DeleteLinkAsync(int id)
        {
            Link link = GetLinkById(id);
            _links.Remove(link);
            return Task.FromResult(link);
        }

        public Link GetLinkById(int id)
        {
            return GetLinkByIdAsync(id).Result;
        }

        public Task<Link> GetLinkByIdAsync(int id)
        {
            return Task.FromResult(_links.Single(l => Equals(l.Id, id)));
        }

        public Task<IEnumerable<Link>> GetLinksAsync()
        {
            return Task.FromResult(_links.AsEnumerable());
        }

        public Task<IEnumerable<Link>> GetLinksByUserIdAsync(int userId)
        {
            return Task.FromResult(_links.Where(l => Equals(l.UserId, userId)).AsEnumerable());
        }
    }

    public interface ILinkService
    {
        Task<Link> CreateLinkAsync(Link link);
        Task<Link> DeleteLinkAsync(int id);
        Link GetLinkById(int id);
        Task<Link> GetLinkByIdAsync(int id);
        Task<IEnumerable<Link>> GetLinksAsync();
        Task<IEnumerable<Link>> GetLinksByUserIdAsync(int userId);

    }
}

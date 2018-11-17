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
            _links.Add(new Link(1,"Google", "https://www.google.com/"));
            _links.Add(new Link(2,"Facebook", "https://www.facebook.com/"));
            _links.Add(new Link(3,"Triquimas", "https://www.triquimas.cr/"));
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
    }

    public interface ILinkService
    {
        Link GetLinkById(int id);
        Task<Link> GetLinkByIdAsync(int id);
        Task<IEnumerable<Link>> GetLinksAsync();

    }
}

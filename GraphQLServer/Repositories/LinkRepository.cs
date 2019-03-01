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

    public class LinkRepository : ILinkRepository
    {
        
        public LinkRepository() {
            this.whenLinkCreated = new Subject<Link>();
            this.whenLinkDeleted = new Subject<Link>();
        }

        private readonly Subject<Link> whenLinkCreated;
        private readonly Subject<Link> whenLinkDeleted;

        public IObservable<Link> WhenLinkCreated => this.whenLinkCreated.AsObservable();
        public IObservable<Link> WhenLinkDeleted => this.whenLinkDeleted.AsObservable();

        public Task<Link> GetLink(int id, CancellationToken cancellationToken) =>
            Task.FromResult(Database.Links.FirstOrDefault(x => x.Id == id));

        public Task<List<Link>> GetLinksByUserId(
            int id,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Links.Where(x => x.UserId == id).ToList());
        }

        public Task<List<Link>> GetLinks(
            string filter,
            int? first,
            int? createdAfter,
            CancellationToken cancellationToken) =>
            Task.FromResult(Database
                .Links
                .If(createdAfter.HasValue, x => x.Where(y => y.Id > createdAfter.Value))
                .If(!string.IsNullOrEmpty(filter), x => x.Where(y => y.Description.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0))
                .If(first.HasValue, x => x.Take(first.Value))
                .ToList());

        public Task<List<Link>> GetLinksReverse(
            string filter,
            int? last,
            int? createdBefore,
            CancellationToken cancellationToken) =>
            Task.FromResult(Database
                .Links                
                .If(createdBefore.HasValue, x => x.Where(y => y.Id < createdBefore.Value))
                .If(!string.IsNullOrEmpty(filter), x => x.Where(y => y.Description.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0))
                .If(last.HasValue, x => x.TakeLast(last.Value))
                .ToList());

        public Task<bool> GetHasNextPage(
            string filter,
            int? first,
            int? createdAfter,
            CancellationToken cancellationToken) =>
            Task.FromResult(Database
                .Links
                .If(createdAfter.HasValue, x => x.Where(y => y.Id > createdAfter.Value))
                .If(!string.IsNullOrEmpty(filter), x => x.Where(y => y.Description.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0))
                .Skip(first.Value)
                .Any());

        public Task<bool> GetHasPreviousPage(
            string filter,
            int? last,
            int? createdBefore,
            CancellationToken cancellationToken) =>
            Task.FromResult(Database
                .Links
                .If(createdBefore.HasValue, x => x.Where(y => y.Id < createdBefore.Value))
                .If(!string.IsNullOrEmpty(filter), x => x.Where(y => y.Description.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0))
                .SkipLast(last.Value)
                .Any());

        public Task<int> GetTotalCount(            
            string filter, 
            CancellationToken cancellationToken) =>
            Task.FromResult(Database
                .Links
                .If(!string.IsNullOrEmpty(filter), x => x.Where(y => y.Description.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0))
                .Count());

        public Task<Link> AddLink(
            Link link)
        {
            link.Id = Database.Links.Max(u => u.Id) + 1;
            Database.Links.Add(link);
            this.whenLinkCreated.OnNext(link);
            return Task.FromResult(link);
        }

        public Task<Link> DeleteLink(int id)
        {
            Link link = GetLink(id, new CancellationToken()).Result;
            this.whenLinkDeleted.OnNext(link);
            Database.Links.Remove(link);
            return Task.FromResult(link);
        }
    }
}

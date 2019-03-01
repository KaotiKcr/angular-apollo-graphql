namespace GraphQLServer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using GraphQLServer.Models;

    public interface ILinkRepository
    {
        IObservable<Link> WhenLinkCreated { get; }
        IObservable<Link> WhenLinkDeleted { get; }

        Task<Link> GetLink(
            int id, 
            CancellationToken cancellationToken
            );

        Task<List<Link>> GetLinksByUserId(
           int id,
           CancellationToken cancellationToken);

        Task<List<Link>> GetLinks(
            string filter, 
            int? first,
            int? createdAfter,
            CancellationToken cancellationToken);       

        Task<List<Link>> GetLinksReverse(
            string filter,
            int? last,
            int? createdBefore,
            CancellationToken cancellationToken);

        Task<bool> GetHasNextPage(
            string filter,
            int? first,
            int? createdAfter,
            CancellationToken cancellationToken);

        Task<bool> GetHasPreviousPage(
            string filter,
            int? last,
            int? createdBefore,
            CancellationToken cancellationToken);
        
        Task<int> GetTotalCount(
            string filter,
            CancellationToken cancellationToken);

        Task<Link> AddLink(
            Link link);

        Task<Link> DeleteLink(
            int id);        
    }
}

namespace GraphQLServer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using GraphQLServer.Models;

    public interface IVoteRepository
    {
        IObservable<Vote> WhenVoteCreated { get; }

        Task<Vote> GetVote(int id, CancellationToken cancellationToken);

        Task<List<Vote>> GetVotes(
            int? first,
            int? id,
            CancellationToken cancellationToken);

        Task<List<Vote>> GetVotesByUserId(
            int id,
            CancellationToken cancellationToken);

        Task<List<Vote>> GetVotesByLinkId(
            int id,
            CancellationToken cancellationToken);

        Task<List<Vote>> GetVotesReverse(
            int? last,
            int? id,
            CancellationToken cancellationToken);

        Task<bool> GetHasNextPage(
            int? first,
            int? id,
            CancellationToken cancellationToken);

        Task<bool> GetHasPreviousPage(
            int? last,
            int? id,
            CancellationToken cancellationToken);

        Task<int> GetTotalCount(CancellationToken cancellationToken);

        Task<Vote> AddVote(
            int userId,
            int linkId,
            CancellationToken cancellationToken);
    }
}

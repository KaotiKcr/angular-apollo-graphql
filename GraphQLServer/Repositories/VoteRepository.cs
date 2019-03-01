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

    public class VoteRepository : IVoteRepository
    {
        public VoteRepository()
        {
            this.whenVoteCreated = new Subject<Vote>();
        }

        private readonly Subject<Vote> whenVoteCreated;
        public IObservable<Vote> WhenVoteCreated => this.whenVoteCreated.AsObservable();

        public Task<Vote> GetVote(int id, CancellationToken cancellationToken) =>
            Task.FromResult(Database.Votes.FirstOrDefault(x => x.Id == id));

        public Task<List<Vote>> GetVotesByUserId(int id, CancellationToken cancellationToken) =>
            Task.FromResult(Database.Votes.Where(x => x.UserId == id).ToList());

        public Task<List<Vote>> GetVotesByLinkId(int id, CancellationToken cancellationToken) =>
            Task.FromResult(Database.Votes.Where(x => x.LinkId == id).ToList());


        public Task<List<Vote>> GetVotes(
            int? first,
            int? id,
            CancellationToken cancellationToken) =>
            Task.FromResult(Database
                .Votes
                .If(id.HasValue, x => x.Where(y => y.Id > id.Value))
                .If(first.HasValue, x => x.Take(first.Value))
                .ToList());

        public Task<List<Vote>> GetVotesReverse(
            int? last,
            int? id,
            CancellationToken cancellationToken) =>
            Task.FromResult(Database
                .Votes
                .If(id.HasValue, x => x.Where(y => y.Id < id.Value))
                .If(last.HasValue, x => x.TakeLast(last.Value))
                .ToList());

        public Task<bool> GetHasNextPage(
            int? first,
            int? id,
            CancellationToken cancellationToken) =>
            Task.FromResult(Database
                .Votes
                .If(id.HasValue, x => x.Where(y => y.Id > id.Value))
                .Skip(first.Value)
                .Any());

        public Task<bool> GetHasPreviousPage(
            int? last,
            int? id,
            CancellationToken cancellationToken) =>
            Task.FromResult(Database
                .Votes
                .If(id.HasValue, x => x.Where(y => y.Id < id.Value))
                .SkipLast(last.Value)
                .Any());

        public Task<Vote> AddVote(
            int userId,
            int linkId,
            CancellationToken cancellationToken)
        {
            Vote vote = new Vote() {
                Id = Database.Votes.Max(u => u.Id) + 1,
                UserId = userId,
                LinkId= linkId
            };
            Database.Votes.Add(vote);
            this.whenVoteCreated.OnNext(vote);
            return Task.FromResult(vote);
        }

        public Task<int> GetTotalCount(CancellationToken cancellationToken) =>
            Task.FromResult(Database.Votes.Count);

        
    }
}

using Links.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Links.Services
{
    public class VoteService : IVoteService
    {
        private IList<Vote> _votes;

        public VoteService(IVoteEventService events)
        {
            _votes = new List<Vote>();            
            _votes.Add(new Vote(1, 1, 1));
            _votes.Add(new Vote(2, 1, 3));
            _votes.Add(new Vote(3, 2, 1));
            _votes.Add(new Vote(4, 2, 2));
            _votes.Add(new Vote(5, 2, 3));
            _events = events;
        }

        public IVoteEventService _events { get; }

        public Task<Vote> CreateVoteAsync(int userId, int linkId)
        {
            Vote vote = new Vote((_votes.Count > 0) ? _votes.Max(u => u.Id) + 1 : 1, userId, linkId);
            _votes.Add(vote);
            VoteEvent voteEvent = new VoteEvent(vote.Id, vote.Id, DateTime.Now);
            _events.AddEvent(voteEvent);
            return Task.FromResult(vote);
        }

        public Task<IEnumerable<Vote>> GetVotesByLinkIdAsync(int id)
        {
            return Task.FromResult(_votes.Where(l => Equals(l.LinkId, id)).AsEnumerable());
        }

        public Task<IEnumerable<Vote>> GetVotesByUserIdAsync(int id)
        {
            return Task.FromResult(_votes.Where(l => Equals(l.UserId, id)).AsEnumerable());
        }
    }

    public interface IVoteService
    {
        Task<IEnumerable<Vote>> GetVotesByUserIdAsync(int id);
        Task<IEnumerable<Vote>> GetVotesByLinkIdAsync(int id);
        Task<Vote> CreateVoteAsync(int userId, int linkId);

    }
}

using System;

namespace Links.Models
{
    public class VoteEvent
    {
        public VoteEvent(int id, int voteId, DateTime timestamp)
        {
            Id = id;
            VoteId = voteId;            
            Timestamp = timestamp;
        }

        public int Id { get; set; }
        public int VoteId { get; set; }        
        public DateTime Timestamp { get; private set; }
    }
}

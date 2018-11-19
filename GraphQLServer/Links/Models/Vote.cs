using System;
using System.Collections.Generic;
using System.Text;

namespace Links.Models
{
    public class Vote
    {
        public Vote(int id, int userId, int linkId)
        {
            Id = id;
            UserId = userId;
            LinkId = linkId;
        }

        public int Id { get; private set; }
        public int UserId { get; }
        public int LinkId { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLServer.Models
{
    using System;

    public class Link
    {
        public Link()
        {
            CreatedAt = DateTimeOffset.Now;
            UpdatedAt = DateTimeOffset.Now;
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public int[] Votes { get; set; }

    }
}

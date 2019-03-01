using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLServer.Models
{
    using System;

    public class Vote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LinkId { get; set; }
    }
}

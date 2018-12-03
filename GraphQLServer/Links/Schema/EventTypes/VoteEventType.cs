using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Links.Models;
namespace Links.Schema.EventTypes
{
    public class VoteEventType : ObjectGraphType<VoteEvent>
    {
        public VoteEventType()
        {
            Field(e => e.Id);
            Field(e => e.VoteId);
            Field(e => e.Timestamp);
        }
    }
}

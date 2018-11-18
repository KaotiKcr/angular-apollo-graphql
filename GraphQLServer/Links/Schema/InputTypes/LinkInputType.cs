using GraphQL.Types;
using Links.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Links.Schema.InputTypes
{
    public class LinkInputType : InputObjectGraphType<Link>
    {
        public LinkInputType()
        {
            Name = "LinkInput";            
            Field(x => x.Description);
            Field(x => x.Url);
            Field(x => x.UserId);
        }
    }
}

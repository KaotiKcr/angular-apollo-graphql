using GraphQL.Types;
using Links.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Links.Schema.InputTypes
{
    public class UserInputType : InputObjectGraphType<User>
    {
        public UserInputType()
        {
            Name = "UserInput";            
            Field(x => x.Name, nullable: true);
            Field(x => x.Email);
            Field(x => x.Password);
        }
    }
}

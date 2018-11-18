using System;
using System.Collections.Generic;
using System.Text;

namespace Links.Models
{
    public class SignupUserPayload
    {
        public SignupUserPayload(int id, string token)
        {
            Id = id;
            Token = token;
        }

        public int Id { get; private set; }
        public string Token { get; private set; }
    }
}

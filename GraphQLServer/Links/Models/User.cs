using System;

namespace Links.Models
{
    public class User
    {
        public User(int id, string name, string email, string password, int[] links)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;            
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Links = links;
        }

        public int Id { get; private set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }        
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public int[] Links { get; set; }
    }
}


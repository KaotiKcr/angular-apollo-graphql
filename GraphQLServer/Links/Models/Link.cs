using System;

namespace Links.Models
{
    public class Link
    {
        public Link(int id, string description, string url, int userId)
        {
            Id = id;
            Description = description;
            Url = url;
            UserId = userId;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public int Id { get; private set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int UserId { get; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; private set; }
    }
}


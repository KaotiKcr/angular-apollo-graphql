using System;

namespace Links.Models
{
    public class Link
    {
        public Link(int id, string description, string url)
        {
            Id = id;
            Description = description;
            Url = url;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public int Id { get; private set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; private set; }
    }
}


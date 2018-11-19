using System;

namespace Links.Models
{
    public class Link
    {
        public Link()
        {

        }
        public Link(int id, string description, string url, int userId, int[] votes)
        {
            Id = id;
            Description = description;
            Url = url;
            UserId = userId;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Votes = votes;
        }

        public int Id { get; private set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; private set; }
        public int[] Votes { get; set; }
    }
}


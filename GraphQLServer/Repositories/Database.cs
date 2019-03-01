namespace GraphQLServer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GraphQLServer.Models;

    public static class Database
    {
        static Database()
        {
            Links = new List<Link>() {
                new Link (){
                    Id = 1,
                    Description = "Google",
                    Url = "https://www.google.com/",
                    UserId = 1,
                    Votes = new int[] { 1 }
                },
                new Link (){
                    Id = 2,
                    Description = "Facebook",
                    Url = "https://www.facebook.com/",
                    UserId = 2,
                    Votes = new int[] {  }
                },
                new Link (){
                    Id = 3,
                    Description = "Triquimas",
                    Url = "https://www.triquimas.cr/",
                    UserId = 2,
                    Votes = new int[] { }
                },
                new Link (){
                    Id = 4,
                    Description = "Spotify",
                    Url = "https://www.spotify.com/",
                    UserId = 3,
                    Votes = new int[] { 2 }
                },
            };

            Users = new List<User>() {
                new User (){
                    Id = 1,
                    Name = "Ivan",
                    Email = "ivan.castillo@triquimas.cr",
                    Password = "pass",
                    Votes = new int[] { }
                },
                new User (){
                    Id = 2,
                    Name = "Mateo",
                    Email = "mateo.castillo@triquimas.cr",
                    Password = "pass",
                    Votes = new int[] { 1 }
                },
                new User (){
                    Id = 3,
                    Name = "Sil",
                    Email = "sil.urena@triquimas.cr",
                    Password = "pass",
                    Votes = new int[] { 2 }
                },
            };

            Votes = new List<Vote>() {                
                new Vote (){
                    Id = 1,
                    LinkId = 1,
                    UserId = 2
                },
                new Vote (){
                    Id = 2,
                    LinkId = 4,
                    UserId = 3
                },
            };            
        }

        public static List<Link> Links { get; }
        public static List<User> Users { get; }
        public static List<Vote> Votes { get; }
    }
}

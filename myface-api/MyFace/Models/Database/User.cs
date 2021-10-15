using System.Collections.Generic;

namespace MyFace.Models.Database
{
    public enum RoleType
    {
        MEMBER = 0,
        ADMIN = 1,
    }
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public string hashed_password { get; set;}
        public byte[] salt { get; set;}
        public RoleType Role { get; set;}
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
    }
}

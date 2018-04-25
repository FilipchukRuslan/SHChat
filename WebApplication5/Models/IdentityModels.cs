using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication5.Models
{
     public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<FriendRequest> FriendRequests { get; set; }
        public virtual ICollection<FriendShip> Friendships { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public ApplicationUser()
        {
            FriendRequests = new HashSet<FriendRequest>();
            Friendships = new HashSet<FriendShip>();
            Messages = new HashSet<Message>();
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<FriendRequest> FriendRequests { get; set; }
        public virtual DbSet<FriendShip> Friendships { get; set; }
        public virtual DbSet<Message> Messages { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
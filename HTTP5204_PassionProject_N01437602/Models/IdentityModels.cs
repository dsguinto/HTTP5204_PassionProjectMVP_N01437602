using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HTTP5204_PassionProject_N01437602.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;

        }
    }

    public class StoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public StoreDbContext()
            : base("name=StoreDataContext", throwIfV1Schema: false)
        {
        }

        public static StoreDbContext Create()
        {
            return new StoreDbContext();
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Cart> Cart { get; set; }

    }
}
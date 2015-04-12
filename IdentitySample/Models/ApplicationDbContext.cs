using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentitySample.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(string connectionstring)
            : base(connectionstring, throwIfV1Schema: false)
        {
        }
        //public ApplicationDbContext()
        //    : base("Foo", throwIfV1Schema: false)
        //{
        //}

        //static ApplicationDbContext()
        //{
        //    // Set the database intializer which is run once during application start
        //    // This seeds the database with admin user credentials and admin role
        //    Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        //}

        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}
    }
}
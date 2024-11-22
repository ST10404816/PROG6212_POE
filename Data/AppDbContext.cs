// Lisha Naidoo
// ST10404816
// Group 1

// References
// https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/

using Microsoft.EntityFrameworkCore;

namespace POE.Data
{
    //------------------------------------------------------------------------------------------------------------------------//
    public class AppDbContext : DbContext
    {
        // Constructor that accepts options for configuring the DbContext.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet property representing the "Claims" table in the database.
        public DbSet<Claim> Claims { get; set; }
    }
}
//------------------------------------------...ooo000 END OF FILE 000ooo...------------------------------------------------------//

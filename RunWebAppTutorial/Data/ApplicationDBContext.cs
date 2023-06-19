using Microsoft.EntityFrameworkCore;
using RunWebAppTutorial.Models;

namespace RunWebAppTutorial.Data
{
    public class ApplicationDBContext : DbContext 
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Race> Races { get; set; }
        public DbSet<Club> Clubs{ get; set; }
        public DbSet<Address> Addresses{ get; set; }
    }
}

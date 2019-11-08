using Microsoft.EntityFrameworkCore;

namespace sendITAPI.Models
{
    public class UserParcelContext : DbContext 
    {
        
        public UserParcelContext(DbContextOptions<UserParcelContext> options): base(options) {}
        public DbSet<User>  Users { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
    }
}
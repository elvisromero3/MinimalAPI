using Microsoft.EntityFrameworkCore;
namespace MinimalAPI
{
    public class InfoDb:DbContext 
    {
        public InfoDb(DbContextOptions<InfoDb> options)
            : base(options)
        {
            
        }
        public DbSet<InfoValue> infoValue { get; set; }
    }
}

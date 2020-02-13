using DataContext.Models;
using Microsoft.EntityFrameworkCore;

namespace DataContext.DataContext
{
   public class EFDBContext :DbContext
    {
        public EFDBContext()
        {

        }

        public EFDBContext(DbContextOptions<EFDBContext> options) : base(options)
        {

        }

        public DbSet<Product> Product { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=tcp:atvehicle1dbserver.database.windows.net,1433;Initial Catalog=ATsqlvehicle_db;Persist Security Info=False;User ID=robjsinc;Password=12Mnbcxz;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}

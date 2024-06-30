using Data_Access_Layer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer
{
    public class DataContext : DbContext
    {
         
     public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Cupboard> Cupboard { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<RequestResource> Requests { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<NotificationUser> NotificationUser { get; set; }
        public DbSet<FirebaseConnection> FirebaseConnections { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=SQL8005.site4now.net;Initial Catalog=db_aa9686_easylibro;User Id=db_aa9686_easylibro_admin;Password=kavidil2001");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
             .HasOne(a => a.IssuedBy)
             .WithMany()
             .HasForeignKey(a => a.IssuedByID);

        }



    
}
}

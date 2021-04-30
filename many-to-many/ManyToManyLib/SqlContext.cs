using Microsoft.EntityFrameworkCore;

namespace ManyToManyLib
{
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasMany(p => p.Addresses)
                .WithMany(a => a.Residents)
                .UsingEntity<Resident>(
                    r => r.HasOne<Address>().WithMany(),
                    r => r.HasOne<Person>().WithMany())
                .Property(r => r.LocatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Resident>().ToTable("AddressPerson");

            modelBuilder.ApplyConfiguration(new PersonConfig());
            modelBuilder.ApplyConfiguration(new AddressConfig());
            modelBuilder.ApplyConfiguration(new ResidentConfig());

        }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManyToManyLib
{
    public class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property<DateTime>("Created").IsRequired().HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd();
            builder.Property<DateTime>("Modified").IsRequired().HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAddOrUpdate();
        }
    }

    public class PersonConfig : BaseEntityTypeConfiguration<Person>
    {

    }

    public class AddressConfig : BaseEntityTypeConfiguration<Address>
    {

    }

    public class ResidentConfig : BaseEntityTypeConfiguration<Resident>
    {

    }
}

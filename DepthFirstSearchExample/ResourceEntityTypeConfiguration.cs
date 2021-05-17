using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepthFirstSearchExample
{
    public class ResourceEntityTypeConfiguration : IEntityTypeConfiguration<ResourceEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<ResourceEntity> builder)
        {
            builder.ToTable("Resource");

            builder.HasIndex(e => new { e.ParentResourceId, e.Name }, "AK_Resource_Parent_Name")
                .IsUnique();

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(e => e.Path).IsRequired();

            builder.HasOne(d => d.ParentResource)
                .WithMany(p => p.InverseParentResource)
                .HasForeignKey(d => d.ParentResourceId)
                .HasConstraintName("FK_Resource_Parent");
        }
    }
}

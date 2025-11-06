using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Action = TIVIT.CIPA.Api.Domain.Model.Action;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class ActionConfig : IEntityTypeConfiguration<Action>
    {
        public void Configure(EntityTypeBuilder<Action> builder)
        {
            builder.ToTable("Action");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.ModuleId)
                .IsRequired(false);

            builder.Property(a => a.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(a => a.Name)
                .IsUnique();

            builder.Property(a => a.Code)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.HasOne(a => a.Module)
                .WithMany(m => m.Actions)
                .HasForeignKey(a => a.ModuleId);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class ProfileActionConfig : IEntityTypeConfiguration<ProfileAction>
    {
        public void Configure(EntityTypeBuilder<ProfileAction> builder)
        {
            builder.ToTable("ProfileAction");

            builder.HasKey(pa => new { pa.ProfileId, pa.ActionId });

            builder.HasOne(pa => pa.Profile)
                .WithMany()
                .HasForeignKey(pa => pa.ProfileId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ProfileAction_Profile");

            builder.HasOne(pa => pa.Action)
                .WithMany()
                .HasForeignKey(pa => pa.ActionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ProfileAction_Action");

            builder.Property(pa => pa.IsReadOnly)
                .HasDefaultValue(false);

            builder.Property(pa => pa.IsChecked)
                .HasDefaultValue(true);
        }
    }
}

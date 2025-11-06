using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.CorporateId)
                .HasMaxLength(50);

            builder.Property(u => u.BirthDate)
                .HasColumnType("date");

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.Property(u => u.CreateDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(u => u.CreateUser)
                .HasMaxLength(200);

            builder.Property(u => u.UpdateUser)
                .HasMaxLength(200);

            builder.HasOne(u => u.Profile)
                .WithMany()
                .HasForeignKey(u => u.ProfileId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_User_Profile");
        }
    }
}

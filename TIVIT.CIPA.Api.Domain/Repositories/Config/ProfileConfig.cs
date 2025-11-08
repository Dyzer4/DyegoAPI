using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class ProfileConfig : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("Profile");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.Code)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.HasIndex(p => p.Code)
                   .IsUnique();

            builder.Property(p => p.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(p => p.Description)
                   .HasMaxLength(255)
                   .IsRequired(false);

            builder.Property(p => p.IsActive)
                   .HasDefaultValue(true);

            builder.Property(p => p.CreateDate)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.CreateUser)
                   .HasMaxLength(200)
                   .IsRequired(false);

            builder.Property(p => p.UpdateDate)
                   .HasColumnType("datetime")
                   .IsRequired(false);

            builder.Property(p => p.UpdateUser)
                   .HasMaxLength(200)
                   .IsRequired(false);

            builder.HasMany(p => p.Users)
                   .WithOne(u => u.Profile)
                   .HasForeignKey(u => u.ProfileId)
                   .HasConstraintName("FK_User_Profile");

            builder.HasMany(p => p.Voters)
                   .WithOne(v => v.Profile)
                   .HasForeignKey(v => v.ProfileId)
                   .HasConstraintName("FK_Voter_Profile");

            builder.HasMany(p => p.ProfileActions)
                   .WithOne(pa => pa.Profile)
                   .HasForeignKey(pa => pa.ProfileId)
                   .HasConstraintName("FK_ProfileAction_Profile");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class UserAuthConfig : IEntityTypeConfiguration<UserAuth>
    {
        public void Configure(EntityTypeBuilder<UserAuth> builder)
        {
            builder.ToTable("UserAuth");

            builder.HasKey(ua => ua.Id);

            builder.Property(ua => ua.Id)
                .ValueGeneratedOnAdd();

            builder.Property(ua => ua.Password)
                .HasColumnType("VARBINARY(255)")
                .IsRequired();

            builder.Property(ua => ua.RefreshToken)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(ua => ua.LastLogin)
                .HasColumnType("datetime2(3)")
                .IsRequired(false);

            builder.Property(ua => ua.IsActive)
                .HasDefaultValue(true);

            builder.HasOne(ua => ua.User)
                .WithOne()
                .HasForeignKey<UserAuth>(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserAuth_User");

            builder.Ignore(ua => ua.PasswordRecoverKey);
            builder.Ignore(ua => ua.PasswordRecoverKeyExp);
        }
    }
}

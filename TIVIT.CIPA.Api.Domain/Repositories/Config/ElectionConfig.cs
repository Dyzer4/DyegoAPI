using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;
using Action = TIVIT.CIPA.Api.Domain.Model.Action;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class ElectionConfig : IEntityTypeConfiguration<Election>
    {
        public void Configure(EntityTypeBuilder<Election> builder)
        {
            builder.ToTable("Election");

            // 🔑 Primary Key
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            // 📦 Campos obrigatórios e tipos
            builder.Property(e => e.CompanyId)
                .IsRequired();

            builder.Property(e => e.Code)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(e => e.Code)
                .IsUnique();

            builder.Property(e => e.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.Property(e => e.StartDate)
                .HasColumnType("datetime2(3)")
                .IsRequired();

            builder.Property(e => e.EndDate)
                .HasColumnType("datetime2(3)")
                .IsRequired();

            builder.Property(e => e.RegistrationStartDate)
                .HasColumnType("datetime2(3)")
                .IsRequired(false);

            builder.Property(e => e.RegistrationEndDate)
                .HasColumnType("datetime2(3)")
                .IsRequired(false);

            builder.Property(e => e.ElectionStartDate)
                .HasColumnType("datetime2(3)")
                .IsRequired(false);

            builder.Property(e => e.ElectionEndDate)
                .HasColumnType("datetime2(3)")
                .IsRequired(false);

            builder.Property(e => e.Type)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(e => e.InvitationMessage)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(e => e.IsActive)
                .HasDefaultValue(true);

            builder.Property(e => e.CreateDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.CreateUser)
                .HasMaxLength(200);

            builder.Property(e => e.UpdateUser)
                .HasMaxLength(200);

            builder.HasOne<Company>()
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Election_Company");

            builder.HasMany(e => e.Candidates)
                .WithOne(c => c.Election)
                .HasForeignKey(c => c.ElectionId);
        }
    }
}

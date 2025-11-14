using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class ElectionConfig : IEntityTypeConfiguration<Election>
    {
        public void Configure(EntityTypeBuilder<Election> builder)
        {
            builder.ToTable("Election");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.CompanyId)
                .IsRequired();

            builder.HasOne<Company>()
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Election_Company");

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
                .HasColumnType("datetime2(3)");

            builder.Property(e => e.RegistrationEndDate)
                .HasColumnType("datetime2(3)");

            builder.Property(e => e.ElectionStartDate)
                .HasColumnType("datetime2(3)");

            builder.Property(e => e.ElectionEndDate)
                .HasColumnType("datetime2(3)");

            builder.Property(e => e.Type)
                .HasMaxLength(20);

            builder.Property(e => e.InvitationMessage)
                .HasColumnType("varchar(max)");

            builder.Property(e => e.IsActive)
                .HasDefaultValue(true);

            builder.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.CreateUser)
                .HasMaxLength(200);

            builder.Property(e => e.UpdateDate)
                .HasColumnType("datetime");

            builder.Property(e => e.UpdateUser)
                .HasMaxLength(200);

            builder.HasMany(e => e.ElectionSites)
                .WithOne(es => es.Election)
                .HasForeignKey(es => es.ElectionId);

            builder.HasMany(e => e.Voters)
                .WithOne(v => v.Election)
                .HasForeignKey(v => v.ElectionId);

            builder.HasMany(e => e.Votes)
                .WithOne(v => v.Election)
                .HasForeignKey(v => v.ElectionId);

            builder.HasMany(e => e.VoteLogs)
                .WithOne(vl => vl.Election)
                .HasForeignKey(vl => vl.ElectionId);

            builder.HasMany(e => e.ExclusionLogs)
                .WithOne(el => el.Election)
                .HasForeignKey(el => el.ElectionId);
        }
    }
}
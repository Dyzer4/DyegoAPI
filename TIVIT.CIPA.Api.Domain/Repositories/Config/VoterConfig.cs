using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class VoterConfig : IEntityTypeConfiguration<Voter>
    {
        public void Configure(EntityTypeBuilder<Voter> builder)
        {
            builder.ToTable("Voter");

            builder.HasKey(v => v.Id)
                   .HasName("PK_Voter");

            builder.HasOne(v => v.Election)
                   .WithMany(e => e.Voters)
                   .HasForeignKey(v => v.ElectionId)
                   .HasConstraintName("FK_Voter_Election");


            builder.HasOne<Site>(v => v.Site)
                   .WithMany(e => e.Voters)
                   .HasForeignKey(v => v.SiteId)
                   .HasConstraintName("FK_Voter_Site");

            builder.HasOne<Profile>(v => v.Profile)
                   .WithMany(e => e.Voters)
                   .HasForeignKey(v => v.ProfileId)
                   .HasConstraintName("FK_Voter_Profile");


            builder.Property(v => v.CorporateId)
                   .HasMaxLength(10)
                   .IsUnicode(false);

            builder.Property(v => v.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(v => v.Email)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(v => v.CorporateEmail)
                   .HasMaxLength(150);

            builder.Property(v => v.CorporatePhone)
                   .HasMaxLength(20);

            builder.Property(v => v.ContactPhone)
                   .HasMaxLength(20);

            builder.Property(v => v.Area)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(v => v.Department)
                   .HasMaxLength(100);

            builder.Property(v => v.Token)
                   .HasMaxLength(32)
                   .IsUnicode(false);

            builder.HasIndex(v => v.Token)
                   .IsUnique()
                   .HasFilter("[Token] IS NOT NULL");

            builder.Property(v => v.HasVoted)
                   .HasDefaultValue(false);

            builder.Property(v => v.IsActive)
                   .HasDefaultValue(true);

            builder.Property(v => v.CreateDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(v => v.ExclusionReason)
                   .HasMaxLength(300);

            builder.Property(v => v.ExcludedBy)
                   .HasMaxLength(100);

            builder.Property(v => v.CreateUser)
                   .HasMaxLength(200);

            builder.Property(v => v.UpdateUser)
                   .HasMaxLength(200);

            builder.Property(v => v.ExclusionDate)
                   .HasColumnType("datetime2(3)");

            builder.Property(v => v.BirthDate)
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property<DateTime>("AdmissionDate")
                   .HasColumnType("date")
                   .IsRequired();
        }
    }
}
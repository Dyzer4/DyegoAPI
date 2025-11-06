using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;
using Action = TIVIT.CIPA.Api.Domain.Model.Action;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class VoterConfig : IEntityTypeConfiguration<Voter>
    {
        public void Configure(EntityTypeBuilder<Voter> builder)
        {
            builder.ToTable("Voter");

            builder.HasKey(v => v.Id)
                   .HasName("PK_Voter");

            builder.Property(v => v.Registry)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(v => v.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(v => v.JobTitle)
                   .HasMaxLength(100);

            builder.Property(v => v.Email)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(v => v.CorporateEmail)
                   .HasMaxLength(150);

            builder.Property(v => v.ContactEmail)
                   .HasMaxLength(150);

            builder.Property(v => v.CorportatePhone)
                   .HasMaxLength(20);

            builder.Property(v => v.ContactPhone)
                   .HasMaxLength(20);

            builder.Property(v => v.Site)
                   .HasMaxLength(100);

            builder.Property(v => v.Department)
                   .HasMaxLength(100);

            builder.Property(v => v.Token)
                   .HasMaxLength(32)
                   .IsUnicode(false);

            builder.HasIndex(v => v.Token)
                   .IsUnique()
                   .HasFilter("[Token] IS NOT NULL");

            builder.Property(v => v.ExclusionReason)
                   .HasMaxLength(300);

            builder.Property(v => v.ExcludedBy)
                   .HasMaxLength(100);

            builder.Property(v => v.IsActive)
                   .HasDefaultValue(true);

            builder.Property(v => v.HasVoted)
                   .HasDefaultValue(false);

            builder.Property(v => v.CreateDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(v => v.Election)
                   .WithMany(e => e.Voters)
                   .HasForeignKey(v => v.ElectionId)
                   .HasConstraintName("FK_Voter_Election")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Company>()
                   .WithMany()
                   .HasForeignKey(v => v.CompanyID)
                   .HasConstraintName("FK_Voter_Company")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Profile>()
                   .WithMany()
                   .HasForeignKey(v => v.ProfileId)
                   .HasConstraintName("FK_Voter_Profile")
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(v => v.Candidate)
                   .WithOne(c => c.Voter)
                   .HasForeignKey<Candidate>(c => c.VoterID)
                   .HasConstraintName("FK_Candidate_Voter");
        }
    }
}
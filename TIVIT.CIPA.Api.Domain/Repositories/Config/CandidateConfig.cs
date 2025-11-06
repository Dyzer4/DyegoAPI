using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class CandidateConfig : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.ToTable("Candidate");

            builder.HasKey(c => c.Id)
                   .HasName("PK_Candidate");

            builder.Property(c => c.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(c => c.Area)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(c => c.Department)
                   .HasMaxLength(100);

            builder.Property(c => c.SiteName)
                   .HasColumnName("Site")
                   .HasMaxLength(100);

            builder.Property(c => c.CorporateId)
                   .HasColumnName("CorporateId")
                   .HasMaxLength(10);

            builder.Property(c => c.PhotoBase64)
                   .HasColumnType("VARBINARY(MAX)");

            builder.Property(c => c.PhotoMimeType)
                   .HasMaxLength(50);

            builder.Property(c => c.IsActive)
                   .HasDefaultValue(true);

            builder.Property(c => c.CreateDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(c => c.CreateUser)
                   .HasMaxLength(200);

            builder.Property(c => c.UpdateUser)
                   .HasMaxLength(200);

            builder.HasOne(c => c.Election)
                   .WithMany(e => e.Candidates)
                   .HasForeignKey(c => c.ElectionId)
                   .HasConstraintName("FK_Candidate_Election");

            builder.HasOne(c => c.Voter)
                   .WithOne(v => v.Candidate)
                   .HasForeignKey<Candidate>(c => c.VoterID)
                   .HasConstraintName("FK_Candidate_Voter");

            builder.HasOne(c => c.Site)
                   .WithMany()
                   .HasForeignKey(c => c.SiteId)
                   .HasConstraintName("FK_Candidate_Site");
        }
    }
}
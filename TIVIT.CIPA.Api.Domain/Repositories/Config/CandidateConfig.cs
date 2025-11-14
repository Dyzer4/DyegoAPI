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

            builder.Property(c => c.VoterId)
                   .IsRequired();

            builder.Property(c => c.PhotoBase64)
                   .HasColumnType("VARBINARY(MAX)");

            builder.Property(c => c.PhotoMimeType)
                   .HasMaxLength(50);

            builder.Property(c => c.IsActive)
                   .HasDefaultValue(true);

            builder.Property(c => c.IsResultRelease)
                   .HasDefaultValue(false);

            builder.Property(c => c.CreateDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(c => c.CreateUser)
                   .HasMaxLength(200);

            builder.Property(c => c.UpdateUser)
                   .HasMaxLength(200);

            builder.HasOne(c => c.Voter)
                   .WithMany(v => v.Candidates)
                   .HasForeignKey(c => c.VoterId)
                   .HasConstraintName("FK_Candidate_Voter");
        }
    }
}

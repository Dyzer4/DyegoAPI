using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;
using Action = TIVIT.CIPA.Api.Domain.Model.Action;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class VoteConfig : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.ToTable("Vote");

            builder.HasKey(v => v.Id)
                   .HasName("PK_Vote");

            builder.Property(v => v.EncryptedVote)
                   .HasColumnType("VARBINARY(MAX)")
                   .IsRequired();

            builder.Property(v => v.IsActive)
                   .HasDefaultValue(true);

            builder.Property(v => v.CreateDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(v => v.Election)
                   .WithMany(e => e.Votes)
                   .HasForeignKey(v => v.ElectionId)
                   .HasConstraintName("FK_Vote_Election");


            builder.HasOne(v => v.Candidate)
                   .WithMany()
                   .HasForeignKey(v => v.CandidateId)
                   .HasConstraintName("FK_Vote_Candidate");

        }
    }
}
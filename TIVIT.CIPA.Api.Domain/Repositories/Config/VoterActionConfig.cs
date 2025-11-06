using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;
using Action = TIVIT.CIPA.Api.Domain.Model.Action;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class VoterActionConfig : IEntityTypeConfiguration<VoterAction>
    {
        public void Configure(EntityTypeBuilder<VoterAction> builder)
        {
            builder.ToTable("VoterAction");

            builder.HasKey(va => new { va.VoterId, va.ActionId })
                   .HasName("PK_VoterAction");

            builder.HasOne<Voter>()
                   .WithMany()
                   .HasForeignKey(va => va.VoterId)
                   .HasConstraintName("FK_VoterAction_Voter")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Action>()
                   .WithMany()
                   .HasForeignKey(va => va.ActionId)
                   .HasConstraintName("FK_VoterAction_Action")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
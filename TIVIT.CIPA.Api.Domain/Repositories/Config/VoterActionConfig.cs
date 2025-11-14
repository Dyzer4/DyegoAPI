using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class VoterActionConfig : IEntityTypeConfiguration<VoterAction>
    {
        public void Configure(EntityTypeBuilder<VoterAction> builder)
        {
            builder.ToTable("VoterAction");

            builder.HasKey(va => new { va.VoterId, va.ActionId })
                   .HasName("PK_VoterAction");

        }
    }
}
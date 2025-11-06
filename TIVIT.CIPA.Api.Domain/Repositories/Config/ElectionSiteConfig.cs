using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class ElectionSiteConfig : IEntityTypeConfiguration<ElectionSite>
    {
        public void Configure(EntityTypeBuilder<ElectionSite> builder)
        {
            builder.ToTable("ElectionSite");

            builder.HasKey(es => new { es.ElectionId, es.SiteId })
                   .HasName("PK_ElectionSite");

            builder.HasOne(es => es.Election)
                   .WithMany(e => e.ElectionSites)
                   .HasForeignKey(es => es.ElectionId)
                   .HasConstraintName("FK_ElectionSite_Election")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(es => es.Site)
                   .WithMany(s => s.ElectionSites)
                   .HasForeignKey(es => es.SiteId)
                   .HasConstraintName("FK_ElectionSite_Site")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class SiteConfig : IEntityTypeConfiguration<Site>
    {
        public void Configure(EntityTypeBuilder<Site> builder)
        {
            builder.ToTable("Site");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.CompanyId)
                .IsRequired();

            builder.Property(s => s.ProtheusCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(s => s.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(s => s.IsActive)
                .HasDefaultValue(true);

            builder.HasOne(s => s.Company)
                .WithOne(c => c.Site)
                .HasForeignKey<Site>(s => s.CompanyId);

            builder.HasMany(s => s.Candidates)
                   .WithOne(c => c.Site)
                   .HasForeignKey(c => c.SiteId)
                   .HasConstraintName("FK_Site_Candidate");
        }
    }
}

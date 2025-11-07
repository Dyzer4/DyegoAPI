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
                   .WithMany(c => c.Sites)
                   .HasForeignKey(s => s.CompanyId)
                   .HasConstraintName("FK_Site_Company");

            builder.HasMany(s => s.Candidates)
                   .WithOne(c => c.Site)
                   .HasForeignKey(c => c.SiteId)
                   .HasConstraintName("FK_Site_Candidate");

            builder.HasMany(s => s.Voters)
                   .WithOne(v => v.Site)
                   .HasForeignKey(v => v.SiteId)
                   .HasConstraintName("FK_Site_Voter");

            builder.HasMany(s => s.ElectionSites)
                   .WithOne(es => es.Site)
                   .HasForeignKey(es => es.SiteId)
                   .HasConstraintName("FK_Site_ElectionSite");
        }
    }
}
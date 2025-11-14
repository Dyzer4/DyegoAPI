using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIVIT.CIPA.Api.Domain.Model;

namespace TIVIT.CIPA.Api.Domain.Repositories.Config
{
    internal class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company");

            builder.HasKey(c => c.Id)
                   .HasName("PK_Company");

            builder.Property(c => c.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(c => c.CNPJ)
                   .HasMaxLength(18)
                   .IsUnicode(false)
                   .IsRequired();

            builder.HasIndex(c => c.CNPJ)
                   .IsUnique();

            builder.Property(c => c.LegalName)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(c => c.ProtheusCode)
                   .HasMaxLength(50)
                   .IsUnicode(false);

            builder.Property(c => c.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(c => c.CreatedAt)
                   .HasColumnType("datetime2(3)")
                   .HasDefaultValueSql("SYSDATETIME()")
                   .IsRequired();

            builder.Property(c => c.IsActive)
                   .HasDefaultValue(true);

            builder.HasMany(c => c.Sites)
                   .WithOne(s => s.Company)
                   .HasForeignKey(s => s.CompanyId)
                   .HasConstraintName("FK_Site_Company");
        }
    }
}

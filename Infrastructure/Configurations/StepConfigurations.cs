using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations
{
    public class StepConfigurations : IEntityTypeConfiguration<Step>
    {
        public void Configure(EntityTypeBuilder<Step> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Description)
                .IsRequired()
                .HasMaxLength(800);

            builder.HasOne(i => i.Recipe)
                .WithMany(r => r.Steps)
                .HasForeignKey(i => i.RecipeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(nameof(StoreContext.Steps));
        }
    }
}

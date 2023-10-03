using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inpastructure.Configurations
{
    public class PlansConfigurations : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Recipe)
                .WithMany(r => r.Plans)
                .HasForeignKey(p => p.RecipeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(nameof(StoreContext.Plans));
        }
    }
}

using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations
{
    public class IngredientConfigurations : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Description)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasOne(i => i.Recipe)
                .WithMany(r => r.Ingredients)
                .HasForeignKey(i => i.RecipeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(nameof(StoreContext.Ingredients));
        }
    }
}

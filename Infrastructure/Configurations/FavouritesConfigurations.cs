using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class FavouritesConfigurations : IEntityTypeConfiguration<FavouriteRecipes>
    {
        public void Configure(EntityTypeBuilder<FavouriteRecipes> builder)
        {
            builder.HasKey(fr => new { fr.UserId, fr.RecipeId });

            
            builder.HasOne(fr => fr.User)
                .WithMany()
                .HasForeignKey(fr => fr.UserId)
                .IsRequired();

            builder.HasOne(fr => fr.Recipe)
                .WithMany()
                .HasForeignKey(fr => fr.RecipeId)
                .IsRequired();

            builder.ToTable("FavouriteRecipes");
        }
    }
}

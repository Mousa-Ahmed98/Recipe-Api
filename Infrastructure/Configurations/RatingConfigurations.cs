using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Core.Entities;
using Infrastructure.Data;

namespace Infrastructure.Configurations
{
    public class RatingConfigurations : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.Property(r => r.Id)
                .IsRequired();

            builder.Property(r => r.Content)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(r => r.NumberOfStars)
                .IsRequired();

            builder.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(r => r.Recipe)
                .WithMany(re => re.Ratings)
                .HasForeignKey(r => r.RecipeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(nameof(StoreContext.Ratings));
        }
    }
}

using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class FollowConfigurations : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.HasKey(f => f.Id);

            builder.HasOne(f => f.Follower)
                    .WithMany()
                    .HasForeignKey(f => f.FollowerId).IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Followee)
                    .WithMany()
                    .HasForeignKey(f => f.FolloweeId).IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(nameof(StoreContext.Follows));
        }
    }
}


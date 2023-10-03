using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

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
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(f => f.Followee)
                    .WithMany()
                    .HasForeignKey(f => f.FolloweeId).IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable(nameof(StoreContext.Follows));
        }
    }
}

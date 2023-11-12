using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;
using Infrastructure.Data;

namespace Infrastructure.Configurations
{
    public class RepliesConfigurations : IEntityTypeConfiguration<Reply>
    {
        public void Configure(EntityTypeBuilder<Reply> builder)
        {
            builder.HasKey(f => f.Id);

            builder.HasOne(fr => fr.User)
                .WithMany()
                .HasForeignKey(fr => fr.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(x => x.Comment)
                .WithMany(fr => fr.Replies)
                .HasForeignKey(fr => fr.CommentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(fr => fr.Content)
                .HasMaxLength(500)
                .IsRequired();

            builder.Navigation(e => e.User).AutoInclude();

            builder.ToTable(nameof(StoreContext.Replies));
        }
    }
}

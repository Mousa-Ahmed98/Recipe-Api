using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Infrastructure.Configurations
{
    public class CommentsConfigurations : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(f => f.Id);  

            builder.HasOne(fr => fr.User)
                .WithMany()
                .HasForeignKey(fr => fr.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(fr => fr.Recipe)
                .WithMany(x => x.Comments)
                .HasForeignKey(fr => fr.RecipeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(fr => fr.Content)
                .HasMaxLength(500)
                .IsRequired();
            
            builder.Navigation(e => e.User).AutoInclude();

            builder.ToTable(nameof(StoreContext.Comments));
        }
    }
}

using Core.Entities;
using Infrastructure.Configurations;
using Inpastructure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public class StoreContext : IdentityDbContext<ApplicationUser>
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.Category)
                .WithMany(c => c.Recipes)
                .HasForeignKey(r => r.CategoryId);
            
            modelBuilder.ApplyConfiguration(new FavouritesConfigurations());
            modelBuilder.ApplyConfiguration(new PlansConfigurations());
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<FavouriteRecipes> FavouriteRecipes { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<ShoppingItem> ShoppingList { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }
    }
}

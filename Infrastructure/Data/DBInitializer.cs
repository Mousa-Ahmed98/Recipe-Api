using Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Infrastructure.Data.DBInitializer
{
    public static class DBInitializer
    {
        public static async Task InitDataAsync(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //await SeedDataAsync(serviceScope);
            }
        }

        public static async Task SeedDataAsync(IServiceScope serviceScope)
        {
            var context = serviceScope.ServiceProvider.GetService<StoreContext>();

            if (context == null) { throw new ArgumentNullException(nameof(context)); }

            context.Database.EnsureCreated();


            if (!context.Categories.Any())
            {
                var categories = new[]
                {
                    new Category() { Name = "Main" },
                    new Category() { Name = "Drinks" },
                    new Category() { Name = "Desserts" },
                    new Category() { Name = "Cocktails" },
                    new Category() { Name = "Seafood" },
                    new Category() { Name = "Vegan" },
                    new Category() { Name = "Mexican" },
                    new Category() { Name = "Asian" },
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }


            if (!context.Recipes.Any())
            {
                var mainCategory = context.Categories.FirstOrDefault();

                var recipes = new[]
                {
                    new Recipe(){
                        Name = "recipe1",
                        CategoryId = mainCategory.Id,
                        Image = "https://www.bbcgoodfoodme.com/assets/legacy/recipe/recipe-image/2020/07/cherry-pie.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "ing1" },
                            new Ingredient(){ Description = "ing2" },
                            new Ingredient(){ Description = "ing3" },
                            new Ingredient(){ Description = "ing4" },
                        },
                        Steps = new []{
                            new Step(){ Description = "step1" },
                            new Step(){ Description = "step2" },
                            new Step(){ Description = "step3" },
                            new Step(){ Description = "step4" },
                        },
                        Reviews = new []{
                            new Review(),
                        }
                    },
                    new Recipe(){
                        Name = "recipe2",
                        CategoryId = mainCategory.Id,
                        Image = "https://www.bbcgoodfoodme.com/assets/legacy/recipe/recipe-image/2020/07/cherry-pie.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "ing1" },
                            new Ingredient(){ Description = "ing2" },
                            new Ingredient(){ Description = "ing3" },
                            new Ingredient(){ Description = "ing4" },
                        },
                        Steps = new []{
                            new Step(){ Description = "step1" },
                            new Step(){ Description = "step2" },
                            new Step(){ Description = "step3" },
                            new Step(){ Description = "step4" },
                        },
                        Reviews = new []{
                            new Review(),
                        }
                    },
                    new Recipe(){
                        Name = "recipe3",
                        CategoryId = mainCategory.Id,
                        Image = "https://www.bbcgoodfoodme.com/assets/legacy/recipe/recipe-image/2020/07/cherry-pie.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "ing1" },
                            new Ingredient(){ Description = "ing2" },
                            new Ingredient(){ Description = "ing3" },
                            new Ingredient(){ Description = "ing4" },
                        },
                        Steps = new []{
                            new Step(){ Description = "step1" },
                            new Step(){ Description = "step2" },
                            new Step(){ Description = "step3" },
                            new Step(){ Description = "step4" },
                        },
                        Reviews = new []{
                            new Review(),
                        }
                    },

                    new Recipe(){
                        Name = "recipe4",
                        CategoryId = mainCategory.Id,
                        Image = "https://www.bbcgoodfoodme.com/assets/legacy/recipe/recipe-image/2020/07/cherry-pie.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "ing1" },
                            new Ingredient(){ Description = "ing2" },
                            new Ingredient(){ Description = "ing3" },
                            new Ingredient(){ Description = "ing4" },
                        },
                        Steps = new []{
                            new Step(){ Description = "step1" },
                            new Step(){ Description = "step2" },
                            new Step(){ Description = "step3" },
                            new Step(){ Description = "step4" },
                        },
                        Reviews = new []{
                            new Review(),
                        }
                    },
                    new Recipe(){
                        Name = "recipe5",
                        CategoryId = mainCategory.Id,
                        Image = "https://www.bbcgoodfoodme.com/assets/legacy/recipe/recipe-image/2020/07/cherry-pie.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "ing1" },
                            new Ingredient(){ Description = "ing2" },
                            new Ingredient(){ Description = "ing3" },
                            new Ingredient(){ Description = "ing4" },
                        },
                        Steps = new []{
                            new Step(){ Description = "step1" },
                            new Step(){ Description = "step2" },
                            new Step(){ Description = "step3" },
                            new Step(){ Description = "step4" },
                        },
                        Reviews = new []{
                            new Review(),
                        }
                    },
                    new Recipe(){
                        Name = "recipe6",
                        CategoryId = mainCategory.Id,
                        Image = "https://www.bbcgoodfoodme.com/assets/legacy/recipe/recipe-image/2020/07/cherry-pie.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "ing1" },
                            new Ingredient(){ Description = "ing2" },
                            new Ingredient(){ Description = "ing3" },
                            new Ingredient(){ Description = "ing4" },
                        },
                        Steps = new []{
                            new Step(){ Description = "step1" },
                            new Step(){ Description = "step2" },
                            new Step(){ Description = "step3" },
                            new Step(){ Description = "step4" },
                        },
                        Reviews = new []{
                            new Review(),
                        }
                    },
                    new Recipe(){
                        Name = "recipe7",
                        CategoryId = mainCategory.Id,
                        Image = "https://www.bbcgoodfoodme.com/assets/legacy/recipe/recipe-image/2020/07/cherry-pie.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "ing1" },
                            new Ingredient(){ Description = "ing2" },
                            new Ingredient(){ Description = "ing3" },
                            new Ingredient(){ Description = "ing4" },
                        },
                        Steps = new []{
                            new Step(){ Description = "step1" },
                            new Step(){ Description = "step2" },
                            new Step(){ Description = "step3" },
                            new Step(){ Description = "step4" },
                        },
                        Reviews = new []{
                            new Review(),
                        }
                    },

                };
            
                await context.Recipes.AddRangeAsync(recipes);
            }

            //context.SaveChanges();
        }
    }
}
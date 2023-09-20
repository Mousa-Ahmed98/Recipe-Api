using Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;
using System;
using System.Linq;
using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DBInitializer
{
    public static class DBInitializer
    {
        public static async Task InitDataAsync(this IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                await SeedDataAsync(serviceScope);
            }
        }

        public static async Task SeedDataAsync(IServiceScope serviceScope)
        {
            var context = serviceScope.ServiceProvider.GetService<StoreContext>();

            if (context == null) { throw new ArgumentNullException(nameof(context)); }

            context.Database.EnsureCreated();

            #region SeedCategories
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
            #endregion


            #region SeedRoles
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new IdentityRole
                    {
                        Name = Roles.User,
                        NormalizedName = Roles.User.ToUpper()
                    },
                    new IdentityRole
                    {
                        Name = Roles.Admin,
                        NormalizedName = Roles.Admin.ToUpper()
                    }
                );

                context.SaveChanges();
            }
            #endregion


            #region SeedUsers
            if (!context.Users.Any()) { 

                var user1 =
                    new ApplicationUser
                    {
                        FirstName = "john",
                        LastName = "doe",
                        Email = "johndoe@gmail.com",
                        UserName = "john123",
                    };

                var Admin =
                    new ApplicationUser
                    {
                        FirstName = "admin",
                        LastName = "ln",
                        Email = "admin@gmail.com",
                        UserName = "admin123",

                    };

                var userManager = serviceScope.ServiceProvider
                    .GetService<UserManager<ApplicationUser>>();

                await userManager.CreateAsync(user1, "user@A123");
                await userManager.AddToRoleAsync(user1, Roles.User);

                await userManager.CreateAsync(Admin, "admin@A123");
                await userManager.AddToRoleAsync(Admin, Roles.Admin);
            }
            #endregion


            #region SeedRecipes
            if (!context.Recipes.Any())
            {
                var mainCategory = context.Categories.FirstOrDefault();
                var drinksCategory = context.Categories.FirstOrDefault(x => x.Name == "Drinks");
                
                var user = await context.Users.Where(x => x.UserName == "john123").FirstOrDefaultAsync();
                if(user == null ) { throw new ArgumentNullException(nameof(user)); }
                
                var recipes = new[]
                {
                    new Recipe(){
                        Name = "Frito Pie",
                        CategoryId = mainCategory.Id,
                        Image = "Frito-Pie.jpg",
                        Author = user,
                        Ingredients = new []{
                            new Ingredient(){ Description = "1/2 pound ground beef" },
                            new Ingredient(){ Description = "1/4 cup water" },
                            new Ingredient(){ Description = "1 tablespoon tomato paste" },
                            new Ingredient(){ Description = "1 tablespoon chili powder, or to taste" },
                            new Ingredient(){ Description = "1/2 teaspoon ground cumin" },
                            new Ingredient(){ Description = "1/4 teaspoon onion powder" },
                            new Ingredient(){ Description = "1/4 teaspoon garlic powder" },
                            new Ingredient(){ Description = "1/4 cup chili beans" },
                        },
                        Steps = new []{
                            new Step(){ Description = "Heat a large skillet over medium-high heat. Cook and stir ground beef in the hot skillet until browned and crumbly, 5 to 7 minutes. Drain and discard grease. Stir in water, tomato paste, chili powder, cumin, onion powder, and garlic powder. Stir in beans; cook until heated through, about 3 minutes. " },
                            new Step(){ Description = "Divide corn chips into 4 bowls, top with the chili mix, then sprinkle with diced onions, jalapeño slices, and Cheddar cheese. Serve immediately. " },
                        }
                        
                    },
                    new Recipe(){
                        Name = "Carrot Cake Cupcakes with Cream Cheese Frosting",
                        CategoryId = mainCategory.Id,
                        Author = user,
                        Image = "Carrot-Cake-Cupcakes.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "2 cups all-purpose flour" },
                            new Ingredient(){ Description = "1 tablespoon ground cinnamon" },
                            new Ingredient(){ Description = "2 teaspoons baking soda" },
                            new Ingredient(){ Description = "2 teaspoons baking powder" },
                            new Ingredient(){ Description = "½ teaspoon ground allspice" },
                            new Ingredient(){ Description = "½ teaspoon salt" },
                            new Ingredient(){ Description = "½ teaspoon ground nutmeg" },
                            new Ingredient(){ Description = "⅛ teaspoon ground cloves" },
                            new Ingredient(){ Description = "1 ½ cups dark brown sugar" },
                            new Ingredient(){ Description = "¾ cup sour cream" },
                            new Ingredient(){ Description = "½ cup white sugar" },
                            new Ingredient(){ Description = "½ cup vegetable oil" },
                            new Ingredient(){ Description = "4 large eggs" },
                            new Ingredient(){ Description = "2 teaspoons vanilla extract" },
                            new Ingredient(){ Description = "3 cups freshly grated carrots" },
                            new Ingredient(){ Description = "1 cup chopped pecans" },
                        },
                        Steps = new []{
                            new Step(){ Description = "Preheat the oven to 350 degrees F (175 degrees C). Line 18 standard muffin cups with paper liners. " },
                            new Step(){ Description = "Sift flour, cinnamon, baking soda, baking powder, allspice, salt, nutmeg, and cloves together in a large mixing bowl. " },
                            new Step(){ Description = "Mix brown sugar, sour cream, white sugar, vegetable oil, eggs, and vanilla together until smooth. Add wet ingredients to dry ingredients, and stir until just combined. Stir in carrots, then fold in pecans. Divide batter evenly between the prepared muffin cups. " },
                            new Step(){ Description = "Bake in the preheated oven until a toothpick inserted in the centers comes out clean, 17 to 20 minutes. Cool in the tin for 10 minutes. Transfer to a wire rack and let cool completely, about 20 minutes. " },
                            new Step(){ Description = "While the cupcakes are cooling, beat cream cheese, butter, vanilla, and lemon juice in a mixing bowl with an electric mixer until smooth. Add confectioners' sugar, a bit a time, until smooth and desired sweetness is achieved. Spread onto cooled cupcakes. " },
                        }
                    },
                    new Recipe(){
                        Name = "French Toast Pancakes",
                        CategoryId = mainCategory.Id,
                        Author = user,
                        Image = "french-toast-pancakes.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "2 teaspoons vegetable oil" },
                            new Ingredient(){ Description = "1 cup all-purpose flour" },
                            new Ingredient(){ Description = "1 tablespoon white sugar" },
                            new Ingredient(){ Description = "1 teaspoon baking powder" },
                            new Ingredient(){ Description = "½ teaspoon baking soda" },
                            new Ingredient(){ Description = "¼ teaspoon salt" },
                            new Ingredient(){ Description = "1 pinch ground cinnamon, or to taste" },
                            new Ingredient(){ Description = "1 cup milk" },
                            new Ingredient(){ Description = "1 egg" },
                            new Ingredient(){ Description = "2 tablespoons vegetable oil" },
                        },
                        Steps = new []{
                            new Step(){ Description = "Heat 2 teaspoons oil in a griddle or skillet over medium-high heat. " },
                            new Step(){ Description = "Whisk flour, sugar, baking powder, baking soda, salt, nutmeg, and cinnamon together in a bowl; make a well in the center. Beat milk, egg, 2 tablespoons oil, and vanilla extract together in a separate bowl; pour into the well in the flour mixture and stir until batter is smooth. " },
                            new Step(){ Description = "Drop about 1/4 cup batter per pancake onto the griddle and cook until bubbles form and the edges are dry, 3 to 4 minutes. Flip and cook until browned on the other side, 2 to 3 minutes. Repeat with remaining batter. " },
                        }
                    },

                    new Recipe(){
                        Name = "Cajun Shrimp and Sausage Pasta Bake",
                        CategoryId = mainCategory.Id,
                        Image = "Cajun-Shrimp-and-Sausage-Pasta-Bake.jpg",
                        Author = user,
                        Ingredients = new []{
                            new Ingredient(){ Description = "10 ounces penne pasta" },
                            new Ingredient(){ Description = "1 tablespoon butter" },
                            new Ingredient(){ Description = "1/2 onion, chopped" },
                            new Ingredient(){ Description = "1/2 red bell pepper, chopped" },
                            new Ingredient(){ Description = "8 ounces shrimp, peeled and deveined" },
                            new Ingredient(){ Description = "8 ounces andouille sausage, quartered" },
                            new Ingredient(){ Description = "1 tablespoon flour" },
                            new Ingredient(){ Description = "2 cloves garlic, minced" },
                        },
                        Steps = new []{
                            new Step(){ Description = "Preheat the oven to 375 degrees F (190 degrees C). Grease a 9x13-inch casserole dish. Bring a pot of lightly salted water to a boil.  " },
                            new Step(){ Description = "Meanwhile, melt 1 tablespoon butter in a heavy skillet. Add onion and bell pepper, and cook until softened, 3 to 5 minutes. Stir in sausage and shrimp. Turn off heat; the residual heat will cook the shrimp and warm the sausage. Remove skillet from heat after a few minutes. " },
                            new Step(){ Description = "Stir penne into boiling water and return to a boil. Cook pasta uncovered, stirring occasionally, for 6 minutes. " },
                            new Step(){ Description = "Using a slotted spoon, transfer about half of pasta to the skillet. Transfer remaining pasta to prepared casserole. Reserve 1/2 cup pasta water. " },
                            new Step(){ Description = "In a saucepan, melt 1 tablespoon butter over medium heat and whisk in flour. Add garlic and Cajun seasoning. Whisking constantly, slowly pour in milk, half and half, and reserved pasta water until sauce is smooth and bubbly, 2 to 3 minutes. Stir in Parmesan cheese and allow it to melt, 1 to 2 minutes more. " },
                            new Step(){ Description = "Pour half of sauce over pasta in the casserole. Transfer sausage mixture to the casserole, and use a spoon to gently blend all ingredients. Pour remaining sauce over pasta mixture. Cover casserole with foil. " },
                            new Step(){ Description = "Bake in the preheated oven until heated through and bubbly, about 25 minutes. Serve immediately. " },
                        }
                    },
                    new Recipe(){
                        Name = "Steak and Potato Foil Packets",
                        CategoryId = mainCategory.Id,
                        Author = user,
                        Image = "Steak-and-Potatoes-Foil-Packs.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "1 pound small Yukon Gold potatoes, halved" },
                            new Ingredient(){ Description = "1 1/2 pounds sirloin steak, cut into bite-sized strips" },
                            new Ingredient(){ Description = "1 yellow onion, sliced" },
                            new Ingredient(){ Description = "1 green bell pepper, sliced" },
                            new Ingredient(){ Description = "1 red bell pepper, sliced" },
                            new Ingredient(){ Description = "1 carrot, thinly sliced" },
                            new Ingredient(){ Description = "4 tablespoons butter, or as needed" },
                            new Ingredient(){ Description = "1 teaspoon steak seasoning, or as needed" },
                            new Ingredient(){ Description = "5 cloves garlic, chopped, or more to taste" },
                        },
                        Steps = new []{
                            new Step(){ Description = "Prepare an outdoor grill, preferably a charcoal grill, for medium-high heat. " },
                            new Step(){ Description = "Place potatoes in a microwave-safe dish and cook on high until they are tender with a bite, about 3 minutes. " },
                            new Step(){ Description = "Divide steak, potatoes, onions, red and green bell pepper, and carrots evenly onto 4 to 6 squares of aluminum foil. Add 1 tablespoon butter and a little fresh garlic to each pack; fold foil over the top to close each packet tightly. " },
                            new Step(){ Description = "Place foil packs on the hot side of the grill. Cook for 5 minutes. Carefully open one foil pack to check for steak doneness. " },
                            new Step(){ Description = "If steak is not cooked to your taste after 5 minutes, continue to cook, checking for doneness every few minutes. An instant-read thermometer inserted into the center will read 130 degrees F (54 degrees C) for medium rare. " },
                        }
                    },
                    new Recipe(){
                        Name = "Shrimp and Pepper Stir-Fry",
                        CategoryId = mainCategory.Id,
                        Author = user,
                        Image = "Shrimp-and-Pepper-Stir-Fry.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "1/2 cup chicken broth" },
                            new Ingredient(){ Description = "1/4 cup low-sodium soy sauce" },
                            new Ingredient(){ Description = "2 tablespoons rice wine vinegar" },
                            new Ingredient(){ Description = "1 tablespoon brown sugar" },
                            new Ingredient(){ Description = "1 tablespoon cornstarch" },
                            new Ingredient(){ Description = "1/2 teaspoon garlic powder" },
                            new Ingredient(){ Description = "1/4 teaspoon crushed red pepper" },
                        },
                        Steps = new []{
                            new Step(){ Description = "Whisk together chicken broth, soy sauce, rice wine vinegar, brown sugar, cornstarch, garlic powder, and crushed red pepper in a small bowl; set aside. " },
                            new Step(){ Description = "Heat 1 tablespoon oil in a large wok or skillet over high heat. Add red pepper, yellow pepper, red onion, snow peas, and green onion, and saute until vegetables are crisp-tender, 3 to 4 minutes. Remove vegetables to a large bowl. " },
                            new Step(){ Description = "Lower heat to medium-high and add remaining 1 tablespoon oil. Cook shrimp for 2 minutes, flipping over halfway through. Return vegetables to the wok. Stir sauce and pour over shrimp and vegetables; stir constantly until sauce comes to a simmer and thickens, about 1 to 2 minutes. " },
                        }
                    },
                    new Recipe(){
                        Name = "Bibimbap (Korean Rice With Mixed Vegetables)",
                        CategoryId = mainCategory.Id,
                        Author = user,
                        Image = "KoreanRiceWithMixedVegtables.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "1 English cucumber, cut into matchsticks" },
                            new Ingredient(){ Description = "¼ cup gochujang (Korean hot pepper paste) (Optional)" },
                            new Ingredient(){ Description = "1 bunch fresh spinach, cut into thin strips" },
                            new Ingredient(){ Description = "1 tablespoon soy sauce" },
                            new Ingredient(){ Description = "2 teaspoons olive oil, divided" },
                            new Ingredient(){ Description = "2 carrots, cut into matchsticks " },
                            new Ingredient(){ Description = "1 clove garlic, minced" },
                            new Ingredient(){ Description = "1 pinch red pepper flakes" },
                            new Ingredient(){ Description = "1 pound thinly-sliced beef top round steak" },
                            new Ingredient(){ Description = "4 large eggs" },
                            new Ingredient(){ Description = "4 cups cooked white rice" },
                            new Ingredient(){ Description = "4 teaspoons toasted sesame oil, divided" },
                            new Ingredient(){ Description = "1 teaspoon sesame seeds" },
                            new Ingredient(){ Description = "2 teaspoons gochujang (Korean hot pepper paste), divided (Optional)" },
                        },
                        Steps = new []{
                            new Step(){ Description = "Stir together cucumber pieces and gochujang paste in a bowl; set aside. " },
                            new Step(){ Description = "Stir together cucumber pieces and gochujang paste in a bowl; set aside. " },
                            new Step(){ Description = "Drain spinach and squeeze out as much moisture as possible; set spinach aside in a bowl and stir in soy sauce. " },
                            new Step(){ Description = "Heat 1 teaspoon olive oil in a large nonstick skillet; cook and stir carrots until softened, about 3 minutes. " },
                            new Step(){ Description = "Stir in garlic and cook just until fragrant, about 1 minute. Stir in cucumber mixture; sprinkle with red pepper flakes. Set carrot mixture aside in a bowl. " },
                            new Step(){ Description = "Brown beef in a clean nonstick skillet over medium heat, about 5 minutes per side; set aside. " },
                            new Step(){ Description = "Heat remaining 1 teaspoon olive oil in another nonstick skillet over medium-low heat. Fry eggs just on one side until yolks are runny, but whites are firm, 2 to 4 minutes. " },
                            new Step(){ Description = "Divide cooked rice into 4 large serving bowls; top with spinach mixture, a few pieces of beef, and cucumber mixture. Place 1 egg atop each serving. Drizzle each bowl with 1 teaspoon sesame oil, a sprinkle of sesame seeds, and a small amount of gochujang paste if desired. " },
                        }
                    },
                    new Recipe(){
                        Name = "Fried Chicken",
                        CategoryId = mainCategory.Id,
                        Author = user,
                        Image = "fired-chicken.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "30 saltine crackers" },
                            new Ingredient(){ Description = "2 tablespoons all-purpose flour" },
                            new Ingredient(){ Description = "2 tablespoons dry potato flakes" },
                            new Ingredient(){ Description = "1 teaspoon seasoned salt" },
                            new Ingredient(){ Description = "½ teaspoon ground black pepper" },
                            new Ingredient(){ Description = "1 egg" },
                            new Ingredient(){ Description = "6 skinless, boneless chicken breast halves" },
                            new Ingredient(){ Description = "2 cups vegetable oil for frying" },
                        },
                        Steps = new []{
                            new Step(){ Description = "Place crackers in a large resealable plastic bag; seal bag and crush crackers with a rolling pin until they are coarse crumbs. Add the flour, potato flakes, seasoned salt, and pepper and mix well. " },
                            new Step(){ Description = "Beat egg in a shallow dish or bowl. One by one, dredge chicken pieces in egg, then place in bag with crumb mixture. Seal bag and shake to coat. " },
                            new Step(){ Description = "Heat oil in a deep-fryer or large saucepan to 350 degrees F (175 degrees C). " },
                            new Step(){ Description = "Fry chicken, turning frequently, until golden brown and juices run clear, 15 to 20 minutes. " },
                        }
                    },
                    new Recipe(){
                        Name = "Fast Chicken Soup Base",
                        CategoryId = mainCategory.Id,
                        Author = user,
                        Image = "Fast-Chicken-Soup-Base.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "2 quarts chicken broth" },
                            new Ingredient(){ Description = "1 quart water" },
                            new Ingredient(){ Description = "1 store-bought roast chicken" },
                            new Ingredient(){ Description = "3 tablespoons vegetable oil" },
                            new Ingredient(){ Description = "2 large onions, cut into medium dice" },
                            new Ingredient(){ Description = "2 large carrots, peeled and cut into rounds or half rounds, depending on size" },
                            new Ingredient(){ Description = "2 large stalks celery, sliced 1/4 inch thick" },
                            new Ingredient(){ Description = "1 teaspoon dried thyme leaves" },
                        },
                        Steps = new []{
                            new Step(){ Description = "Bring broth and water to a simmer over medium-high heat in a large soup kettle. Meanwhile, separate chicken meat from skin and bones; reserve meat. Add skin and bones to the simmering broth. Reduce heat to low, partially cover and simmer until bones release their flavor, 20 to 30 minutes. " },
                            new Step(){ Description = "Strain broth through a colander into a large container; reserve broth and discard skin and bones. Return kettle to burner set on medium-high. " },
                            new Step(){ Description = "Add oil, then onions, carrots and celery. Saute until soft, about 8 to 10 minutes. Add chicken, broth and thyme. Bring to a simmer. (Can be refrigerated up to 3 days in advance. Return to a simmer before adding the extras of your choice.) " },
                        }
                    },
                    new Recipe(){
                        Name = "Baked Beer Can Chicken",
                        CategoryId = mainCategory.Id,
                        Author = user,
                        Image = "Baked-Can-Chicken.png",
                        Ingredients = new []{
                            new Ingredient(){ Description = "¼ cup garlic powder" },
                            new Ingredient(){ Description = "2 tablespoons seasoned salt" },
                            new Ingredient(){ Description = "2 tablespoons onion powder" },
                            new Ingredient(){ Description = "1 tablespoon salt" },
                            new Ingredient(){ Description = "1 ½ teaspoons ground black pepper" },
                            new Ingredient(){ Description = "1 (12 fluid ounce) can light-flavored beer (such as Bud Light®)" },
                            new Ingredient(){ Description = "1 (3 pound) whole chicken" },
                            new Ingredient(){ Description = "4 green onions, sliced" },
                            new Ingredient(){ Description = "4 green onions, cut in half crosswise" },
                        },
                        Steps = new []{
                            new Step(){ Description = "Preheat the oven to 350 degrees F (175 degrees C). " },
                            new Step(){ Description = "Mix garlic powder, seasoned salt, onion powder, dried oregano, salt, and pepper in a small bowl; set aside. " },
                            new Step(){ Description = "Pour 1/3 of one can of beer into the bottom of a 9x13-inch baking dish. Place the open beer can in the center of the baking dish. " },
                            new Step(){ Description = "Rinse chicken under cold running water. Discard giblets and neck from chicken; drain and pat dry. Fit whole chicken over the open beer can with the legs on the bottom. With the chicken breasts facing you, use a paring knife to cut a small slit on each side. Press the tip of each wing into the slit to encourage even cooking. " },
                            new Step(){ Description = "Rub the reserved seasoning mixture to taste over entire chicken. Pat green onion slices evenly onto the chicken; it's okay if some fall off. Press green onion halves into the top cavity of the chicken. Open the second beer can and pour 1/2 of it into the pan; set the remaining beer aside to use during baking. " },
                            new Step(){ Description = "Bake chicken in the preheated oven for 45 minutes. Pour the remaining beer into the pan under the chicken and continue baking until no longer pink at the bone and the juices run clear, about 30 additional minutes. An instant-read thermometer inserted into the thickest part of the thigh, near the bone should read 180 degrees F (82 degrees C). " },
                            new Step(){ Description = "Bake chicken in the preheated oven for 45 minutes. Pour the remaining beer into the pan under the chicken and continue baking until no longer pink at the bone and the juices run clear, about 30 additional minutes. An instant-read thermometer inserted into the thickest part of the thigh, near the bone should read 180 degrees F (82 degrees C). " },
                            new Step(){ Description = "Remove from the oven and discard the beer can. Cover chicken with a doubled sheet of aluminum foil; let rest in a warm area for 10 minutes before slicing. " },
                        }
                    },
                    new Recipe(){
                        Name = "Buttermilk Pancakes",
                        CategoryId = mainCategory.Id,
                        Author = user,
                        Image = "ButtermilkPancakes.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "3 cups all-purpose flour" },
                            new Ingredient(){ Description = "3 tablespoons white sugar" },
                            new Ingredient(){ Description = "3 teaspoons baking powder" },
                            new Ingredient(){ Description = "1 ½ teaspoons baking soda" },
                            new Ingredient(){ Description = "¾ teaspoon salt" },
                            new Ingredient(){ Description = "3 cups buttermilk" },
                            new Ingredient(){ Description = "½ cup milk" },
                            new Ingredient(){ Description = "3 eggs" },
                            new Ingredient(){ Description = "⅓ cup butter, melted" },
                        },
                        Steps = new []{
                            new Step(){ Description = "Combine flour, sugar, baking powder, baking soda, and salt in a large bowl. Beat together buttermilk, milk, eggs, and melted butter in a separate bowl. Keep the two mixtures separate until you are ready to cook. " },
                            new Step(){ Description = "Heat a lightly oiled griddle or frying pan over medium-high heat. You can flick water across the surface and if it beads up and sizzles, it's ready. " },
                            new Step(){ Description = "Pour the wet mixture into the dry mixture; use a wooden spoon or fork to mix until it's just blended together. The batter will be a little lumpy which is what you want. " },
                            new Step(){ Description = "Pour or scoop the batter onto the preheated griddle, using approximately 1/2 cup for each pancake. Cook until bubbles appear on the surface, 1 to 2 minutes; flip with a spatula and cook until browned on the other side. Repeat with remaining batter. " },
                            new Step(){ Description = "Serve hot and enjoy! " },
                        }
                    },
                    new Recipe(){
                        Name = "Chocolate Banana Milkshake",
                        CategoryId = drinksCategory.Id,
                        Author = user,
                        Image = "Chocolate-Banana-Milkshake.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "1 banana, frozen and chunked" },
                            new Ingredient(){ Description = "6 tablespoons powdered chocolate-flavored malt drink mix (such as Ovaltine®)" },
                            new Ingredient(){ Description = "1 cup milk" },
                            new Ingredient(){ Description = "2 cups vanilla ice cream" },
                        },
                        Steps = new []{
                            new Step(){ Description = " Place the frozen banana chunks, powdered drink mix, milk, and vanilla ice cream into a blender, and blend until smooth and creamy. Pour into large glasses. " },
                        }
                    },

                    new Recipe(){
                        Name = "Hot Chocolate",
                        CategoryId = drinksCategory.Id,
                        Author = user,
                        Image = "Hot-Chocolate.jpg",
                        Ingredients = new []{
                            new Ingredient(){ Description = "1 cup whole milk" },
                            new Ingredient(){ Description = "1 ½ teaspoons brown sugar" },
                            new Ingredient(){ Description = "2 ounces dark chocolate, finely chopped" },
                            new Ingredient(){ Description = "1 tablespoon heavy whipping cream" },
                            new Ingredient(){ Description = "1 pinch ground cinnamon" },
                        },
                        Steps = new []{
                            new Step(){ Description = "Heat milk in a saucepan over medium heat for 3 to 4 minutes." },
                            new Step(){ Description = "Add brown sugar and stir until dissolved." },
                            new Step(){ Description = "Stir dark chocolate until it's completely melted. About 2 to 3 minutes." },
                            new Step(){ Description = "Remove the saucepan from the heat and stir in the cream and cinnamon." },
                            new Step(){ Description = "Top with whipped cream, marshmallows, crushed peppermint, or any other topping of your choice." },
                        }
                    },


                };
                await context.Recipes.AddRangeAsync(recipes);
                await context.SaveChangesAsync();
            }
            #endregion
        }
    }
}
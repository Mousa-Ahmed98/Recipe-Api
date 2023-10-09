# 🍰 Recipe Realm

Welcome to RecipeRealm, a food blog project developed by [Mohammed Ebrahim](https://github.com/mohammed0xff) and [Mousa Ahmed](https://github.com/Mousa-Ahmed98) as part of a summer internship at [LuminSoft](https://www.linkedin.com/company/lumin-soft/). Under the guidance of our coach, Senior Engineer Ahmed Jafar, we were able to bring this project to life and create a platform where food enthusiasts can discover and share delicious recipes. <br>

[See live preview 🍕](https://reciperealmui-d092e.web.app)


#### ⬅ [Go to frontend repo ](https://github.com/Mousa-Ahmed98/Recipe-UI#-recipe-realm) 


### Table of contents
- [Overview](#-overview)
- [Demo Video](#-demo-video)
- [Features](#-features)
- [Used Technologies](#-used-technologies)
- [How to run](#-how-to-run)
- [License](#-license)



## 🍉 Overview 
RecipeRealm is a web application that allows users to explore a wide variety of recipes, share their own recipes, and engage in discussions with other food enthusiasts. The project was developed as part of a summer internship, and it served as a learning experience.

Throughout the development process, we gained practical experience in various aspects of software development, including requirement analysis, system design, implementation, and project management. We had the opportunity to apply our programming skills and learn new technologies to build a functional and user-friendly food blog platform.



## 🍿 Demo Video
Pretty soon 



## 🍖 Features

#### 🍽️️ Recipe Management
- **Browse recipes:** Users can browse through a collection of recipes.
- **View a Specific Recipe:** Users can view detailed information about a specific recipe.
- **Add Recipe:** Users can add their own recipes to the platform.
- **Edit Recipe:** Users can edit the recipes they have added.
- **Delete Recipe:** Users can delete the recipes they have added.
- **Rate and Review Other Users' Recipes:** Users can rate and leave reviews on recipes shared by other users.

#### 📆 Organizer & Planner
- **Save recipes to favorite:** Users can save their favorite recipes for easy access.
- **Plan meals for the week:** Users can plan their meals for the week.
- **View meal plans on a calendar:** Users can view their meal plans on a calendar.
- **Adjust and rearrange meal plans:** Users can make adjustments and rearrange their meal plans.

#### 🛒 Shopping List
- **Generate a shopping list from the meal plans:** Users can generate a shopping list based on their meal plans.
- **Automatically & Manually add items to the shopping list:** Users can add items to the shopping list either automatically or manually.
- **Mark items as purchased on the shopping list:** Users can mark items as purchased on the shopping list.

#### 💬 Social Features
- **Follow other users:** Users can follow other users on the platform.
- **View recipes by user:** Users can view recipes shared by a specific user.
- **Comment on recipes:** Users can leave comments on recipes.
- **Share recipes on social media:** Users can share their recipes on social media platforms.

#### 🔔 Notifications
- **Receive notifications :** Users receive notifications when someone rates or comments on their recipes.
- **Get reminders for meal plans:** Users receive reminders for their meal plans and new recipe posts for followed users.

#### 📊 User Dashboard
- **My recipes:** Users can view the recipes they have added.
- **Meal plans:** Users can view their meal plans.
- **Shopping lists:** Users can view their shopping lists.
- **Summary of activities:** Users can view a summary of their activities on the platform.



## 🍜 Used Technologies

### Backend:
- [ASP.NET Core 7](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-7.0?view=aspnetcore-7.0)
- Clean Architecture 
- [AutoMapper](https://automapper.org/) for object-object mapping
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) for database access
- [SQL Server](https://www.microsoft.com/en-us/sql-server) for database management
- [Swagger](https://swagger.io/) for API documentation
- [Quartz.NET](https://www.quartz-scheduler.net/) for background job processing
- [Identity Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-7.0) for user authentication and authorization
- [Smarter](https://www.smarterasp.net ) for Hosting

### Frontend:
- [Angular 16](https://angular.io/) 
- [PrimeNG](https://primeng.org/) for UI components
- [Bootstrap](https://getbootstrap.com/) for responsive design
- [Angular Material](https://material.angular.io ) for UI components
- [Modernize](https://demos.adminmart.com/free/angular/modernize-angular-free/landingpage/index.html) Angular Template for Seamless User Experience
- [FullCalendar](https://fullcalendar.io/) for calendar implementation
- [Firebase](https://firebase.google.com) for Hosting






## 🔥 How to run 

Make a folder to hold both repos

```bash
mkdir recipe-realm ; cd recipe-realm
```

### Backend (ASP.NET Core 7)
1. Clone the repo

```
git clone https://github.com/Mousa-Ahmed98/Recipe-Api
```
2. Run the following commands.
```bash
cd Recipe-Api
dotnet restore
dotnet build
dotnet run
```
Or if you have VisualStudio just open `.sln` file and press `f5` to build and run the project.


### Frontend (Angular 16)
1. Return back to root directory 
```
cd ..

```
2. Clone the repo

```
git clone https://github.com/Mousa-Ahmed98/Recipe-UI
```
3. Run the following commands:
   

```bash
cd Recipe-UI
npm install
ng serve --open
```   




## 🍳 License

RecipeRealm is licensed under the MIT License. Please refer to the `LICENSE.md` file for more details.
Feel free to reach out to for any further inquiries or collaboration.



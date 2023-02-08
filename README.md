# BugBully

[![dotnetVersion](https://img.shields.io/badge/.NET_Framework-4.7.2-777BB4.svg?style=flat-square)](https://laravel.com/docs/8.x)
![EntityFrameworkVersion](https://img.shields.io/badge/Entity_Framework-6.4.4-343ertfds.svg?style=flat-square)
![MoqVersion](https://img.shields.io/badge/Moq-4.18.4-823BB4.svg?style=flat-square)
![AutoFacVersion](https://img.shields.io/badge/AutoFac-6.5.0-4235B4.svg?style=flat-square)

> *This project was build as a technical test to serve as a Bug Tracker App. The initial version is intended to cover the features outlined below.*

## Your Goal
Implement a simple bug tracker. The features we would like, in no particular order:
- It should be possible to view the list of open bugs, including their titles
- It should be possible to view the detail of individual bugs, including the title the full
description, and when it was opened
- It should be possible to create bugs
- It should be possible to close bugs
- It should be possible to assign bugs to people
- It should be possible to add people to the system
- It should be possible to change people’s names
- The web application should look nice
- The web application should expose some sort of API
- The data should be stored in some sort of database

## Features
- **Bug Tracking** Use the bugs page to add, remove or update bugs, each bug can be assigned a status and assigned to a registered user.
- **User Registration:** The user page allows the creation of new users and the modification or deletion of existing users.
- **API:** The web app exposes an API with a method called GetBugs which returns a list in json format of all the current listed bugs in the database.

## Requirements
- Entity Framework
- .NET Framework 4.7.2

## Quick Start
### The app is design to run out of the box. Following these steps to get setup and run the app.
---
1. Clone the repo
```
> git clone <remote>
```
2. Load the application up inside Visual Studio
3. Open up the package manager console
4. Ensure Entity Framework is installed and run the following command to enable migrations
```
> Enable-Migrations
```
5. Once migrations are enabled, run the next command to populate and seed the database
```
> Update-Database
```
6. Once the preceeding steps have been complete.
7. Build your application and run

---
## Documention

The following steps outline the work undertaken to complete the task and fulfill the requirements of the brief. Each step will detail the stages of development.

### Project Dependencies
```
> Entity Framework 6
> AutoFac for DI in .NET Framework
> Moq
> Fluent Assertions
```
### **Solution Setup**
1. New ASP MVC .NET Framework Web App (4.7.2)

The first step was to choose the desired application that best suited the needs of the project. As the brief outlined the project should be a web app, the decision was made to move forward with an ASP .NET Framework web app using the MVC architecture. The .NET Framework version targetted is 4.7.2

2. New SQL Database Project (BugBullyDB)

Next, as the app required the use of a database. A new SQL database project was added to the solution and the connectionstring configuration was added to the web.config
```
<connectionStrings>
	<add name="BugBullyDbConnection" connectionString="" providerName="System.Data.SqlClient" />
</connectionStrings>
```
3. New Unit Test Project

Finally, add a new unit test project to include unit tests for the application along with installing the moq and fluent assertion libraries.

# **Development Log**

### 1. Model Design

Implementing the data models began by outlining the data fields required to fulfill the brief. The three models chosen are:

**Bugs**
```
+ Id: int
+ Title: string
+ Description: string
+ DateReported: DateTime
+ StatusId: int
+ UserId: int
+ User: Users (Relational Entity Object)
+ Status: Statues (Relational Entity Object)
```
**Users**
```
+ Id: int
+ Name: string
+ Username: string
+ Password: string
```
**Statuses**
```
+ Id: int
+ Name: string
```

### 2. Database Context

Next up, set up the dB context class to expose the data entities. This involved adding the web.config to the base context base class and creating the DbSet properties.
```
public class BugBullyContext : DbContext
{
    public BugBullyContext() : base("name=BugBullyDbConnection") {}

    public DbSet<Users> Users { get; set; }
    public DbSet<Statuses> Statuses { get; set; }
    public DbSet<Bugs> Bugs { get; set; }
}
```

### 3. Controllers & Views

Using visual studio, added new scaffolded MVC 5 controllers with views using Entity Framework selecting the relevant models and database context class.

Additionally, added a new scaffolded item selectig the Web API with Entity Framework.

### 4. Created Dependency Services

The next step was to create the interface for the database context class for DI with DbSet properties and additional dependant methods.
```
public interface IRepository
{
    DbSet<Users> Users { get; set; }
    DbSet<Statuses> Statuses { get; set; }
    DbSet<Bugs> Bugs { get; set; }

    bool Save();
    bool SaveEntry<T>(T entity);
    Task<bool> SaveAsync();
    Task<bool> SaveEntryAsync<T>(T entity);
    void Dispose();
}
```
Implemented the interface into the database context class for a concrete class implementation
```
public class BugBullyContext : DbContext, IRepository
```
Created UserService for UsersController Methods
```
public class UserService
{
    private readonly IRepository _context;

    public UserService(IRepository repository)
    {
        _context = repository;
    }
}
```
Created BugService for BugsController Methods
```
public class BugService
{
    private readonly IRepository _context;

    public BugService(IRepository dependency)
    {
        _context = dependency;
    }
}
```
Created APIService for APIController Methods
```
public class APIService
{
    private readonly IRepository _context;

    public APIService(IRepository dependency)
    {
        _context = dependency;
    }
}
```
### 5. Configurations
Added route configs for controller actions
```
public class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        );

        // Register the routes for the bugs, users, and statuses controllers
        routes.MapRoute(
            name: "Bugs",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Bugs", action = "Index", id = UrlParameter.Optional }
        );
        routes.MapRoute(
            name: "Users",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Users", action = "Index", id = UrlParameter.Optional }
        );
    }
}
```
Added route configs for API action
```
public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        // Web API routes
        config.MapHttpAttributeRoutes();

        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
    }
}
```
Further configurations added to the API route class to enable JSON formatting
```
// Web API configuration and services
GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
```
Added AutoFac configuration to pass IRepository through the controllers
```
public class MvcApplication : HttpApplication
{
    protected void Application_Start()
    {
        //Global configs
        AreaRegistration.RegisterAllAreas();
        GlobalConfiguration.Configure(WebApiConfig.Register);
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);

        //Dependency Injection
        var builder = new ContainerBuilder();
        builder.RegisterType<BugBullyContext>().As<IRepository>();
        builder.RegisterControllers(typeof(MvcApplication).Assembly);
        var container = builder.Build();
        DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
    }
}
```

### 6. Test, test, test
- Created test methods for user authentication (Outside of scope)
- Created test methods for user service using Mocking and Fluent Assertions
- Created test methods for bug service using Mocking and Fluent Assertions

### 7. Service Class Implementations
- Added authentication methods to UserService
- Created methods for userService
- Created methods for bugService
- Modified UsersController to pass UserService
- Modified BugsController to pass BugService
- Modified APIController to pass APIService

### 8. Front-end touch ups
- Modified scaffolded views to a more defined structure
- Implemented some bootstrap styling across the board
- Added small JS function to hide/show password fields

### 9. Migrations
- Added Entity Framework migration for BugBullyDB
Inside the package manager console
```
> Enable-Migrations
```
```
> Add-Migration BugBullyMigration
```
- Modified snapshot migration scaffolded from the models declared
- Added Seed data to the configurations class
```
> Update-Database
```

If you've found a bug in BugBully feel free to [open a new issue](https://github.com/hudds-awp2021-cht2520/assignment-01-ajameslarner/issues/new) and let me know!

---
---
## Additional Resources
- [Entity Framework Documention](https://learn.microsoft.com/en-us/ef/ef6/)
- [Boostrap Documentation](https://getbootstrap.com/docs/5.1/getting-started/introduction/)


## Author

👤 Anthony Larner
- LinkedIn: [ajameslarner](www.linkedin.com/in/ajameslarner)
- Twitter: [@ajameslarner](https://twitter.com/ajameslarner)
- Github: [@ajameslarner](https://github.com/ajameslarner)
# AccountOwnerServer

This is an ASP.NET Core Web API application. It uses a MySQL database and employs a Repository pattern, generics, LINQ, and Entity Framework Core. The architecture uses multiple projects and services to demonstrate good practices, and to make the code more readable and maintainable.

It is deployed on Heroku as a Docker container at https://brokerage.herokuapp.com

SUMMARY

1. Created database schemas, tables and relations and populated the tables with data using MySQL Workbench.
2. Created ASP.NET Core application. Used extension methods to configure CORS, IIS and other services.
3. Created an external logging service. Used dependency injection and inversion of control to inject the service via controller constructors.
4. Implemented a Repository pattern. Created models and model attributes. Created a context class and database connection. Created a wrapper around individual repository classes.
5. Implemented routing, GET, POST, PUT and DELETE requests, and Data Transfer Objects (DTOs).




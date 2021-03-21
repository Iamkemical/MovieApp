# Project Name
Movie App API and Client

# Description
Movie App API and Client is a web application project, where the API is built using .NET and is consumed by a .NET MVC Client.
The project was originally built on the .NET Core 3.1 (LTS) but has been moved to .NET 5.
## Features of API
1. CRUD Operations
2. Repository Pattern
3. DTOs
4. Open API Specification
5. Swagger Documentation
6. XML Comments
7. Authentication and Authorization
8. Password Hashing
9. Unit testing with Xunit and Moq
10. JWT and Bearer Token
11. Serilog for log management

## Features of MVC Client
1. Use of Http Client
2. Repository Pattern
3. MVVM Pattern
4. JWT and Bearer Token

## Background of API
The API is built using best practices and standard. The API performs CRUD operation on the database built on the repository pattern.
The use of Data Transfer Objects in abstracting data and mapping to the appropraite model.
Open API Specification highlights the use of Swagger for documenting and testing all individual endpoint of the API.
XML Comments help in detailing each individual endpoint on the Swagger Documentation.

## Background of MVC Client
The MVC Client uses Http Client to communicate with the API.
The repository pattern is used in calling all the individual controllers in the API

# Installation
## VISUAL STUDIO
1. Clone the project on your local computer or download as Zip folder. 
2. Open the .sln project. 
3. Change the startup project on the properties of the project to mutiple, this would launch both the API and MVC Client project.
4. Add Migrations and Update Database on the Package Manager Console.
``` C#
add-migration <migrationname>

update-database
```
5. Build project.
6. Run project.

## Visual Studio Code
1. Clone the project on your local computer or download as Zip folder. 
2. Using the Command Prompt go to the directory of the project.
3. Open the project at the root of the folder.
4. Change the configuration of the project to launch both API and MVC project together.
5. Add Migrations and Update Database on the Terminal.
``` C#
dotnet add migration <migrationname>

dotnet update database
```
6. Clean the project using
``` C#
dotnet clean
```
7. Build project on the command line
``` C#
dotnet build
```
8. Run project.
``` C#
dotnet run
```
# Support
If you want to see the .NET documentation for building APIs and consuming them you can visit https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0

# Contributing
With the API and Web Project fully secured using JWT and Cookie Auth, contributions are needed to build other security functionalities into the API and Web Project.
Unit testing of MVC Controller hasn't been done yet

# Contributors
Isaac Gabriel

Joshua Afolayan


# LICENSE
MIT

# Project Status
Project has slowed down. You can choose to continue by forking the project to your own repository.

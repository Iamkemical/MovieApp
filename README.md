# Project Name
Movie App API and Client

# Description
Movie App API and Client is a web application project, where the API is built using .NET and is consumed by a .NET MVC Client.
The project is built on the .NET Core 3.1 (LTS).
## Features of API
1. CRUD Operations
2. Repository Pattern
3. DTOs
4. Open API Specification
5. Swagger Documentation
6. XML Comments

## Features of MVC Client
1. Use of Http Client
2. Repository Pattern
3. MVVM Pattern

## Background of API
The API is built using best practices and standard. The API performs CRUD operatin on the database built on the repository pattern.
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
3. Change the startup project on the properties of the project to mutiple, this would lanch both the API and MVC Client project.
4. Build project.
5. Run project.

## Visual Studio Code
1. Clone the project on your local computer or download as Zip folder. 
2. Using the Command Prompt go to the directory of the project.
3. Open the project at the root of the folder.
4. Change the configuration of the project to launch both API and MVC project together.
5. Clean the project using
``` C#
dotnet clean
```
6. Build project on the command line
``` C#
dotnet build
```
7. Run project.
``` C#
dotnet run
```
# Support
If you want to see the .NET documentation for building APIs and consuming them you can visit https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0

# Contributing
By the time of writing this, there's no implementation for security on the application, so if you're interested in building the security into the API and consuming it on the MVC Client you're urged to go ahead and make a pull request when you're done.

# LICENSE
MIT

# Project Status
Project has slowed down. You can choose to continue by forking the project to your own repository.
Since project is built using .NET Core 3.1 (LTS), you can move the project to .NET 5 as you see fit.

# Fleet Management - Case Study

A small-scale fleet management system where vehicles make deliveries to predetermined locations along a certain route. FleetManagement Web Api developed with .NET 6.0

## Built With
* C# 10.0
* .NET 6.0
* EntityFrameworkCore 6.0.3
* MsSQL
* Autofac 7.2
* AutoMapper 11.0
* FluentValidation 10.4
* Swashbuckle.AspNetCore 6.2.3
* Moq 4.17.2
* xunit 2.4.1

## Getting Started

### Installation
1. Clone the repo
   ```sh
   git clone https://github.com/volkansk/FleetManagement.git
   ```
2. Edit ConnectionStrings in appsettings.json
   > ConnectionStrings.SqlConnection
   > [appsettings](FleetManagement.API/appsettings.json)
3. Execute Migration SQL Script
   > FleetManagement.Repository/Migrations/EFSQLScripts/FleetManagement.Repository.DataContext.sql
   > [EF SQL Script](FleetManagement.Repository/Migrations/EFSQLScripts/FleetManagement.Repository.DataContext.sql)

### Build & Test
   ```sh
   dotnet build
   ```
   ```sh
   dotnet test --filter "(Category!~CaseResult&Category!~PartialDistribute)"
   ```
   ```sh
   dotnet test --filter "Category=CaseResult"
   ```
   ```sh
   dotnet test --filter "Category=PartialDistribute"
   ```
   
### Publish
   ```sh
   dotnet publish "FleetManagement.API.csproj" -c Release
   ```
   
### Run
   ```sh
   dotnet FleetManagement.API.dll
   ```
   
## Documentations 

### API Documentation
     While the application is running, the document can be accessed at {baseUrl}/swagger/index.html.
     
### Postman Collection
[Import PostmanCollection](FleetManagement.API.Tests/PostmanCollection/FleetManagement.postman_collection.json)
     
### Add New Migration
   ```
    Open Tools -> Nuget Package Manager -> Package Manager Console
    Startup Project: FleetManagement.API
    Default Project: FleetManagement.Repository

    Commands:
      1- add-migration {migrationName}
      2- update-database
   ```


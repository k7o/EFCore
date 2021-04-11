# EFCore Tests

## Setup

### Download a sqlserver container
https://hub.docker.com/_/microsoft-mssql-server

docker pull mcr.microsoft.com/mssql/server:2019-latest

### Run it

docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=gN7quwVBof5BEMPpwQLU' -p 1433:1433 -v G:\Projects\Dotnet\EFCore:/sql -d mcr.microsoft.com/mssql/server:2019-latest

### Easy access to your db instance (with vscode)
https://marketplace.visualstudio.com/items?itemName=ms-mssql.mssql

use localhost not docker ip as db location

https://blog.sqlauthority.com/2019/03/06/sql-server-how-to-get-started-with-docker-containers-with-latest-sql-server/
https://blog.sqlauthority.com/2019/04/20/sql-server-docker-volume-and-persistent-storage/


## Get started

https://docs.microsoft.com/en-us/ef/core/get-started/overview/install
https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli

dotnet new classlib -o BlogDL
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design

dotnet ef migrations add InitialCreate
dotnet ef database update

## Frontend

https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-5.0&tabs=visual-studio-code

dotnet new webapp -o BlogWebapp

dotnet add reference ..\BlogDL\BlogDL.csproj

dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design -v 5.0.0-*
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design -v 5.0.0-*
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore -v 5.0.0-* 

dotnet tool install --global dotnet-aspnet-codegenerator

dotnet aspnet-codegenerator razorpage -m BlogDL.Blog -dc BlogDL.BloggingContext -udl -outDir Pages\Blogs
dotnet aspnet-codegenerator razorpage -m BlogDL.Post -dc BlogDL.BloggingContext -udl -outDir Pages\BlogPosts

https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/complex-data-model?view=aspnetcore-5.0&tabs=visual-studio


## Misc

### Intellisense issues VSCode
https://stackoverflow.com/questions/29975152/intellisense-not-automatically-working-vscode

1. Ctrl + Shift + p
2. Write "OmniSharp: Select Project" and press Enter.
3. Choose the solution workspace entry.
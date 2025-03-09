Run Database Migrations
Before starting the API, apply database migrations:

dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API
dotnet ef database update --project Infrastructure --startup-project API

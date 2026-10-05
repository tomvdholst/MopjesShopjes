# Overview

MopjesShopjes is a personal shopping list app, built as a learning project. 

- Backend: ASP .NET Core, EF core, PostgreSQL
- Frontend: Angular

# Local development

## Prerequisites

- .NET sdk
- Node.js
- PostgreSQL

## How to

### Run backend

Install PostgreSQL locally, then configure the connection.
```
cd backend/MopjesShopjes.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=mopjesshopjes;Username=postgres;Password=YOUR_PASSWORD_HERE"
dotnet ef database update
dotnet run
```
### Test backend

```
dotnet test
```
### Run frontend
Install dependencies.
```
cd frontend
npm install
```
Then run frontend.
```
ng serve
```
### Test frontend
```
cd frontend
ng test --watch=false
```



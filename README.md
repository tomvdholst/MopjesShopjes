# Overview

MopjesShopjes is a personal shopping list app, built as a learning project. 

- Backend: ASP .NET Core, EF core, PostgreSQL
- Frontend: Angular

# Deploy

The service can be deployed using docker compose. For example:

```
services:
  db:
    image: postgres:16
    environment:
      POSTGRES_DB: DB_NAME
      POSTGRES_PASSWORD: YOUR_PASSWORD
    volumes:
      - /path/to/data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U postgres"]
      interval: 5s
      timeout: 5s
      retries: 5

  backend:
    image: ghcr.io/tomvdholst/mopjesshopjes-backend:latest
    environment:
      ConnectionStrings__DefaultConnection: "Host=db;Port=5432;Database=mopjesshopjes;Username=postgres;Password=${POSTGRES_PASSWORD}"
    depends_on:
      db:
        condition: service_healthy

  frontend:
    image: ghcr.io/tomvdholst/mopjesshopjes-frontend:latest
    ports:
      - "4201:80"
    environment:
      #- BACKEND_HOST= #OPTIONAL, name of the backend docker container (default: 'backend')
    depends_on:
      - backend
```

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



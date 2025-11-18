# eshop-rest-api

A small REST API built with ```.NET 8 / ASP.NET Core``` featuring API versioning (v1/v2), SQLite data storage, and easy switching to an in-memory mock data layer for testing.

## Features
* Endpoints

  * ```GET /api/v1/Products``` | return all products

  * ```GET /api/v2/Products?page=1&pageSize=10``` | return paginated products with metadata about pagination

  * ```GET /api/v1/Products/{id}``` | return product by ID

  * ```PATCH /api/v1/Products/{id}/description``` | partial update of product description via request body, return updated product

* API Versioning: ```api/v{version}/Products``` (v1 & v2)

* OpenAPI/Swagger: ```/swagger```

## Prerequisites
* ```.NET SDK 8.0+```

## Configuration
* ```appsettings.json```
```json
{
  "Data": { "UseMock": false },
  "ConnectionStrings": {
    "Default": "Data Source=./Data/eshop.db;Mode=ReadWriteCreate"
  }
}
```
* Set ```Data:UseMock = true``` to run with mock data (no database needed).
* With ```UseMock = false```, the app uses ```SQLite```. Migrations are applied automatically on startup and the DB file is created under ```bin/.../Data/eshop.db```

## How to Run
```bash
dotnet restore
dotnet build
dotnet run --project eshop-rest-api
```
* Application should be running on ```http:\\localhost:5122``` or ```http:\\localhost:5000```

## How to Run Tests
* Set ```Data:UseMock = true``` in config file.
```bash
dotnet test
```

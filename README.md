# E-Commerce Payment Integration API

This project implements a backend API for an e-commerce platform that integrates with a Balance Management service for payment processing.

## Features

- Product listing and details
- User balance information
- Order creation with payment reservation
- Order completion with payment finalization
- Robust error handling and retry mechanisms

## API Documentation

The API is documented using Swagger/OpenAPI. When running the application, navigate to `/swagger` to view the interactive documentation.

### Main Endpoints:

#### Products
- `GET /api/products` - Get all available products
- `GET /api/products/{id}` - Get a specific product by ID

#### Balance
- `GET /api/balance` - Get current user balance information

#### Orders
- `POST /api/orders/create` - Create a new order and reserve payment
- `POST /api/orders/{id}/complete` - Complete an existing order and finalize payment
- `GET /api/orders/{id}` - Get information about a specific order
- `GET /api/orders/buyer/{buyerId}` - Get all orders for a specific buyer

## Technical Details

- Built on ASP.NET Core 9.0
- Clean Architecture (Onion Architecture) design
- Entity Framework Core for data persistence
- Integration with Balance Management API
- Resilient HTTP communication with Polly

## Getting Started

### Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 or any other code editor
- Docker Desktop (for containerized run)

### Running the Application Locally (Without Docker)
1. Clone the repository
2. Navigate to the project directory
3. Run `dotnet restore` to restore dependencies
4. Run `dotnet build` to build the project
5. Run `dotnet run --project src/ECommerce.WebApi/ECommerce.WebApi.csproj` to start the API server
6. Open a browser and navigate to `https://localhost:5001/swagger` to view the API documentation

---

## 🐳 Running the Application with Docker

> Dockerfile proje kök dizinindeyse bu komutları kullanabilirsiniz:

```bash
docker build -t ecommerce-api .
docker run -d -p 5000:8080 --name ecommerce-api ecommerce-api

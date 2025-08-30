

# 🛒 SmartShop API

[![.NET](https://github.com/supunsarachitha/SmartShop-API/actions/workflows/dotnet.yml/badge.svg)](https://github.com/supunsarachitha/SmartShop-API/actions/workflows/dotnet.yml)
[![Dependency Review](https://github.com/supunsarachitha/SmartShop-API/actions/workflows/dependency-review.yml/badge.svg)](https://github.com/supunsarachitha/SmartShop-API/actions/workflows/dependency-review.yml)

SmartShop API is a **.NET-based backend service** designed to power e-commerce applications.  
It provides endpoints for managing products, categories, customers, and orders, enabling developers to build scalable and maintainable online store solutions.

---

## 📌 Features

- **Product Management** – Create, update, delete, and list products.
- **Category Management** – Organize products into categories.
- **Customer Management** – Handle customer profiles and authentication.
- **Order Processing** – Manage shopping carts, checkout, and order history.
- **RESTful API** – Clean and predictable endpoints.
- **Entity Models** – Strongly typed C# model classes for application data.
- **Extensible Architecture** – Easy to integrate with payment gateways, inventory systems, and analytics.

---

## 🛠️ Tech Stack

- **Language:** C#  
- **Framework:** .NET 6+  
- **Database:** (e.g. PostgreSQL — update as applicable)  
- **ORM:** Entity Framework Core  
- **Authentication:** JWT-based authentication (if implemented)  
- **Hosting:** Compatible with Azure, AWS, or on-premise deployment

---

## 📂 Project Structure

```
SmartShop-API/
│
├── SmartShop.API/           # Main API project
│   ├── Controllers/         # API endpoints
│   ├── Models/              # Entity and DTO classes
│   ├── Services/            # Business logic
│   ├── Data/                # Database context and migrations
│   └── Program.cs           # Application entry point
│
├── .github/workflows/       # CI/CD pipelines
├── SmartShop.API.sln        # Solution file
├── LICENSE.txt              # MIT License
└── README.md                # Project documentation
```

---

## 🚀 Getting Started

### 1️⃣ Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- A running database instance (SQL Server, PostgreSQL, etc.)
- Git

### 2️⃣ Clone the Repository
```bash
git clone https://github.com/supunsarachitha/SmartShop-API.git
cd SmartShop-API
```

### 3️⃣ Configure Environment
Create an `appsettings.json` file in `SmartShop.API/` with your database connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SmartShop;User Id=youruser;Password=yourpassword;"
  }
}
```

### 4️⃣ Run Migrations
```bash
dotnet ef database update
```

### 5️⃣ Run the API
```bash
dotnet run --project SmartShop.API
```

The API will be available at:  
```
https://localhost:5001
```

---

## 📖 API Documentation

Once running, you can explore the API via Swagger UI:  
```
https://localhost:5001/swagger
```

---

## 🧪 Testing

Run unit tests with:
```bash
dotnet test
```

---

## 📜 License

This project is licensed under the [MIT License](LICENSE.txt).

---

## 🤝 Contributing

We welcome contributions of all kinds—bug fixes, feature enhancements, documentation improvements, and more.

📌 See our [Contributing Guidelines](CONTRIBUTING.md) to get started.


---

## 📬 Contact

**Author:** Supun Sarachitha  
**GitHub:** [supunsarachitha](https://github.com/supunsarachitha)

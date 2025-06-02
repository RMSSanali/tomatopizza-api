# 🍕 TomatoPizza API

Welcome to the **TomatoPizza** backend API — a fully-featured ASP.NET Core Web API project designed for a pizza ordering system. Built with real-world functionality in mind and integrated with **Azure**, this API supports dish management, orders, ingredients, user roles, bonus point rewards, and secure JWT-based authentication.

## 🔧 Features

- ✅ **User Management**
  - Register, login, and view user profile
  - Role-based access (RegularUser, PremiumUser, Admin)
  - Bonus points system with promotional upgrades

- 🍕 **Order Management**
  - Place orders with multiple dishes
  - Automatic bonus calculation and discounts for Premium users
  - Use bonus points for free pizza 🎉
  - Order history per user

- 🧂 **Dish & Ingredient Management**
  - Admins can create, update, delete dishes and ingredients
  - Public endpoints to fetch available menu items

- 🔐 **Authentication & Authorization**
  - Secure JWT tokens for login
  - Role-based protected routes

- ☁️ **Azure Integration**
  - Azure Key Vault for secret management
  - Azure App Service for deployment
  - Application Insights for monitoring

## 🧰 Technologies Used

- ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- Identity + JWT Authentication
- Azure Key Vault, App Service, Application Insights
- Swagger/OpenAPI

## 🚀 Getting Started

### Prerequisites

- .NET 7 or later
- SQL Server (or Azure SQL)
- Visual Studio / VS Code
- Azure account (for full deployment + Key Vault usage)

### Clone the repository

```bash
git clone https://github.com/RMSSanali/tomatopizza-api.git
cd tomatopizza-api
---

### 🔧 Configuration

This project uses `appsettings.json` for local development.

In production, secrets like the **JWT key** and **SQL connection string** are securely stored in **Azure Key Vault**.

To connect the Key Vault:

- Set the `KeyVault:KeyVaultURL` in `appsettings.json`
- Ensure your **App Service** has the correct role (Managed Identity)
- Configure Key Vault access via Azure RBAC

You can retrieve values like:

- `Jwt:Key`
- `ConnectionStrings:DefaultConnection`

using `DefaultAzureCredential` and `Azure.Security.KeyVault.Secrets`.
📂 Project Structure
TomatoPizza/
├── Controllers/
├── Data/
│   ├── Entities/
│   ├── Identity/
├── Core/
│   ├── Services/
│   ├── Interfaces/
├── Repos/
├── DTO/
├── Extensions/
├── Security/
└── Program.cs

🧪 Bonus Logic
⭐ Users earn 10 bonus points per ordered dish

🥇 Premium users get 20% discount if ordering 3+ dishes

🎁 Use 100 bonus points to get the cheapest dish free

📈 Monitoring
Enabled via Azure Application Insights

Auto-Heal + Health Checks configured

📦 Deployment
Deployed to Azure App Service

🙋‍♀️ Author
Sanali Sewwandi – GitHub

📄 License
🎓 This project is part of a school assignment (JENSEN YH – Cloud Developer).
Not licensed for commercial use.


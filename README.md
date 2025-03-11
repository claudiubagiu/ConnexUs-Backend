# Backend - .NET API

This is the backend for the application, built with .NET and SQL Server. It provides RESTful APIs for managing data and business logic.

## 🚀 Technologies Used

- **.NET 8**

- **Entity Framework Core for ORM**

- **MS SQL Server as the database**

- **JWT Authentication**

- **Postman for API documentation**

## 📌 Prerequisites

Before running the project, make sure you have the following installed:

- **.NET SDK**

- **SQL Server**

- **Visual Studio or Visual Studio Code**

## 📥 Installation

### 1️⃣ Clone the repository

```sh
git clone https://github.com/claudiubagiu/ConnexUs-Backend.git
cd repo-backend
```

### 2️⃣ Set up the database

Update appsettings.json with your SQL Server connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=YourDatabase;User Id=sa;Password=YourPassword;"
}
```

### 3️⃣ Apply migrations

```sh
dotnet ef database update
```

### ▶️ Run the project

```sh
dotnet run
```

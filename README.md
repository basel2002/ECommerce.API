# 🛒 E-Commerce API

A scalable, cleanly structured **E-Commerce Backend API** built using modern backend practices and design principles. This project delivers core e-commerce functionality such as product browsing, cart management, and order processing, all secured with robust authentication and authorization.

---

## 🚀 Overview

This API is designed with **Clean Architecture** and **N-Tier Architecture**, ensuring maintainability, scalability, and separation of concerns. It follows industry best practices to provide a solid foundation for real-world e-commerce systems.

---

## ✨ Key Features

### 🔐 Authentication & Authorization

* JWT-based authentication
* Policy-based authorization
* Integration with Microsoft Identity
* Secure user handling (UserId extracted from JWT claims)

---

### 🛍️ Product Management

* Browse products with:

  * Filtering
  * Search
  * Pagination
* Full CRUD operations

---

### 📂 Category Management

* Create, update, and delete categories
* Assign images to categories

---

### 🛒 Cart Management

* Add items to cart
* Update quantities
* Remove items
* Retrieve current user cart

---

### 📦 Order Management

* Place orders
* View order history
* Retrieve order details

---

### 🖼️ File Upload

* Upload images for:

  * Products
  * Categories

---

## 🏗️ Architecture

This project follows **Clean Architecture** principles:

```
ECommerceAPI/
│
├── Common Layer            # Shared utilities
├── API Layer               # Controllers & Endpoints
├── Buisness Logic Layer    # DTOs & Services & Validations
├── Data Access Layer       # Entities & Core Models & Repos
```

### 🧩 Design Patterns Used

* Repository Pattern (Generic & Specific)
* Unit of Work Pattern
* Result Pattern (Standardized API responses)

---

## ⚙️ Technologies

* ASP.NET Core Web API
* Entity Framework Core
* Microsoft Identity
* JWT Authentication
* FluentValidation
* SQL Server (or any compatible DB)

---

## 🔑 Authentication Notes

* Users authenticate using **JWT tokens**
* No need to pass `UserId` in requests
* User identity is automatically extracted from JWT claims

---

## 📡 API Endpoints

### 🔐 Authentication

| Method | Endpoint             | Description       |
| ------ | -------------------- | ----------------- |
| POST   | `/api/Auth/Register` | Register new user |
| POST   | `/api/Auth/Login`    | Login user        |

---

### 📂 Categories

| Method | Endpoint               |
| ------ | ---------------------- |
| GET    | `/api/Categories`      |
| GET    | `/api/Categories/{id}` |
| POST   | `/api/Categories`      |
| PUT    | `/api/Categories/{id}` |
| DELETE | `/api/Categories/{id}` |

---

### 🛍️ Products

| Method | Endpoint                    |
| ------ | --------------------------- |
| GET    | `/api/Products`             |
| GET    | `/api/Products/{id}`        |
| GET    | `/api/Products/Pagination/` |
| POST   | `/api/Products`             |
| PUT    | `/api/Products/{id}`        |
| DELETE | `/api/Products/{id}`        |

#### 🔎 Query Parameters Example:

```
/api/Products/Pagination/?categoryId=1&search=phone&pageNumber=1&pageSize=10&minPrice=1000&maxPrice=5000
```

---

### 🛒 Cart

| Method | Endpoint                 |
| ------ | ------------------------ |
| POST   | `/api/Carts`             |
| PUT    | `/api/Carts`             |
| DELETE | `/api/Carts/{productId}` |
| GET    | `/api/Carts`             |

---

### 📦 Orders

| Method | Endpoint           |
| ------ | ------------------ |
| POST   | `/api/Orders`      |
| GET    | `/api/Orders`      |
| GET    | `/api/Orders/{id}` |

---

### 🖼️ File Upload

| Method | Endpoint                     |
| ------ | ---------------------------- |
| POST   | `/api/Images/upload`         |
| POST   | `/api/Images/products/{id}/image`   |
| POST   | `/api/Images/categories/{id}/image` |

---

## 🚀 Getting Started

### ✅ Prerequisites

* .NET SDK
* SQL Server
* Visual Studio or VS Code

---

### ⚙️ Setup

```bash
# Clone the repository
git clone https://github.com/YoussefKassab1/E-Commerce-API-Project.git

# Navigate to project folder
cd ecommerce-api

# Apply database migrations
dotnet ef database update

# Run the application
dotnet run
```

---

## 🧪 Testing

* Tested using **Postman**
* Demo video available in the Readme file

👉 **Postman Testing Video:** *(https://drive.google.com/drive/folders/1T1wniI5-6---_SiPSxfd1iSXzgfNtLtz?usp=sharing)*
👉 **Postman API Collection:** *(https://baselasherief02-7200933.postman.co/workspace/Basel-Ashraf's-Workspace~d073ce0f-31d9-4255-8574-17db536a4e29/collection/53193153-f2a24d7b-54f2-4dc1-963e-c881228f8cd7?action=share&source=copy-link&creator=53193153)*

---

## 📌 Best Practices Applied

* DTOs for clean data transfer
* FluentValidation for input validation
* Async/Await for better performance
* Dependency Injection for loose coupling
* Clean separation of concerns

---

## 👨‍💻 Author

**Basel Ashraf**

---

## ⭐ Final Notes

This project is a strong foundation for building scalable e-commerce systems and demonstrates real-world backend architecture and practices. Feel free to extend it with features like payments, reviews, or admin dashboards.

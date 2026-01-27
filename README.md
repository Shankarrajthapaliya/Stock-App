📈 Stock Commentary & Portfolio Platform

A full-stack stock application that allows users to analyze, discuss, and track stocks.
The platform enables public stock discussions, authenticated portfolio management, and is designed to support AI-driven comment analysis in future iterations.

🎯 Purpose of the Application

The goal of this application is to create a centralized platform for stock discussion and portfolio tracking, where:

Users can share insights and opinions on stocks

Comments are publicly visible and attached to specific stocks

Authenticated users can build and manage a personal stock portfolio

Future AI features will help analyze and rank comments based on realism, consistency, and relevance

🔮 Planned AI Enhancement

A future version of this application will introduce AI-based comment evaluation, which will:

Analyze user comments

Rank them based on realism and data consistency

Highlight highquality insights for readers

The current architecture is intentionally designed to support this extension.

🚀 Features
🔓 Public (No Login Required)

View list of stocks

View stock details

Read comments attached to stocks

🔐 Authenticated Users (JWT)

Register and log in using ASP.NET Identity

Secure JWT authentication

Add stocks to a personal portfolio

View “My Portfolio” (user-specific data)

🛡️ Authorization & Policies

Policy-based authorization using JWT claims

Fine-grained access control (example: stock deletion permissions)

Frontend dynamically enables or disables UI actions based on user permissions

🧱 Tech Stack
Backend

ASP.NET Core Web API

Entity Framework Core

PostgreSQL

ASP.NET Identity

JWT Authentication

Policy-based Authorization

Swagger UI

Frontend

React (Vite)

React Router

Context API for authentication state

JWT persistence via localStorage

Modern dark-themed UI

🗂️ Project Structure
Backend (/web)
Controllers/
 ├── AuthController.cs
 ├── StockController.cs
 ├── CommentController.cs
 └── PortfolioController.cs

Services/
 ├── JwtTokenService.cs
 ├── StockService.cs
 ├── CommentService.cs
 └── PortfolioService.cs

Repo/
DTO/
Models/
Seed/
Program.cs
appsettings.json

Frontend (/ui/stock-ui)
src/
 auth/
   ├── authContext.jsx
   ├── authStorage.js
   └── ProtectedRoute.jsx

 pages/
   └── Login.jsx

 StockList.jsx
 CommentList.jsx
 Portfolio.jsx
 api.js
 App.jsx
 main.jsx
 index.css

🔑 Authentication Flow (JWT)

User logs in via:

POST /api/auth/login


Backend returns:

{
  "username": "user1",
  "token": "JWT_TOKEN",
  "expiration": "2026-01-28T10:00:00Z"
}


Frontend:

Stores token securely in localStorage

Automatically attaches token to API requests

Decodes JWT claims for role and permission checks

Enables protected routes and UI actions accordingly

🔒 Authorization Example
Backend Policy
options.AddPolicy("CanDeleteStock", policy =>
    policy.RequireClaim("permission", "delete:stock"));

Frontend Conditional Rendering
hasPermission("delete:stock") && (
  <button>Delete Stock</button>
)

🌐 API Endpoints (Sample)
Auth

POST /api/auth/register

POST /api/auth/login

Stocks

GET /api/stock

POST /api/stock (authenticated)

DELETE /api/stock/{id} (policy-based)

Comments

GET /api/comment/symbol/{symbol}

POST /api/comment (authenticated)

Portfolio

GET /api/portfolio (authenticated)

POST /api/portfolio (authenticated)

⚙️ Running the Project Locally
Backend
cd web
dotnet restore
dotnet ef database update
dotnet run


Swagger UI:

https://localhost:{port}/swagger

Frontend
cd ui/stock-ui
npm install
npm run dev


Frontend runs at:

http://localhost:5173

🔄 CORS & Development Proxy

During local development:

Vite proxies /api requests to the backend

No explicit CORS configuration is required

server: {
  proxy: {
    "/api": {
      target: "http://localhost:5045",
      changeOrigin: true
    }
  }
}

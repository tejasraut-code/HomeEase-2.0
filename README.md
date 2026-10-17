# 🏠 HomeEase 2.0 — Home Services Booking Platform

## 🔍 Overview

HomeEase 2.0 is a web-based home services booking application built using **ASP.NET Core MVC, C#, Entity Framework Core, and SQL Server**.

The application allows customers to explore home services, filter services by category, and create bookings. It also includes service and category management, booking administration, and service-provider workflows.

The project demonstrates practical implementation of MVC architecture, database operations, session-based login, and AJAX-based interactions.

## ✨ Key Features

### 🛠️ Service and Category Management
- Create, view, update, and delete service categories.
- Manage services with descriptions, prices, estimated durations, and images.
- Filter services by category.
- Upload and replace service images.

### 📅 Service Booking
- Select a service and schedule an appointment.
- Provide a service address and additional customer notes.
- View booking history and cancel eligible pending bookings.
- Preserve the service name, price, and duration recorded when a booking is created.

### 👥 User and Provider Workflows
- User registration and login with password hashing.
- Session-based user identity and role-specific workflows.
- Service-provider registration and approval.
- Provider assignment to bookings and provider-side booking completion.

### ⚡ AJAX and API Integration
- Retrieve service data through a JSON endpoint.
- Filter and refresh service listings without a full page reload.
- Delete services asynchronously and update the displayed list.
- Use jQuery AJAX to communicate with an API controller within the MVC application.

### 🗂️ Booking Administration
- View and manage bookings through the administration workflow.
- Update booking statuses.
- Coordinate provider assignment and service completion.

## 🛠️ Tech Stack

- **Language:** C#
- **Framework:** ASP.NET Core MVC (.NET 8)
- **ORM:** Entity Framework Core
- **Database:** Microsoft SQL Server
- **Frontend:** HTML5, CSS3, JavaScript, Razor Views
- **UI Framework:** Bootstrap 5
- **AJAX:** jQuery
- **API:** ASP.NET Core API controller with JSON responses
- **IDE:** Visual Studio 2022
- **Version Control:** Git and GitHub

## 🏗️ Architecture

The application follows the MVC pattern:

- **Models:** Represent application data and view-specific data.
- **Views:** Render pages using Razor and Bootstrap.
- **Controllers:** Handle incoming requests, validation, application workflows, and responses.
- **Entity Framework Core:** Connects the application to SQL Server.
- **API Controller:** Exposes service data through the `/api/services` endpoint.
- **ViewModels and DTOs:** Carry data between application layers and shape data for specific views or API responses.

## 🚀 Getting Started

### Prerequisites

- Visual Studio 2022 with the .NET 8 SDK.
- Microsoft SQL Server.
- Entity Framework Core tools.

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/tejasraut-code/HomeEase-2.0.git
   ```

2. Open `HomeEase 2.0 MVC.csproj` in Visual Studio.

3. Configure the `DefaultConnection` connection string in `appsettings.json` for your local SQL Server instance. Do not use another developer's machine-specific connection string.

4. Apply the existing database migrations using the Package Manager Console:

   ```powershell
   Update-Database
   ```

5. Build the solution and run the application from Visual Studio.

## 📚 What I Learned

- Building a database-driven web application using ASP.NET Core MVC.
- Performing CRUD operations with Entity Framework Core.
- Using ViewModels and DTOs to organize application data.
- Managing login sessions and hashing passwords.
- Implementing AJAX requests and consuming JSON responses.
- Handling image uploads and related file operations.
- Designing booking workflows involving customers, administrators, and service providers.
- Managing code changes using Git and GitHub.

## 🚧 Development Status

The core application workflows are implemented and tested locally. The user interface, usability, and overall portfolio presentation are still being improved.

## 🔗 Repository

[HomeEase 2.0 MVC — GitHub](https://github.com/tejasraut-code/HomeEase-2.0)
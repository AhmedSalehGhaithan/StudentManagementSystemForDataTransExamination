# Student Management System

A comprehensive ASP.NET Core application for managing students, teachers, classes, grades, and schedules in an educational institution. 

## Key Features

- 🏫 **User Management**: Integrated with ASP.NET Core Identity for authentication (Students, Teachers, Admins)
- 📚 **Academic Management**: Track classes, subjects, and student grades
- 🧑‍🏫 **Teacher Portal**: Manage teaching assignments and schedules
- 📅 **Schedule System**: Organize class timetables and room allocations
- 📊 **Grade Tracking**: Record and analyze student performance
- 🔐 **Role-Based Access**: Secure endpoints with proper authorization

## Technology Stack

- Backend: ASP.NET Core 7, Entity Framework Core
- Authentication: JWT, Refresh Tokens
- Database: SQL Server (or PostgreSQL via configuration)
- Architecture: Clean Architecture, CQRS Pattern
- Testing: xUnit (optional)

## Getting Started

1. Clone the repository
2. Configure your database connection in `appsettings.json`
3. Run migrations to set up the database
4. Start the application

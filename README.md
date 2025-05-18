# Student Management System

A comprehensive ASP.NET Core application for managing students, teachers, classes, grades, and schedules in an educational institution. 

## Key Features

- 🏫 **User Management**: Integrated with ASP.NET Core Identity for authentication (Students, Teachers,...)
- 📚 **School Management**: Track classes, subjects, and student grades
- 🧑‍🏫 **Teacher Portal**: Manage teaching assignments and schedules
- 📅 **Schedule System**: Organize class timetables and room allocations
- 📊 **Grade Tracking**: Record and analyze student performance
- 🔐 **Role-Based Access**: Secure endpoints with proper authorization

## Technology Stack

- Backend: ASP.NET Core 9, Entity Framework Core
- Authentication: JWT, Refresh Tokens
- Database: SQL Server 
- Architecture: Clean Architecture, CQRS Pattern

## Getting Started

1. Clone the repository
2. Configure your database connection
3. Rebuild the project to install package needed
4. Open the package Manager Console and run the command 'Update-database`appsettings.json`
6. Start the application
7. some api endpoint need authrization , first add role then  register new user and set the role as Admin, then login and use the endpoints

### Note
- I forget to link the relashinship between Users with teacher and student table
- I don't notice the the need of creating Profile endpoint required


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


  --------------------------------------------------------
  # School Management System - Database Schema

![School Management System](https://via.placeholder.com/800x400?text=School+Management+System)

## Overview
This document describes the database schema for a comprehensive School Management System built with ASP.NET. The system supports multiple user roles, class management, grading, attendance tracking, and more.

## Database Tables

### Core Tables

| Table Name       | Description                                                                 |
|------------------|-----------------------------------------------------------------------------|
| `Users`          | Base table for all system users with authentication details                 |
| `Subjects`       | Contains all subjects offered by the school                                 |
| `Teachers`       | Teacher-specific information linked to Users                                |
| `Classes`        | Class groups with homeroom teacher assignment                               |
| `Students`       | Student information linked to Users and Classes                             |
| `Parents`        | Parent/guardian information (optional module)                               |

### Relationship Tables

| Table Name       | Description                                                                 |
|------------------|-----------------------------------------------------------------------------|
| `ClassSubjects`  | Many-to-many relationship between Classes and Subjects                      |
| `StudentParents` | Many-to-many relationship between Students and Parents                      |

### Operational Tables

| Table Name       | Description                                                                 |
|------------------|-----------------------------------------------------------------------------|
| `Schedules`      | Class schedules with day/time information                                   |
| `Grades`         | Student grades for specific class subjects                                  |
| `Attendance`     | Daily attendance records for students                                       |

## Detailed Schema

-- Create Database
CREATE DATABASE SchoolManagementSystem;
GO

USE SchoolManagementSystem;
GO

-- Users Table (for authentication)
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Admin', 'Teacher', 'Student', 'Parent')),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    LastLogin DATETIME NULL
);

-- Subjects Table
CREATE TABLE Subjects (
    SubjectId INT PRIMARY KEY IDENTITY(1,1),
    SubjectName NVARCHAR(100) NOT NULL,
    SubjectCode NVARCHAR(20) NOT NULL UNIQUE,
    Description NVARCHAR(500) NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

-- Teachers Table
CREATE TABLE Teachers (
    TeacherId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL UNIQUE,
    HireDate DATE NOT NULL,
    Specialization NVARCHAR(100) NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- Classes Table
CREATE TABLE Classes (
    ClassId INT PRIMARY KEY IDENTITY(1,1),
    ClassName NVARCHAR(50) NOT NULL,
    ClassCode NVARCHAR(20) NOT NULL UNIQUE,
    AcademicYear NVARCHAR(20) NOT NULL,
    HomeroomTeacherId INT NULL,
    FOREIGN KEY (HomeroomTeacherId) REFERENCES Teachers(TeacherId)
);

-- Students Table
CREATE TABLE Students (
    StudentId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL UNIQUE,
    EnrollmentDate DATE NOT NULL,
    ClassId INT NULL,
    ParentContact NVARCHAR(100) NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId)
);

-- ClassSubjects (Many-to-Many between Classes and Subjects)
CREATE TABLE ClassSubjects (
    ClassSubjectId INT PRIMARY KEY IDENTITY(1,1),
    ClassId INT NOT NULL,
    SubjectId INT NOT NULL,
    TeacherId INT NOT NULL,
    UNIQUE (ClassId, SubjectId),
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId),
    FOREIGN KEY (SubjectId) REFERENCES Subjects(SubjectId),
    FOREIGN KEY (TeacherId) REFERENCES Teachers(TeacherId)
);

-- Schedules Table
CREATE TABLE Schedules (
    ScheduleId INT PRIMARY KEY IDENTITY(1,1),
    ClassSubjectId INT NOT NULL,
    DayOfWeek INT NOT NULL CHECK (DayOfWeek BETWEEN 1 AND 7),
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    RoomNumber NVARCHAR(20) NULL,
    FOREIGN KEY (ClassSubjectId) REFERENCES ClassSubjects(ClassSubjectId)
);

-- Grades Table
CREATE TABLE Grades (
    GradeId INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    ClassSubjectId INT NOT NULL,
    GradeValue DECIMAL(5,2) NOT NULL,
    GradeDate DATE NOT NULL DEFAULT GETDATE(),
    Comments NVARCHAR(500) NULL,
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    FOREIGN KEY (ClassSubjectId) REFERENCES ClassSubjects(ClassSubjectId)
);

-- Attendance Table (Bonus)
CREATE TABLE Attendance (
    AttendanceId INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    ClassSubjectId INT NOT NULL,
    Date DATE NOT NULL,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Present', 'Absent', 'Late', 'Excused')),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    FOREIGN KEY (ClassSubjectId) REFERENCES ClassSubjects(ClassSubjectId),
    UNIQUE (StudentId, ClassSubjectId, Date)
);

-- Parents Table (Bonus)
CREATE TABLE Parents (
    ParentId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20) NOT NULL,
    Address NVARCHAR(200) NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- StudentParents (Many-to-Many between Students and Parents)
CREATE TABLE StudentParents (
    StudentParentId INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    ParentId INT NOT NULL,
    Relationship NVARCHAR(50) NOT NULL,
    UNIQUE (StudentId, ParentId),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    FOREIGN KEY (ParentId) REFERENCES Parents(ParentId)
);


-----------------------------\
erDiagram
    Users ||--o{ Teachers : "1:1"
    Users ||--o{ Students : "1:1"
    Users ||--o{ Parents : "1:1"
    Teachers ||--o{ Classes : "Homeroom Teacher"
    Classes ||--|{ ClassSubjects : "1:M"
    Subjects ||--|{ ClassSubjects : "1:M"
    Teachers ||--|{ ClassSubjects : "1:M"
    ClassSubjects ||--|{ Schedules : "1:M"
    ClassSubjects ||--|{ Grades : "1:M"
    Students ||--|{ Grades : "1:M"
    Students ||--|{ Attendance : "1:M"
    Students }|--|| Classes : "Belongs to"
    Students }|--|{ StudentParents : "M:M"
    Parents }|--|{ StudentParents : "M:M"

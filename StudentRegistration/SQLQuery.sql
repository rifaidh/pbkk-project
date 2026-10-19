-- 1. Pembuatan Database
CREATE DATABASE StudentRegistrationDB;
GO

USE StudentRegistrationDB;
GO

-- 2. Pembuatan Tabel Programs
CREATE TABLE Programs
(
    ProgramId INT IDENTITY(1,1) CONSTRAINT PK_Programs PRIMARY KEY,
    ProgramCode VARCHAR(20) NOT NULL CONSTRAINT UQ_Programs_ProgramCode UNIQUE,
    ProgramName VARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Programs_IsActive DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Programs_CreatedAt DEFAULT SYSDATETIME()
);
GO

-- 3. Pembuatan Tabel Students
CREATE TABLE Students
(
    StudentId INT IDENTITY(1,1) CONSTRAINT PK_Students PRIMARY KEY,
    NIM VARCHAR(20) NOT NULL CONSTRAINT UQ_Students_NIM UNIQUE,
    Name VARCHAR(100) NOT NULL,
    ProgramId INT NOT NULL,
    BirthDate DATE NULL,
    Address VARCHAR(250) NULL,
    PhoneNumber VARCHAR(20) NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Students_CreatedAt DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL,
    CONSTRAINT FK_Students_Programs FOREIGN KEY (ProgramId) REFERENCES Programs (ProgramId)
);
GO

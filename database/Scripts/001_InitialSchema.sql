IF DB_ID(N'inventoryDB') IS NULL
BEGIN
    CREATE DATABASE inventoryDB;
END
GO

USE inventoryDB;
GO

IF OBJECT_ID(N'dbo.Products', N'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Products;
END
GO

IF OBJECT_ID(N'dbo.Categories', N'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Categories;
END
GO

CREATE TABLE dbo.Categories
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE dbo.Products
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(120) NOT NULL,
    Description NVARCHAR(500) NULL,
    Price DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL,
    MinimumStock INT NOT NULL,
    CategoryId INT NOT NULL,
    CreatedAtUtc DATETIME2 NOT NULL CONSTRAINT DF_Products_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES dbo.Categories(Id)
);
GO

CREATE INDEX IX_Products_CategoryId ON dbo.Products(CategoryId);
GO

CREATE INDEX IX_Products_Stock_MinimumStock ON dbo.Products(Stock, MinimumStock);
GO

INSERT INTO dbo.Categories (Name)
VALUES (N'Perifericos'),
       (N'Monitores'),
       (N'Accesorios');
GO

INSERT INTO dbo.Products (Name, Description, Price, Stock, MinimumStock, CategoryId)
VALUES (N'Teclado mecanico', N'Teclado con switch blue', 75.50, 4, 10, 1),
       (N'Monitor 24 pulgadas', N'Panel IPS Full HD', 189.99, 2, 5, 2),
       (N'Mouse inalambrico', N'Mouse ergonomico', 24.90, 15, 8, 3);
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetInventoryValueByCategory
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Id AS CategoryId,
        c.Name AS CategoryName,
        SUM(p.Price * p.Stock) AS TotalInventoryValue
    FROM dbo.Categories c
    INNER JOIN dbo.Products p ON p.CategoryId = c.Id
    GROUP BY c.Id, c.Name
    ORDER BY c.Name;
END
GO

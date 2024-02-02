CREATE DATABASE ProductSales;
USE ProductSales;

CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    CONSTRAINT UC_ProductName UNIQUE (Name)
);


CREATE TABLE Sales (
    SaleID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT,
    SaleDate DATETIME,
    Quantity INT,
    CONSTRAINT FK_ProductSales FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);


CREATE TABLE TrackingNumbers (
    TrackingNumberID INT PRIMARY KEY IDENTITY(1,1),
    SaleID INT,
    TrackingNumber NVARCHAR(50) NOT NULL,
    Location NVARCHAR(255) NOT NULL,
    CONSTRAINT FK_SaleTracking FOREIGN KEY (SaleID) REFERENCES Sales(SaleID),
    CONSTRAINT UC_TrackingNumber UNIQUE (TrackingNumber)
);

-- Insert 5 records into Products table
INSERT INTO Products (Name, Price) VALUES
    ('Laptop', 899.99),
    ('Smartphone', 599.99),
    ('Tablet', 299.95),
    ('Headphones', 79.99),
    ('Camera', 449.99);

-- Insert 5 records into Sales table with corrected date format
INSERT INTO Sales (ProductID, SaleDate, Quantity) VALUES
    (1, '2024-01-15T12:00:00', 10),
    (2, '2024-02-01T14:30:00', 15),
    (3, '2024-02-10T10:45:00', 20),
    (4, '2024-02-20T16:20:00', 5),
    (5, '2024-03-05T08:15:00', 8);


-- Insert 5 records into TrackingNumbers table
INSERT INTO TrackingNumbers (SaleID, TrackingNumber, Location) VALUES
    (1, 'TN123456', 'Shipping Center A'),
    (2, 'TN789012', 'Shipping Center B'),
    (3, 'TN345678', 'Shipping Center C'),
    (4, 'TN901234', 'Shipping Center D'),
    (5, 'TN567890', 'Shipping Center E');

	SELECT TOP (1000) [ProductID]
      ,[Name]
      ,[Price]
  FROM [ProductSales].[dbo].[Products]
  SELECT DATABASEPROPERTYEX('[ProductSales]', 'Collation') AS Collation;
  ALTER DATABASE [ProductSales] COLLATE SQL_Latin1_General_CP1_CS_AS;
  USE master;
GO
SELECT
   db.name DBName,
   spid,
   login_time,
   last_batch,
   status,
   hostname,
   program_name
FROM
   sys.databases db
JOIN
   sys.sysprocesses sp ON db.database_id = sp.dbid
WHERE
   db.name = 'ProductSales';
   USE master;
GO
ALTER DATABASE [ProductSales] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
ALTER DATABASE [ProductSales] COLLATE SQL_Latin1_General_CP1_CS_AS;
ALTER DATABASE [ProductSales] SET MULTI_USER;

USE master;
GO

DECLARE @killCommand NVARCHAR(MAX) = '';

SELECT @killCommand = @killCommand + 'KILL ' + CAST(spid AS NVARCHAR) + ';'
FROM sys.sysprocesses
WHERE DB_NAME(dbid) = 'ProductSales';

EXEC sp_executesql @killCommand;
ALTER DATABASE [ProductSales] COLLATE SQL_Latin1_General_CP1_CS_AS;
-- Средняя цена продукта
SELECT AVG(Price) AS AvgPrice FROM Products;

-- Общая сумма продаж
SELECT SUM(Price * Quantity) AS TotalSales
FROM Sales
JOIN Products ON Sales.ProductID = Products.ProductID;

-- Топ 5 продуктов по количеству продаж
SELECT TOP 5 ProductID, SUM(Quantity) AS TotalQuantitySold
FROM Sales
GROUP BY ProductID
ORDER BY TotalQuantitySold DESC;

-- Среднее количество продукта в каждой продаже
SELECT AVG(Quantity) AS AvgQuantity FROM Sales;

-- Продукт с максимальной ценой
SELECT TOP 1 * FROM Products ORDER BY Price DESC;

-- Продукт с минимальной ценой
SELECT TOP 1 * FROM Products ORDER BY Price;

-- Сумма продаж по дням
SELECT SaleDate, SUM(Price * Quantity) AS DailySales
FROM Sales
JOIN Products ON Sales.ProductID = Products.ProductID
GROUP BY SaleDate;

-- Сумма продаж по месяцам
SELECT DATEPART(MONTH, SaleDate) AS SaleMonth, SUM(Price * Quantity) AS MonthlySales
FROM Sales
JOIN Products ON Sales.ProductID = Products.ProductID
GROUP BY DATEPART(MONTH, SaleDate);

-- Продукты, не проданные ни разу
SELECT * FROM Products
WHERE ProductID NOT IN (SELECT ProductID FROM Sales);

-- Общее количество проданных продуктов
SELECT SUM(Quantity) AS TotalQuantitySold FROM Sales;

-- Среднее количество продаж в день
SELECT AVG(CAST(Quantity AS FLOAT) / DATEDIFF(DAY, MIN(SaleDate), MAX(SaleDate) + 1)) AS AvgSalesPerDay
FROM Sales;

-- Суммарная стоимость каждого продукта
SELECT ProductID, SUM(Price * Quantity) AS TotalCost
FROM Sales
JOIN Products ON Sales.ProductID = Products.ProductID
GROUP BY ProductID;

-- Самый прибыльный продукт
SELECT TOP 1 ProductID, SUM(Price * Quantity) AS TotalRevenue
FROM Sales
JOIN Products ON Sales.ProductID = Products.ProductID
GROUP BY ProductID
ORDER BY TotalRevenue DESC;

-- Количество продаж для каждого продукта
SELECT ProductID, COUNT(SaleID) AS SaleCount
FROM Sales
GROUP BY ProductID;

-- Средняя стоимость продажи
SELECT AVG(Price * Quantity) AS AvgSalePrice FROM Sales
JOIN Products ON Sales.ProductID = Products.ProductID;

-- Продукты, проданные более 10 раз
SELECT ProductID, COUNT(SaleID) AS SaleCount
FROM Sales
GROUP BY ProductID
HAVING COUNT(SaleID) > 10;

-- Дни с наибольшим количеством продаж
SELECT SaleDate, SUM(Quantity) AS TotalSales
FROM Sales
GROUP BY SaleDate
ORDER BY TotalSales DESC;

-- Среднее количество продаж для каждого продукта в месяц
SELECT ProductID, AVG(Quantity) AS AvgSalesPerMonth
FROM Sales
GROUP BY ProductID;

-- Продажи, совершенные в последний месяц
SELECT * FROM Sales
WHERE SaleDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 1, 0)
AND SaleDate < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0);

-- Средний чек
SELECT AVG(Price * Quantity) AS AvgOrderValue FROM Sales
JOIN Products ON Sales.ProductID = Products.ProductID;

SELECT SaleDate, SUM(Quantity) AS TotalQuantitySold
FROM Sales GROUP BY SaleDate ORDER BY TotalQuantitySold DESC;


WITH MonthlyProductSales AS (
    SELECT ProductID, DATEADD(MONTH, DATEDIFF(MONTH, 0, SaleDate), 0) AS MonthStart, SUM(Quantity) AS TotalQuantitySold
    FROM Sales GROUP BY ProductID, DATEADD(MONTH, DATEDIFF(MONTH, 0, SaleDate), 0)
)
SELECT mps.ProductID
FROM MonthlyProductSales mps
JOIN MonthlyProductSales prev ON mps.ProductID = prev.ProductID AND DATEADD(MONTH, 1, prev.MonthStart) = mps.MonthStart
WHERE mps.TotalQuantitySold > prev.TotalQuantitySold;

SELECT DATEADD(MONTH, DATEDIFF(MONTH, 0, SaleDate), 0) AS MonthStart, COUNT(DISTINCT ProductID) AS UniqueProductCount
FROM Sales GROUP BY DATEADD(MONTH, DATEDIFF(MONTH, 0, SaleDate), 0);


SELECT p.*
FROM Products p
LEFT JOIN Sales s ON p.ProductID = s.ProductID
LEFT JOIN TrackingNumbers t ON s.SaleID = t.SaleID
WHERE t.SaleID IS NULL;

WITH MonthlyProductSales AS (
    SELECT ProductID, DATEADD(MONTH, DATEDIFF(MONTH, 0, SaleDate), 0) AS MonthStart, SUM(Quantity) AS TotalQuantitySold
    FROM Sales GROUP BY ProductID, DATEADD(MONTH, DATEDIFF(MONTH, 0, SaleDate), 0)
)
SELECT mps.*
FROM MonthlyProductSales mps
JOIN (SELECT ProductID, MAX(TotalQuantitySold) AS MaxTotalQuantitySold FROM MonthlyProductSales GROUP BY ProductID) x
ON mps.ProductID = x.ProductID AND mps.TotalQuantitySold = x.MaxTotalQuantitySold;

SELECT DISTINCT p.*
FROM Products p
JOIN Sales s ON p.ProductID = s.ProductID
JOIN TrackingNumbers t ON s.SaleID = t.SaleID;


SELECT p.*, s.TotalQuantitySold
FROM Products p
JOIN (SELECT ProductID, SUM(Quantity) AS TotalQuantitySold FROM Sales GROUP BY ProductID) s
ON p.ProductID = s.ProductID
WHERE s.TotalQuantitySold = (SELECT MIN(TotalQuantitySold) FROM (SELECT SUM(Quantity) AS TotalQuantitySold FROM Sales GROUP BY ProductID) x);


SELECT p.*
FROM Products p
LEFT JOIN Sales s ON p.ProductID = s.ProductID AND s.SaleDate >= DATEADD(MONTH, -1, GETDATE())
WHERE s.SaleID IS NULL;

SELECT p.*
FROM Products p
JOIN (SELECT ProductID, AVG(Quantity) AS AvgQuantity FROM Sales GROUP BY ProductID) s
ON p.ProductID = s.ProductID
JOIN Sales sa ON p.ProductID = sa.ProductID
WHERE sa.Quantity > s.AvgQuantity;






USE InventoryManagementDB;
GO

MERGE INTO dbo.Products AS target
USING (VALUES
    (N'ELEC-LAP-1001', N'Dell Latitude 5540 Laptop', 850),
    (N'ELEC-MOU-1002', N'Logitech MX Master 3S Mouse', 2400),
    (N'ELEC-KEY-1003', N'Logitech MX Keys Keyboard', 1600),
    (N'ELEC-MON-1004', N'Dell UltraSharp 27" Monitor', 720),
    (N'ELEC-PRT-1005', N'HP LaserJet Pro Printer', 310)
) AS source (SKU, Name, StockQuantity)
ON target.SKU = source.SKU
WHEN MATCHED THEN
    UPDATE SET
        Name = source.Name,
        StockQuantity = source.StockQuantity
WHEN NOT MATCHED BY TARGET THEN
    INSERT (SKU, Name, StockQuantity)
    VALUES (source.SKU, source.Name, source.StockQuantity);
GO

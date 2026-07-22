USE InventoryManagementDB;
GO

CREATE TABLE dbo.Products
(
    ProductId INT IDENTITY(1,1) NOT NULL,
    SKU NVARCHAR(50) NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    StockQuantity INT NOT NULL
        CONSTRAINT DF_Products_StockQuantity DEFAULT (0),
    RowVersion ROWVERSION NOT NULL,

    CONSTRAINT PK_Products
        PRIMARY KEY (ProductId),

    CONSTRAINT UQ_Products_SKU
        UNIQUE (SKU),

    CONSTRAINT CK_Products_StockQuantity_NonNegative
        CHECK (StockQuantity >= 0)
);

CREATE TABLE dbo.InventoryAudit
(
    AuditId BIGINT IDENTITY(1,1) NOT NULL,
    ProductId INT NOT NULL,
    OldQuantity INT NOT NULL,
    NewQuantity INT NOT NULL,
    ChangeAmount INT NOT NULL,
    ChangedAt DATETIMEOFFSET NOT NULL,
    ChangedBy NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_InventoryAudit
        PRIMARY KEY (AuditId),

    CONSTRAINT FK_InventoryAudit_Products
        FOREIGN KEY (ProductId)
        REFERENCES dbo.Products(ProductId)
);


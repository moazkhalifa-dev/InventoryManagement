USE InventoryManagementDB;
GO

CREATE PROCEDURE dbo.sp_AdjustStock
    @ProductId INT,
    @ChangeAmount INT,
    @RowVersion BINARY(8),
    @ChangedBy NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @OldQuantity INT;
        DECLARE @NewQuantity INT;
        DECLARE @CurrentRowVersion BINARY(8);

        SELECT
            @OldQuantity = StockQuantity,
            @CurrentRowVersion = RowVersion
        FROM dbo.Products
        WHERE ProductId = @ProductId;

        IF @OldQuantity IS NULL
        BEGIN
            ;THROW 50001, 'Product not found.', 1;
        END

        IF @CurrentRowVersion <> @RowVersion
        BEGIN
            ;THROW 50002, 'The product has been modified by another user.', 1;
        END

        SET @NewQuantity = @OldQuantity + @ChangeAmount;

        IF @NewQuantity < 0
        BEGIN
            ;THROW 50003, 'Insufficient stock. The adjustment would make stock negative.', 1;
        END

        UPDATE dbo.Products
        SET StockQuantity = @NewQuantity
        WHERE ProductId = @ProductId
          AND RowVersion = @RowVersion;

        IF @@ROWCOUNT = 0
        BEGIN
            ;THROW 50002, 'The product has been modified by another user.', 1;
        END

        INSERT INTO dbo.InventoryAudit
        (
            ProductId,
            OldQuantity,
            NewQuantity,
            ChangeAmount,
            ChangedAt,
            ChangedBy
        )
        VALUES
        (
            @ProductId,
            @OldQuantity,
            @NewQuantity,
            @ChangeAmount,
            SYSDATETIMEOFFSET(),
            @ChangedBy
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
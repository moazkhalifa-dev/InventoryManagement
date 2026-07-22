using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using InventoryManagement.Data;
using InventoryManagement.Models;

namespace InventoryManagement.Repositories
{
    public class ProductRepository
    {
        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();

            using (var connection = SqlConnectionFactory.Create())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(
                    @"SELECT
                        ProductId,
                        SKU,
                        Name,
                        StockQuantity,
                        RowVersion
                      FROM dbo.Products
                      ORDER BY ProductId;",
                    connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            products.Add(MapProduct(reader));
                        }
                    }
                }
            }

            return products;
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            using (var connection = SqlConnectionFactory.Create())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(
                    @"SELECT
                        ProductId,
                        SKU,
                        Name,
                        StockQuantity,
                        RowVersion
                      FROM dbo.Products
                      WHERE ProductId = @ProductId;",
                    connection))
                {
                    command.Parameters.Add(
                        "@ProductId",
                        SqlDbType.Int).Value = productId;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (!await reader.ReadAsync())
                        {
                            return null;
                        }

                        return MapProduct(reader);
                    }
                }
            }
        }

        public async Task UpdateStockAsync(
            int productId,
            int changeAmount,
            byte[] rowVersion,
            string changedBy)
        {
            using (var connection = SqlConnectionFactory.Create())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(
                    "dbo.sp_AdjustStock",
                    connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(
                        "@ProductId",
                        SqlDbType.Int).Value = productId;

                    command.Parameters.Add(
                        "@ChangeAmount",
                        SqlDbType.Int).Value = changeAmount;

                    command.Parameters.Add(
                        "@RowVersion",
                        SqlDbType.Binary,
                        8).Value = rowVersion;

                    command.Parameters.Add(
                        "@ChangedBy",
                        SqlDbType.NVarChar,
                        100).Value = changedBy;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> ImportStockAdjustmentsAsync(
            IReadOnlyList<StockAdjustmentRecord> records,
            string changedBy,
            IProgress<int> progress)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            if (records.Count == 0)
            {
                return 0;
            }

            int importedCount = 0;

            using (var connection = SqlConnectionFactory.Create())
            {
                await connection.OpenAsync();

                foreach (var record in records)
                {
                    byte[] rowVersion =
                        await GetCurrentRowVersionAsync(
                            connection,
                            record.ProductId);

                    if (rowVersion == null)
                    {
                        throw new InvalidOperationException(
                            $"ProductId {record.ProductId} was not found.");
                    }

                    await ExecuteStockAdjustmentAsync(
                        connection,
                        record.ProductId,
                        record.ChangeAmount,
                        rowVersion,
                        changedBy);

                    importedCount++;

                    int percentage =
                        importedCount * 100 / records.Count;

                    progress?.Report(percentage);
                }
            }

            return importedCount;
        }

        private static async Task<byte[]> GetCurrentRowVersionAsync(
            SqlConnection connection,
            int productId)
        {
            using (var command = new SqlCommand(
                @"SELECT RowVersion
                  FROM dbo.Products
                  WHERE ProductId = @ProductId;",
                connection))
            {
                command.Parameters.Add(
                    "@ProductId",
                    SqlDbType.Int).Value = productId;

                object result =
                    await command.ExecuteScalarAsync();

                if (result == null || result == DBNull.Value)
                {
                    return null;
                }

                return (byte[])result;
            }
        }

        private static async Task ExecuteStockAdjustmentAsync(
            SqlConnection connection,
            int productId,
            int changeAmount,
            byte[] rowVersion,
            string changedBy)
        {
            using (var command = new SqlCommand(
                "dbo.sp_AdjustStock",
                connection))
            {
                command.CommandType =
                    CommandType.StoredProcedure;

                command.Parameters.Add(
                    "@ProductId",
                    SqlDbType.Int).Value = productId;

                command.Parameters.Add(
                    "@ChangeAmount",
                    SqlDbType.Int).Value = changeAmount;

                command.Parameters.Add(
                    "@RowVersion",
                    SqlDbType.Binary,
                    8).Value = rowVersion;

                command.Parameters.Add(
                    "@ChangedBy",
                    SqlDbType.NVarChar,
                    100).Value = changedBy;

                await command.ExecuteNonQueryAsync();
            }
        }

        private static Product MapProduct(
            SqlDataReader reader)
        {
            return new Product
            {
                ProductId = reader.GetInt32(
                    reader.GetOrdinal("ProductId")),

                SKU = reader.GetString(
                    reader.GetOrdinal("SKU")),

                Name = reader.GetString(
                    reader.GetOrdinal("Name")),

                StockQuantity = reader.GetInt32(
                    reader.GetOrdinal("StockQuantity")),

                RowVersion = (byte[])reader["RowVersion"]
            };
        }
    }
}
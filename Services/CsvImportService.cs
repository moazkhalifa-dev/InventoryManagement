using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InventoryManagement.Models;

namespace InventoryManagement.Services
{
    public class CsvImportService
    {
        public async Task<List<StockAdjustmentRecord>> ReadAsync(
            string filePath)
        {
            var records = new List<StockAdjustmentRecord>();

            using (var reader = new StreamReader(filePath))
            {
                string headerLine = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(headerLine))
                {
                    throw new InvalidOperationException(
                        "The CSV file is empty.");
                }

                string[] headers = headerLine.Split(',');

                if (headers.Length != 2 ||
                    !headers[0].Trim().Equals(
                        "ProductId",
                        StringComparison.OrdinalIgnoreCase) ||
                    !headers[1].Trim().Equals(
                        "ChangeAmount",
                        StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        "The CSV file must contain the columns ProductId and ChangeAmount.");
                }

                int lineNumber = 1;

                while (!reader.EndOfStream)
                {
                    lineNumber++;

                    string line = await reader.ReadLineAsync();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string[] values = line.Split(',');

                    if (values.Length != 2)
                    {
                        throw new InvalidOperationException(
                            $"Invalid CSV format at line {lineNumber}.");
                    }

                    if (!int.TryParse(
                        values[0].Trim(),
                        out int productId))
                    {
                        throw new InvalidOperationException(
                            $"Invalid ProductId at line {lineNumber}.");
                    }

                    if (!int.TryParse(
                        values[1].Trim(),
                        out int changeAmount))
                    {
                        throw new InvalidOperationException(
                            $"Invalid ChangeAmount at line {lineNumber}.");
                    }

                    records.Add(
                        new StockAdjustmentRecord
                        {
                            ProductId = productId,
                            ChangeAmount = changeAmount
                        });
                }
            }

            return records;
        }
    }
}
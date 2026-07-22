namespace InventoryManagement.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string SKU { get; set; }

        public string Name { get; set; }

        public int StockQuantity { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
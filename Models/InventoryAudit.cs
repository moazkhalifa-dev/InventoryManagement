using System;

namespace InventoryManagement.Models
{
    public class InventoryAudit
    {
        public long AuditId { get; set; }

        public int ProductId { get; set; }

        public int OldQuantity { get; set; }

        public int NewQuantity { get; set; }

        public int ChangeAmount { get; set; }

        public DateTimeOffset ChangedAt { get; set; }

        public string ChangedBy { get; set; }
    }
}
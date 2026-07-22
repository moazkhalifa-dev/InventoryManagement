# High-Throughput Inventory & Audit Log Management System

A Windows Forms (.NET Framework 4.8) desktop application for inventory management with Microsoft SQL Server integration, optimistic concurrency control, immutable audit logging, and asynchronous bulk CSV import.

---

## Features

- Product inventory management
- SQL Server integration
- Repository Pattern (Separation of Concerns)
- Stored Procedure (`sp_AdjustStock`)
- Explicit SQL Transactions
- Immutable Inventory Audit Log
- Optimistic Concurrency using SQL Server `RowVersion`
- Conflict Resolution Dialog
- Async/Await database operations
- Bulk CSV Import
- Progress Bar during long-running imports

---

## Technology Stack

- C#
- .NET Framework 4.8
- Windows Forms (WinForms)
- Microsoft SQL Server
- ADO.NET
- T-SQL

---

## Database Structure

### Products

- ProductId
- SKU (Unique)
- Name
- StockQuantity
- RowVersion

### InventoryAudit

- AuditId
- ProductId
- OldQuantity
- NewQuantity
- ChangeAmount
- ChangedAt
- ChangedBy

---

## Project Structure

```
InventoryManagement
│
├── Data
├── Database
│   └── Scripts
├── Forms
├── Models
├── Repositories
├── Services
└── Properties
```

---

## Setup

1. Execute the SQL scripts in order:

```
Database/Scripts/01_CreateDatabase.sql
Database/Scripts/02_CreateTables.sql
Database/Scripts/03_CreateStoredProcedures.sql
```

2. Update the connection string inside:

```
App.config
```

Example:

```xml
<connectionStrings>
  <add
      name="InventoryDb"
      connectionString="Data Source=YOUR_SERVER;
                        Initial Catalog=InventoryManagementDB;
                        Integrated Security=True;
                        TrustServerCertificate=True"
      providerName="System.Data.SqlClient"/>
</connectionStrings>
```

3. Open:

```
InventoryManagement.sln
```

4. Build and Run.

---

## CSV Format

```csv
ProductId,ChangeAmount
1,10
1,-5
2,20
```

---

## Implemented Requirements

-  Database Schema
-  Stored Procedure (`sp_AdjustStock`)
-  SQL Transactions
-  Audit Logging
-  Repository Pattern
-  Async/Await
-  Optimistic Concurrency
-  Conflict Resolution
-  DataGridView Inline Editing
-  Background CSV Import
-  Progress Reporting

---

Developed as part of a technical evaluation task.

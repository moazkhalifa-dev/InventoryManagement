using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventoryManagement.Models;
using InventoryManagement.Repositories;
using InventoryManagement.Services;

namespace InventoryManagement.Forms
{
    public partial class MainForm : Form
    {
        private readonly ProductRepository _productRepository;
        private readonly CsvImportService _csvImportService;
        private readonly Dictionary<int, int> _originalQuantities;

        private List<Product> _products;

        public MainForm()
        {
            InitializeComponent();

            _productRepository = new ProductRepository();
            _csvImportService = new CsvImportService();
            _originalQuantities = new Dictionary<int, int>();
            _products = new List<Product>();

            Load += MainForm_Load;
            btnRefresh.Click += BtnRefresh_Click;
            btnUpdateStock.Click += BtnUpdateStock_Click;
            btnImportCsv.Click += BtnImportCsv_Click;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                progressBarImport.Minimum = 0;
                progressBarImport.Maximum = 100;
                progressBarImport.Value = 0;

                await LoadProductsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                SetControlsEnabled(false);

                await LoadProductsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Refresh Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        private async void BtnUpdateStock_Click(object sender, EventArgs e)
        {
            try
            {
                dgvProducts.EndEdit();

                var changedProducts = GetChangedProducts();

                if (changedProducts.Count == 0)
                {
                    MessageBox.Show(
                        "No stock quantities were changed.",
                        "Update Stock",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                SetControlsEnabled(false);

                int updatedCount = 0;
                bool operationCancelled = false;

                foreach (var product in changedProducts)
                {
                    bool? updateResult =
                        await UpdateProductWithConflictResolutionAsync(product);

                    if (updateResult == null)
                    {
                        operationCancelled = true;
                        break;
                    }

                    if (updateResult.Value)
                    {
                        updatedCount++;
                    }
                }

                await LoadProductsAsync();

                if (operationCancelled)
                {
                    MessageBox.Show(
                        "The update operation was cancelled.",
                        "Update Stock",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                MessageBox.Show(
                    $"{updatedCount} product(s) updated successfully.",
                    "Update Stock",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Update Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        private async void BtnImportCsv_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title =
                    "Select Stock Adjustments CSV File";

                openFileDialog.Filter =
                    "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    SetControlsEnabled(false);

                    progressBarImport.Value = 0;

                    List<StockAdjustmentRecord> records =
                        await _csvImportService.ReadAsync(
                            openFileDialog.FileName);

                    if (records.Count == 0)
                    {
                        MessageBox.Show(
                            "The CSV file does not contain any stock adjustment records.",
                            "Import CSV",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        return;
                    }

                    var progress = new Progress<int>(
                        percentage =>
                        {
                            if (percentage < progressBarImport.Minimum)
                            {
                                percentage = progressBarImport.Minimum;
                            }

                            if (percentage > progressBarImport.Maximum)
                            {
                                percentage = progressBarImport.Maximum;
                            }

                            progressBarImport.Value = percentage;
                        });

                    int importedCount =
                        await _productRepository
                            .ImportStockAdjustmentsAsync(
                                records,
                                Environment.UserName,
                                progress);

                    progressBarImport.Value = 100;

                    await LoadProductsAsync();

                    MessageBox.Show(
                        $"{importedCount} stock adjustment record(s) imported successfully.",
                        "Import CSV",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "CSV Import Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                finally
                {
                    SetControlsEnabled(true);
                }
            }
        }

        private async Task<bool?> UpdateProductWithConflictResolutionAsync(
            Product product)
        {
            int desiredQuantity = product.StockQuantity;

            int originalQuantity =
                _originalQuantities[product.ProductId];

            int changeAmount =
                desiredQuantity - originalQuantity;

            byte[] rowVersion = product.RowVersion;

            while (true)
            {
                try
                {
                    await _productRepository.UpdateStockAsync(
                        product.ProductId,
                        changeAmount,
                        rowVersion,
                        Environment.UserName);

                    return true;
                }
                catch (SqlException ex) when (ex.Number == 50002)
                {
                    Product currentProduct =
                        await _productRepository.GetProductByIdAsync(
                            product.ProductId);

                    if (currentProduct == null)
                    {
                        MessageBox.Show(
                            "The product no longer exists.",
                            "Concurrency Conflict",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return null;
                    }

                    using (var conflictForm = new UpdateStockForm(
                        currentProduct.StockQuantity,
                        desiredQuantity))
                    {
                        DialogResult dialogResult =
                            conflictForm.ShowDialog(this);

                        if (dialogResult != DialogResult.OK)
                        {
                            return null;
                        }

                        if (conflictForm.SelectedResolution ==
                            UpdateStockForm
                                .ConflictResolution
                                .UseDatabaseValue)
                        {
                            return false;
                        }

                        if (conflictForm.SelectedResolution ==
                            UpdateStockForm
                                .ConflictResolution
                                .RetryMyValue)
                        {
                            changeAmount =
                                desiredQuantity -
                                currentProduct.StockQuantity;

                            rowVersion =
                                currentProduct.RowVersion;

                            continue;
                        }

                        return null;
                    }
                }
            }
        }

        private async Task LoadProductsAsync()
        {
            _products =
                await _productRepository.GetAllProductsAsync();

            StoreOriginalQuantities();

            dgvProducts.AutoGenerateColumns = true;
            dgvProducts.DataSource = null;
            dgvProducts.DataSource = _products;

            ConfigureProductsGrid();
        }

        private List<Product> GetChangedProducts()
        {
            var changedProducts = new List<Product>();

            foreach (var product in _products)
            {
                if (!_originalQuantities.ContainsKey(product.ProductId))
                {
                    continue;
                }

                int originalQuantity =
                    _originalQuantities[product.ProductId];

                if (product.StockQuantity != originalQuantity)
                {
                    changedProducts.Add(product);
                }
            }

            return changedProducts;
        }

        private void StoreOriginalQuantities()
        {
            _originalQuantities.Clear();

            foreach (var product in _products)
            {
                _originalQuantities[product.ProductId] =
                    product.StockQuantity;
            }
        }

        private void ConfigureProductsGrid()
        {
            dgvProducts.ReadOnly = false;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;

            dgvProducts.Columns["ProductId"].ReadOnly = true;
            dgvProducts.Columns["SKU"].ReadOnly = true;
            dgvProducts.Columns["Name"].ReadOnly = true;
            dgvProducts.Columns["StockQuantity"].ReadOnly = false;

            dgvProducts.Columns["RowVersion"].Visible = false;
            dgvProducts.Columns["RowVersion"].ReadOnly = true;
        }

        private void SetControlsEnabled(bool enabled)
        {
            btnRefresh.Enabled = enabled;
            btnUpdateStock.Enabled = enabled;
            btnImportCsv.Enabled = enabled;
            dgvProducts.Enabled = enabled;
        }
    }
}
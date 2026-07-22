using System;
using System.Windows.Forms;

namespace InventoryManagement.Forms
{
    public partial class UpdateStockForm : Form
    {
        public enum ConflictResolution
        {
            None,
            RetryMyValue,
            UseDatabaseValue
        }

        public ConflictResolution SelectedResolution { get; private set; }

        public UpdateStockForm(
            int databaseQuantity,
            int userQuantity)
        {
            InitializeComponent();

            lblDatabaseQuantityValue.Text =
                databaseQuantity.ToString();

            lblUserQuantityValue.Text =
                userQuantity.ToString();

            SelectedResolution = ConflictResolution.None;
        }

        private void BtnRetryMyValue_Click(
            object sender,
            EventArgs e)
        {
            SelectedResolution =
                ConflictResolution.RetryMyValue;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnUseDatabaseValue_Click(
            object sender,
            EventArgs e)
        {
            SelectedResolution =
                ConflictResolution.UseDatabaseValue;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(
            object sender,
            EventArgs e)
        {
            SelectedResolution =
                ConflictResolution.None;

            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
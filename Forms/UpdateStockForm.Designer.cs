namespace InventoryManagement.Forms
{
    partial class UpdateStockForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblDatabaseQuantity = new System.Windows.Forms.Label();
            this.lblDatabaseQuantityValue = new System.Windows.Forms.Label();
            this.lblUserQuantity = new System.Windows.Forms.Label();
            this.lblUserQuantityValue = new System.Windows.Forms.Label();
            this.btnRetryMyValue = new System.Windows.Forms.Button();
            this.btnUseDatabaseValue = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(18, 18);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(478, 48);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "This product was modified by another user. Choose how you want to resolve the con" +
    "flict.";
            // 
            // lblDatabaseQuantity
            // 
            this.lblDatabaseQuantity.AutoSize = true;
            this.lblDatabaseQuantity.Location = new System.Drawing.Point(18, 79);
            this.lblDatabaseQuantity.Name = "lblDatabaseQuantity";
            this.lblDatabaseQuantity.Size = new System.Drawing.Size(177, 20);
            this.lblDatabaseQuantity.TabIndex = 1;
            this.lblDatabaseQuantity.Text = "Current database value:";
            // 
            // lblDatabaseQuantityValue
            // 
            this.lblDatabaseQuantityValue.AutoSize = true;
            this.lblDatabaseQuantityValue.Font = new System.Drawing.Font(
                "Microsoft Sans Serif",
                8F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblDatabaseQuantityValue.Location = new System.Drawing.Point(216, 79);
            this.lblDatabaseQuantityValue.Name = "lblDatabaseQuantityValue";
            this.lblDatabaseQuantityValue.Size = new System.Drawing.Size(19, 20);
            this.lblDatabaseQuantityValue.TabIndex = 2;
            this.lblDatabaseQuantityValue.Text = "0";
            // 
            // lblUserQuantity
            // 
            this.lblUserQuantity.AutoSize = true;
            this.lblUserQuantity.Location = new System.Drawing.Point(18, 116);
            this.lblUserQuantity.Name = "lblUserQuantity";
            this.lblUserQuantity.Size = new System.Drawing.Size(143, 20);
            this.lblUserQuantity.TabIndex = 3;
            this.lblUserQuantity.Text = "Your entered value:";
            // 
            // lblUserQuantityValue
            // 
            this.lblUserQuantityValue.AutoSize = true;
            this.lblUserQuantityValue.Font = new System.Drawing.Font(
                "Microsoft Sans Serif",
                8F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.lblUserQuantityValue.Location = new System.Drawing.Point(216, 116);
            this.lblUserQuantityValue.Name = "lblUserQuantityValue";
            this.lblUserQuantityValue.Size = new System.Drawing.Size(19, 20);
            this.lblUserQuantityValue.TabIndex = 4;
            this.lblUserQuantityValue.Text = "0";
            // 
            // btnRetryMyValue
            // 
            this.btnRetryMyValue.Location = new System.Drawing.Point(18, 171);
            this.btnRetryMyValue.Name = "btnRetryMyValue";
            this.btnRetryMyValue.Size = new System.Drawing.Size(148, 42);
            this.btnRetryMyValue.TabIndex = 5;
            this.btnRetryMyValue.Text = "Retry My Value";
            this.btnRetryMyValue.UseVisualStyleBackColor = true;
            this.btnRetryMyValue.Click += new System.EventHandler(
                this.BtnRetryMyValue_Click);
            // 
            // btnUseDatabaseValue
            // 
            this.btnUseDatabaseValue.Location = new System.Drawing.Point(172, 171);
            this.btnUseDatabaseValue.Name = "btnUseDatabaseValue";
            this.btnUseDatabaseValue.Size = new System.Drawing.Size(192, 42);
            this.btnUseDatabaseValue.TabIndex = 6;
            this.btnUseDatabaseValue.Text = "Use Database Value";
            this.btnUseDatabaseValue.UseVisualStyleBackColor = true;
            this.btnUseDatabaseValue.Click += new System.EventHandler(
                this.BtnUseDatabaseValue_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(370, 171);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(126, 42);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(
                this.BtnCancel_Click);
            // 
            // UpdateStockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(518, 235);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUseDatabaseValue);
            this.Controls.Add(this.btnRetryMyValue);
            this.Controls.Add(this.lblUserQuantityValue);
            this.Controls.Add(this.lblUserQuantity);
            this.Controls.Add(this.lblDatabaseQuantityValue);
            this.Controls.Add(this.lblDatabaseQuantity);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle =
                System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateStockForm";
            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Stock Conflict Resolution";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblDatabaseQuantity;
        private System.Windows.Forms.Label lblDatabaseQuantityValue;
        private System.Windows.Forms.Label lblUserQuantity;
        private System.Windows.Forms.Label lblUserQuantityValue;
        private System.Windows.Forms.Button btnRetryMyValue;
        private System.Windows.Forms.Button btnUseDatabaseValue;
        private System.Windows.Forms.Button btnCancel;
    }
}
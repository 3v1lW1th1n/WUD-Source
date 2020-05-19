namespace WUD
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class formManage : Form
    {
        private Button buttonDelete;
        private DataGridViewTextBoxColumn columnLastUpdated;
        private DataGridViewTextBoxColumn columnProduct;
        private IContainer components;
        private DataGridView gridviewUpdateLists;

        public formManage()
        {
            this.InitializeComponent();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if ((this.gridviewUpdateLists.SelectedRows.Count > 0) && (MessageBox.Show("Are you sure you want to delete these update lists?", "Windows Updates Downloader", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                for (int i = 0; i < this.gridviewUpdateLists.SelectedRows.Count; i++)
                {
                    File.Delete(this.gridviewUpdateLists.SelectedRows[i].Tag.ToString());
                }
                Program.ULManager = new UpdateListManager(Path.GetDirectoryName(Program.SettingsPath));
                this.gridviewUpdateLists.Rows.Clear();
                for (int j = 0; j < Program.ULManager.Files.Count; j++)
                {
                    this.gridviewUpdateLists.Rows.Add(new object[] { Program.ULManager.Files[j].Product + " " + Program.ULManager.Files[j].Platform + " " + Program.ULManager.Files[j].Language, Program.ULManager.Files[j].LastUpdate.ToString("yyyy-MM-dd") });
                    this.gridviewUpdateLists.Rows[this.gridviewUpdateLists.Rows.Count - 1].Tag = Program.ULManager.Files[j].Filename;
                }
                this.gridviewUpdateLists.PerformLayout();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void formManage_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Program.ULManager.Files.Count; i++)
            {
                if (Program.ULManager.Files[i].ServicePack == "any")
                {
                    this.gridviewUpdateLists.Rows.Add(new object[] { Program.ULManager.Files[i].Product + " " + Program.ULManager.Files[i].Platform + " " + Program.ULManager.Files[i].Language, Program.ULManager.Files[i].LastUpdate.ToString("yyyy-MM-dd") });
                }
                else
                {
                    this.gridviewUpdateLists.Rows.Add(new object[] { Program.ULManager.Files[i].Product + " " + Program.ULManager.Files[i].ServicePack + " " + Program.ULManager.Files[i].Platform + " " + Program.ULManager.Files[i].Language, Program.ULManager.Files[i].LastUpdate.ToString("yyyy-MM-dd") });
                }
                this.gridviewUpdateLists.Rows[this.gridviewUpdateLists.Rows.Count - 1].Tag = Program.ULManager.Files[i].Filename;
            }
        }

        private void InitializeComponent()
        {
            this.gridviewUpdateLists = new DataGridView();
            this.columnProduct = new DataGridViewTextBoxColumn();
            this.columnLastUpdated = new DataGridViewTextBoxColumn();
            this.buttonDelete = new Button();
            ((ISupportInitialize) this.gridviewUpdateLists).BeginInit();
            base.SuspendLayout();
            this.gridviewUpdateLists.AllowUserToAddRows = false;
            this.gridviewUpdateLists.AllowUserToDeleteRows = false;
            this.gridviewUpdateLists.AllowUserToResizeRows = false;
            this.gridviewUpdateLists.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridviewUpdateLists.Columns.AddRange(new DataGridViewColumn[] { this.columnProduct, this.columnLastUpdated });
            this.gridviewUpdateLists.Location = new Point(12, 12);
            this.gridviewUpdateLists.Name = "gridviewUpdateLists";
            this.gridviewUpdateLists.ReadOnly = true;
            this.gridviewUpdateLists.RowHeadersVisible = false;
            this.gridviewUpdateLists.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.gridviewUpdateLists.Size = new Size(0x1cb, 0xa9);
            this.gridviewUpdateLists.TabIndex = 0;
            this.columnProduct.HeaderText = "Update List";
            this.columnProduct.Name = "columnProduct";
            this.columnProduct.ReadOnly = true;
            this.columnProduct.Width = 300;
            this.columnLastUpdated.HeaderText = "Last Updated";
            this.columnLastUpdated.Name = "columnLastUpdated";
            this.columnLastUpdated.ReadOnly = true;
            this.columnLastUpdated.Width = 0x7d;
            this.buttonDelete.Location = new Point(0x18c, 0xbb);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new Size(0x4b, 0x17);
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new EventHandler(this.buttonDelete_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1e3, 0xde);
            base.Controls.Add(this.buttonDelete);
            base.Controls.Add(this.gridviewUpdateLists);
            this.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "formManage";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Manage Update Lists";
            base.Load += new EventHandler(this.formManage_Load);
            ((ISupportInitialize) this.gridviewUpdateLists).EndInit();
            base.ResumeLayout(false);
        }
    }
}


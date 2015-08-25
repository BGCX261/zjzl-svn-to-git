namespace zjzl
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.参数设置ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.重新登录ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxMaterialLevel = new System.Windows.Forms.ComboBox();
            this.comboBoxMaterialName = new System.Windows.Forms.ComboBox();
            this.textBoxQuantity = new System.Windows.Forms.TextBox();
            this.textBoxPrice = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxCash = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCustomerTag = new System.Windows.Forms.TextBox();
            this.buttonMore = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.customerID = new System.Windows.Forms.ColumnHeader();
            this.material = new System.Windows.Forms.ColumnHeader();
            this.level = new System.Windows.Forms.ColumnHeader();
            this.price = new System.Windows.Forms.ColumnHeader();
            this.quantity = new System.Windows.Forms.ColumnHeader();
            this.cash = new System.Windows.Forms.ColumnHeader();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.参数设置ToolStripMenuItem1,
            this.重新登录ToolStripMenuItem1,
            this.退出ToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(674, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 参数设置ToolStripMenuItem1
            // 
            this.参数设置ToolStripMenuItem1.Name = "参数设置ToolStripMenuItem1";
            this.参数设置ToolStripMenuItem1.Size = new System.Drawing.Size(80, 20);
            this.参数设置ToolStripMenuItem1.Text = "参数设置";
            // 
            // 重新登录ToolStripMenuItem1
            // 
            this.重新登录ToolStripMenuItem1.Name = "重新登录ToolStripMenuItem1";
            this.重新登录ToolStripMenuItem1.Size = new System.Drawing.Size(80, 20);
            this.重新登录ToolStripMenuItem1.Text = "重新登录";
            // 
            // 退出ToolStripMenuItem1
            // 
            this.退出ToolStripMenuItem1.Name = "退出ToolStripMenuItem1";
            this.退出ToolStripMenuItem1.Size = new System.Drawing.Size(50, 20);
            this.退出ToolStripMenuItem1.Text = "退出";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.comboBoxMaterialLevel, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxMaterialName, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxQuantity, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxPrice, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxCash, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxCustomerTag, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonMore, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 148);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.93103F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62.06897F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(651, 59);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // comboBoxMaterialLevel
            // 
            this.comboBoxMaterialLevel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxMaterialLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMaterialLevel.FormattingEnabled = true;
            this.comboBoxMaterialLevel.Location = new System.Drawing.Point(308, 30);
            this.comboBoxMaterialLevel.Name = "comboBoxMaterialLevel";
            this.comboBoxMaterialLevel.Size = new System.Drawing.Size(72, 20);
            this.comboBoxMaterialLevel.TabIndex = 3;
            this.comboBoxMaterialLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxMaterialLevel_SelectedIndexChanged);
            // 
            // comboBoxMaterialName
            // 
            this.comboBoxMaterialName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxMaterialName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMaterialName.FormattingEnabled = true;
            this.comboBoxMaterialName.Location = new System.Drawing.Point(217, 30);
            this.comboBoxMaterialName.Name = "comboBoxMaterialName";
            this.comboBoxMaterialName.Size = new System.Drawing.Size(84, 20);
            this.comboBoxMaterialName.TabIndex = 2;
            this.comboBoxMaterialName.SelectedIndexChanged += new System.EventHandler(this.comboBoxMaterialName_SelectedIndexChanged);
            // 
            // textBoxQuantity
            // 
            this.textBoxQuantity.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxQuantity.Location = new System.Drawing.Point(468, 30);
            this.textBoxQuantity.Name = "textBoxQuantity";
            this.textBoxQuantity.Size = new System.Drawing.Size(82, 21);
            this.textBoxQuantity.TabIndex = 5;
            this.textBoxQuantity.Leave += new System.EventHandler(this.textBoxQuantity_Leave);
            // 
            // textBoxPrice
            // 
            this.textBoxPrice.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxPrice.Location = new System.Drawing.Point(387, 30);
            this.textBoxPrice.Name = "textBoxPrice";
            this.textBoxPrice.Size = new System.Drawing.Size(74, 21);
            this.textBoxPrice.TabIndex = 4;
            this.textBoxPrice.Leave += new System.EventHandler(this.textBoxPrice_Leave);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(494, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "数量";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(409, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "单价";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(317, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "物品等级";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(232, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "物品名称";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(587, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "金额";
            // 
            // textBoxCash
            // 
            this.textBoxCash.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxCash.Location = new System.Drawing.Point(560, 30);
            this.textBoxCash.Name = "textBoxCash";
            this.textBoxCash.ReadOnly = true;
            this.textBoxCash.Size = new System.Drawing.Size(83, 21);
            this.textBoxCash.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Location = new System.Drawing.Point(80, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "会员编号";
            // 
            // textBoxCustomerTag
            // 
            this.textBoxCustomerTag.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxCustomerTag.Location = new System.Drawing.Point(4, 30);
            this.textBoxCustomerTag.Name = "textBoxCustomerTag";
            this.textBoxCustomerTag.Size = new System.Drawing.Size(179, 21);
            this.textBoxCustomerTag.TabIndex = 0;
            // 
            // buttonMore
            // 
            this.buttonMore.Location = new System.Drawing.Point(190, 26);
            this.buttonMore.Name = "buttonMore";
            this.buttonMore.Size = new System.Drawing.Size(20, 23);
            this.buttonMore.TabIndex = 1;
            this.buttonMore.Text = "..";
            this.buttonMore.UseVisualStyleBackColor = true;
            this.buttonMore.Click += new System.EventHandler(this.buttonMore_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(317, 222);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "提交 &C";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(488, 222);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(74, 23);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.Text = "重置 &R";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.customerID,
            this.material,
            this.level,
            this.price,
            this.quantity,
            this.cash});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 37);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(651, 97);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // customerID
            // 
            this.customerID.Text = "会员编号";
            this.customerID.Width = 210;
            // 
            // material
            // 
            this.material.Text = "物品名称";
            this.material.Width = 90;
            // 
            // level
            // 
            this.level.Text = "物品等级";
            this.level.Width = 80;
            // 
            // price
            // 
            this.price.Text = "单价";
            this.price.Width = 80;
            // 
            // quantity
            // 
            this.quantity.Text = "数量";
            this.quantity.Width = 90;
            // 
            // cash
            // 
            this.cash.Text = "金额";
            this.cash.Width = 90;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 267);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "pur";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 参数设置ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 重新登录ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxCustomerTag;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.TextBox textBoxQuantity;
        private System.Windows.Forms.TextBox textBoxPrice;
        private System.Windows.Forms.ComboBox comboBoxMaterialLevel;
        private System.Windows.Forms.ComboBox comboBoxMaterialName;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader customerID;
        private System.Windows.Forms.ColumnHeader material;
        private System.Windows.Forms.ColumnHeader level;
        private System.Windows.Forms.ColumnHeader price;
        private System.Windows.Forms.ColumnHeader quantity;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxCash;
        private System.Windows.Forms.ColumnHeader cash;
        private System.Windows.Forms.Button buttonMore;
    }
}


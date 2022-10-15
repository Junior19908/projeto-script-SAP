namespace SistemaGSG.Almoxarifado
{
    partial class FormRelatorioGeralPorFornecedor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRelatorioGeralPorFornecedor));
            this.ZMM039 = new System.Windows.Forms.DataGridView();
            this.btnRelatorio = new System.Windows.Forms.Button();
            this.calendarioMes = new System.Windows.Forms.MonthCalendar();
            this.btnRC = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ZMM039)).BeginInit();
            this.SuspendLayout();
            // 
            // ZMM039
            // 
            this.ZMM039.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ZMM039.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ZMM039.Location = new System.Drawing.Point(12, 211);
            this.ZMM039.Name = "ZMM039";
            this.ZMM039.Size = new System.Drawing.Size(232, 0);
            this.ZMM039.TabIndex = 0;
            this.ZMM039.Visible = false;
            // 
            // btnRelatorio
            // 
            this.btnRelatorio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRelatorio.Location = new System.Drawing.Point(169, 184);
            this.btnRelatorio.Name = "btnRelatorio";
            this.btnRelatorio.Size = new System.Drawing.Size(75, 23);
            this.btnRelatorio.TabIndex = 1;
            this.btnRelatorio.Text = "&Relatório";
            this.btnRelatorio.UseVisualStyleBackColor = true;
            this.btnRelatorio.Click += new System.EventHandler(this.btnRelatorio_Click);
            // 
            // calendarioMes
            // 
            this.calendarioMes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.calendarioMes.Location = new System.Drawing.Point(18, 18);
            this.calendarioMes.Name = "calendarioMes";
            this.calendarioMes.TabIndex = 0;
            // 
            // btnRC
            // 
            this.btnRC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRC.Location = new System.Drawing.Point(88, 184);
            this.btnRC.Name = "btnRC";
            this.btnRC.Size = new System.Drawing.Size(75, 23);
            this.btnRC.TabIndex = 2;
            this.btnRC.Text = "&Correções";
            this.btnRC.UseVisualStyleBackColor = true;
            this.btnRC.Click += new System.EventHandler(this.btnRC_Click);
            // 
            // FormRelatorioGeralPorFornecedor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 218);
            this.ControlBox = false;
            this.Controls.Add(this.btnRC);
            this.Controls.Add(this.calendarioMes);
            this.Controls.Add(this.btnRelatorio);
            this.Controls.Add(this.ZMM039);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRelatorioGeralPorFornecedor";
            this.Style = MetroFramework.MetroColorStyle.Orange;
            this.Load += new System.EventHandler(this.FormRelatorioGeralPorFornecedor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ZMM039)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ZMM039;
        private System.Windows.Forms.Button btnRelatorio;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.MonthCalendar calendarioMes;
        private System.Windows.Forms.Button btnRC;
    }
}
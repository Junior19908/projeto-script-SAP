namespace SistemaGSG
{
    partial class FormSolicitacao
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSolicitacao));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgBar = new System.Windows.Forms.ToolStripProgressBar();
            this.lblSolicitacao = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblConexao = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusSAP = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataGridViewSolc = new System.Windows.Forms.DataGridView();
            this.ID_TB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RESERVA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATARESERVA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USERSOLIC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FINALIZADO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExecTemp = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.Hora = new System.Windows.Forms.Timer(this.components);
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataReserva2 = new System.Windows.Forms.DateTimePicker();
            this.txtUsuarioSAP = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtRecebedor = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDeposito = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCentroCusto = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtUMB = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtQuantPedida = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMaterial = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTpMovimento = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtItemReserva = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtReserva = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dataGridViewBaixaSAP = new System.Windows.Forms.DataGridView();
            this.txtUsuarioBalcao = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gerarRelatórioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSolc)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBaixaSAP)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.ProgBar,
            this.lblSolicitacao,
            this.lblConexao,
            this.StatusSAP});
            this.statusStrip1.Location = new System.Drawing.Point(20, 613);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(866, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(92, 17);
            this.toolStripStatusLabel1.Text = "SistmeaGSGV2.0";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // ProgBar
            // 
            this.ProgBar.Name = "ProgBar";
            this.ProgBar.Size = new System.Drawing.Size(100, 16);
            this.ProgBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // lblSolicitacao
            // 
            this.lblSolicitacao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblSolicitacao.Name = "lblSolicitacao";
            this.lblSolicitacao.Size = new System.Drawing.Size(142, 17);
            this.lblSolicitacao.Text = "Aguardando Solicitação...";
            this.lblSolicitacao.Visible = false;
            // 
            // lblConexao
            // 
            this.lblConexao.ForeColor = System.Drawing.Color.Red;
            this.lblConexao.Name = "lblConexao";
            this.lblConexao.Size = new System.Drawing.Size(172, 17);
            this.lblConexao.Text = "Sem Conexão com o Servidor...";
            this.lblConexao.Visible = false;
            // 
            // StatusSAP
            // 
            this.StatusSAP.ForeColor = System.Drawing.Color.Purple;
            this.StatusSAP.Name = "StatusSAP";
            this.StatusSAP.Size = new System.Drawing.Size(88, 17);
            this.StatusSAP.Text = "SAP Fechado!...";
            this.StatusSAP.Visible = false;
            // 
            // dataGridViewSolc
            // 
            this.dataGridViewSolc.AllowUserToAddRows = false;
            this.dataGridViewSolc.AllowUserToDeleteRows = false;
            this.dataGridViewSolc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSolc.BackgroundColor = System.Drawing.Color.NavajoWhite;
            this.dataGridViewSolc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSolc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID_TB,
            this.RESERVA,
            this.DATARESERVA,
            this.USERSOLIC,
            this.FINALIZADO});
            this.dataGridViewSolc.Location = new System.Drawing.Point(8, 19);
            this.dataGridViewSolc.Name = "dataGridViewSolc";
            this.dataGridViewSolc.ReadOnly = true;
            this.dataGridViewSolc.Size = new System.Drawing.Size(869, 140);
            this.dataGridViewSolc.TabIndex = 1;
            // 
            // ID_TB
            // 
            this.ID_TB.DataPropertyName = "ID_TB";
            this.ID_TB.HeaderText = "ID_TB";
            this.ID_TB.Name = "ID_TB";
            this.ID_TB.ReadOnly = true;
            this.ID_TB.Visible = false;
            // 
            // RESERVA
            // 
            this.RESERVA.DataPropertyName = "RESERVA";
            this.RESERVA.HeaderText = "RESERVA";
            this.RESERVA.Name = "RESERVA";
            this.RESERVA.ReadOnly = true;
            // 
            // DATARESERVA
            // 
            this.DATARESERVA.DataPropertyName = "DATARESERVA";
            this.DATARESERVA.HeaderText = "DATA RESERVA";
            this.DATARESERVA.Name = "DATARESERVA";
            this.DATARESERVA.ReadOnly = true;
            this.DATARESERVA.Width = 150;
            // 
            // USERSOLIC
            // 
            this.USERSOLIC.DataPropertyName = "USERSOLIC";
            this.USERSOLIC.HeaderText = "USUÁRIO SOLIC";
            this.USERSOLIC.Name = "USERSOLIC";
            this.USERSOLIC.ReadOnly = true;
            this.USERSOLIC.Width = 150;
            // 
            // FINALIZADO
            // 
            this.FINALIZADO.DataPropertyName = "FINALIZADO";
            this.FINALIZADO.HeaderText = "FINALIZADO";
            this.FINALIZADO.Name = "FINALIZADO";
            this.FINALIZADO.ReadOnly = true;
            this.FINALIZADO.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ExecTemp
            // 
            this.ExecTemp.Enabled = true;
            this.ExecTemp.Interval = 1000;
            this.ExecTemp.Tick += new System.EventHandler(this.ExecTemp_Tick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(734, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Data Hora: 00/00/000 00:00:00";
            // 
            // Hora
            // 
            this.Hora.Enabled = true;
            this.Hora.Interval = 1000;
            this.Hora.Tick += new System.EventHandler(this.Hora_Tick);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Enabled = false;
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker.Location = new System.Drawing.Point(212, 41);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(99, 20);
            this.dateTimePicker.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dataGridViewSolc);
            this.groupBox1.Location = new System.Drawing.Point(12, 442);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(881, 165);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Solicitações Balcão";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(13, 271);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(881, 165);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Reservas Solicitadas";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.Color.NavajoWhite;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(869, 140);
            this.dataGridView1.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.dataReserva2);
            this.groupBox3.Controls.Add(this.txtUsuarioSAP);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.dateTimePicker);
            this.groupBox3.Controls.Add(this.txtRecebedor);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.txtDeposito);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtCentroCusto);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtUMB);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtQuantPedida);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtMaterial);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtTpMovimento);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txtItemReserva);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtReserva);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(13, 47);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(881, 14);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            // 
            // dataReserva2
            // 
            this.dataReserva2.CustomFormat = "yyyy-MM-dd";
            this.dataReserva2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dataReserva2.Location = new System.Drawing.Point(106, 25);
            this.dataReserva2.Name = "dataReserva2";
            this.dataReserva2.Size = new System.Drawing.Size(100, 20);
            this.dataReserva2.TabIndex = 44;
            // 
            // txtUsuarioSAP
            // 
            this.txtUsuarioSAP.Location = new System.Drawing.Point(477, 40);
            this.txtUsuarioSAP.Name = "txtUsuarioSAP";
            this.txtUsuarioSAP.Size = new System.Drawing.Size(100, 20);
            this.txtUsuarioSAP.TabIndex = 43;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(371, 43);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 13);
            this.label11.TabIndex = 42;
            this.label11.Text = "Usuário";
            // 
            // txtRecebedor
            // 
            this.txtRecebedor.Location = new System.Drawing.Point(477, 25);
            this.txtRecebedor.Name = "txtRecebedor";
            this.txtRecebedor.Size = new System.Drawing.Size(100, 20);
            this.txtRecebedor.TabIndex = 41;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(371, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 40;
            this.label10.Text = "Recebedor";
            // 
            // txtDeposito
            // 
            this.txtDeposito.Location = new System.Drawing.Point(477, -21);
            this.txtDeposito.Name = "txtDeposito";
            this.txtDeposito.Size = new System.Drawing.Size(100, 20);
            this.txtDeposito.TabIndex = 39;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(371, -18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 38;
            this.label9.Text = "Depósito";
            // 
            // txtCentroCusto
            // 
            this.txtCentroCusto.Location = new System.Drawing.Point(477, -38);
            this.txtCentroCusto.Name = "txtCentroCusto";
            this.txtCentroCusto.Size = new System.Drawing.Size(100, 20);
            this.txtCentroCusto.TabIndex = 37;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(371, -35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Centro de Custo";
            // 
            // txtUMB
            // 
            this.txtUMB.Location = new System.Drawing.Point(106, 118);
            this.txtUMB.Name = "txtUMB";
            this.txtUMB.Size = new System.Drawing.Size(100, 20);
            this.txtUMB.TabIndex = 35;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Únidade de Médida";
            // 
            // txtQuantPedida
            // 
            this.txtQuantPedida.Location = new System.Drawing.Point(106, 92);
            this.txtQuantPedida.Name = "txtQuantPedida";
            this.txtQuantPedida.Size = new System.Drawing.Size(100, 20);
            this.txtQuantPedida.TabIndex = 33;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "Quantidade Pedida";
            // 
            // txtMaterial
            // 
            this.txtMaterial.Location = new System.Drawing.Point(106, 66);
            this.txtMaterial.Name = "txtMaterial";
            this.txtMaterial.Size = new System.Drawing.Size(100, 20);
            this.txtMaterial.TabIndex = 31;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Material";
            // 
            // txtTpMovimento
            // 
            this.txtTpMovimento.Location = new System.Drawing.Point(106, 40);
            this.txtTpMovimento.Name = "txtTpMovimento";
            this.txtTpMovimento.Size = new System.Drawing.Size(100, 20);
            this.txtTpMovimento.TabIndex = 29;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Tipo Movimento";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Data Reserva";
            // 
            // txtItemReserva
            // 
            this.txtItemReserva.Location = new System.Drawing.Point(106, -21);
            this.txtItemReserva.Name = "txtItemReserva";
            this.txtItemReserva.Size = new System.Drawing.Size(100, 20);
            this.txtItemReserva.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, -18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Item Reserva";
            // 
            // txtReserva
            // 
            this.txtReserva.Location = new System.Drawing.Point(106, -38);
            this.txtReserva.Name = "txtReserva";
            this.txtReserva.Size = new System.Drawing.Size(100, 20);
            this.txtReserva.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(0, -35);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 13);
            this.label12.TabIndex = 23;
            this.label12.Text = "Reserva";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.dataGridViewBaixaSAP);
            this.groupBox4.Location = new System.Drawing.Point(13, 100);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(881, 165);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Baixa no SAP";
            // 
            // dataGridViewBaixaSAP
            // 
            this.dataGridViewBaixaSAP.AllowUserToAddRows = false;
            this.dataGridViewBaixaSAP.AllowUserToDeleteRows = false;
            this.dataGridViewBaixaSAP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewBaixaSAP.BackgroundColor = System.Drawing.Color.NavajoWhite;
            this.dataGridViewBaixaSAP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBaixaSAP.Location = new System.Drawing.Point(6, 19);
            this.dataGridViewBaixaSAP.Name = "dataGridViewBaixaSAP";
            this.dataGridViewBaixaSAP.ReadOnly = true;
            this.dataGridViewBaixaSAP.Size = new System.Drawing.Size(869, 140);
            this.dataGridViewBaixaSAP.TabIndex = 1;
            // 
            // txtUsuarioBalcao
            // 
            this.txtUsuarioBalcao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsuarioBalcao.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtUsuarioBalcao.Enabled = false;
            this.txtUsuarioBalcao.Location = new System.Drawing.Point(794, 83);
            this.txtUsuarioBalcao.Name = "txtUsuarioBalcao";
            this.txtUsuarioBalcao.Size = new System.Drawing.Size(100, 20);
            this.txtUsuarioBalcao.TabIndex = 9;
            this.txtUsuarioBalcao.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(694, 86);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(94, 13);
            this.label13.TabIndex = 10;
            this.label13.Text = "Última Solicitação:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(20, 60);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(866, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gerarRelatórioToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // gerarRelatórioToolStripMenuItem
            // 
            this.gerarRelatórioToolStripMenuItem.Name = "gerarRelatórioToolStripMenuItem";
            this.gerarRelatórioToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.gerarRelatórioToolStripMenuItem.Text = "Gerar Relatório";
            this.gerarRelatórioToolStripMenuItem.Click += new System.EventHandler(this.gerarRelatórioToolStripMenuItem_Click);
            // 
            // FormSolicitacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(906, 655);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtUsuarioBalcao);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSolicitacao";
            this.Style = MetroFramework.MetroColorStyle.Brown;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormSolicitacao_FormClosed);
            this.Load += new System.EventHandler(this.FormSolicitacao_Load);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FormSolicitacao_MouseDoubleClick);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSolc)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBaixaSAP)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.DataGridView dataGridViewSolc;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID_TB;
        private System.Windows.Forms.DataGridViewTextBoxColumn RESERVA;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATARESERVA;
        private System.Windows.Forms.DataGridViewTextBoxColumn USERSOLIC;
        private System.Windows.Forms.DataGridViewTextBoxColumn FINALIZADO;
        private System.Windows.Forms.ToolStripProgressBar ProgBar;
        private System.Windows.Forms.ToolStripStatusLabel lblSolicitacao;
        private System.Windows.Forms.ToolStripStatusLabel lblConexao;
        private System.Windows.Forms.Timer ExecTemp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer Hora;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DateTimePicker dataReserva2;
        private System.Windows.Forms.TextBox txtUsuarioSAP;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtRecebedor;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDeposito;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCentroCusto;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtUMB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtQuantPedida;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMaterial;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTpMovimento;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtItemReserva;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtReserva;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ToolStripStatusLabel StatusSAP;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dataGridViewBaixaSAP;
        private System.Windows.Forms.TextBox txtUsuarioBalcao;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gerarRelatórioToolStripMenuItem;
    }
}
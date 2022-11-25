using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SAPFEWSELib;
using SapROTWr;
using System.IO;
using static System.Collections.Specialized.BitVector32;
using SistemaGSG.Almoxarifado;

namespace SistemaGSG
{
    public partial class FormSolicitacao : MetroFramework.Forms.MetroForm
    {
        public FormSolicitacao()
        {
            InitializeComponent();
        }
        DataTable table = new DataTable();

        private void atualizahora()
        {
            label1.Text = "Data Hora: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
        private void FormSolicitacao_Load(object sender, EventArgs e)
        {
            CarregarGridTable();
            DataGridBaixaSAP();
        }
        private void CarregarGridTable()
        {
            table.Columns.Add("-*-", typeof(string));
            table.Columns.Add("RESERVA", typeof(string));
            table.Columns.Add("ITEM", typeof(string));
            table.Columns.Add("--", typeof(string));
            table.Columns.Add("DATA RESERVA", typeof(string));
            table.Columns.Add("TIPO MOVIMENTO", typeof(string));
            table.Columns.Add("MATERIAL", typeof(string));
            table.Columns.Add("QUANTIDADE", typeof(string));
            table.Columns.Add("ÚNIDADE MÉDIDA", typeof(string));
            table.Columns.Add("CENTRO DE CUSTO", typeof(string));
            table.Columns.Add("DEPOSITO", typeof(string));
            table.Columns.Add("------", typeof(string));
            table.Columns.Add("RECEBEDOR", typeof(string));
            table.Columns.Add("--------", typeof(string));
            table.Columns.Add("USER", typeof(string));
            table.Columns.Add("*-", typeof(string));
            dataGridView1.DataSource = table;
        }
        private void DataGridBaixaSAP()
        {
            try
            {
                MySqlDataAdapter BaixaSAP = new MySqlDataAdapter("SELECT * FROM  tb_almox_reserva WHERE TPNEC = 0", ConexaoDados.GetConnectionAlmoxarifado());
                DataTable Baixa = new DataTable();
                BaixaSAP.Fill(Baixa);
                dataGridViewBaixaSAP.DataSource = Baixa;
            }
            catch
            {

            }
            finally
            {
                ConexaoDados.GetConnectionAlmoxarifado().Close();
            }
        }
        private void CarregarDataGrid()
        {
            try
            {
                try
                {
                    MySqlDataAdapter Solicitacao = new MySqlDataAdapter("SELECT * FROM  tb_almox_solicitacao WHERE FINALIZADO != 1", ConexaoDados.GetConnectionAlmoxarifado());
                    DataTable Solict = new DataTable();
                    Solicitacao.Fill(Solict);
                    dataGridViewSolc.DataSource = Solict;
                    lblConexao.Visible = false;
                }
                catch
                {

                }

                int Countag = dataGridViewSolc.RowCount;
                if (Countag == 0)
                {

                }
                else
                {
                    txtUsuarioBalcao.Text = dataGridViewSolc.Rows[0].Cells[3].Value.ToString();
                    if (txtUsuarioBalcao.Text == "25")
                    {
                        txtUsuarioBalcao.Text = "Luana";
                    }
                    if (txtUsuarioBalcao.Text == "1")
                    {
                        txtUsuarioBalcao.Text = "Júnior";
                    }
                }
            }
            catch (MySqlException)
            {
                lblConexao.Visible = true;
            }
            finally
            {
                ConexaoDados.GetConnectionAlmoxarifado().Close();
            }
            int countg = dataGridViewSolc.RowCount;
            if (countg == 0)
            {
                lblSolicitacao.Visible = true;
            }
            else
            {
                SolicitacaoSAP();
                if (StatusSAP.Visible == true)
                {

                }
                else
                {
                    lblSolicitacao.Visible = false;
                    ImportarTXT();
                    PreencherTextBox();
                    UpdateStatusFinalizado();
                }
            }
        }
        private void SolicitacaoSAP()
        {
            try
            {
                //Pega a tela de execução do Windows
                CSapROTWrapper sapROTWrapper = new CSapROTWrapper();
                //Pega a entrada ROT para o SAP Gui para conectar-se ao COM
                object SapGuilRot = sapROTWrapper.GetROTEntry("SAPGUI");
                //Pega a referência de Scripting Engine do SAP
                object engine = SapGuilRot.GetType().InvokeMember("GetScriptingEngine", System.Reflection.BindingFlags.InvokeMethod, null, SapGuilRot, null);
                //Pega a referência da janela de aplicativos em execução no SAP
                GuiApplication GuiApp = (GuiApplication)engine;
                //Pega a primeira conexão aberta do SAP
                GuiConnection connection = (GuiConnection)GuiApp.Connections.ElementAt(0);
                //Pega a primeira sessão aberta
                GuiSession Session = (GuiSession)connection.Children.ElementAt(0);
                //Pega a referência ao "FRAME" principal para enviar comandos de chaves virtuais o SAP
                GuiFrameWindow guiWindow = Session.ActiveWindow;
                //Maximisa Janela
                guiWindow.Maximize();
                //Abre Transação

                Session.SendCommand("/NMB23");
                //Inicia a Barra de Progresso em 25%
                ProgBar.Value = 15;
                //Tecla Enter
                guiWindow.SendVKey(0);
                guiWindow.SendVKey(4);

                dateTimePicker.Text = dataGridViewSolc.Rows[0].Cells[2].Value.ToString();
                dateTimePicker.Format = DateTimePickerFormat.Custom;
                dateTimePicker.CustomFormat = "dd.MM.yyyy";

                ((GuiTextField)Session.FindById("wnd[1]/usr/ctxtRM07M-WERKS")).Text = "USGA";
                ((GuiTextField)Session.FindById("wnd[1]/usr/ctxtRM07M-BDTER")).Text = this.dateTimePicker.Text;
                ((GuiCheckBox)Session.FindById("wnd[1]/usr/chkRM07M-KZEAR")).Selected = false;
                ProgBar.Value = 35;
                ((GuiButton)Session.FindById("wnd[1]/tbar[0]/btn[0]")).Press();
                ((GuiMenu)Session.FindById("wnd[0]/mbar/menu[3]/menu[2]/menu[1]")).Select();
                ProgBar.Value = 45;
                ((GuiGridView)Session.FindById("wnd[1]/usr/ssubD0500_SUBSCREEN:SAPLSLVC_DIALOG:0501/cntlG51_CONTAINER/shellcont/shell")).SetCurrentCell(4, "TEXT");
                ((GuiGridView)Session.FindById("wnd[1]/usr/ssubD0500_SUBSCREEN:SAPLSLVC_DIALOG:0501/cntlG51_CONTAINER/shellcont/shell")).SelectedRows = "4";
                ((GuiGridView)Session.FindById("wnd[1]/usr/ssubD0500_SUBSCREEN:SAPLSLVC_DIALOG:0501/cntlG51_CONTAINER/shellcont/shell")).ClickCurrentCell();
                ((GuiMenu)Session.FindById("wnd[0]/mbar/menu[0]/menu[1]/menu[2]")).Select();
                ((GuiButton)Session.FindById("wnd[1]/tbar[0]/btn[0]")).Press();
                ProgBar.Value = 75;
                ((GuiTextField)Session.FindById("wnd[1]/usr/ctxtDY_PATH")).Text = @"C:\ArquivosSAP\";
                ProgBar.Value = 80;
                ((GuiTextField)Session.FindById("wnd[1]/usr/ctxtDY_FILENAME")).Text = "ALMOX.txt";
                ((GuiTextField)Session.FindById("wnd[1]/usr/ctxtDY_FILENAME")).CaretPosition = 5;
                ProgBar.Value = 98;
                ((GuiButton)Session.FindById("wnd[1]/tbar[0]/btn[11]")).Press();
                ProgBar.Value = 100;
                Session.SendCommand("/N");
            }
            catch
            {
                StatusSAP.Visible = true;
                ExecTemp.Enabled = false;
                MessageBox.Show("O programa SAP encontra-se fechado!\n quando estiver pronto me avise!...\n Dê um duplo clique em qualquer parte!");
            }
        }
        private void FormSolicitacao_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void UpdateStatusFinalizado()
        {
            if (ConexaoDados.GetConnectionAlmoxarifado().State == ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE tb_almox_solicitacao SET FINALIZADO='1' WHERE ID_TB='" + dataGridViewSolc.Rows[0].Cells[0].Value.ToString() + "'", ConexaoDados.GetConnectionAlmoxarifado());
                cmd.ExecuteNonQuery();
                ProgBar.Value = 0;
            }
            ConexaoDados.GetConnectionAlmoxarifado().Close();
            table.Rows.Clear();
        }
        private void ExecTemp_Tick(object sender, EventArgs e)
        {
            CarregarDataGrid();
        }
        private void Hora_Tick(object sender, EventArgs e)
        {
            atualizahora();
        }
        private void FormSolicitacao_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ExecTemp.Enabled = true;
            StatusSAP.Visible = false;
        }
        private void ImportarTXT()
        {
            string[] lines = File.ReadAllLines(@"C:\ArquivosSAP\ALMOX.txt");
            string[] values;

            for (int i = 5; i < lines.Length; i++)
            {
                values = lines[i].ToString().Split('|');
                string[] row = new string[values.Length];

                for (int j = 0; j < values.Length; j++)
                {
                    row[j] = values[j].Trim('-');
                }
                table.Rows.Add(row);
            }
        }
        private void PreencherTextBox()
        {
            int countg = dataGridView1.RowCount;
            int numero = 0;
            while (numero < countg)
            {
                try
                {
                    txtReserva.Text = dataGridView1.Rows[numero].Cells[1].Value.ToString().Trim();
                    txtItemReserva.Text = dataGridView1.Rows[numero].Cells[2].Value.ToString().Trim();
                    dataReserva2.Text = dataGridView1.Rows[numero].Cells[4].Value.ToString().Trim();
                    txtTpMovimento.Text = dataGridView1.Rows[numero].Cells[5].Value.ToString().Trim();
                    txtMaterial.Text = dataGridView1.Rows[numero].Cells[6].Value.ToString().Trim();
                    txtQuantPedida.Text = dataGridView1.Rows[numero].Cells[7].Value.ToString().Trim();
                    txtUMB.Text = dataGridView1.Rows[numero].Cells[8].Value.ToString().Trim();
                    txtCentroCusto.Text = dataGridView1.Rows[numero].Cells[9].Value.ToString().Trim();
                    txtDeposito.Text = dataGridView1.Rows[numero].Cells[10].Value.ToString().Trim();
                    txtRecebedor.Text = dataGridView1.Rows[numero].Cells[12].Value.ToString().Trim();
                    txtUsuarioSAP.Text = dataGridView1.Rows[numero].Cells[14].Value.ToString().Trim();
                    ImportarDataGrid();
                    numero++;
                }
                catch
                {
                    break;
                }
            }
        }
        private void ImportarDataGrid()
        {
            try
            {
                if (string.IsNullOrEmpty(txtReserva.Text))
                {

                }
                else
                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO tb_almox_reserva (RESERVA, ITEM, DATA, TPMOVIMETNO, MATERIAL, QTDPEDIDA, QDTSOLICITADA, UMB, CUSTO, DEPOSITO, TPNEC, RECEBEDOR, USUARIOSAP) " +
                        "VALUES " +
                        "('" + txtReserva.Text + "','" + txtItemReserva.Text + "','" + this.dataReserva2.Text + "','" + txtTpMovimento.Text + "','" + txtMaterial.Text + "','" + txtQuantPedida.Text + "','0' ,'" + txtUMB.Text + "','" + txtCentroCusto.Text + "','" + txtDeposito.Text + "','0','" + txtRecebedor.Text + "','" + txtUsuarioSAP.Text + "')", ConexaoDados.GetConnectionAlmoxarifado());
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    MySqlCommand commando = new MySqlCommand("INSERT INTO tb_almox_solicitacao (RESERVA, DATARESERVA, USERSOLIC, FINALIZADO, STATUS) " +
                        "VALUES " +
                        "('" + txtReserva.Text + "','" + this.dataReserva2.Text + "','17', '1','4')", ConexaoDados.GetConnectionAlmoxarifado());
                    commando.ExecuteNonQuery();
                    commando.Connection.Close();
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
            finally
            {
                ConexaoDados.GetConnectionAlmoxarifado().Close();
            }
        }
        private void gerarRelatórioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRelatorioGeralPorFornecedor formSolicitacao = new FormRelatorioGeralPorFornecedor();
            formSolicitacao.ShowDialog();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja Voltar?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frm_Main back = new frm_Main();
                back.Show();
                Close();
            }
        }
    }
}

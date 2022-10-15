using MySql.Data.MySqlClient;
using SAPFEWSELib;
using SapROTWr;
using System;
using System.Data;
using System.Windows.Forms;

namespace SistemaGSG
{
    public partial class FormAcesso : MetroFramework.Forms.MetroForm
    {
        public FormAcesso()
        {
            InitializeComponent();
            UserTXT.Text = dados.Usuario;
            lblUnd.Text = "SACOS";
            UltimNumSacaria();
            MotoristaeCliente();
        }
        String URL1 = ConexaoDados.ACESSO();
        String URL2 = ConexaoDados.CHECKLIST();
        string usuarioLogado = dados.Usuario;
        MySqlCommand MySqlCommand = new MySqlCommand();
        String CCliente, NMotor, NMotorBag, CMotor, CMotorBag, CTransportador, CTransportadorBag;
        double QuantidadeBags;
        double QuantidadeBagsConvertSc;
        private void MotoristaeCliente()
        {
            try
            {
                MySqlCommand.Connection = ConexaoDados.GetConnectionFaturameto();
                MySqlCommand.CommandText = "SELECT * FROM `tb_cliente` ORDER BY `tb_cliente`.`col_nome` ASC ";
                MySqlDataReader dr = MySqlCommand.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                cmbCliente.DataSource = dt;
                cmbCliente.DisplayMember = "col_nome";
                cmbCliente.ValueMember = "col_cod_id";
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message);
            }
        }
        private void UltimNumSacaria()
        {
            try
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = ConexaoDados.GetConnectionFaturameto();
                MyCommand.CommandText = "SELECT * FROM tb_acesso ORDER BY id DESC";
                MySqlDataReader dreader = MyCommand.ExecuteReader();
                while (dreader.Read())
                {
                    if (dreader["col_tipo_sac"].ToString() == "BAG")
                    {

                    }
                    else
                    {
                        txtSacariaInic.Text = dreader["col_sacaria_fim"].ToString();
                        break;
                    }
                }

                MySqlCommand mySqlCommand = new MySqlCommand();
                mySqlCommand.Connection = ConexaoDados.GetConnectionFaturameto();
                mySqlCommand.CommandText = "SELECT col_acesso FROM tb_acesso ORDER BY id DESC LIMIT 1";
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    ultAcesso.Text = mySqlDataReader["col_acesso"].ToString();
                    break;
                }

                ConexaoDados.GetConnectionFaturameto().Close();
                double SomaUm = Convert.ToDouble(txtSacariaInic.Text.Trim()) + 1;
                txtSacariaInic.Text = SomaUm.ToString();
            }
            catch (MySqlException ErroDB)
            {
                MessageBox.Show("Banco de Dados, Desligado! " + ErroDB, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void AbrirPDFs()
        {
            var AbrirNavegador1 = new Navegador(ConexaoDados.CHECKLIST());
            AbrirNavegador1.Show();

            var AbrirNavegador2 = new Navegador(ConexaoDados.ACESSO());
            AbrirNavegador2.Show();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            AbrirPDFs();
        }
        private void SAPAcessoBAG()
        {
            ProgressBar.Value = 0;
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
            //Abre Transação
            Session.SendCommand("/nxk03");
            guiWindow.SendVKey(4);
            ProgressBar.Value = 10;
            MySqlCommand.Connection = ConexaoDados.GetConnectionFaturameto();
            MySqlCommand.CommandText = "SELECT * FROM `tb_motorista` WHERE col_cpf ='" + txtCPF.Text + "';";
            MySqlDataReader dr = MySqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(dr);
            int ContagemMot = dataTable.Rows.Count;
            if (ContagemMot > 0)
            {
                try
                {
                    MySqlDataReader dreader = MySqlCommand.ExecuteReader();
                    while (dreader.Read())
                    {
                        NMotorBag = dreader["col_nome_mot"].ToString();
                        CMotorBag = dreader["col_cod_id"].ToString();
                        CTransportadorBag = dreader["col_transportadora"].ToString();
                        break;
                    }
                    ConexaoDados.GetConnectionFaturameto().Close();
                }
                catch (MySqlException ErroDB)
                {
                    MessageBox.Show("Banco de Dados, Desligado! " + ErroDB, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message);
                }
            }
            else
            {
                Session.SendCommand("/NBP");
                ProgressBar.Value = 10;
                ((GuiMenu)Session.FindById("wnd[0]/mbar/menu[0]/menu[0]")).Select();
                guiWindow.SendVKey(4);
                ((GuiTextField)Session.FindById("wnd[2]/usr/tabsG_SELONETABSTRIP/tabpTAB014/ssubSUBSCR_PRESEL:SAPLSDH4:0220/sub:SAPLSDH4:0220/ctxtG_SELFLD_TAB-LOW[0,24]")).Text = "BR2";
                ((GuiTextField)Session.FindById("wnd[2]/usr/tabsG_SELONETABSTRIP/tabpTAB014/ssubSUBSCR_PRESEL:SAPLSDH4:0220/sub:SAPLSDH4:0220/txtG_SELFLD_TAB-LOW[1,24]")).Text = txtCPF.Text;
                guiWindow.SendVKey(0);
                guiWindow.SendVKey(0);
                guiWindow.SendVKey(0);
                ((GuiComboBox)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/subSCREEN_1100_ROLE_AND_TIME_AREA:SAPLBUPA_DIALOG_JOEL:1110/cmbBUS_JOEL_MAIN-PARTNER_ROLE")).Key = "ZFTERE";
                ((GuiTab)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01")).Select();
                NMotorBag = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7016/subA02P03:SAPLBUD0:1301/txtBUT000-NAME_FIRST")).Text;
                ((GuiTab)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_08")).Select();
                CMotorBag = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_08/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7030/subA02P01:SAPLCVI_FS_UI_VENDOR:0021/txtGV_VENDOR")).Text;
                ((GuiTab)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_09")).Select();
                CTransportadorBag = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_09/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7031/subA04P01:SAPLCVI_FS_UI_VENDOR:0018/ctxtGS_LFA1-FISKU")).Text;
                try
                {
                    if (String.IsNullOrEmpty(CTransportadorBag))
                    {
                        MySqlCommand command = new MySqlCommand("INSERT INTO `tb_motorista` (`col_cod_id`, `col_nome_mot`, `col_cpf`) VALUES ('" + CMotorBag + "', '" + NMotorBag + "', '" + txtCPF.Text + "')", ConexaoDados.GetConnectionFaturameto());
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        MySqlCommand command = new MySqlCommand("INSERT INTO `tb_motorista` (`col_cod_id`, `col_nome_mot`, `col_cpf`, `col_transportadora`) VALUES ('" + CMotorBag + "', '" + NMotorBag + "', '" + txtCPF.Text + "', '" + CTransportadorBag + "')", ConexaoDados.GetConnectionFaturameto());
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message);
                    ProgressBar.Value = 0;
                }
            }
            txtCPF.Visible = false;
            txtCPFCompleto.Visible = true;
            txtCPFCompleto.Text = NMotorBag;
            ProgressBar.Value = 20;
            if (String.IsNullOrEmpty(CTransportadorBag))
            {

            }
            else
            {
                MySqlCommand.Connection = ConexaoDados.GetConnectionFaturameto();
                MySqlCommand.CommandText = "SELECT * FROM `tb_transport` WHERE col_cod_id ='" + CTransportadorBag + "';";
                MySqlDataReader dataReader = MySqlCommand.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);
                int ContagemTransp = dt.Rows.Count;
                if (ContagemTransp > 0)
                {

                }
                else
                {
                    Session.SendCommand("/nxk03");
                    ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtRF02K-BUKRS")).Text = " ";
                    ((GuiCheckBox)Session.FindById("wnd[0]/usr/chkRF02K-D0110")).Selected = true;
                    ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtRF02K-LIFNR")).Text = CTransportadorBag.Trim();
                    ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtRF02K-EKORG")).Text = "1000";
                    guiWindow.SendVKey(0);
                    ((GuiComboBox)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2036/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/subSCREEN_1100_ROLE_AND_TIME_AREA:SAPLBUPA_DIALOG_JOEL:1110/cmbBUS_JOEL_MAIN-PARTNER_ROLE")).Key = "ZFFPJE";
                    NTransportador = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2036/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7016/subA02P02:SAPLBUD0:1200/txtBUT000-NAME_ORG1")).Text;
                    ((GuiTab)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2036/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_03")).Select();
                    CNPJTransportador = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2036/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_03/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7013/subA07P01:SAPLBUPA_BUTX_DIALOG:0100/tblSAPLBUPA_BUTX_DIALOGTCTRL_BPTAX/txtDFKKBPTAXNUM-TAXNUMXL[2,0]")).Text;
                    IEstTransportador = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2036/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_03/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7013/subA07P01:SAPLBUPA_BUTX_DIALOG:0100/tblSAPLBUPA_BUTX_DIALOGTCTRL_BPTAX/txtDFKKBPTAXNUM-TAXNUMXL[2,1]")).Text;
                    maskTransp.Text = CNPJTransportador;
                    maskTransp.Text.Replace(",", ".");
                    maskCliente.Text = txtCliente.Text;
                    maskCliente.Text.Replace(",", ".");
                    txtTransportadora.Text = NTransportador;
                    txtIEST.Text = IEstTransportador;

                    try
                    {
                        //Seleção da tabela no Banco de Dados.
                        MySqlCommand prompt = new MySqlCommand("SELECT COUNT(*) FROM tb_transport WHERE col_cod_id ='" + CTransportadorBag + "' ", ConexaoDados.GetConnectionFaturameto());
                        //Executa o comando.
                        prompt.ExecuteNonQuery();
                        //Converte o resultado para números inteiros.
                        int consultDB = Convert.ToInt32(prompt.ExecuteScalar());
                        //Verifica se o resultado for maior que zero(0), a execução inicia a Menssagem de que já existe contas, caso contrario faz a inserção no Banco.
                        if (consultDB > 0)
                        {

                        }
                        else
                        {
                            try
                            {
                                MySqlCommand command = new MySqlCommand("INSERT INTO `tb_transport` (`col_cod_id`, `col_nome_transp`, `col_cnpj_transp`, `col_iestadual_transp`) VALUES ('" + CTransportadorBag + "', '" + txtTransportadora.Text + "', '" + maskTransp.Text + "', '" + txtIEST.Text + "')", ConexaoDados.GetConnectionFaturameto());
                                command.ExecuteNonQuery();
                                ProgressBar.Value = 65;
                            }
                            catch (Exception Err)
                            {
                                MessageBox.Show(Err.Message);
                            }
                        }
                    }
                    catch (NullReferenceException)
                    {
                        MessageBox.Show("Olá Srº(a), " + usuarioLogado + " contate o administrador, houve algum erro na\naplicação!.");
                        ProgressBar.Value = 0;
                    }
                    finally
                    {

                    }
                }
            }
            ProgressBar.Value = 30;

            if (String.IsNullOrEmpty(txtCliente.Text))
            {
                CCliente = cmbCliente.SelectedValue.ToString();
                if (cmbCliente.DisplayMember == "")
                {

                }
            }
            else
            {
                maskCliente.Text = txtCliente.Text;
                maskCliente.Text.Replace(",", ".");
                MySqlCommand.Connection = ConexaoDados.GetConnectionFaturameto();
                MySqlCommand.CommandText = "SELECT * FROM `tb_cliente` WHERE col_cnpj ='" + maskCliente.Text.Replace(",", ".") + "';";
                MySqlDataReader dataReader = MySqlCommand.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                int ContagemClient = dt.Rows.Count;
                if (ContagemClient > 0)
                {
                    MySqlDataReader dreader = MySqlCommand.ExecuteReader();
                    while (dreader.Read())
                    {
                        CCliente = dreader["col_cod_id"].ToString();
                        break;
                    }
                }
                else
                {
                    Session.SendCommand("/nxd03");
                    guiWindow.SendVKey(4);
                    ((GuiTextField)Session.FindById("wnd[2]/usr/tabsG_SELONETABSTRIP/tabpTAB006/ssubSUBSCR_PRESEL:SAPLSDH4:0220/sub:SAPLSDH4:0220/txtG_SELFLD_TAB-LOW[0,24]")).Text = txtCliente.Text;
                    ((GuiTextField)Session.FindById("wnd[1]/usr/ctxtRF02D-VKORG")).Text = " ";
                    ((GuiFrameWindow)Session.FindById("wnd[2]")).SendVKey(0);
                    ((GuiFrameWindow)Session.FindById("wnd[2]")).SendVKey(0);
                    ((GuiFrameWindow)Session.FindById("wnd[1]")).SendVKey(0);
                    ((GuiComboBox)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/subSCREEN_1100_ROLE_AND_TIME_AREA:SAPLBUPA_DIALOG_JOEL:1110/cmbBUS_JOEL_MAIN-PARTNER_ROLE")).Key = "ZCCPJE";
                    ((GuiTab)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01")).Select();
                    String NCliente = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7016/subA02P02:SAPLBUD0:1200/txtBUT000-NAME_ORG1")).Text;
                    String RuaCliente = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7016/subA05P01:SAPLBUA0:0400/subADDRESS:SAPLSZA1:0300/subCOUNTRY_SCREEN:SAPLSZA1:0301/txtADDR1_DATA-STREET")).Text;
                    String CidCliente = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7016/subA05P01:SAPLBUA0:0400/subADDRESS:SAPLSZA1:0300/subCOUNTRY_SCREEN:SAPLSZA1:0301/txtADDR1_DATA-CITY1")).Text;
                    String EstCliente = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7016/subA05P01:SAPLBUA0:0400/subADDRESS:SAPLSZA1:0300/subCOUNTRY_SCREEN:SAPLSZA1:0301/ctxtADDR1_DATA-REGION")).Text;

                    ((GuiTab)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_03")).Select();
                    String IesCliente = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_03/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7013/subA07P01:SAPLBUPA_BUTX_DIALOG:0100/tblSAPLBUPA_BUTX_DIALOGTCTRL_BPTAX/txtDFKKBPTAXNUM-TAXNUMXL[2,1]")).Text;
                    ((GuiTab)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_08")).Select();
                    CCliente = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_08/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7018/subA02P01:SAPLCVI_FS_UI_CUSTOMER:0021/txtGV_CUSTOMER")).Text;
                    try
                    {
                        //Seleção da tabela no Banco de Dados.
                        MySqlCommand prompt = new MySqlCommand("SELECT COUNT(*) FROM tb_cliente WHERE col_cod_id ='" + CCliente + "' ", ConexaoDados.GetConnectionFaturameto());
                        //Executa o comando.
                        prompt.ExecuteNonQuery();
                        //Converte o resultado para números inteiros.
                        int consultDB = Convert.ToInt32(prompt.ExecuteScalar());
                        //Verifica se o resultado for maior que zero(0), a execução inicia a Menssagem de que já existe contas, caso contrario faz a inserção no Banco.
                        if (consultDB > 0)
                        {

                        }
                        else
                        {
                            try
                            {
                                MySqlCommand command = new MySqlCommand("INSERT INTO `tb_cliente` (`col_cod_id`, `col_nome`, `col_cnpj`, `col_rua`, `col_cidade`, `col_iestadual`, `col_estado`) VALUES ('" + CCliente + "', '" + NCliente + "', '" + maskCliente.Text.Replace(',', '.') + "', '" + RuaCliente + "', '" + CidCliente + "', '" + IesCliente + "', '" + EstCliente + "')", ConexaoDados.GetConnectionFaturameto());
                                command.ExecuteNonQuery();
                            }
                            catch (Exception Err)
                            {
                                MessageBox.Show(Err.Message);
                            }
                        }

                    }
                    catch (NullReferenceException)
                    {
                        MessageBox.Show("Olá Srº(a), " + usuarioLogado + " contate o administrador, houve algum erro na\naplicação!.");
                        ProgressBar.Value = 0;
                    }
                    finally
                    {

                    }
                }
            }
            Session.SendCommand("/NZBL014");
            ProgressBar.Value = 50;

            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtT_1000-LIFNR")).Text = CMotorBag;
            if (String.IsNullOrEmpty(CTransportadorBag))
            {
                ((GuiCheckBox)Session.FindById("wnd[0]/usr/chkT_1000-AUTO")).Selected = true;
                guiWindow.SendVKey(0);
            }
            else
            {
                ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtT_1000-LIFNR2")).Text = CTransportadorBag;
                guiWindow.SendVKey(0);
            }
            ((GuiButton)Session.FindById("wnd[0]/tbar[1]/btn[18]")).Press();
            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtT_2000-PLACA1")).Text = txtPlaca1.Text;
            if (string.IsNullOrEmpty(txtPlaca2.Text))
            {

            }
            else
            {
                ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtT_2000-PLACA2")).Text = txtPlaca2.Text;
            }
            if (string.IsNullOrEmpty(txtPlaca3.Text))
            {

            }
            else
            {
                ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtT_2000-PLACA3")).Text = txtPlaca3.Text;
            }
            if (string.IsNullOrEmpty(txtPlaca4.Text))
            {

            }
            else
            {
                ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtT_2000-PLACA4")).Text = txtPlaca4.Text;
            }
            guiWindow.SendVKey(0);
            ((GuiButton)Session.FindById("wnd[0]/tbar[1]/btn[18]")).Press();
            guiWindow.SendVKey(0);
            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtT001W-BWKEY")).Text = "USGA";
            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-KUNNR")).Text = CCliente;
            if (MessageBox.Show("Acesso para Coca-Cola?", "Aviso!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ((GuiTextField)Session.FindById("wnd[0]/usr/txtINP_MATNR")).Text = "100141";
            }
            else
            {
                ((GuiTextField)Session.FindById("wnd[0]/usr/txtINP_MATNR")).Text = "100001";
            }
            ((GuiTextField)Session.FindById("wnd[0]/usr/txtZMM_STAT_VEHI-QUANTID")).Text = txtQuantidade.Text;
            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-KUNNR")).SetFocus();
            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-KUNNR")).CaretPosition = 10;
            guiWindow.SendVKey(0);
            guiWindow.SendVKey(0);
            string OVenda = ((GuiTextField)Session.FindById("wnd[0]/usr/tblSAPMZMM099TC_DISP/txtTVBAP-VBELN[0,0]")).Text;
            double QuantOV = Convert.ToDouble(((GuiTextField)Session.FindById("wnd[0]/usr/tblSAPMZMM099TC_DISP/txtTVBAP-QUANTID[1,0]")).Text);
            
            if(QuantOV < QuantidadeBagsConvertSc)
            {
                MessageBox.Show("Ordem de Venda com saldo Insuficiente para o Carregamento", "Saldo!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                if (MessageBox.Show("Deseja Concluir?", "Aviso!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ((GuiButton)Session.FindById("wnd[0]/tbar[0]/btn[11]")).Press();
                    GuiStatusbar statusbar = (GuiStatusbar)Session.FindById("wnd[0]/sbar");
                    string resultado = statusbar.Text.Substring(0, 39);
                    try
                    {
                        if (String.IsNullOrEmpty(CTransportadorBag))
                        {
                            MySqlCommand command = new MySqlCommand("INSERT INTO `tb_acesso` (`col_acesso`, `col_data_acesso`, `col_hora_acesso`, `col_cliente`, `col_tipo_sac`, `col_quantBag`, `col_quant`, `col_ov`, `col_sacaria_inicio`, `col_sacaria_fim`, `col_safra`, `col_transport`, `col_motorista`, `col_placa_1`, `col_placa_2`, `col_placa_3`, `col_placa_4`, `Obs`, `col_id_user`) VALUES ('" + resultado.Replace("Acesso", "").Replace("incluído com sucesso!", "").Trim() + "', CURDATE(), NOW(), '" + CCliente + "', '" + lblUnd.Text + "','" + txtQuantidade.Text + "', '" + QUANTBAG + "', '" + txtOv.Text + "', '" + txtSacariaInic.Text + "', '" + txtSacariaFim.Text + "', '" + maskSafra.Text + "', NULL, '" + CMotorBag + "', '" + txtPlaca1.Text + "', '" + txtPlaca2.Text + "', '" + txtPlaca3.Text + "', '" + txtPlaca4.Text + "', '" + richObs.Text + "','" + dados.IdUser.ToString() + "')", ConexaoDados.GetConnectionFaturameto());
                            command.ExecuteNonQuery();
                            ProgressBar.Value = 70;
                        }
                        else
                        {
                            MySqlCommand command = new MySqlCommand("INSERT INTO `tb_acesso` (`col_acesso`, `col_data_acesso`, `col_hora_acesso`, `col_cliente`, `col_tipo_sac`, `col_quantBag`, `col_quant`, `col_ov`, `col_sacaria_inicio`, `col_sacaria_fim`, `col_safra`, `col_transport`, `col_motorista`, `col_placa_1`, `col_placa_2`, `col_placa_3`, `col_placa_4`, `Obs`, `col_id_user`) VALUES ('" + resultado.Replace("Acesso", "").Replace("incluído com sucesso!", "").Trim() + "', CURDATE(), NOW(), '" + CCliente + "', '" + lblUnd.Text + "','" + txtQuantidade.Text + "', '" + QUANTBAG + "', '" + txtOv.Text + "', '" + txtSacariaInic.Text + "', '" + txtSacariaFim.Text + "', '" + maskSafra.Text + "', '" + CTransportadorBag + "', '" + CMotorBag + "', '" + txtPlaca1.Text + "', '" + txtPlaca2.Text + "', '" + txtPlaca3.Text + "', '" + txtPlaca4.Text + "', '" + richObs.Text + "','" + dados.IdUser.ToString() + "')", ConexaoDados.GetConnectionFaturameto());
                            command.ExecuteNonQuery();
                            ProgressBar.Value = 70;
                        }
                    }
                    catch (NullReferenceException)
                    {
                        MessageBox.Show("Olá Srº(a), " + usuarioLogado + " contate o administrador, houve algum erro na\naplicação!.");
                        ProgressBar.Value = 0;
                    }
                    catch (MySqlException eer)
                    {
                        MessageBox.Show(eer.Message);
                    }
                    finally
                    {

                    }
                }
                //Fecha Conexão
                ConexaoDados.GetConnectionFaturameto().Close();
                Session.SendCommand("/N");
                CTransportador = "";
                AbrirPDFs();
            }
            ProgressBar.Value = 100;
        }
        String NTransportador;
        String CNPJTransportador;
        String IEstTransportador;

        private void SAPAcesso()
        {
            ProgressBar.Value = 0;
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
            //Abre Transação
            MySqlCommand.Connection = ConexaoDados.GetConnectionFaturameto();
            MySqlCommand.CommandText = "SELECT * FROM `tb_motorista` WHERE col_cpf ='"+txtCPF.Text+"';";
            MySqlDataReader dr = MySqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(dr);
            int ContagemMot = dataTable.Rows.Count;
            if (ContagemMot > 0)
            {
                try
                {
                    MySqlDataReader dreader = MySqlCommand.ExecuteReader();
                    while (dreader.Read())
                    {
                        NMotor = dreader["col_nome_mot"].ToString();
                        CMotor = dreader["col_cod_id"].ToString();
                        CTransportador = dreader["col_transportadora"].ToString();
                        break;
                    }
                    ConexaoDados.GetConnectionFaturameto().Close();
                }
                catch (MySqlException ErroDB)
                {
                    MessageBox.Show("Banco de Dados, Desligado! " + ErroDB, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message);
                }
            }
            else
            {
                Session.SendCommand("/NBP");
                ProgressBar.Value = 10;
                ((GuiMenu)Session.FindById("wnd[0]/mbar/menu[0]/menu[0]")).Select();
                guiWindow.SendVKey(4);
                ((GuiTextField)Session.FindById("wnd[2]/usr/tabsG_SELONETABSTRIP/tabpTAB014/ssubSUBSCR_PRESEL:SAPLSDH4:0220/sub:SAPLSDH4:0220/ctxtG_SELFLD_TAB-LOW[0,24]")).Text = "BR2";
                ((GuiTextField)Session.FindById("wnd[2]/usr/tabsG_SELONETABSTRIP/tabpTAB014/ssubSUBSCR_PRESEL:SAPLSDH4:0220/sub:SAPLSDH4:0220/txtG_SELFLD_TAB-LOW[1,24]")).Text = txtCPF.Text;
                guiWindow.SendVKey(0);
                guiWindow.SendVKey(0);
                guiWindow.SendVKey(0);
                ((GuiComboBox)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/subSCREEN_1100_ROLE_AND_TIME_AREA:SAPLBUPA_DIALOG_JOEL:1110/cmbBUS_JOEL_MAIN-PARTNER_ROLE")).Key = "ZFTERE";
                ((GuiTab)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01")).Select();
                NMotor = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7016/subA02P03:SAPLBUD0:1301/txtBUT000-NAME_FIRST")).Text;
                ((GuiTab)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_08")).Select();
                CMotor = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_08/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7030/subA02P01:SAPLCVI_FS_UI_VENDOR:0021/txtGV_VENDOR")).Text;
                ((GuiTab)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_09")).Select();
                CTransportador = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_09/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7031/subA04P01:SAPLCVI_FS_UI_VENDOR:0018/ctxtGS_LFA1-FISKU")).Text;
                try
                {
                    if (String.IsNullOrEmpty(CTransportador))
                    {
                        MySqlCommand command = new MySqlCommand("INSERT INTO `tb_motorista` (`col_cod_id`, `col_nome_mot`, `col_cpf`) VALUES ('" + CMotor + "', '" + NMotor + "', '" + txtCPF.Text + "')", ConexaoDados.GetConnectionFaturameto());
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        MySqlCommand command = new MySqlCommand("INSERT INTO `tb_motorista` (`col_cod_id`, `col_nome_mot`, `col_cpf`, `col_transportadora`) VALUES ('" + CMotor + "', '" + NMotor + "', '" + txtCPF.Text + "', '" + CTransportador + "')", ConexaoDados.GetConnectionFaturameto());
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message);
                    ProgressBar.Value = 0;
                }
            }
            txtCPF.Visible = false;
            txtCPFCompleto.Visible = true;
            txtCPFCompleto.Text = NMotor;
            ProgressBar.Value = 20;
            if (String.IsNullOrEmpty(CTransportador))
            {
                
            }
            else
            {

                MySqlCommand.Connection = ConexaoDados.GetConnectionFaturameto();
                MySqlCommand.CommandText = "SELECT * FROM `tb_transport` WHERE col_cod_id ='" + CTransportador + "';";
                MySqlDataReader dataReader = MySqlCommand.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);
                int ContagemTransp = dt.Rows.Count;
                if (ContagemTransp > 0)
                {

                }
                else
                {
                    Session.SendCommand("/nxk03");
                    ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtRF02K-BUKRS")).Text = " ";
                    ((GuiCheckBox)Session.FindById("wnd[0]/usr/chkRF02K-D0110")).Selected = true;
                    ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtRF02K-LIFNR")).Text = CTransportador.Trim();
                    ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtRF02K-EKORG")).Text = "1000";
                    guiWindow.SendVKey(0);
                    ((GuiComboBox)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2036/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/subSCREEN_1100_ROLE_AND_TIME_AREA:SAPLBUPA_DIALOG_JOEL:1110/cmbBUS_JOEL_MAIN-PARTNER_ROLE")).Key = "ZFFPJE";
                    NTransportador = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2036/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7016/subA02P02:SAPLBUD0:1200/txtBUT000-NAME_ORG1")).Text;
                    ((GuiTab)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2036/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_03")).Select();
                    CNPJTransportador = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2036/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_03/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7013/subA07P01:SAPLBUPA_BUTX_DIALOG:0100/tblSAPLBUPA_BUTX_DIALOGTCTRL_BPTAX/txtDFKKBPTAXNUM-TAXNUMXL[2,0]")).Text;
                    IEstTransportador = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2036/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_03/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7013/subA07P01:SAPLBUPA_BUTX_DIALOG:0100/tblSAPLBUPA_BUTX_DIALOGTCTRL_BPTAX/txtDFKKBPTAXNUM-TAXNUMXL[2,1]")).Text;
                    maskTransp.Text = CNPJTransportador;
                    maskTransp.Text.Replace(",", ".");
                    maskCliente.Text = txtCliente.Text;
                    maskCliente.Text.Replace(",", ".");
                    txtTransportadora.Text = NTransportador;
                    txtIEST.Text = IEstTransportador;

                    try
                    {
                        //Seleção da tabela no Banco de Dados.
                        MySqlCommand prompt = new MySqlCommand("SELECT COUNT(*) FROM tb_transport WHERE col_cod_id ='" + CTransportador + "' ", ConexaoDados.GetConnectionFaturameto());
                        //Executa o comando.
                        prompt.ExecuteNonQuery();
                        //Converte o resultado para números inteiros.
                        int consultDB = Convert.ToInt32(prompt.ExecuteScalar());
                        //Verifica se o resultado for maior que zero(0), a execução inicia a Menssagem de que já existe contas, caso contrario faz a inserção no Banco.
                        if (consultDB > 0)
                        {

                        }
                        else
                        {
                            try
                            {
                                MySqlCommand command = new MySqlCommand("INSERT INTO `tb_transport` (`col_cod_id`, `col_nome_transp`, `col_cnpj_transp`, `col_iestadual_transp`) VALUES ('" + CTransportador + "', '" + txtTransportadora.Text + "', '" + maskTransp.Text + "', '" + txtIEST.Text + "')", ConexaoDados.GetConnectionFaturameto());
                                command.ExecuteNonQuery();
                                ProgressBar.Value = 65;
                            }
                            catch (Exception Err)
                            {
                                MessageBox.Show(Err.Message);
                            }
                        }
                    }
                    catch (NullReferenceException)
                    {
                        MessageBox.Show("Olá Srº(a), " + usuarioLogado + " contate o administrador, houve algum erro na\naplicação!.");
                        ProgressBar.Value = 0;
                    }
                    finally
                    {

                    }
                }



            }
            ProgressBar.Value = 30;

            if (String.IsNullOrEmpty(txtCliente.Text))
            {
                CCliente = cmbCliente.SelectedValue.ToString();
                if(cmbCliente.DisplayMember == "")
                {

                }
            }
            else
            {
                maskCliente.Text = txtCliente.Text;
                maskCliente.Text.Replace(",", ".");
                MySqlCommand.Connection = ConexaoDados.GetConnectionFaturameto();
                MySqlCommand.CommandText = "SELECT * FROM `tb_cliente` WHERE col_cnpj ='" + maskCliente.Text.Replace(",", ".") + "';";
                MySqlDataReader dataReader = MySqlCommand.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                int ContagemClient = dt.Rows.Count;
                if (ContagemClient > 0)
                {
                    MySqlDataReader dreader = MySqlCommand.ExecuteReader();
                    while (dreader.Read())
                    {
                        CCliente = dreader["col_cod_id"].ToString();
                        break;
                    }
                }
                else
                {
                    Session.SendCommand("/nxd03");
                    guiWindow.SendVKey(4);
                    ((GuiTextField)Session.FindById("wnd[2]/usr/tabsG_SELONETABSTRIP/tabpTAB006/ssubSUBSCR_PRESEL:SAPLSDH4:0220/sub:SAPLSDH4:0220/txtG_SELFLD_TAB-LOW[0,24]")).Text = txtCliente.Text;
                    ((GuiTextField)Session.FindById("wnd[1]/usr/ctxtRF02D-VKORG")).Text = " ";
                    ((GuiFrameWindow)Session.FindById("wnd[2]")).SendVKey(0);
                    ((GuiFrameWindow)Session.FindById("wnd[2]")).SendVKey(0);
                    ((GuiFrameWindow)Session.FindById("wnd[1]")).SendVKey(0);
                    ((GuiComboBox)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/subSCREEN_1100_ROLE_AND_TIME_AREA:SAPLBUPA_DIALOG_JOEL:1110/cmbBUS_JOEL_MAIN-PARTNER_ROLE")).Key = "ZCCPJE";
                    ((GuiTab)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01")).Select();
                    String NCliente = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7016/subA02P02:SAPLBUD0:1200/txtBUT000-NAME_ORG1")).Text;
                    String RuaCliente = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7016/subA05P01:SAPLBUA0:0400/subADDRESS:SAPLSZA1:0300/subCOUNTRY_SCREEN:SAPLSZA1:0301/txtADDR1_DATA-STREET")).Text;
                    String CidCliente = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7016/subA05P01:SAPLBUA0:0400/subADDRESS:SAPLSZA1:0300/subCOUNTRY_SCREEN:SAPLSZA1:0301/txtADDR1_DATA-CITY1")).Text;
                    String EstCliente = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_01/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7016/subA05P01:SAPLBUA0:0400/subADDRESS:SAPLSZA1:0300/subCOUNTRY_SCREEN:SAPLSZA1:0301/ctxtADDR1_DATA-REGION")).Text;

                    ((GuiTab)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_03")).Select();
                    String IesCliente = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_03/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7013/subA07P01:SAPLBUPA_BUTX_DIALOG:0100/tblSAPLBUPA_BUTX_DIALOGTCTRL_BPTAX/txtDFKKBPTAXNUM-TAXNUMXL[2,1]")).Text;
                    ((GuiTab)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_08")).Select();
                    CCliente = ((GuiTextField)Session.FindById("wnd[0]/usr/subSCREEN_3000_RESIZING_AREA:SAPLBUS_LOCATOR:2000/subSCREEN_1010_RIGHT_AREA:SAPLBUPA_DIALOG_JOEL:1000/ssubSCREEN_1000_WORKAREA_AREA:SAPLBUPA_DIALOG_JOEL:1100/ssubSCREEN_1100_MAIN_AREA:SAPLBUPA_DIALOG_JOEL:1101/tabsGS_SCREEN_1100_TABSTRIP/tabpSCREEN_1100_TAB_08/ssubSCREEN_1100_TABSTRIP_AREA:SAPLBUSS:0028/ssubGENSUB:SAPLBUSS:7018/subA02P01:SAPLCVI_FS_UI_CUSTOMER:0021/txtGV_CUSTOMER")).Text;
                    try
                    {
                        //Seleção da tabela no Banco de Dados.
                        MySqlCommand prompt = new MySqlCommand("SELECT COUNT(*) FROM tb_cliente WHERE col_cod_id ='" + CCliente + "' ", ConexaoDados.GetConnectionFaturameto());
                        //Executa o comando.
                        prompt.ExecuteNonQuery();
                        //Converte o resultado para números inteiros.
                        int consultDB = Convert.ToInt32(prompt.ExecuteScalar());
                        //Verifica se o resultado for maior que zero(0), a execução inicia a Menssagem de que já existe contas, caso contrario faz a inserção no Banco.
                        if (consultDB > 0)
                        {

                        }
                        else
                        {
                            try
                            {
                                MySqlCommand command = new MySqlCommand("INSERT INTO `tb_cliente` (`col_cod_id`, `col_nome`, `col_cnpj`, `col_rua`, `col_cidade`, `col_iestadual`, `col_estado`) VALUES ('" + CCliente + "', '" + NCliente + "', '" + maskCliente.Text.Replace(',', '.') + "', '" + RuaCliente + "', '" + CidCliente + "', '" + IesCliente + "', '" + EstCliente + "')", ConexaoDados.GetConnectionFaturameto());
                                command.ExecuteNonQuery();
                            }
                            catch (Exception Err)
                            {
                                MessageBox.Show(Err.Message);
                            }
                        }

                    }
                    catch (NullReferenceException)
                    {
                        MessageBox.Show("Olá Srº(a), " + usuarioLogado + " contate o administrador, houve algum erro na\naplicação!.");
                        ProgressBar.Value = 0;
                    }
                    finally
                    {

                    }
                }
            }
            Session.SendCommand("/NZBL014");
            ProgressBar.Value = 50;

            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtT_1000-LIFNR")).Text = CMotor;
            if (String.IsNullOrEmpty(CTransportador))
            {
                ((GuiCheckBox)Session.FindById("wnd[0]/usr/chkT_1000-AUTO")).Selected = true;
                guiWindow.SendVKey(0);
            }
            else
            {
                ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtT_1000-LIFNR2")).Text = CTransportador;
                guiWindow.SendVKey(0);
            }
            ((GuiButton)Session.FindById("wnd[0]/tbar[1]/btn[18]")).Press();
            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtT_2000-PLACA1")).Text = txtPlaca1.Text;
            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtT_2000-PLACA2")).Text = txtPlaca2.Text;
            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtT_2000-PLACA3")).Text = txtPlaca3.Text;
            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtT_2000-PLACA4")).Text = txtPlaca4.Text;
            guiWindow.SendVKey(0);
            ((GuiButton)Session.FindById("wnd[0]/tbar[1]/btn[18]")).Press();
            guiWindow.SendVKey(0);
            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtT001W-BWKEY")).Text = "USGA";
            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-KUNNR")).Text = CCliente;
            ((GuiTextField)Session.FindById("wnd[0]/usr/txtINP_MATNR")).Text = "000000000000100000";
            ((GuiTextField)Session.FindById("wnd[0]/usr/txtZMM_STAT_VEHI-QUANTID")).Text = txtQuantidade.Text;
            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-KUNNR")).SetFocus();
            ((GuiCTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-KUNNR")).CaretPosition = 10;
            guiWindow.SendVKey(0);
            guiWindow.SendVKey(0);
            ((GuiCheckBox)Session.FindById("wnd[0]/usr/chkCK_FAT_ACESSO")).Selected = true;
            ((GuiTextField)Session.FindById("wnd[0]/usr/tblSAPMZMM099TC_DISP/txtTVBAP-CODIF[4,0]")).Text = "" + txtSacariaInic.Text + " A " + txtSacariaFim.Text + "";
            string OVenda = ((GuiTextField)Session.FindById("wnd[0]/usr/tblSAPMZMM099TC_DISP/txtTVBAP-VBELN[0,0]")).Text;
            double QuantOV = Convert.ToDouble(((GuiTextField)Session.FindById("wnd[0]/usr/tblSAPMZMM099TC_DISP/txtTVBAP-QUANTID[1,0]")).Text);

            if (QuantOV < QuantidadeBagsConvertSc)
            {
                MessageBox.Show("Ordem de Venda com saldo Insuficiente para o Carregamento", "Saldo!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                if (MessageBox.Show("Deseja Concluir?", "Aviso!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ((GuiButton)Session.FindById("wnd[0]/tbar[0]/btn[11]")).Press();
                    GuiStatusbar statusbar = (GuiStatusbar)Session.FindById("wnd[0]/sbar");
                    string resultado = statusbar.Text.Substring(0, 39);

                    try
                    {
                        if (String.IsNullOrEmpty(CTransportador))
                        {
                            MySqlCommand command = new MySqlCommand("INSERT INTO `tb_acesso` (`col_acesso`, `col_data_acesso`, `col_hora_acesso`, `col_cliente`, `col_tipo_sac`, `col_quantBag`, `col_quant`, `col_ov`, `col_sacaria_inicio`, `col_sacaria_fim`, `col_safra`, `col_transport`, `col_motorista`, `col_placa_1`, `col_placa_2`, `col_placa_3`, `col_placa_4`, `Obs`, `col_id_user`) VALUES ('" + resultado.Replace("Acesso", "").Replace("incluído com sucesso!", "").Trim() + "', CURDATE(), NOW(), '" + CCliente + "', '" + lblUnd.Text + "','" + QuantidadeBags + "', '" + txtQuantidade.Text + "', '" + txtOv.Text + "', '" + txtSacariaInic.Text + "', '" + txtSacariaFim.Text + "', '" + maskSafra.Text + "', NULL, '" + CMotor + "', '" + txtPlaca1.Text + "', '" + txtPlaca2.Text + "', '" + txtPlaca3.Text + "', '" + txtPlaca4.Text + "', '" + richObs.Text + "','" + dados.IdUser.ToString() + "')", ConexaoDados.GetConnectionFaturameto());
                            command.ExecuteNonQuery();
                            ProgressBar.Value = 70;
                        }
                        else
                        {
                            MySqlCommand command = new MySqlCommand("INSERT INTO `tb_acesso` (`col_acesso`, `col_data_acesso`, `col_hora_acesso`, `col_cliente`, `col_tipo_sac`, `col_quantBag`,`col_quant`, `col_ov`, `col_sacaria_inicio`, `col_sacaria_fim`, `col_safra`, `col_transport`, `col_motorista`, `col_placa_1`, `col_placa_2`, `col_placa_3`, `col_placa_4`, `Obs`, `col_id_user`) VALUES ('" + resultado.Replace("Acesso", "").Replace("incluído com sucesso!", "").Trim() + "', CURDATE(), NOW(), '" + CCliente + "', '" + lblUnd.Text + "','" + QuantidadeBags + "', '" + txtQuantidade.Text + "', '" + txtOv.Text + "', '" + txtSacariaInic.Text + "', '" + txtSacariaFim.Text + "', '" + maskSafra.Text + "', '" + CTransportador + "', '" + CMotor + "', '" + txtPlaca1.Text + "', '" + txtPlaca2.Text + "', '" + txtPlaca3.Text + "', '" + txtPlaca4.Text + "', '" + richObs.Text + "','" + dados.IdUser.ToString() + "')", ConexaoDados.GetConnectionFaturameto());
                            command.ExecuteNonQuery();
                            ProgressBar.Value = 70;
                        }
                    }
                    catch (NullReferenceException)
                    {
                        MessageBox.Show("Olá Srº(a), " + usuarioLogado + " contate o administrador, houve algum erro na\naplicação!.");
                        ProgressBar.Value = 0;
                    }
                    catch (MySqlException eer)
                    {
                        MessageBox.Show(eer.Message);
                    }
                    finally
                    {

                    }
                    //Fecha Conexão
                    ConexaoDados.GetConnectionFaturameto().Close();
                    Session.SendCommand("/N");
                    CTransportador = "";
                    AbrirPDFs();
                }
                ProgressBar.Value = 100;
                UltimNumSacaria();
                txtSacariaFim.Text = "";
            }
        }
        private void NFTIJOLO()
        {
            ProgressBar.Value = 0;
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
            //Abre Transação
            Session.SendCommand("/NVA01");
            ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-AUART")).Text = "ZVVD";
            ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-AUART")).CaretPosition = 4;
            ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-VKORG")).Text = "";
            ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-VTWEG")).Text = "";
            ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-SPART")).Text = "";
            ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-VKBUR")).Text = "";
            ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-VKGRP")).Text = "";
            guiWindow.SendVKey(0);
            ((GuiTextField)Session.FindById("wnd[0]/usr/subSUBSCREEN_HEADER:SAPMV45A:4021/subPART-SUB:SAPMV45A:4701/ctxtKUAGV-KUNNR")).Text = txtClienteTijolo.Text;
            ((GuiTextField)Session.FindById("wnd[0]/usr/subSUBSCREEN_HEADER:SAPMV45A:4021/subPART-SUB:SAPMV45A:4701/ctxtKUAGV-KUNNR")).CaretPosition = 10;
            guiWindow.SendVKey(0);
            //((GuiLabel)Session.FindById("wnd[1]/usr/lbl[7,5]")).SetFocus();
            //((GuiLabel)Session.FindById("wnd[1]/usr/lbl[7,5]")).CaretPosition = 2;
            //guiWindow.SendVKey(2);
            ((GuiTextField)Session.FindById("wnd[0]/usr/subSUBSCREEN_HEADER:SAPMV45A:4021/txtVBKD-BSTKD")).Text = txtPedido.Text;
            ((GuiTextField)Session.FindById("wnd[0]/usr/subSUBSCREEN_HEADER:SAPMV45A:4021/txtVBKD-BSTKD")).CaretPosition = 25;
            guiWindow.SendVKey(0);
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/ctxtRV45A-MABNR[1,0]")).Text = txtSacariaFim.Text;
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/txtRV45A-KWMENG[2,0]")).Text = txtQuantidade.Text;
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/ctxtVBAP-VRKME[3,0]")).Text = "";
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/ctxtVBAP-VRKME[3,0]")).SetFocus();
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/ctxtVBAP-VRKME[3,0]")).CaretPosition = 0;
            guiWindow.SendVKey(0);
            guiWindow.SendVKey(0);
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/txtVBAP-ARKTX[5,0]")).Text = txtItem.Text;
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/txtVBAP-ARKTX[5,0]")).SetFocus();
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/txtVBAP-ARKTX[5,0]")).CaretPosition = 17;
            guiWindow.SendVKey(2);
            ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\03")).Select();
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\03/ssubSUBSCREEN_BODY:SAPMV45A:4452/txtVBAP-VOLUM")).Text = txtQuantidade.Text;
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\03/ssubSUBSCREEN_BODY:SAPMV45A:4452/ctxtVBAP-VOLEH")).Text = "UN.";
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\03/ssubSUBSCREEN_BODY:SAPMV45A:4452/ctxtVBAP-VOLEH")).SetFocus();
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\03/ssubSUBSCREEN_BODY:SAPMV45A:4452/ctxtVBAP-VOLEH")).CaretPosition = 3;
            ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\04")).Select();
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\04/ssubSUBSCREEN_BODY:SAPMV45A:4453/ctxtVBKD-ZLSCH")).Text = "T";
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\04/ssubSUBSCREEN_BODY:SAPMV45A:4453/ctxtVBKD-ZLSCH")).SetFocus();
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\04/ssubSUBSCREEN_BODY:SAPMV45A:4453/ctxtVBKD-ZLSCH")).CaretPosition = 1;
            guiWindow.SendVKey(0);
            ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05")).Select();
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTXSDC")).Text = "I1";
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTAXLW1")).Text = "IC0";
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTAXLW2")).Text = "IP1";
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BCFOP")).Text = "6101/AA";
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTAXLW3")).Text = "";
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTAXLW4")).Text = "C01";
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTAXLW5")).Text = "P01";
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTAXLW5")).SetFocus();
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTAXLW5")).CaretPosition = 3;
            ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\06")).Select();
            guiWindow.SendVKey(0);
            ((GuiButton)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\06/ssubSUBSCREEN_BODY:SAPLV69A:6201/subSUBSCREEN_PUSHBUTTONS:SAPLV69A:1000/btnBT_KOAN")).Press();
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\06/ssubSUBSCREEN_BODY:SAPLV69A:6201/tblSAPLV69ATCTRL_KONDITIONEN/ctxtKOMV-KSCHL[1,1]")).Text = "ZPB0";
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\06/ssubSUBSCREEN_BODY:SAPLV69A:6201/tblSAPLV69ATCTRL_KONDITIONEN/txtKOMV-KBETR[3,1]")).Text = txtPeso.Text;
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\06/ssubSUBSCREEN_BODY:SAPLV69A:6201/tblSAPLV69ATCTRL_KONDITIONEN/txtKOMV-KBETR[3,1]")).SetFocus();
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\06/ssubSUBSCREEN_BODY:SAPLV69A:6201/tblSAPLV69ATCTRL_KONDITIONEN/txtKOMV-KBETR[3,1]")).CaretPosition = 16;
            guiWindow.SendVKey(0);
            ((GuiMenu)Session.FindById("wnd[0]/mbar/menu[2]/menu[1]")).Select();
            ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\05")).Select();
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4311/ctxtVBKD-ZLSCH")).Text = "T";
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4311/ctxtVBKD-ZLSCH")).SetFocus();
            ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4311/ctxtVBKD-ZLSCH")).CaretPosition = 1;
            guiWindow.SendVKey(0);
            guiWindow.SendVKey(0);
            if (string.IsNullOrEmpty(txtTransportadora.Text))
            {

            }
            else
            {
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08")).Select();
                ((GuiComboBox)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08/ssubSUBSCREEN_BODY:SAPMV45A:4352/subSUBSCREEN_PARTNER_OVERVIEW:SAPLV09C:1000/tblSAPLV09CGV_TC_PARTNER_OVERVIEW/cmbGVS_TC_DATA-REC-PARVW[0,4]")).Key = "SP";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08/ssubSUBSCREEN_BODY:SAPMV45A:4352/subSUBSCREEN_PARTNER_OVERVIEW:SAPLV09C:1000/tblSAPLV09CGV_TC_PARTNER_OVERVIEW/ctxtGVS_TC_DATA-REC-PARTNER[1,4]")).Text = txtTransportadora.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08/ssubSUBSCREEN_BODY:SAPMV45A:4352/subSUBSCREEN_PARTNER_OVERVIEW:SAPLV09C:1000/tblSAPLV09CGV_TC_PARTNER_OVERVIEW/ctxtGVS_TC_DATA-REC-PARTNER[1,4]")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08/ssubSUBSCREEN_BODY:SAPMV45A:4352/subSUBSCREEN_PARTNER_OVERVIEW:SAPLV09C:1000/tblSAPLV09CGV_TC_PARTNER_OVERVIEW/ctxtGVS_TC_DATA-REC-PARTNER[1,4]")).CaretPosition = 10;
                guiWindow.SendVKey(0);
            }
            if (string.IsNullOrEmpty(txtCliente.Text))
            {

            }
            else
            {
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\09")).Select();
                ((GuiShell)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\09/ssubSUBSCREEN_BODY:SAPMV45A:4152/subSUBSCREEN_TEXT:SAPLV70T:2100/cntlSPLITTER_CONTAINER/shellcont/shellcont/shell/shellcont[1]/shell")).Text = "Motorista: " + txtCliente.Text;
            }
            if (string.IsNullOrEmpty(txtPlaca1.Text))
            {

            }
            else
            {
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\10")).Select();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\10/ssubSUBSCREEN_BODY:SAPMV45A:4351/txtVBKD-IHREZ")).Text = txtPlaca1.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\10/ssubSUBSCREEN_BODY:SAPMV45A:4351/txtVBKD-IHREZ")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\10/ssubSUBSCREEN_BODY:SAPMV45A:4351/txtVBKD-IHREZ")).CaretPosition = 10;
            }
            guiWindow.SendVKey(0);
            if (MessageBox.Show("Deseja Concluir?", "Aviso!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ((GuiButton)Session.FindById("wnd[0]/tbar[0]/btn[11]")).Press();
                Session.SendCommand("/NVF01");
                guiWindow.SendVKey(0);
                guiWindow.SendVKey(0);
                ((GuiButton)Session.FindById("wnd[0]/tbar[0]/btn[11]")).Press();

                Session.SendCommand("/NJ1BNFE");
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtBUKRS-LOW")).Text = "USGA";
                ((GuiTextField)Session.FindById("wnd[0]/usr/txtUSERCRE-LOW")).Text = Session.Info.User;
                ((GuiButton)Session.FindById("wnd[0]/tbar[1]/btn[8]")).Press();
            }
        }
        private void LimparCampos()
        {
            txtCPF.Visible = true;
            txtCPFCompleto.Visible = false;
            txtCPFCompleto.Text = "";
            txtTransportadora.Text = "";
            txtPlaca1.Text = "";
            txtPlaca2.Text = "";
            txtPlaca3.Text = "";
            txtPlaca4.Text = "";
            txtQuantidade.Text = "";
            txtCliente.Text = "";
            richObs.Text = "";
            txtOv.Text = "";
            txtCPF.Text = "";
            UltimNumSacaria();
        }
        private void txtCPF_KeyUp(object sender, KeyEventArgs e)
        {

        }
        private void NFBAG()
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
                //Abre Transação
                Session.SendCommand("/NVL01N");
                ProgressBar.Value = 10;
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtLIKP-VSTEL")).Text = "SG01";
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtLV50C-VBELN")).Text = txtOv.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtLV50C-VBELN")).CaretPosition = 5;
                guiWindow.SendVKey(0);
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\02")).Select();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\02/ssubSUBSCREEN_BODY:SAPMV50A:1104/tblSAPMV50ATC_LIPS_PICK/ctxtLIPS-LGORT[3,0]")).Text = "DEPP";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\02/ssubSUBSCREEN_BODY:SAPMV50A:1104/tblSAPMV50ATC_LIPS_PICK/txtLIPSD-G_LFIMG[4,0]")).Text = txtQuantidade.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\02/ssubSUBSCREEN_BODY:SAPMV50A:1104/tblSAPMV50ATC_LIPS_PICK/txtLIPSD-G_LFIMG[4,0]")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\02/ssubSUBSCREEN_BODY:SAPMV50A:1104/tblSAPMV50ATC_LIPS_PICK/txtLIPSD-G_LFIMG[4,0]")).CaretPosition = 2;
                guiWindow.SendVKey(0);
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\02/ssubSUBSCREEN_BODY:SAPMV50A:1104/tblSAPMV50ATC_LIPS_PICK/txtLIPSD-PIKMG[6,0]")).Text = txtQuantidade.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\02/ssubSUBSCREEN_BODY:SAPMV50A:1104/tblSAPMV50ATC_LIPS_PICK/txtLIPSD-PIKMG[6,0]")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\02/ssubSUBSCREEN_BODY:SAPMV50A:1104/tblSAPMV50ATC_LIPS_PICK/txtLIPSD-PIKMG[6,0]")).CaretPosition = 2;
                guiWindow.SendVKey(0);
                ((GuiMenu)Session.FindById("wnd[0]/mbar/menu[2]/menu[2]/menu[5]")).Select();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV50A:3108/txtLIPS-BRGEW")).Text = txtPeso.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV50A:3108/txtLIPS-NTGEW")).Text = txtPeso.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV50A:3108/txtLIPS-VOLUM")).Text = txtQuantidade.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV50A:3108/ctxtLIPS-VOLEH")).Text = "BIG";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV50A:3108/ctxtLIPS-VOLEH")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV50A:3108/ctxtLIPS-VOLEH")).CaretPosition = 3;
                guiWindow.SendVKey(0);
                ((GuiMenu)Session.FindById("wnd[0]/mbar/menu[2]/menu[1]/menu[3]")).Select();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\04/ssubSUBSCREEN_BODY:SAPMV50A:2108/txtLIKP-BOLNR")).Text = "USGA";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\04/ssubSUBSCREEN_BODY:SAPMV50A:2108/ctxtLIKP-TRATY")).Text = "YB04";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\04/ssubSUBSCREEN_BODY:SAPMV50A:2108/txtLIKP-TRAID")).Text = txtPlaca1.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\04/ssubSUBSCREEN_BODY:SAPMV50A:2108/txtLIKP-TRAID")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\04/ssubSUBSCREEN_BODY:SAPMV50A:2108/txtLIKP-TRAID")).CaretPosition = 10;
                guiWindow.SendVKey(0);
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08")).Select();
                ((GuiComboBox)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08/ssubSUBSCREEN_BODY:SAPMV50A:2114/subSUBSCREEN_PARTNER_OVERVIEW:SAPLV09C:1000/tblSAPLV09CGV_TC_PARTNER_OVERVIEW/cmbGVS_TC_DATA-REC-PARVW[0,2]")).Key = "SP";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08/ssubSUBSCREEN_BODY:SAPMV50A:2114/subSUBSCREEN_PARTNER_OVERVIEW:SAPLV09C:1000/tblSAPLV09CGV_TC_PARTNER_OVERVIEW/ctxtGVS_TC_DATA-REC-PARTNER[1,2]")).Text = txtTransportadora.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08/ssubSUBSCREEN_BODY:SAPMV50A:2114/subSUBSCREEN_PARTNER_OVERVIEW:SAPLV09C:1000/tblSAPLV09CGV_TC_PARTNER_OVERVIEW/ctxtGVS_TC_DATA-REC-PARTNER[1,2]")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08/ssubSUBSCREEN_BODY:SAPMV50A:2114/subSUBSCREEN_PARTNER_OVERVIEW:SAPLV09C:1000/tblSAPLV09CGV_TC_PARTNER_OVERVIEW/ctxtGVS_TC_DATA-REC-PARTNER[1,2]")).CaretPosition = 10;
                guiWindow.SendVKey(0);
                if (MessageBox.Show("Deseja Concluir?", "Aviso!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ((GuiButton)Session.FindById("wnd[0]/tbar[1]/btn[20]")).Press();
                    Session.SendCommand("/NVF01");
                    guiWindow.SendVKey(0);
                    guiWindow.SendVKey(0);
                    ((GuiButton)Session.FindById("wnd[0]/tbar[0]/btn[11]")).Press();
                    Session.SendCommand("/NJ1BNFE");
                    ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtBUKRS-LOW")).Text = "USGA";
                    ((GuiTextField)Session.FindById("wnd[0]/usr/txtUSERCRE-LOW")).Text = Session.Info.User;
                    ((GuiButton)Session.FindById("wnd[0]/tbar[1]/btn[8]")).Press();
                }
                ProgressBar.Value = 100;
            }
            catch (Exception ErroSAP)
            {
                MessageBox.Show(ErroSAP.Message);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (Valor == 0)
            {
               SAPAcesso();
            }
            if (Valor == 1)
            {
                NFBAG();
            }
            if (Valor == 2)
            {
                NFOXIGENIO();
            }
            if (Valor == 3)
            {
                SAPAcessoBAG();
            }
            if (Valor == 4)
            {
                NFTIJOLO();
            }
        }
        private void SomaSacaria()
        {
            double DimSacariaFim = Convert.ToDouble(txtQuantidade.Text.Trim()) - 1;
            double ValorInicio = Convert.ToDouble(txtSacariaInic.Text.Trim());
            double SomaSacariaFim = Convert.ToDouble(DimSacariaFim) + ValorInicio;
            txtSacariaFim.Text = SomaSacariaFim.ToString();
        }
        string QUANTBAG;
        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (ckbAcessoBag.Checked)
            {
                QUANTBAG = txtQuantidade.Text.Trim();
                if (e.KeyCode == Keys.Enter)
                {
                    QuantidadeBags = Convert.ToDouble(txtQuantidade.Text);
                    QuantidadeBagsConvertSc = Convert.ToDouble(txtQuantidade.Text) * 1150;
                    txtQuantidade.Text = QuantidadeBagsConvertSc.ToString();
                }
            }
            else
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SomaSacaria();
                }
            }
        }
        int Valor = 0;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbag.Checked)
            {
                //this.Size = new Size(1248, 437);
                //this.StartPosition = new FormStartPosition();
                txtCPF.Enabled = false;
                txtCPFCompleto.Enabled = false;
                txtTransportadora.Enabled = true;
                txtPlaca2.Enabled = false;
                txtPlaca3.Enabled = false;
                txtPlaca4.Enabled = false;
                txtIEST.Enabled = false;
                txtCliente.Enabled = false;
                maskSafra.Enabled = false;
                richObs.Enabled = false;
                txtPeso.Visible = true;
                lblUnd.Visible = false;
                label10.Visible = true;
                btnCriarAcesso.Text = "NF Bag";
                Valor = 1;
                ckOxig.Checked = false;
            }
            else
            {
                txtCPF.Enabled = true;
                txtCPFCompleto.Enabled = true;
                txtTransportadora.Enabled = false;
                txtPlaca2.Enabled = true;
                txtPlaca3.Enabled = true;
                txtPlaca4.Enabled = true;
                txtCliente.Enabled = true;
                maskSafra.Enabled = true;
                richObs.Enabled = true;
                txtPeso.Visible = false;
                lblUnd.Visible = true;
                label10.Visible = false;
                btnCriarAcesso.Text = "Criar Acesso";
                Valor = 0;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja Voltar?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frm_Main frm_Main = new frm_Main();
                frm_Main.Show();
                this.Visible = false;
            }
        }
        private void NFOXIGENIO()
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
                //Abre Transação
                Session.SendCommand("/NVA01");
                ProgressBar.Value = 10;

                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-AUART")).Text = "ZVVD";
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-AUART")).CaretPosition = 4;
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-VKORG")).Text = "";
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-VTWEG")).Text = "";
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-SPART")).Text = "";
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-VKBUR")).Text = "";
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtVBAK-VKGRP")).Text = "";
                guiWindow.SendVKey(0);
                ((GuiTextField)Session.FindById("wnd[0]/usr/subSUBSCREEN_HEADER:SAPMV45A:4021/subPART-SUB:SAPMV45A:4701/ctxtKUAGV-KUNNR")).Text = "4000000003";
                ((GuiTextField)Session.FindById("wnd[0]/usr/subSUBSCREEN_HEADER:SAPMV45A:4021/subPART-SUB:SAPMV45A:4701/ctxtKUAGV-KUNNR")).CaretPosition = 10;
                guiWindow.SendVKey(0);
                ((GuiLabel)Session.FindById("wnd[1]/usr/lbl[7,5]")).SetFocus();
                ((GuiLabel)Session.FindById("wnd[1]/usr/lbl[7,5]")).CaretPosition = 2;
                guiWindow.SendVKey(2);
                ((GuiTextField)Session.FindById("wnd[0]/usr/subSUBSCREEN_HEADER:SAPMV45A:4021/txtVBKD-BSTKD")).Text = txtPedido.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/subSUBSCREEN_HEADER:SAPMV45A:4021/txtVBKD-BSTKD")).CaretPosition = 25;
                guiWindow.SendVKey(0);
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/ctxtRV45A-MABNR[1,0]")).Text = txtSacariaFim.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/txtRV45A-KWMENG[2,0]")).Text = txtQuantidade.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/ctxtVBAP-VRKME[3,0]")).Text = "";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/ctxtVBAP-VRKME[3,0]")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/ctxtVBAP-VRKME[3,0]")).CaretPosition = 0;
                guiWindow.SendVKey(0);
                guiWindow.SendVKey(0);
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/txtVBAP-ARKTX[5,0]")).Text = txtItem.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/txtVBAP-ARKTX[5,0]")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_OVERVIEW/tabpT\\01/ssubSUBSCREEN_BODY:SAPMV45A:4400/subSUBSCREEN_TC:SAPMV45A:4900/tblSAPMV45ATCTRL_U_ERF_AUFTRAG/txtVBAP-ARKTX[5,0]")).CaretPosition = 17;
                guiWindow.SendVKey(2);
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\03")).Select();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\03/ssubSUBSCREEN_BODY:SAPMV45A:4452/txtVBAP-VOLUM")).Text = txtQuantidade.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\03/ssubSUBSCREEN_BODY:SAPMV45A:4452/ctxtVBAP-VOLEH")).Text = "UN.";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\03/ssubSUBSCREEN_BODY:SAPMV45A:4452/ctxtVBAP-VOLEH")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\03/ssubSUBSCREEN_BODY:SAPMV45A:4452/ctxtVBAP-VOLEH")).CaretPosition = 3;
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\04")).Select();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\04/ssubSUBSCREEN_BODY:SAPMV45A:4453/ctxtVBKD-ZLSCH")).Text = "T";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\04/ssubSUBSCREEN_BODY:SAPMV45A:4453/ctxtVBKD-ZLSCH")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\04/ssubSUBSCREEN_BODY:SAPMV45A:4453/ctxtVBKD-ZLSCH")).CaretPosition = 1;
                guiWindow.SendVKey(0);
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05")).Select();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTXSDC")).Text = "I1";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTAXLW1")).Text = "IC0";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BCFOP")).Text = "6101/AA";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTAXLW3")).Text = "";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTAXLW4")).Text = "C01";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTAXLW5")).Text = "P01";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTAXLW5")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4470/ctxtVBAP-J_1BTAXLW5")).CaretPosition = 3;
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\06")).Select();
                ((GuiButton)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\06/ssubSUBSCREEN_BODY:SAPLV69A:6201/subSUBSCREEN_PUSHBUTTONS:SAPLV69A:1000/btnBT_KOAN")).Press();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\06/ssubSUBSCREEN_BODY:SAPLV69A:6201/tblSAPLV69ATCTRL_KONDITIONEN/ctxtKOMV-KSCHL[1,1]")).Text = "ZPB0";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\06/ssubSUBSCREEN_BODY:SAPLV69A:6201/tblSAPLV69ATCTRL_KONDITIONEN/txtKOMV-KBETR[3,1]")).Text = txtOxig.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\06/ssubSUBSCREEN_BODY:SAPLV69A:6201/tblSAPLV69ATCTRL_KONDITIONEN/txtKOMV-KBETR[3,1]")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_ITEM/tabpT\\06/ssubSUBSCREEN_BODY:SAPLV69A:6201/tblSAPLV69ATCTRL_KONDITIONEN/txtKOMV-KBETR[3,1]")).CaretPosition = 16;
                guiWindow.SendVKey(0);
                ((GuiMenu)Session.FindById("wnd[0]/mbar/menu[2]/menu[1]")).Select();
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\05")).Select();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4311/ctxtVBKD-ZLSCH")).Text = "T";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4311/ctxtVBKD-ZLSCH")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\05/ssubSUBSCREEN_BODY:SAPMV45A:4311/ctxtVBKD-ZLSCH")).CaretPosition = 1;
                guiWindow.SendVKey(0);
                guiWindow.SendVKey(0);
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08")).Select();
                ((GuiComboBox)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08/ssubSUBSCREEN_BODY:SAPMV45A:4352/subSUBSCREEN_PARTNER_OVERVIEW:SAPLV09C:1000/tblSAPLV09CGV_TC_PARTNER_OVERVIEW/cmbGVS_TC_DATA-REC-PARVW[0,4]")).Key = "SP";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08/ssubSUBSCREEN_BODY:SAPMV45A:4352/subSUBSCREEN_PARTNER_OVERVIEW:SAPLV09C:1000/tblSAPLV09CGV_TC_PARTNER_OVERVIEW/ctxtGVS_TC_DATA-REC-PARTNER[1,4]")).Text = txtTransportadora.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08/ssubSUBSCREEN_BODY:SAPMV45A:4352/subSUBSCREEN_PARTNER_OVERVIEW:SAPLV09C:1000/tblSAPLV09CGV_TC_PARTNER_OVERVIEW/ctxtGVS_TC_DATA-REC-PARTNER[1,4]")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\08/ssubSUBSCREEN_BODY:SAPMV45A:4352/subSUBSCREEN_PARTNER_OVERVIEW:SAPLV09C:1000/tblSAPLV09CGV_TC_PARTNER_OVERVIEW/ctxtGVS_TC_DATA-REC-PARTNER[1,4]")).CaretPosition = 10;
                guiWindow.SendVKey(0);
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\09")).Select();
                ((GuiShell)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\09/ssubSUBSCREEN_BODY:SAPMV45A:4152/subSUBSCREEN_TEXT:SAPLV70T:2100/cntlSPLITTER_CONTAINER/shellcont/shellcont/shell/shellcont[1]/shell")).Text = "ONU-1072 - PRODUTO PERIGOSO DECLARAMOS QUE PRODUTO ESTA ADEQUADAMENTE EMBALADO /ACONDICIONADO PARA SUPORTAR RISCO NORMAL DE DESCARREGAMENTO, TRANSPORTE E TRANSBORDO E ATENDE A REGULAMENTACAO EM VIGOR.";
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\10")).Select();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\10/ssubSUBSCREEN_BODY:SAPMV45A:4351/txtVBKD-IHREZ")).Text = txtPlaca1.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\10/ssubSUBSCREEN_BODY:SAPMV45A:4351/txtVBKD-IHREZ")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTAXI_TABSTRIP_HEAD/tabpT\\10/ssubSUBSCREEN_BODY:SAPMV45A:4351/txtVBKD-IHREZ")).CaretPosition = 10;
                guiWindow.SendVKey(0);
                if (MessageBox.Show("Deseja Concluir?", "Aviso!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ((GuiButton)Session.FindById("wnd[0]/tbar[0]/btn[11]")).Press();
                    Session.SendCommand("/NVF01");
                    guiWindow.SendVKey(0);
                    guiWindow.SendVKey(0);
                    ((GuiButton)Session.FindById("wnd[0]/tbar[0]/btn[11]")).Press();
                }
                Session.SendCommand("/NJ1B1N");
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtJ_1BDYDOC-NFTYPE")).Text = "I1";
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtJ_1BDYDOC-BUKRS")).Text = "USGA";
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtJ_1BDYDOC-BRANCH")).Text = "0001";
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtJ_1BDYDOC-PARID")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtJ_1BDYDOC-PARID")).CaretPosition = 0;
                guiWindow.SendVKey(0);
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtJ_1BDYDOC-PARID")).Text = "4000000003";
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtJ_1BDYDOC-PARID")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtJ_1BDYDOC-PARID")).CaretPosition = 10;
                guiWindow.SendVKey(0);
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/tblSAPLJ1BB2ITEM_CONTROL/ctxtJ_1BDYLIN-ITMTYP[1,0]")).Text = "1";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/tblSAPLJ1BB2ITEM_CONTROL/ctxtJ_1BDYLIN-MATNR[4,0]")).Text = txtSacariaInic.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/tblSAPLJ1BB2ITEM_CONTROL/ctxtJ_1BDYLIN-WERKS[6,0]")).Text = "USGA";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/tblSAPLJ1BB2ITEM_CONTROL/txtJ_1BDYLIN-MENGE[10,0]")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/tblSAPLJ1BB2ITEM_CONTROL/txtJ_1BDYLIN-MENGE[10,0]")).CaretPosition = 17;
                guiWindow.SendVKey(0);
                guiWindow.SendVKey(0);
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/tblSAPLJ1BB2ITEM_CONTROL/txtJ_1BDYLIN-MENGE[10,0]")).Text = txtQuantidade.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/tblSAPLJ1BB2ITEM_CONTROL/txtJ_1BDYLIN-NETPR[19,0]")).Text = txtCilin.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/tblSAPLJ1BB2ITEM_CONTROL/ctxtJ_1BDYLIN-CFOP[27,0]")).Text = "6920/AA";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/tblSAPLJ1BB2ITEM_CONTROL/ctxtJ_1BDYLIN-TAXLW1[28,0]")).Text = "ZA3";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/tblSAPLJ1BB2ITEM_CONTROL/ctxtJ_1BDYLIN-TAXLW2[29,0]")).Text = "IP1";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/tblSAPLJ1BB2ITEM_CONTROL/ctxtJ_1BDYLIN-TAXLW4[31,0]")).Text = "C07";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/tblSAPLJ1BB2ITEM_CONTROL/ctxtJ_1BDYLIN-TAXLW5[32,0]")).Text = "P07";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/tblSAPLJ1BB2ITEM_CONTROL/txtJ_1BDYLIN-VTOTTRIB[50,0]")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/tblSAPLJ1BB2ITEM_CONTROL/txtJ_1BDYLIN-VTOTTRIB[50,0]")).CaretPosition = 21;
                ((GuiButton)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB1/ssubHEADER_TAB:SAPLJ1BB2:2100/btn%#AUTOTEXT002")).Press();
                double ValorTotal = Convert.ToDouble(txtQuantidade.Text.Replace("R$ ", "")) * Convert.ToDouble(txtCilin.Text.Replace("R$ ", ""));
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsITEM_TAB/tabpTAX/ssubITEM_TABS:SAPLJ1BB2:3200/tblSAPLJ1BB2TAX_CONTROL/ctxtJ_1BDYSTX-TAXTYP[0,0]")).Text = "ICM3";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsITEM_TAB/tabpTAX/ssubITEM_TABS:SAPLJ1BB2:3200/tblSAPLJ1BB2TAX_CONTROL/ctxtJ_1BDYSTX-TAXTYP[0,1]")).Text = "IPI3";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsITEM_TAB/tabpTAX/ssubITEM_TABS:SAPLJ1BB2:3200/tblSAPLJ1BB2TAX_CONTROL/txtJ_1BDYSTX-EXCBAS[6,0]")).Text = ValorTotal.ToString();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsITEM_TAB/tabpTAX/ssubITEM_TABS:SAPLJ1BB2:3200/tblSAPLJ1BB2TAX_CONTROL/txtJ_1BDYSTX-EXCBAS[6,1]")).Text = ValorTotal.ToString();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsITEM_TAB/tabpTAX/ssubITEM_TABS:SAPLJ1BB2:3200/tblSAPLJ1BB2TAX_CONTROL/txtJ_1BDYSTX-EXCBAS[6,1]")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsITEM_TAB/tabpTAX/ssubITEM_TABS:SAPLJ1BB2:3200/tblSAPLJ1BB2TAX_CONTROL/txtJ_1BDYSTX-EXCBAS[6,1]")).CaretPosition = 21;
                guiWindow.SendVKey(0);
                ((GuiMenu)Session.FindById("wnd[0]/mbar/menu[2]/menu[2]/menu[0]")).Select();
                ((GuiComboBox)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB3/ssubHEADER_TAB:SAPLJ1BB2:2300/tblSAPLJ1BB2PARTNER_CONTROL/cmbJ_1BDYNAD-PARVW[0,1]")).Key = "SP";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB3/ssubHEADER_TAB:SAPLJ1BB2:2300/tblSAPLJ1BB2PARTNER_CONTROL/ctxtJ_1BDYNAD-PARID[1,1]")).Text = txtTransportadora.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB3/ssubHEADER_TAB:SAPLJ1BB2:2300/tblSAPLJ1BB2PARTNER_CONTROL/ctxtJ_1BDYNAD-PARID[1,1]")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB3/ssubHEADER_TAB:SAPLJ1BB2:2300/tblSAPLJ1BB2PARTNER_CONTROL/ctxtJ_1BDYNAD-PARID[1,1]")).CaretPosition = 10;
                guiWindow.SendVKey(0);
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB5")).Select();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB5/ssubHEADER_TAB:SAPLJ1BB2:2500/ctxtJ_1BDYDOC-TRATY")).Text = "YB04";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB5/ssubHEADER_TAB:SAPLJ1BB2:2500/txtJ_1BDYDOC-TRAID")).Text = txtPlaca1.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB5/ssubHEADER_TAB:SAPLJ1BB2:2500/ctxtJ_1BDYDOC-INCO1")).Text = "FOB";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB5/ssubHEADER_TAB:SAPLJ1BB2:2500/txtJ_1BDYDOC-INCO2")).Text = "SAO JOSE DA LAJE";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB5/ssubHEADER_TAB:SAPLJ1BB2:2500/ctxtJ_1BDYDOC-VSTEL")).Text = "SG01";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB5/ssubHEADER_TAB:SAPLJ1BB2:2500/txtJ_1BDYDOC-ANZPK")).Text = txtQuantidade.Text;
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB5/ssubHEADER_TAB:SAPLJ1BB2:2500/ctxtJ_1BDYDOC-SHPUNT")).Text = "CL.";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB5/ssubHEADER_TAB:SAPLJ1BB2:2500/txtJ_1BDYDOC-SHPMRK")).Text = "USGA";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB5/ssubHEADER_TAB:SAPLJ1BB2:2500/txtJ_1BDYDOC-SHPMRK")).SetFocus();
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB5/ssubHEADER_TAB:SAPLJ1BB2:2500/txtJ_1BDYDOC-SHPMRK")).CaretPosition = 4;
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB6")).Select();
                ((GuiComboBox)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB6/ssubHEADER_TAB:SAPLJ1BB2:2600/tblSAPLJ1BB2PAYMENT_CONTROL/cmbJ_1BDYPAYMENT-T_PAG[2,0]")).Key = "16";
                ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB6/ssubHEADER_TAB:SAPLJ1BB2:2600/tblSAPLJ1BB2PAYMENT_CONTROL/txtJ_1BDYPAYMENT-V_PAG[3,0]")).Text = ValorTotal.ToString();
                ((GuiComboBox)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB6/ssubHEADER_TAB:SAPLJ1BB2:2600/tblSAPLJ1BB2PAYMENT_CONTROL/cmbJ_1BDYPAYMENT-TP_INTEGRA[4,0]")).Key = "2";
                ((GuiComboBox)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB6/ssubHEADER_TAB:SAPLJ1BB2:2600/tblSAPLJ1BB2PAYMENT_CONTROL/cmbJ_1BDYPAYMENT-TP_INTEGRA[4,0]")).SetFocus();
                ((GuiTab)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB8")).Select();
                ((GuiComboBox)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB8/ssubHEADER_TAB:SAPLJ1BB2:2800/cmbJ_1BDYDOC-INDINTERMED")).Key = "0";
                ((GuiComboBox)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB8/ssubHEADER_TAB:SAPLJ1BB2:2800/cmbJ_1BDYDOC-INDINTERMED")).SetFocus();
                if (MessageBox.Show("Deseja Concluir?", "Aviso!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ((GuiButton)Session.FindById("wnd[0]/tbar[0]/btn[11]")).Press();

                    Session.SendCommand("/NJ1BNFE");
                    ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtBUKRS-LOW")).Text = "USGA";
                    ((GuiTextField)Session.FindById("wnd[0]/usr/txtUSERCRE-LOW")).Text = Session.Info.User;
                    ((GuiButton)Session.FindById("wnd[0]/tbar[1]/btn[8]")).Press();
                }
                ProgressBar.Value = 100;
            }
            catch (Exception ErroSAP)
            {
                MessageBox.Show(ErroSAP.Message);
            }
        }
        private void ckOxig_CheckedChanged(object sender, EventArgs e)
        {
            if (ckOxig.Checked)
            {
                //this.Size = new Size(1248, 437);
                //this.StartPosition = new FormStartPosition();
                txtCPF.Enabled = false;
                txtCPFCompleto.Enabled = false;
                txtTransportadora.Enabled = true;
                txtPlaca2.Enabled = false;
                txtPlaca3.Enabled = false;
                txtPlaca4.Enabled = false;
                txtIEST.Enabled = false;
                txtCliente.Enabled = false;
                maskSafra.Enabled = false;
                txtOv.Enabled = false;
                richObs.Visible = false;
                label5.Visible = false;
                maskCliente.Visible = false;
                txtPeso.Visible = true;
                lblUnd.Visible = false;
                label10.Visible = true;
                label12.Visible = true;
                label13.Visible = true;
                txtPedido.Visible = true;
                txtItem.Visible = true;
                txtOxig.Visible = true;
                txtCilin.Visible = true;
                btnCriarAcesso.Text = "NF Oxig.";
                txtTransportadora.Text = "1200001907";
                txtPlaca1.Text = "PGP9466/PE";
                txtSacariaFim.Enabled = true;
                txtSacariaFim.Text = "100111";
                txtSacariaInic.Enabled = true;
                txtSacariaInic.Text = "672005";
                label15.Visible = true;
                lblSacaria.Visible = false;
                txtPeso.Visible = false;
                Valor = 2;
                ckbag.Checked = false;
            }
            else
            {
                txtCPF.Enabled = true;
                txtCPFCompleto.Enabled = true;
                txtTransportadora.Enabled = false;
                txtPlaca2.Enabled = true;
                txtPlaca3.Enabled = true;
                txtPlaca4.Enabled = true;
                txtCliente.Enabled = true;
                maskSafra.Enabled = true;
                txtOv.Enabled = true;
                txtPeso.Visible = false;
                richObs.Visible = true;
                maskCliente.Visible = true;
                label5.Visible = true;
                lblUnd.Visible = true;
                label10.Visible = false;
                label12.Visible = false;
                label13.Visible = false;
                txtPedido.Visible = false;
                txtItem.Visible = false;
                btnCriarAcesso.Text = "Criar Acesso";
                txtTransportadora.Text = "";
                txtSacariaFim.Enabled = false;
                txtSacariaFim.Text = "";
                txtSacariaInic.Enabled = false;
                txtSacariaInic.Text = "";
                lblSacaria.Visible = true;
                txtSacariaInic.Visible = true;
                label15.Visible = false;
                txtPlaca1.Visible = true;
                txtOxig.Visible = false;
                txtCilin.Visible = false;
                txtPeso.Text = "";
                Valor = 0;
                UltimNumSacaria();
            }
        }
        private void txtQuantidade_DoubleClick(object sender, EventArgs e)
        {

        }
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ckbAcessoBag.Checked)
            {
                Valor = 3;
                lblUnd.Text = "BAG";
                txtQuantidade.Text = "";
            }
            else
            {
                Valor = 0;
                lblUnd.Text = "SACOS";
                txtQuantidade.Text = "";
            }
        }
        private void checkBox1_CheckedChanged_2(object sender, EventArgs e)
        {
            if (chkTij.Checked)
            {
                //this.Size = new Size(1248, 437);
                //this.StartPosition = new FormStartPosition();
                txtCPF.Enabled = false;
                txtCPFCompleto.Enabled = false;
                txtTransportadora.Enabled = true;
                txtPlaca2.Enabled = false;
                txtPlaca3.Enabled = false;
                txtPlaca4.Enabled = false;
                txtIEST.Enabled = false;
                txtCliente.Enabled = false;
                maskSafra.Enabled = false;
                txtOv.Enabled = false;
                richObs.Visible = false;
                label5.Visible = false;
                maskCliente.Visible = false;
                txtPeso.Visible = true;
                lblUnd.Visible = false;
                label10.Visible = true;
                label12.Visible = true;
                label13.Visible = true;
                txtPedido.Visible = true;
                txtItem.Visible = true;
                txtCliente.Enabled = true;
                btnCriarAcesso.Text = "NF Tij.";
                txtTransportadora.Text = "";
                txtPlaca1.Text = "NMG1693/AL";
                txtPedido.Text = "VENDA TIJOLOS";
                txtItem.Text = "TIJOLOS 8 FUROS";
                txtClienteTijolo.Visible = true;
                lblClienteTijolo.Visible = true;
                txtCliente.Text = "ROBERTO VICENTE";
                txtPeso.Text = "0,48";
                lblCliente.Text = "Motorista";
                txtSacariaFim.Enabled = true;
                txtSacariaFim.Text = "100102";
                label15.Visible = true;
                lblSacaria.Visible = false;
                txtSacariaInic.Visible = false;
                Valor = 4;
                ckbag.Checked = false;
            }
            else
            {
                lblCliente.Text = "Cliente";
                txtCPF.Enabled = true;
                txtCPFCompleto.Enabled = true;
                txtTransportadora.Enabled = false;
                txtPlaca2.Enabled = true;
                txtPlaca3.Enabled = true;
                txtPlaca4.Enabled = true;
                txtCliente.Enabled = true;
                maskSafra.Enabled = true;
                txtOv.Enabled = true;
                txtPeso.Visible = false;
                richObs.Visible = true;
                maskCliente.Visible = true;
                label5.Visible = true;
                lblUnd.Visible = true;
                label10.Visible = false;
                label12.Visible = false;
                label13.Visible = false;
                txtPedido.Visible = false;
                txtItem.Visible = false;
                txtClienteTijolo.Visible = false;
                lblClienteTijolo.Visible = false;
                btnCriarAcesso.Text = "Criar Acesso";
                txtTransportadora.Text = "";
                txtCliente.Text = "";
                txtPedido.Text = "";
                txtItem.Text = "";
                txtPlaca1.Text = "";
                txtPeso.Text = "";
                lblSacaria.Visible = true;
                txtSacariaInic.Visible = true;
                txtSacariaFim.Enabled = false;
                label15.Visible = false;
                txtSacariaFim.Text = "";
                Valor = 0;
                UltimNumSacaria();
            }
        }
        private void txtPlaca1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    //Consulta Itens no Banco de Dados
                    MySqlCommand MyCommand = new MySqlCommand();
                    MyCommand.Connection = ConexaoDados.GetConnectionFaturameto();
                    MyCommand.CommandText = "SELECT * FROM tb_acesso WHERE col_placa_1='" + txtPlaca1.Text + "'";
                    MySqlDataReader dreader = MyCommand.ExecuteReader();
                    while (dreader.Read())
                    {
                        txtPlaca2.Text = dreader["col_placa_2"].ToString();
                        txtPlaca3.Text = dreader["col_placa_3"].ToString();
                        txtPlaca4.Text = dreader["col_placa_4"].ToString();
                    }
                    ConexaoDados.GetConnectionFaturameto().Close();
                }
                catch (Exception Err)
                {
                    txtPlaca2.Focus();
                }
            }
        }
        private void txtSacariaInic_DoubleClick(object sender, EventArgs e)
        {
        }
        private void txtSacariaInic_Click(object sender, EventArgs e)
        {
        }
        private void lblSacaria_Click(object sender, EventArgs e)
        {
            var conexaoform = new FormCadSacaria();
            conexaoform.Show();
        }
        private void cmbCliente_SelectedValueChanged(object sender, EventArgs e)
        {
            CCliente = cmbCliente.SelectedValue.ToString();
        }

        private void lblCliente_Click(object sender, EventArgs e)
        {
            var FichaCadastroCliente = new FichaCadastroCliente(CCliente);
            FichaCadastroCliente.Show();
        }
    }
}

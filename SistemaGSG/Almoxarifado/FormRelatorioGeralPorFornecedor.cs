﻿using java.util;
using javax.swing.text.html.parser;
using K4os.Compression.LZ4.Internal;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Renci.SshNet;
using SAPFEWSELib;
using SapROTWr;
using SistemaGSG.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaGSG.Almoxarifado
{
    public partial class FormRelatorioGeralPorFornecedor : MetroFramework.Forms.MetroForm
    {
        DataTable table = new DataTable();

        public FormRelatorioGeralPorFornecedor()
        {
            InitializeComponent();
        }

        private void CarregarGridView()
        {
            table.Columns.Add("-*-", typeof(string));
            table.Columns.Add("CÓDIGO DO FORNECEDOR", typeof(string));
            table.Columns.Add("NOME DO FORNECEDOR", typeof(string));
            table.Columns.Add("CÓDIGO DO MATERIAL", typeof(string));
            table.Columns.Add("DESCRIÇÃO DO MATERIAL", typeof(string));
            table.Columns.Add("NOTA FISCAL", typeof(string));
            table.Columns.Add("SÉRIE", typeof(string));
            table.Columns.Add("QUANTIDADE", typeof(string));
            table.Columns.Add("UNIDADE DE MEDIDA", typeof(string));
            table.Columns.Add("VALOR", typeof(string));
            table.Columns.Add("DOCUMENTO", typeof(string));
            table.Columns.Add("7", typeof(string));
            table.Columns.Add("8", typeof(string));
            ZMM039.DataSource = table;
        }
        string codigoFornecedor;
        string nomeFornecedor;
        string materialSAP;
        string materialDescricaoSAP;
        string notaFiscal;
        double quantidade;
        string unidadeMedida;
        double valor;
        string docNumeroMiro;
        string[] pedidoCompra;
        string[] requisicaoCompra;
        string[] requisitanteCompra;
        int posicao = 40000;
        string MARCA;
        string REFERENCIA;
        string REQC;
        string MATERIAL;
        string ITEM;
        string docNumeroMiroVerifica;
        string materialSAPCod;
        string[] materialSAPCodVer;
        string[] nomeMaterialSAPCodVer;
        string updateRelRequisicoes;
        string updateRelPedido;
        string[] quantidadeItemPedido;
        string grupoCompradores;
        string iconMarcaRC;
        string idPedido;

        private void PreencherTextBox()
        {
            int count = ZMM039.RowCount;
            int linha = 0;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    codigoFornecedor = ZMM039.Rows[linha].Cells["CÓDIGO DO FORNECEDOR"].Value.ToString().Trim();
                    nomeFornecedor = ZMM039.Rows[linha].Cells["NOME DO FORNECEDOR"].Value.ToString().Trim();
                    materialSAP = ZMM039.Rows[linha].Cells["CÓDIGO DO MATERIAL"].Value.ToString().Trim();
                    materialDescricaoSAP = ZMM039.Rows[linha].Cells["DESCRIÇÃO DO MATERIAL"].Value.ToString().Trim();
                    notaFiscal = ZMM039.Rows[linha].Cells["NOTA FISCAL"].Value.ToString().Trim();
                    quantidade = double.Parse(ZMM039.Rows[linha].Cells["QUANTIDADE"].Value.ToString().Trim());
                    unidadeMedida = ZMM039.Rows[linha].Cells["UNIDADE DE MEDIDA"].Value.ToString().Trim();
                    valor = double.Parse(ZMM039.Rows[linha].Cells["VALOR"].Value.ToString().Trim());
                    docNumeroMiro = ZMM039.Rows[linha].Cells["DOCUMENTO"].Value.ToString().Trim();
                    ImportarDataGrid();
                    linha++;
                    posicao++;
                }
                catch (FormatException Errp)
                {
                    log.WriteLog("Warning : " + Errp.Message);
                }
                catch (Exception Error)
                {
                    log.WriteLog("Warning : " + Error.Message);
                    MessageBox.Show(Error.Message);
                }

            }
        }
        private void ImportarDataGrid()
        {
            try
            {
                if (string.IsNullOrEmpty(codigoFornecedor) || string.IsNullOrEmpty(docNumeroMiro))
                {

                }
                else
                {
                    try
                    {
                        //Abrir Conexão.
                        MySqlCommand prompt = new MySqlCommand("SELECT COUNT(*) FROM `tb_relatorioentradafornecedor` WHERE col_codigoFornecedor='" + codigoFornecedor + "'", ConexaoDados.GetConnectionAlmoxarifado());
                        prompt.ExecuteNonQuery();//Executa o comando.
                        int consultDB = Convert.ToInt32(prompt.ExecuteScalar());//Converte o resultado para números inteiros.
                        if (consultDB == 0)//Verifica se o resultado for maior que zero(0), a execução inicia a Menssagem de que já existe contas, caso contrario faz a inserção no Banco.
                        {
                            try
                            {
                                MySqlCommand cmd = new MySqlCommand("INSERT INTO tb_relatorioentradafornecedor (col_codigoFornecedor, col_descricaoFornecedor, col_materialCodigoSAP, col_textoBreveMaterial, col_numeroNotaFiscal, col_quantidade, col_unidadeMedida, col_valorMaterialSAP, col_documentoMiroSAP, col_pedido, col_requisicao,col_posicao,col_dataImportacao) " +
                                    "VALUES " +
                                    "('" + codigoFornecedor + "','" + nomeFornecedor + "','" + materialSAP + "','" + materialDescricaoSAP.Replace("'", "") + "','" + notaFiscal + "','" + quantidade.ToString().Replace(",", ".") + "','" + unidadeMedida.ToString().Replace("******", "PC") + "','" + valor.ToString().Replace(",", ".") + "','" + docNumeroMiro + "','0','0','" + posicao + "', NOW())", ConexaoDados.GetConnectionAlmoxarifado());
                                cmd.ExecuteNonQuery();
                                cmd.Connection.Close();
                                prompt.Connection.Close();
                            }
                            catch (Exception Err)
                            {
                                log.WriteLog("Warning : " + Err.Message);
                                MessageBox.Show(Err.Message);
                            }
                            finally
                            {
                                ConexaoDados.GetConnectionAlmoxarifado().Close();
                            }
                        }
                        else if (consultDB > 0)
                        {

                            try
                            {
                                MySqlCommand MyCommand = new MySqlCommand();
                                MyCommand.Connection = ConexaoDados.GetConnectionAlmoxarifado();
                                MyCommand.CommandText = "SELECT col_posicao,col_codigoFornecedor FROM `tb_relatorioentradafornecedor` WHERE col_codigoFornecedor='" + codigoFornecedor + "'";
                                MySqlDataReader dreader = MyCommand.ExecuteReader();
                                while (dreader.Read())
                                {
                                    posicao = int.Parse(dreader["col_posicao"].ToString());
                                    break;
                                }
                                MySqlCommand cmd = new MySqlCommand("INSERT INTO tb_relatorioentradafornecedor (col_codigoFornecedor, col_descricaoFornecedor, col_materialCodigoSAP, col_textoBreveMaterial, col_numeroNotaFiscal, col_quantidade, col_unidadeMedida, col_valorMaterialSAP, col_documentoMiroSAP, col_pedido, col_requisicao,col_posicao,col_dataImportacao) " +
                                    "VALUES " +
                                    "('" + codigoFornecedor + "','" + nomeFornecedor + "','" + materialSAP + "','" + materialDescricaoSAP.Replace("'", "") + "','" + notaFiscal + "','" + quantidade.ToString().Replace(",", ".") + "','" + unidadeMedida.ToString().Replace("******", "PC") + "','" + valor.ToString().Replace(",", ".") + "','" + docNumeroMiro + "','0','0','" + posicao + "', NOW())", ConexaoDados.GetConnectionAlmoxarifado());
                                cmd.ExecuteNonQuery();
                                cmd.Connection.Close();
                                MyCommand.Connection.Close();
                            }
                            catch (Exception Err)
                            {
                                log.WriteLog("Warning : " + Err.Message);
                                MessageBox.Show(Err.Message);
                            }
                            finally
                            {
                                ConexaoDados.GetConnectionAlmoxarifado().Close();
                            }
                        }
                    }
                    catch (NullReferenceException)
                    {
                        MessageBox.Show("Olá Srº(a), " + dados.Usuario + " Erro na plicação!.");
                    }
                }
            }
            catch (Exception Err)
            {
                log.WriteLog("Warning : " + Err.Message);
                MessageBox.Show(Err.Message);
            }
        }
        string[] lines;
        private void EnviarParaOBancoDeDados()
        {
            lines = File.ReadAllLines("C:\\ArquivosSAP\\ZMM039.txt", Encoding.UTF7);
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
        private void PreencherTextBoxZMM040()
        {
            int count = ZMM039.RowCount;
            int linha = 0;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    codigoFornecedor = ZMM039.Rows[linha].Cells["CÓDIGO DO FORNECEDOR"].Value.ToString().Trim();
                    nomeFornecedor = ZMM039.Rows[linha].Cells["NOME DO FORNECEDOR"].Value.ToString().Trim();
                    materialSAP = ZMM039.Rows[linha].Cells["DESCRIÇÃO DO MATERIAL"].Value.ToString().Trim();
                    ImportarDataGridZMM040();
                    linha++;
                    posicao++;
                }
                catch (FormatException Errp)
                {
                    log.WriteLog("Warning : " + Errp.Message);
                }
                catch (Exception Error)
                {
                    log.WriteLog("Warning : " + Error.Message);
                    MessageBox.Show(Error.Message);
                }

            }
        }
        private void EnviarParaOBancoDeDadosZMM040()
        {
            lines = File.ReadAllLines("C:\\ArquivosSAP\\ZMM040.txt", Encoding.UTF7);
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
        private void ImportarDataGridZMM040()
        {
            try
            {
                if (nomeFornecedor == "ITEM BLOQUEADO")
                {

                }
                else
                {
                    MySqlCommand prompt = new MySqlCommand("SELECT COUNT(*) FROM `tb_materiais` WHERE col_codigoMaterial='" + codigoFornecedor + "'", ConexaoDados.GetConnectionAlmoxarifado());
                    prompt.ExecuteNonQuery();
                    int consultDB = Convert.ToInt32(prompt.ExecuteScalar());
                    if (consultDB == 0)
                    {
                        if (string.IsNullOrEmpty(codigoFornecedor))
                        {

                        }
                        else
                        {
                            try
                            {
                                MySqlCommand cmd = new MySqlCommand("INSERT INTO tb_materiais (col_codigoMaterial, col_descricaoMaterial, col_depositoMaterial) " +
                                    "VALUES " +
                                    "('" + codigoFornecedor + "','" + nomeFornecedor.Replace("'", "") + "','" + materialSAP + "')", ConexaoDados.GetConnectionAlmoxarifado());
                                cmd.ExecuteNonQuery();
                                cmd.Connection.Close();
                            }
                            catch (Exception Err)
                            {
                                log.WriteLog("Warning : " + Err.Message);
                                MessageBox.Show(Err.Message);
                            }
                        }
                    }
                    prompt.Connection.Close();
                }
            }
            catch (Exception Err)
            {
                log.WriteLog("Warning : " + Err.Message);
                MessageBox.Show(Err.Message);
            }
            finally
            {
                ConexaoDados.GetConnectionAlmoxarifado().Close();
            }
        }
        private void RelatorioZMM039()
        {
            CSapROTWrapper sapROTWrapper = new CSapROTWrapper();
            object SapGuilRot = sapROTWrapper.GetROTEntry("SAPGUI");
            object engine = SapGuilRot.GetType().InvokeMember("GetScriptingEngine", System.Reflection.BindingFlags.InvokeMethod, null, SapGuilRot, null);
            GuiApplication GuiApp = (GuiApplication)engine;
            GuiConnection connection = (GuiConnection)GuiApp.Connections.ElementAt(0);
            GuiSession Session = (GuiSession)connection.Children.ElementAt(0);
            GuiFrameWindow guiWindow = Session.ActiveWindow;
            guiWindow.Maximize();
            Session.SendCommand("/NZMM039");
            ((GuiCheckBox)Session.FindById("wnd[0]/usr/chkP_DEVOL")).Selected = true;
            ((GuiCheckBox)Session.FindById("wnd[0]/usr/chkP_CANC")).Selected = true;
            ((GuiCheckBox)Session.FindById("wnd[0]/usr/chkP_CANCD")).Selected = true;
            ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtGP_BUKRS")).Text = "USGA";
            ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtGSO_PERI-LOW")).Text = this.calendarioMes.SelectionStart.ToString("dd.MM.yyyy");
            ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtGSO_PERI-HIGH")).Text = this.calendarioMes.SelectionEnd.ToString("dd.MM.yyyy");
            //((GuiTextField)Session.FindById("wnd[0]/usr/ctxtGP_PARID-LOW")).Text = "1200000123";
            ((GuiButton)Session.FindById("wnd[0]/tbar[1]/btn[8]")).Press();
            ((GuiButton)Session.FindById("wnd[0]/tbar[1]/btn[33]")).Press();
            ((GuiComboBox)Session.FindById("wnd[1]/usr/ssubD0500_SUBSCREEN:SAPLSLVC_DIALOG:0501/cmbG51_USPEC_LBOX")).Key = "X";
            ((GuiGridView)Session.FindById("wnd[1]/usr/ssubD0500_SUBSCREEN:SAPLSLVC_DIALOG:0501/cntlG51_CONTAINER/shellcont/shell")).CurrentCellColumn = "TEXT";
            ((GuiGridView)Session.FindById("wnd[1]/usr/ssubD0500_SUBSCREEN:SAPLSLVC_DIALOG:0501/cntlG51_CONTAINER/shellcont/shell")).SelectedRows = "0";
            ((GuiGridView)Session.FindById("wnd[1]/usr/ssubD0500_SUBSCREEN:SAPLSLVC_DIALOG:0501/cntlG51_CONTAINER/shellcont/shell")).ClickCurrentCell();
            ((GuiMenu)Session.FindById("wnd[0]/mbar/menu[0]/menu[3]/menu[2]")).Select();
            ((GuiButton)Session.FindById("wnd[1]/tbar[0]/btn[0]")).Press();
            ((GuiTextField)Session.FindById("wnd[1]/usr/ctxtDY_PATH")).Text = "C:\\ArquivosSAP\\";
            ((GuiTextField)Session.FindById("wnd[1]/usr/ctxtDY_FILENAME")).Text = "ZMM039.txt";
            ((GuiButton)Session.FindById("wnd[1]/tbar[0]/btn[11]")).Press();
            MySqlCommand CommandDelete = new MySqlCommand("TRUNCATE `tb_relatorioentradafornecedor`", ConexaoDados.GetConnectionAlmoxarifado());
            CommandDelete.ExecuteNonQuery();
            CommandDelete.Connection.Close();
            MySqlCommand CommandoDelete = new MySqlCommand("TRUNCATE `tb_marcareferencia`", ConexaoDados.GetConnectionAlmoxarifado());
            CommandoDelete.ExecuteNonQuery();
            CommandoDelete.Connection.Close();
            ConexaoDados.GetConnectionAlmoxarifado().Close();
            EnviarParaOBancoDeDados();
            PreencherTextBox();
            log.WriteLog("Info : ZMM039 Iniciada");
            int linha = 0;
            int i;
            int contRows = 0;
            pedidoCompra = new string[lines.Length];
            materialSAPCodVer = new string[lines.Length];
            requisicaoCompra = new string[lines.Length];
            requisitanteCompra = new string[lines.Length];
            ((GuiGridView)Session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell")).CurrentCellColumn = "BELNR";
            int linha1 = 1;
            while (linha <= lines.Length)
            {
                try
                {
                    docNumeroMiro = ((GuiGridView)Session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell")).GetCellValue(linha, "BELNR");
                }
                catch (Exception errPedido)
                {
                    log.WriteLog(errPedido.Message + " " + docNumeroMiro);
                }
                if (docNumeroMiro == docNumeroMiroVerifica)
                {
                    linha++;
                    contRows++;
                }
                else if (string.IsNullOrEmpty(docNumeroMiro))
                {
                    linha++;
                    contRows++;
                }
                else
                {
                    ((GuiGridView)Session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell")).CurrentCellRow = contRows;
                    ((GuiGridView)Session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell")).SelectedRows = contRows.ToString();
                    docNumeroMiro = ((GuiGridView)Session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell")).GetCellValue(linha, "BELNR");
                    ((GuiGridView)Session.FindById("wnd[0]/usr/cntlGRID1/shellcont/shell")).ClickCurrentCell();
                    ((GuiTextField)Session.FindById("wnd[0]/usr/subHEADER_AND_ITEMS:SAPLMR1M:6005/subITEMS:SAPLMR1M:6010/tabsITEMTAB/tabpITEMS_PO/ssubTABS:SAPLMR1M:6020/subITEM:SAPLMR1M:6310/tblSAPLMR1MTC_MR1M/txtDRSEG-EBELN[5,0]")).SetFocus();
                    ((GuiTextField)Session.FindById("wnd[0]/usr/subHEADER_AND_ITEMS:SAPLMR1M:6005/subITEMS:SAPLMR1M:6010/tabsITEMTAB/tabpITEMS_PO/ssubTABS:SAPLMR1M:6020/subITEM:SAPLMR1M:6310/tblSAPLMR1MTC_MR1M/txtDRSEG-EBELN[5,0]")).CaretPosition = 7;

                    bool rowT = true;
                    int rowD = 0;
                    int rowE = 0;
                    int v = 0;
                    int position = 1;
                    Array.Clear(pedidoCompra, 0, pedidoCompra.Length);
                    Array.Clear(materialSAPCodVer, 0, materialSAPCodVer.Length);
                    while (rowT == true)
                    {
                        try
                        {
                            if (rowD == 16)
                            {
                                ((GuiTableControl)Session.FindById("wnd[0]/usr/subHEADER_AND_ITEMS:SAPLMR1M:6005/subITEMS:SAPLMR1M:6010/tabsITEMTAB/tabpITEMS_PO/ssubTABS:SAPLMR1M:6020/subITEM:SAPLMR1M:6310/tblSAPLMR1MTC_MR1M")).VerticalScrollbar.Position = position;
                                rowD--;
                                position++;
                            }
                            pedidoCompra[v] = ((GuiTextField)Session.FindById("wnd[0]/usr/subHEADER_AND_ITEMS:SAPLMR1M:6005/subITEMS:SAPLMR1M:6010/tabsITEMTAB/tabpITEMS_PO/ssubTABS:SAPLMR1M:6020/subITEM:SAPLMR1M:6310/tblSAPLMR1MTC_MR1M/txtDRSEG-EBELN[5," + rowD + "]")).Text;
                            materialSAPCod = ((GuiTextField)Session.FindById("wnd[0]/usr/subHEADER_AND_ITEMS:SAPLMR1M:6005/subITEMS:SAPLMR1M:6010/tabsITEMTAB/tabpITEMS_PO/ssubTABS:SAPLMR1M:6020/subITEM:SAPLMR1M:6310/tblSAPLMR1MTC_MR1M/ctxtDRSEG-MATNR[72," + rowD + "]")).Text;
                            if (pedidoCompra[v] == "__________")
                            {
                                break;
                            }
                            else
                            {
                                MySqlCommand mySql = new MySqlCommand("UPDATE `tb_relatorioentradafornecedor` SET `col_pedido` = '" + pedidoCompra[v] + "' WHERE col_materialCodigoSAP = '" + materialSAPCod + "' AND col_documentoMiroSAP='" + docNumeroMiro + "'", ConexaoDados.GetConnectionAlmoxarifado());
                                mySql.ExecuteNonQuery();
                                mySql.Connection.Close();
                                ConexaoDados.GetConnectionAlmoxarifado().Close();
                            }
                        }
                        catch (ArgumentException ErroSAP)
                        {
                            log.WriteLog("Warning : " + ErroSAP.Message);
                            break;
                        }
                        catch (Exception ErroProg)
                        {
                            log.WriteLog("Warning : " + ErroProg.Message);
                            MessageBox.Show("Erro Printar a tela e Enviar ao Administrador do Programa." + ErroProg.Message);
                        }
                        materialSAPCodVer[v] = materialSAPCod;
                        rowE = rowD;
                        rowD++;
                        v++;
                    }
                    ((GuiButton)Session.FindById("wnd[0]/tbar[0]/btn[3]")).Press();
                    //((GuiButton)Session.FindById("wnd[0]/tbar[0]/btn[3]")).Press();
                    docNumeroMiroVerifica = docNumeroMiro;
                    linha++;
                    contRows++;
                }
            }
            log.WriteLog("Info : ZMM039 Finalizado");
        }
        private void criarPeriodo()
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand("INSERT INTO `tb_periodos` (`col_inicio`, `col_fim`) " +
                "VALUES " +
                "('" + this.calendarioMes.SelectionStart.ToString("yyyy-MM-dd") + "', '" + this.calendarioMes.SelectionEnd.ToString("yyyy-MM-dd") + "')", ConexaoDados.GetConnectionAlmoxarifado());
                mySqlCommand.ExecuteNonQuery();
                mySqlCommand.Connection.Close();
                log.WriteLog("Info : Período Inserido com Sucesso!.");
            }
            catch(Exception ErroProg)
            {
                log.WriteLog("Warning : " + ErroProg.Message);
            }
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            log.WriteLog("Info : Iniciado o relatório de materiais por fornecedor - Almoxarifado.");
            var sw = new Stopwatch();
            sw.Start();

            criarPeriodo();
            RelatorioZMM039();
            VerificaPedido();
            VerificarRequisicao();

            sw.Stop();
            MessageBox.Show("Tempo Decorrido... " + sw.Elapsed.ToString(@"hh\:mm\:ss"), "SAP for Windows 7.70 - Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            log.WriteLog("Info : Tempo Decorrido... " + sw.Elapsed.ToString(@"hh\:mm\:ss"));
        }
        private void VerificaPedido()
        {
            CSapROTWrapper sapROTWrapper = new CSapROTWrapper();
            object SapGuilRot = sapROTWrapper.GetROTEntry("SAPGUI");
            object engine = SapGuilRot.GetType().InvokeMember("GetScriptingEngine", System.Reflection.BindingFlags.InvokeMethod, null, SapGuilRot, null);
            GuiApplication GuiApp = (GuiApplication)engine;
            GuiConnection connection = (GuiConnection)GuiApp.Connections.ElementAt(0);
            GuiSession Session = (GuiSession)connection.Children.ElementAt(0);
            GuiFrameWindow guiWindow = Session.ActiveWindow;
            //guiWindow.Maximize();
            Session.SendCommand("/NME23N");
            log.WriteLog("Info : ME23N Iniciado");
            bool IniWhile = true;

            while (IniWhile == true)
            {
                ((GuiButton)Session.FindById("wnd[0]/tbar[1]/btn[17]")).Press();
                try
                {
                    MySqlCommand MyCommand = new MySqlCommand();
                    MyCommand.Connection = ConexaoDados.GetConnectionAlmoxarifado();
                    MyCommand.CommandText = "SELECT * FROM `tb_relatorioentradafornecedor` WHERE col_statusPedido IS null ORDER BY `col_descricaoFornecedor` ASC";
                    MySqlDataReader dreader = MyCommand.ExecuteReader();
                    while (dreader.Read())
                    {
                        updateRelPedido = dreader["col_pedido"].ToString();
                        break;
                    }
                    if (string.IsNullOrEmpty(updateRelPedido))
                    {
                        break;
                    }
                    MyCommand.Connection.Close();
                }
                catch (Exception Error)
                {
                    log.WriteLog("Warning : " + Error.Message);
                    MessageBox.Show(Error.Message);
                }
                finally
                {
                    ConexaoDados.GetConnectionAlmoxarifado().Close();
                }
                ((GuiTextField)Session.FindById("wnd[1]/usr/subSUB0:SAPLMEGUI:0003/ctxtMEPO_SELECT-EBELN")).Text = updateRelPedido;
                guiWindow.SendVKey(0);
                bool LigarWhile = true;
                int i = 0;
                materialSAPCodVer = new string[100];
                requisicaoCompra = new string[100];
                requisitanteCompra = new string[100];
                nomeMaterialSAPCodVer = new string[100];
                quantidadeItemPedido = new string[100];
                try
                {
                    Array.Clear(materialSAPCodVer, 0, materialSAPCodVer.Length);
                    Array.Clear(nomeMaterialSAPCodVer, 0, nomeMaterialSAPCodVer.Length);
                    Array.Clear(requisicaoCompra, 0, requisicaoCompra.Length);
                    Array.Clear(requisitanteCompra, 0, requisitanteCompra.Length);
                    Array.Clear(quantidadeItemPedido, 0, quantidadeItemPedido.Length);
                }
                catch
                {

                }
                int rowD = 0;
                int position = 1;
                while (LigarWhile == true)
                {
                    int TelaSAP = 1;
                    int TelaSAP2 = 1;
                    try
                    {
                        if (rowD == 10)
                        {
                            while (TelaSAP2 < 99)
                            {
                                try
                                {
                                    TelaSAP2++;
                                    ((GuiTableControl)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:00" + TelaSAP2 + "/subSUB2:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1211/tblSAPLMEGUITC_1211")).VerticalScrollbar.Position = position;
                                    break;
                                }
                                catch
                                {
                                }
                            }
                            rowD--;
                            position++;
                        }

                        while (TelaSAP < 99)
                        {
                            try
                            {
                                TelaSAP++;
                                materialSAPCodVer[i] = ((GuiTextField)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:00" + TelaSAP + "/subSUB2:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1211/tblSAPLMEGUITC_1211/ctxtMEPO1211-EMATN[4," + rowD + "]")).Text;
                                quantidadeItemPedido[i] = ((GuiTextField)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:00" + TelaSAP + "/subSUB2:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1211/tblSAPLMEGUITC_1211/txtMEPO1211-MENGE[6," + rowD + "]")).Text;
                                nomeMaterialSAPCodVer[i] = ((GuiTextField)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:00" + TelaSAP + "/subSUB2:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1211/tblSAPLMEGUITC_1211/txtMEPO1211-TXZ01[5," + rowD + "]")).Text;
                                requisicaoCompra[i] = ((GuiTextField)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:00" + TelaSAP + "/subSUB2:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1211/tblSAPLMEGUITC_1211/ctxtMEPO1211-BANFN[28," + rowD + "]")).Text;
                                requisitanteCompra[i] = ((GuiTextField)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:00" + TelaSAP + "/subSUB2:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1211/tblSAPLMEGUITC_1211/txtMEPO1211-AFNAM[21," + rowD + "]")).Text;
                                break;
                            }
                            catch
                            {

                            }
                        }
                        if (string.IsNullOrEmpty(materialSAPCodVer[0]))
                        {
                            if (string.IsNullOrEmpty(nomeMaterialSAPCodVer[i]))
                            {
                                break;
                            }
                            else
                            {
                                MySqlCommand _mySqlCommand = new MySqlCommand("SELECT COUNT(*) FROM `tb_relatorioentradafornecedor` WHERE col_pedido = '" + updateRelPedido + "'", ConexaoDados.GetConnectionAlmoxarifado());
                                _mySqlCommand.ExecuteNonQuery();
                                int _consultDB = Convert.ToInt32(_mySqlCommand.ExecuteScalar());
                                if (_consultDB > 1)
                                {
                                    MySqlCommand ConsultIDPEdido = new MySqlCommand();
                                    ConsultIDPEdido.Connection = ConexaoDados.GetConnectionAlmoxarifado();
                                    ConsultIDPEdido.CommandText = "SELECT * FROM `tb_relatorioentradafornecedor` WHERE col_pedido = '" + updateRelPedido + "' AND col_statusPedido IS null ORDER BY `col_descricaoFornecedor` ASC";
                                    MySqlDataReader dataReader = ConsultIDPEdido.ExecuteReader();
                                    while (dataReader.Read())
                                    {
                                        idPedido = dataReader["col_id"].ToString();
                                        break;
                                    }
                                    MySqlCommand mySqlCommand = new
                                    MySqlCommand("UPDATE `tb_relatorioentradafornecedor` SET `col_textoBreveMaterial` = '" + nomeMaterialSAPCodVer[i] + "', `col_statusPedido` = 1 " +
                                    "WHERE " +
                                    "`col_pedido` = '" + updateRelPedido + "' AND `col_quantidade` = '" + quantidadeItemPedido[i].Replace(".", "").Replace(",000", "") + "' AND `col_id`='" + idPedido + "' ", ConexaoDados.GetConnectionAlmoxarifado());
                                    mySqlCommand.ExecuteNonQuery();
                                    mySqlCommand.Connection.Close();
                                    ConsultIDPEdido.Connection.Close();
                                }
                                else
                                {
                                    MySqlCommand mySqlCommand = new
                                    MySqlCommand("UPDATE `tb_relatorioentradafornecedor` SET `col_textoBreveMaterial` = '" + nomeMaterialSAPCodVer[i] + "', `col_statusPedido` = 1 " +
                                    "WHERE " +
                                    "`col_pedido` = '" + updateRelPedido + "' AND `col_quantidade` = '" + quantidadeItemPedido[i].Replace(".", "").Replace(",000", "") + "'", ConexaoDados.GetConnectionAlmoxarifado());
                                    mySqlCommand.ExecuteNonQuery();
                                    mySqlCommand.Connection.Close();
                                }
                            }
                        }
                        else if (string.IsNullOrEmpty(materialSAPCodVer[i]))
                        {
                            break;
                        }
                        else
                        {
                            MySqlCommand _mySqlCommand = new MySqlCommand("SELECT COUNT(*) FROM `tb_relatorioentradafornecedor` WHERE col_statusPedido = '" + updateRelPedido + "' AND `col_quantidade` = '" + quantidadeItemPedido[i].Replace(".", "").Replace(",000", "") + "' ", ConexaoDados.GetConnectionAlmoxarifado());
                            _mySqlCommand.ExecuteNonQuery();
                            int _consultDB = Convert.ToInt32(_mySqlCommand.ExecuteScalar());
                            if (_consultDB > 0)
                            {
                                MySqlCommand mySqlCommand = new
                                MySqlCommand("UPDATE `tb_relatorioentradafornecedor` SET `col_requisicao` = '" + requisicaoCompra[i] + "', `col_statusPedido` = 1, `col_requisitanteRC` = '" + requisitanteCompra[i] + "' " +
                                "WHERE " +
                                "`col_materialCodigoSAP` = '" + materialSAPCodVer[i] + "' AND `col_pedido` = '" + updateRelPedido + "' AND `col_quantidade` = '" + quantidadeItemPedido[i].Replace(".", "").Replace(",000", "") + "' ", ConexaoDados.GetConnectionAlmoxarifado());
                                mySqlCommand.ExecuteNonQuery();
                                mySqlCommand.Connection.Close();
                            }
                            else
                            {
                                MySqlCommand mySqlCommandQ = new
                                MySqlCommand("UPDATE `tb_relatorioentradafornecedor` SET `col_requisicao` = '" + requisicaoCompra[i] + "', `col_statusPedido` = 1, `col_requisitanteRC` = '" + requisitanteCompra[i] + "' " +
                                "WHERE " +
                                "`col_materialCodigoSAP` = '" + materialSAPCodVer[i] + "' AND `col_pedido` = '" + updateRelPedido + "'", ConexaoDados.GetConnectionAlmoxarifado());
                                mySqlCommandQ.ExecuteNonQuery();
                                mySqlCommandQ.Connection.Close();
                            }
                            _mySqlCommand.Connection.Close();
                        }
                        i++;
                        rowD++;
                    }
                    catch
                    {

                    }
                }
                //Abrir Conexão.
                MySqlCommand prompt = new MySqlCommand("SELECT COUNT(*) FROM `tb_relatorioentradafornecedor` WHERE col_statusPedido IS null ORDER BY `col_descricaoFornecedor` ASC", ConexaoDados.GetConnectionAlmoxarifado());
                prompt.ExecuteNonQuery();
                int consultDB = Convert.ToInt32(prompt.ExecuteScalar());
                if (consultDB == 0)
                {
                    IniWhile = false;
                }
            }
            log.WriteLog("Info : ME23N Finalizado");
        }
        private void VerificarRequisicao()
        {
            CSapROTWrapper sapROTWrapper = new CSapROTWrapper();
            object SapGuilRot = sapROTWrapper.GetROTEntry("SAPGUI");
            object engine = SapGuilRot.GetType().InvokeMember("GetScriptingEngine", System.Reflection.BindingFlags.InvokeMethod, null, SapGuilRot, null);
            GuiApplication GuiApp = (GuiApplication)engine;
            GuiConnection connection = (GuiConnection)GuiApp.Connections.ElementAt(0);
            GuiSession Session = (GuiSession)connection.Children.ElementAt(0);
            GuiFrameWindow guiWindow = Session.ActiveWindow;
            //guiWindow.Maximize();
            Session.SendCommand("/NME53N");
            log.WriteLog("Info : ME53N Iniciado");
            bool LigarWhile = true;
            while (LigarWhile == true)
            {
                try
                {
                    MySqlCommand MyCommand = new MySqlCommand();
                    MyCommand.Connection = ConexaoDados.GetConnectionAlmoxarifado();
                    MyCommand.CommandText = "SELECT * FROM `tb_relatorioentradafornecedor` WHERE col_requisicao NOT IN(0,'',' ') AND `col_status` IS NULL AND col_materialCodigoSAP NOT BETWEEN '270000' AND '279999' ORDER BY `col_descricaoFornecedor` ASC";
                    MySqlDataReader dreader = MyCommand.ExecuteReader();
                    while (dreader.Read())
                    {
                        updateRelRequisicoes = dreader["col_requisicao"].ToString();
                        break;
                    }
                    if (string.IsNullOrEmpty(updateRelRequisicoes))
                    {
                        break;
                    }
                    MyCommand.Connection.Close();
                }
                catch (Exception Error)
                {
                    log.WriteLog("Warning : " + Error.Message);
                    MessageBox.Show(Error.Message);
                }
                finally
                {
                    ConexaoDados.GetConnectionAlmoxarifado().Close();
                }


                ((GuiButton)Session.FindById("wnd[0]/tbar[1]/btn[17]")).Press();
                ((GuiTextField)Session.FindById("wnd[1]/usr/subSUB0:SAPLMEGUI:0003/ctxtMEPO_SELECT-BANFN")).Text = updateRelRequisicoes;
                guiWindow.SendVKey(0);

                grupoCompradores = ((GuiGridView)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:0010/subSUB2:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:3212/cntlGRIDCONTROL/shellcont/shell")).GetCellValue(0, "EKGRP");





                guiWindow.SendVKey(2);
                ((GuiTab)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:0010/subSUB3:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1301/subSUB2:SAPLMEGUI:3303/tabsREQ_ITEM_DETAIL/tabpTABREQDT16")).Select();
                ((GuiGridView)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:0010/subSUB3:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1301/subSUB2:SAPLMEGUI:3303/tabsREQ_ITEM_DETAIL/tabpTABREQDT16/ssubTABSTRIPCONTROL1SUB:SAPLMEGUI:1318/ssubCUSTOMER_DATA_ITEM:SAPLXM02:0111/cntlCC_REQC_REF/shellcont/shell")).PressToolbarContextButton("&MB_FILTER");
                ((GuiGridView)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:0010/subSUB3:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1301/subSUB2:SAPLMEGUI:3303/tabsREQ_ITEM_DETAIL/tabpTABREQDT16/ssubTABSTRIPCONTROL1SUB:SAPLMEGUI:1318/ssubCUSTOMER_DATA_ITEM:SAPLXM02:0111/cntlCC_REQC_REF/shellcont/shell")).SelectContextMenuItem("&DELETE_FILTER");
                //Aqui é definida as colunas
                ((GuiGridView)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:0010/subSUB3:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1301/subSUB2:SAPLMEGUI:3303/tabsREQ_ITEM_DETAIL/tabpTABREQDT16/ssubTABSTRIPCONTROL1SUB:SAPLMEGUI:1318/ssubCUSTOMER_DATA_ITEM:SAPLXM02:0111/cntlCC_REQC_REF/shellcont/shell")).PressToolbarButton("&COL0");
                ((GuiGridView)Session.FindById("wnd[1]/usr/tabsG_TS_ALV/tabpALV_M_R1/ssubSUB_DYN0510:SAPLSKBH:0620/cntlCONTAINER1_LAYO/shellcont/shell")).CurrentCellRow = 2;
                ((GuiGridView)Session.FindById("wnd[1]/usr/tabsG_TS_ALV/tabpALV_M_R1/ssubSUB_DYN0510:SAPLSKBH:0620/cntlCONTAINER1_LAYO/shellcont/shell")).SelectedRows = "0,2,1";
                ((GuiButton)Session.FindById("wnd[1]/usr/tabsG_TS_ALV/tabpALV_M_R1/ssubSUB_DYN0510:SAPLSKBH:0620/btnAPP_WL_SING")).Press();
                ((GuiButton)Session.FindById("wnd[1]/tbar[0]/btn[0]")).Press();
                //UpdateDataBase();
                try
                {
                    bool k = true;
                    int row = 0;
                    while (k == true)
                    {
                        try
                        {

                            MARCA = ((GuiGridView)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:0010/subSUB3:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1301/subSUB2:SAPLMEGUI:3303/tabsREQ_ITEM_DETAIL/tabpTABREQDT16/ssubTABSTRIPCONTROL1SUB:SAPLMEGUI:1318/ssubCUSTOMER_DATA_ITEM:SAPLXM02:0111/cntlCC_REQC_REF/shellcont/shell")).GetCellValue(row, "NAME1");
                            REFERENCIA = ((GuiGridView)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:0010/subSUB3:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1301/subSUB2:SAPLMEGUI:3303/tabsREQ_ITEM_DETAIL/tabpTABREQDT16/ssubTABSTRIPCONTROL1SUB:SAPLMEGUI:1318/ssubCUSTOMER_DATA_ITEM:SAPLXM02:0111/cntlCC_REQC_REF/shellcont/shell")).GetCellValue(row, "MFRPN");
                            REQC = ((GuiGridView)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:0010/subSUB3:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1301/subSUB2:SAPLMEGUI:3303/tabsREQ_ITEM_DETAIL/tabpTABREQDT16/ssubTABSTRIPCONTROL1SUB:SAPLMEGUI:1318/ssubCUSTOMER_DATA_ITEM:SAPLXM02:0111/cntlCC_REQC_REF/shellcont/shell")).GetCellValue(row, "BANFN");
                            MATERIAL = ((GuiGridView)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:0010/subSUB3:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1301/subSUB2:SAPLMEGUI:3303/tabsREQ_ITEM_DETAIL/tabpTABREQDT16/ssubTABSTRIPCONTROL1SUB:SAPLMEGUI:1318/ssubCUSTOMER_DATA_ITEM:SAPLXM02:0111/cntlCC_REQC_REF/shellcont/shell")).GetCellValue(row, "MATNR");
                            ITEM = ((GuiGridView)Session.FindById("wnd[0]/usr/subSUB0:SAPLMEGUI:0010/subSUB3:SAPLMEVIEWS:1100/subSUB2:SAPLMEVIEWS:1200/subSUB1:SAPLMEGUI:1301/subSUB2:SAPLMEGUI:3303/tabsREQ_ITEM_DETAIL/tabpTABREQDT16/ssubTABSTRIPCONTROL1SUB:SAPLMEGUI:1318/ssubCUSTOMER_DATA_ITEM:SAPLXM02:0111/cntlCC_REQC_REF/shellcont/shell")).GetCellValue(row, "BNFPO");

                            //Abrir Conexão.
                            MySqlCommand ComandoMSQL = new MySqlCommand("SELECT COUNT(*) FROM `tb_marcareferencia` WHERE col_materialCodigoSAP='" + MATERIAL + "'", ConexaoDados.GetConnectionAlmoxarifado());
                            ComandoMSQL.ExecuteNonQuery();
                            int RetornoMarca = Convert.ToInt32(ComandoMSQL.ExecuteScalar());
                            if (RetornoMarca == 0)
                            {
                                SubmitDataBase();
                            }
                            SubmitDataBaseRC();

                            row++;
                        }
                        catch (ArgumentException ErroSAP)
                        {
                            log.WriteLog("Warning : " + ErroSAP.Message);
                            SubmitDataBaseRC();
                            break;
                        }
                        catch (Exception ErroProg)
                        {
                            MessageBox.Show("Erro Printar a tela e Enviar ao Administrador do Programa.");
                        }
                    }
                }
                catch (Exception)
                {
                    SubmitDataBaseRCeRRO();
                }
                //Abrir Conexão.
                MySqlCommand prompt = new MySqlCommand("SELECT COUNT(*) FROM `tb_relatorioentradafornecedor` WHERE col_requisicao NOT IN(0,'',' ') AND `col_status` IS NULL AND col_materialCodigoSAP NOT BETWEEN '270000' AND '279999' ORDER BY `col_descricaoFornecedor` ASC", ConexaoDados.GetConnectionAlmoxarifado());
                prompt.ExecuteNonQuery();
                int consultDB = Convert.ToInt32(prompt.ExecuteScalar());
                if (consultDB == 0)
                {
                    LigarWhile = false;
                }
            }
            log.WriteLog("Info : ME53N Finalizado");
        }
        private void SubmitDataBase()
        {
            MySqlCommand mySqlCommand = new
                MySqlCommand("INSERT INTO `tb_marcareferencia` (`col_marcaDescricao`, `col_referenciaDescricao`, `col_requisicao`, `col_itemDaRequisicao`, `col_materialCodigoSAP`) " +
                "VALUES " +
                "('" + MARCA.Replace("'","") + "', '" + REFERENCIA.Replace("'", "") + "', '" + REQC + "', '" + ITEM + "', '" + MATERIAL + "')", ConexaoDados.GetConnectionAlmoxarifado());
            mySqlCommand.ExecuteNonQuery();
            mySqlCommand.Connection.Close();
        }
        private void SubmitDataBaseRC()
        {
            MySqlCommand mySqlCommand = new
            MySqlCommand("UPDATE `tb_relatorioentradafornecedor` SET `col_status` = '1',`col_grupoDeCompradores` = '" + grupoCompradores + "' " +
            "WHERE " +
            "`col_requisicao` = '" + updateRelRequisicoes + "'", ConexaoDados.GetConnectionAlmoxarifado());
            mySqlCommand.ExecuteNonQuery();
            mySqlCommand.Connection.Close();

        }
        private void SubmitDataBaseRCeRRO()
        {
            MySqlCommand mySqlCommand = new
            MySqlCommand("UPDATE `tb_relatorioentradafornecedor` SET `col_status` = '2',`col_grupoDeCompradores` = '" + grupoCompradores + "' " +
            "WHERE " +
            "`col_requisicao` = '" + updateRelRequisicoes + "'", ConexaoDados.GetConnectionAlmoxarifado());
            mySqlCommand.ExecuteNonQuery();
            mySqlCommand.Connection.Close();

        }

        private void FormRelatorioGeralPorFornecedor_Load(object sender, EventArgs e)
        {
            CarregarGridView();
        }

        private void btnRC_Click(object sender, EventArgs e)
        {
            VerificaPedido();
            VerificarRequisicao();
            //EnviarParaOBancoDeDadosZMM040();
            //PreencherTextBoxZMM040();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string _fileName = txtCodBarra.Text;
            System.Diagnostics.Process.Start(_fileName);
            log.WriteLog("Info : PDF Aberto.");
        }
    }
}

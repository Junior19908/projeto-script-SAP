using Microsoft.Win32;
using MySql.Data.MySqlClient;
using SAPFEWSELib;
using SapROTWr;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using SistemaGSG.Email;
using org.junit.@internal.runners.statements;
using javax.security.auth;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Xml;
using HtmlAgilityPack;
using System.Windows.Controls;
using javax.swing.text.html;
using static sun.awt.geom.AreaOp;
using System.Collections.Generic;
using SistemaGSG.NotasFiscais;
using javax.lang.model.util;
using org.w3c.dom.html;
using SistemaGSG.Log;

namespace SistemaGSG
{
    public partial class frmProtocolo : MetroFramework.Forms.MetroForm
    {
        private const string Texto = "Duplicidade!, Esta Chave já existe no Banco de Dados.";
        string usuarioLogado = dados.Usuario;
        string[] destinatarios = { "junior@usga.com.br", "evaristo@usga.com.br", "jackson@usga.com.br" , "luciano.eduardo@usga.com.br" };
        //string[] destinatarios = { "junior@usga.com.br", "sigtisistemasintegrados@gmail.com", "luanabritosilva8@gmail.com" , "luanacaetano346@gmail.com" };
        //string[] destinatarios = { "sigtisistemasintegrados@gmail.com" };
        private void ConsultaNotasFiscaisBD()
        {
            ConsultaNotasFiscais consultaNotas = new ConsultaNotasFiscais();
            txtNotasOmissas.Text = consultaNotas.OmissasNotasFiscais().ToString();
            txtNotasRegistradas.Text = consultaNotas.RegistradasNotasFiscais().ToString();
            txtTotalNotas.Text = consultaNotas.TotalNotasFiscais().ToString();
            txtNotasCanceladas.Text = consultaNotas.CanceladaNotasFiscais().ToString();
            DateTime dataConsulta = consultaNotas.DataConsulta().Date;
            DateTime dataHoje = DateTime.Today;

            TimeSpan diff = dataConsulta.Subtract(dataHoje);
            int dias = Math.Abs(diff.Days);

            lblDias.Text = dias.ToString("0");
        }
        public frmProtocolo()
        {
            InitializeComponent();
            VerifyVersion(webBrowser);
          webBrowser.Navigate("https://www.nfe.fazenda.gov.br/portal/manifestacaoDestinatario.aspx?tipoConteudo=o9MkXc+hmKs=");
           //webBrowser.Navigate("http://127.0.0.1/teste/");
            webBrowser.ScriptErrorsSuppressed = true;
            ConsultaNotasFiscaisBD();
        }
        public void VerifyVersion(System.Windows.Forms.WebBrowser webbrowser)
        {
            string appName = "";
            try
            {
                appName = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";

                RegistryKey fbeKey = null;
                ////For 64 bit Machine 
                fbeKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\Wow6432Node\\Microsoft\\Internet Explorer\\MAIN\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                if (fbeKey == null)
                    fbeKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\\Wow6432Node\\Microsoft\\Internet Explorer\\MAIN\\FeatureControl\\FEATURE_BROWSER_EMULATION");
                using (fbeKey)
                {
                    fbeKey.SetValue(appName, 11001, RegistryValueKind.DWord);
                }


                //For 32 bit Machine 
                fbeKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                if (fbeKey == null)
                    fbeKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION");
                using (fbeKey)
                {
                    fbeKey.SetValue(appName, 11001, RegistryValueKind.DWord);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(appName + "\n" + ex.ToString(), "Unexpected error setting browser mode!");
            }
        }
        public string Mensagem { get; private set; }
        string onclickValue;
        DataHora dataHora;
        string dataString;
        string horaString;
        string razaoSocial;
        string tipoOperacao;
        string chaveAcessoRegistros;
        string valorNotaFiscal;
        decimal valorNotaFiscalDec;
        string statusNotaFiscal;
        private void webBrowser_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //webBrowser.Document.GetElementById("ctl00_ContentPlaceHolder1_rbtSemChave").InvokeMember("click");
        }
        private void UpdateNotaFiscalCancelada()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE `tb_chave` SET `status` = @canceladaNotaFiscal WHERE `col_chave` = @chaveNotaFiscal;", ConexaoDados.GetConnectionXML());
                cmd.Parameters.AddWithValue("@canceladaNotaFiscal", "CANCELADA");
                cmd.Parameters.AddWithValue("@chaveNotaFiscal", txtChave.Text.Trim());
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                log.WriteLog("Erro Update Chave : "+ex.Message);
            }
        }

        private void ConsultaChaveSefaz()
        {
            try
            {
                webBrowser.Document.GetElementById("ctl00_ContentPlaceHolder1_rbtSemChave").InvokeMember("click");

                HtmlElementCollection Botao = this.webBrowser.Document.GetElementsByTagName("input");
                foreach (HtmlElement Funcao in Botao)
                {
                    if (Funcao.GetAttribute("id").Equals("ctl00_ContentPlaceHolder1_btnPesquisar"))
                    {
                        Funcao.InvokeMember("Click");
                    }
                }
                int countg = dataGridViewSefaz.RowCount;
                label6.Text = countg.ToString();
                HtmlElementCollection Pesquisa = this.webBrowser.Document.GetElementsByTagName("table");
                foreach (HtmlElement Funcao in Pesquisa)
                {
                    //Verificar Este momento do código
                    if (Funcao.GetAttribute("id").Equals("ctl00_ContentPlaceHolder1_gdvResultadoPesquisa"))
                    {
                        textBox4.Text = Funcao.GetAttribute("innerText");
                        if (textBox4.Text.Contains("Próxima >>"))
                        {
                            var links = webBrowser.Document.GetElementsByTagName("input");
                            foreach (HtmlElement link in links)
                            {
                                txtChave.Text = link.GetAttribute("value");
                                if (string.IsNullOrWhiteSpace(txtChave.Text))
                                {

                                }
                                else if (txtChave.Text.Length == 44)
                                {
                                    HtmlElement divElement = webBrowser.Document.GetElementById("footnote" + txtChave.Text);
                                    if (divElement != null)
                                    {
                                        string divInnerHtml = divElement.InnerHtml;
                                        razaoSocial = ExtrairRazaoSocial(divInnerHtml);
                                        tipoOperacao = ExtrairValorCampo(divInnerHtml);
                                        valorNotaFiscal = ExtrairValorNotaFiscal(divInnerHtml);
                                        valorNotaFiscalDec = decimal.Parse(valorNotaFiscal.Replace(".", ","));


/*NOTASFISCAL CANCELADA AJUSTAR O <TD>
 * A LOGICA ESTÁ EXECUTANDO PARA TODAS
 * ÁS CHAVES INDEPENDENTE DE ESTAR
 * CACELADA
 */
                                        //HtmlElementCollection trElements = this.webBrowser.Document.GetElementsByTagName("tr");
                                        //
                                        //foreach (HtmlElement trElement in trElements)
                                        //{
                                        //    // Verifica se é uma linha de dados
                                        //    if (trElement.GetAttribute("className") == "linhaImparCentralizada" || trElement.GetAttribute("className") == "linhaParCentralizada")
                                        //    {
                                        //        // Obtém as células da linha
                                        //        HtmlElementCollection tdElements = trElement.GetElementsByTagName("td");
                                        //
                                        //        // Verifica se a célula da coluna "Situação NFe" contém o valor "Cancelada"
                                        //        if (tdElements.Count > 3 && tdElements[3].InnerText.Trim() == "Cancelada")
                                        //        {
                                        //            // Realize o update para a linha correspondente aqui
                                        //            UpdateNotaFiscalCancelada();
                                        //        }
                                        //    }
                                        //}

                                    }
                                    txtNFE.Text = txtChave.Text.Substring(26, 9);
                                    try
                                    {
                                        onclickValue = link.GetAttribute("atualizarChaveSelecionada");
                                        onclickValue = link.OuterHtml;
                                        dataHora = ExtrairDataHora(onclickValue);
                                        VerificarChaveNoBanco();
                                    }
                                    catch (Exception Erro)
                                    {
                                        log.WriteLog("Erro : "+Erro.Message);
                                    }
                                }
                            }
                            textBox4.Text = "";
                        }
                        else
                        {
                            var links = webBrowser.Document.GetElementsByTagName("input");
                            foreach (HtmlElement link in links)
                            {
                                txtChave.Text = link.GetAttribute("value");
                                if (string.IsNullOrWhiteSpace(txtChave.Text))
                                {

                                }
                                else
                                {
                                    if (txtChave.Text == "rbtComChave")
                                    {

                                    }
                                    else if (txtChave.Text == "rbtSemChave")
                                    {

                                    }
                                    else if (txtChave.Text == "12706289")
                                    {

                                    }
                                    else if (txtChave.Text == "000148")
                                    {

                                    }
                                    else if (txtChave.Text == "Pesquisar")
                                    {

                                    }
                                    else if (txtChave.Text == "OK")
                                    {

                                    }
                                    else
                                    {
                                        onclickValue = link.GetAttribute("atualizarChaveSelecionada");
                                        onclickValue = link.OuterHtml;
                                        dataHora = ExtrairDataHora(onclickValue);
                                        VerificarChaveNoBanco();
                                    }
                                }
                            }
                            textBox4.Text = "";
                            checkBox.Checked = false;
                            webBrowser.Refresh();
                        }
                    }
                }
                HtmlElementCollection elc = this.webBrowser.Document.GetElementsByTagName("a");
                foreach (HtmlElement el in elc)
                {
                    string linkText = el.GetAttribute("innerText");
                    if (!string.IsNullOrEmpty(linkText) && linkText.Trim().Equals("Próxima >>"))
                    {
                        el.InvokeMember("Click");
                    }
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }

            LoadDataGrid();
        }
        private string ExtrairValorCampo(string html)
        {
            string startTag = "<span>Tipo de Operação: </span>";
            string endTag = "<br>";

            int startIndex = html.IndexOf(startTag);
            int endIndex = html.IndexOf(endTag, startIndex);

            if (startIndex != -1 && endIndex != -1)
            {
                startIndex += startTag.Length;
                string valorCampo = html.Substring(startIndex, endIndex - startIndex).Trim();
                return valorCampo;
            }

            return null;
        }
        private string ExtrairValorNotaFiscal(string html)
        {
            string startTag = "<span>Valor Total da NF-e: </span>";
            string endTag = "<br>";

            int startIndex = html.IndexOf(startTag);
            int endIndex = html.IndexOf(endTag, startIndex);

            if (startIndex != -1 && endIndex != -1)
            {
                startIndex += startTag.Length;
                string valorCampo = html.Substring(startIndex, endIndex - startIndex).Trim();
                return valorCampo;
            }

            return null;
        }
        private string ExtrairRazaoSocial(string html)
        {
            string startTag = "<span>Razão Social do Emitente:</span>";
            string endTag = "<br>";

            int startIndex = html.IndexOf(startTag);
            int endIndex = html.IndexOf(endTag, startIndex);

            if (startIndex != -1 && endIndex != -1)
            {
                startIndex += startTag.Length;
                string razaoSocial = html.Substring(startIndex, endIndex - startIndex).Trim();
                return razaoSocial;
            }

            return null;
        }
        private DataHora ExtrairDataHora(string onclickValue)
        {
            if (!string.IsNullOrEmpty(onclickValue))
            {
                string pattern = @"(\d{2}/\d{2}/\d{4}) (\d{2}:\d{2}:\d{2})";
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(onclickValue, pattern);
                
                if (match.Success)
                {
                    string dataString = match.Groups[1].Value;
                    string horaString = match.Groups[2].Value;
                    DateTime data = DateTime.ParseExact(dataString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime hora = DateTime.ParseExact(horaString, "HH:mm:ss", CultureInfo.InvariantCulture);
                    DataHora dataHora = new DataHora
                    {
                        Data = data,
                        Hora = hora
                    };
                    //string dataHora = match.Value;
                    return dataHora;
                }
            }
            return null;
        }
        public class DataHora
        {
            public DateTime Data { get; set; }
            public DateTime Hora { get; set; }
        }
        private void LimparTexts()
        {
            //Limpar Campos apos a inserção no banco de dados.
            txtChave.Text = "";
            txtNSU.Text = "";
        }
        private void VerificarChaveNoBanco()
        {
            try
            {
                MySqlCommand prompt = new MySqlCommand("SELECT COUNT(*) FROM tb_chave WHERE col_chave =@chaveAcesso ", ConexaoDados.GetConnectionXML());
                prompt.Parameters.AddWithValue("@chaveAcesso", txtChave.Text.Trim());
                prompt.ExecuteNonQuery();
                int consultDB = Convert.ToInt32(prompt.ExecuteScalar());
                ConexaoDados.GetConnectionXML().Close();
                if (consultDB > 0)
                {
                    lblChaveDuplicidade.Text = txtChave.Text + ".xml";
                    LimparTexts();
                }
                else
                {
                    try
                    {
                        InserirChave();
                    }
                    catch (Exception Err)
                    {
                        MessageBox.Show(Err.Message);
                    }
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show("Erro: " + Err.Message);
            }
        }
        int operadorTipo;
        private void InserirChave()
        {
            try
            {
                if(txtChave.Text.Length >= 45)
                {

                }else if (txtChave.Text.Length < 44)
                {

                }
                else
                {
                    if(dataHora != null)
                    {
                        if (tipoOperacao == "Saída")
                        {
                            operadorTipo = 1;
                        }
                        else
                        {
                            operadorTipo = 0;
                        }
                        try
                        {
                            MySqlCommand prompt_cmd = new MySqlCommand("INSERT INTO tb_chave (col_chave,empresa,status,col_Downl,col_dataHoraCriacao,emisao,tpNF,vNF,n_nfe) VALUES (@chaveAcesso, @razaoSocialEmpresa, @statusNF, @downloadXML, @dataHoraData, @dataData, @tipoOperador, @valorNF, @numeroNF)", ConexaoDados.GetConnectionXML());
                            prompt_cmd.Parameters.AddWithValue("@chaveAcesso", txtChave.Text.Trim());
                            prompt_cmd.Parameters.AddWithValue("@razaoSocialEmpresa", razaoSocial);
                            prompt_cmd.Parameters.AddWithValue("@statusNF", ".");
                            prompt_cmd.Parameters.AddWithValue("@downloadXML", "1");
                            prompt_cmd.Parameters.AddWithValue("@dataHoraData", dataHora.Data.ToString("yyyy-MM-dd") +" "+ dataHora.Hora.ToString("HH:mm:ss"));
                            prompt_cmd.Parameters.AddWithValue("@dataData", dataHora.Data.ToString("yyyy-MM-dd"));
                            prompt_cmd.Parameters.AddWithValue("@tipoOperador", operadorTipo);
                            prompt_cmd.Parameters.AddWithValue("@valorNF", valorNotaFiscalDec.ToString("C"));
                            prompt_cmd.Parameters.AddWithValue("@numeroNF", txtNFE.Text);
                            prompt_cmd.ExecuteNonQuery();
                            ConexaoDados.GetConnectionXML().Close();
                            SenderEmail_();
                        }
                        catch (Exception Err)
                        {
                            Log.log.WriteLog("Erro : " + Err.Message);
                        }
                    }
                }
            }
            catch (MySqlException ErroR)
            {
                MessageBox.Show("Erro ao Inserir Chave. " + ErroR.Message);
            }
            catch (Exception err)
            {
                MessageBox.Show("Erro: " + err.Message);
            }
        }
        private void SenderEmail_()
        {
            EmailSender emailSender = new EmailSender();
            foreach (string destinatario in destinatarios)
            {
                emailSender.SendEmail(destinatario, dataHora.Data, dataHora.Hora, txtChave.Text, razaoSocial, valorNotaFiscalDec, tipoOperacao);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            ConsultaChaveSefaz();
        }
        private void SenderEmailReg()
        {
            EmailSender emailSender = new EmailSender();
            foreach (string destinatario in destinatarios)
            {
                emailSender.SendEmailRegistros(destinatario);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SenderEmailReg();
           //if (MessageBox.Show("Deseja Baixar XML's?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
           //{
           //    FormDownloadXML frm_Main = new FormDownloadXML();
           //    frm_Main.Show();
           //    Close();
           //}
        }
        private void frmProtocolo_Load(object sender, EventArgs e)
        {
            DateTime dataInicioAvaliacao = DateTime.ParseExact("02/09/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dataAtual = DateTime.Now;
            TimeSpan duracaoAvaliacao = dataAtual - dataInicioAvaliacao;
            TimeSpan duracaoAvaliacaolbl = dataInicioAvaliacao.AddDays(1) - DateTime.Now;
            if (duracaoAvaliacaolbl.TotalDays > 0)
            {
                lblperiodoAvaliacao.Text = duracaoAvaliacaolbl.Days + " dias restantes";
                LoadDataGrid();
                LoadDataGridPress();
                txtUrl.Text = @"C:\ArquivosSAP\xmlDownload\";
                label7.Text = @"C:\ArquivosSAP\xmlDownload\";
                lblChaveDuplicidade.Visible = false;
                ConsultaNotasFiscaisBD();
            }
            else
            {
                MessageBox.Show("Período Expirado!");
                Application.Exit();
            }
        }
        private void LoadDataGrid()
        {
            MySqlDataAdapter ADAP = new MySqlDataAdapter("SELECT TipoDoc.col_desc_NFe, TbChave.* FROM `tb_chave` AS TbChave LEFT JOIN tb_tipo_nfe AS TipoDoc ON TbChave.tpNF=TipoDoc.col_id WHERE status!='LANÇADA' AND status NOT IN('CANCELADA') ORDER BY col_dataHoraCriacao DESC", ConexaoDados.GetConnectionXML());
            DataTable SS = new DataTable();
            ADAP.Fill(SS);
            dataGridViewSefaz.DataSource = SS;
            ConexaoDados.GetConnectionXML().Close();
            int countg = dataGridViewSefaz.RowCount;
            label6.Text = countg.ToString();
            ProgBar.Maximum = countg;
        }
        private void LoadDataGridPress()
        {
            MySqlDataAdapter ADAP = new MySqlDataAdapter("SELECT * FROM `tb_chave` WHERE status IS NULL", ConexaoDados.GetConnectionXML());
            DataTable SS = new DataTable();
            ADAP.Fill(SS);
            dataGridViewRestante.DataSource = SS;
            ConexaoDados.GetConnectionXML().Close();
            int countg = dataGridViewRestante.RowCount;
            label11.Text = countg.ToString();

        }
        private void dataGridViewSefaz_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }
        string[] str;
        private void carregarDataXML()
        {
            int countg = dataGridViewSefaz.RowCount;
            label6.Text = countg.ToString();
            int XMLCont = 0;
            int XMLContResult = 1;
            double TotaldeLinhas = countg;
            while (XMLCont <= countg)
            {
                ProgBar.Value = XMLCont;
                //txtURLxml.Text = dataGridViewSefaz.Rows[XMLCont].Cells["Column2"].Value.ToString();
                using (DataSet ds = new DataSet())
                {
                    try
                    {
                        string xmlFilePath = txtUrl.Text;

                        DirectoryInfo d = new DirectoryInfo(txtUrl.Text);
                        FileInfo[] Files = d.GetFiles("*.xml");

                        int i = 0;
                        int l = 0;
                        foreach (FileInfo file1 in Files)
                        {
                            str = new string[Files.Length];
                        }
                        foreach (FileInfo file in Files)
                        {
                            str[i] = file.Name;
                            i++;
                        }
                        while (l <= str.Length)
                        {
                            ds.ReadXml(xmlFilePath + str[l]);
                            try
                            {
                                dtXML.DataSource = ds.Tables["emit"];
                                txtCNPJ.Text = dtXML.Rows[l].Cells["CNPJ"].Value.ToString().Trim();
                                txtEmpresa.Text = dtXML.Rows[l].Cells["xNome"].Value.ToString().Trim();
                            }
                            catch (Exception ErroEmissor)
                            {
                                MessageBox.Show("Emit: " + ErroEmissor.Message);
                            }
                            try
                            {
                                NFEGRID.DataSource = ds.Tables["ide"];
                                txtNFE.Text = NFEGRID.Rows[l].Cells["nNF"].Value.ToString().Trim();
                                txtserie.Text = NFEGRID.Rows[l].Cells["serie"].Value.ToString().Trim();
                                txtdate.Text = NFEGRID.Rows[l].Cells["dhEmi"].Value.ToString().Trim().Substring(0, 10);
                                tpNF.Text = NFEGRID.Rows[l].Cells["tpNF"].Value.ToString().Trim();
                                dateTimePicker1.Text = txtdate.Text;
                            }
                            catch (Exception ErroIdent)
                            {
                                MessageBox.Show("Ide: " + ErroIdent.Message);
                            }
                            try
                            {
                                DADOSGRID.DataSource = ds.Tables["infAdic"];
                                txtDados.Text = DADOSGRID.Rows[l].Cells["infCpl"].Value.ToString().Trim();
                            }
                            catch (Exception)
                            {
                                //MessageBox.Show("InfAdic: " + ErroInfAdc.Message);
                            }
                            try
                            {
                                dataGridViewProdutos.DataSource = ds.Tables["ICMSTot"];
                                vNF.Text = dataGridViewProdutos.Rows[l].Cells["vNF"].Value.ToString().Replace(".", ",");
                                vNF.Focus();
                                textValor1.Focus();
                            }
                            catch (Exception ErroEmissor)
                            {
                                MessageBox.Show("total: " + ErroEmissor.Message);
                            }
                            try
                            {
                                CHAVEGRID.DataSource = ds.Tables["infProt"];
                                txtChavedeAcesso.Text = CHAVEGRID.Rows[l].Cells["chNFe"].Value.ToString().Trim();
                                txtProtocolo.Text = CHAVEGRID.Rows[l].Cells["nProt"].Value.ToString().Trim();
                            }
                            catch (Exception ErroInfProt)
                            {
                                MessageBox.Show("InfProt: " + ErroInfProt.Message);
                                txtCNPJ.Text = "";
                                txtEmpresa.Text = "";
                                txtNFE.Text = "";
                                txtserie.Text = "";
                                txtdate.Text = "";
                                txtDados.Text = "";
                                txtChavedeAcesso.Text = "";
                                txtProtocolo.Text = "";
                                vNF.Text = "";
                                tpNF.Text = "";
                            }
                            try
                            {
                                //MySqlCommand prompt_cmd = new MySqlCommand("UPDATE `tb_chave` SET empresa='" + txtEmpresa.Text.Trim() + "' ,n_nfe='" + txtNFE.Text.Trim() + "',emisao='" + txtdate.Text.Trim() + "', tpNF='" + tpNF.Text.Trim() + "', vNF='" + vNF.Text.Trim() + "' WHERE col_chave='" + txtURLxml.Text.Replace(".xml", "") + "'", ConexaoDados.GetConnectionXML());
                                MySqlCommand prompt_cmd = new MySqlCommand("INSERT INTO `tb_chave` (`col_chave`, `col_nsu`, `empresa`, `n_nfe`, `emisao`, `lancamento_sap`, `protocolo`, `user_sap`, `status`, `col_Downl`, `col_link`, `tpNF`, `vNF`, `ACTION_REQU`) VALUES " +
                                    "('" + str[l].Replace("-procNfe.xml", "") + "', NULL, '" + txtEmpresa.Text.Trim() + "', '" + txtNFE.Text.Trim() + "', '" + txtdate.Text.Trim() + "', NULL, NULL, NULL, '.', NULL, NULL, NULL, '" + vNF.Text + "', NULL)", ConexaoDados.GetConnectionXML());

                                prompt_cmd.ExecuteNonQuery();
                                ConexaoDados.GetConnectionXML().Close();
                            }
                            catch (MySqlException)
                            {
                                //MessageBox.Show("Não Inserido, no Banco de Dados!");
                            }
                            catch (Exception Err)
                            {
                                MessageBox.Show("Geral: " + Err.Message);
                            }
                            l++;
                            File.Delete(@"C:\ArquivosSAP\xmlDownload\" + str[l]);
                            XMLCont++;
                            XMLContResult++;
                            try
                            {
                                double percentual = (XMLCont / TotaldeLinhas) * 100.0;
                                string percentualFormatado = percentual.ToString("0.00") + " %";
                                lblPorcentagem.Text = percentualFormatado;
                            }
                            catch (Exception)
                            {
                                //MessageBox.Show("Porcentagem Error: " + ErrorR.Message);
                            }
                            label10.Text = XMLCont.ToString();
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        //MessageBox.Show(Error.Message + " " + XMLCont.ToString());
                    }
                    catch (Exception ErMXL)
                    {
                        //MessageBox.Show("Geral 2: " + ErMXL.Message);
                    }
                }
            }
            LoadDataGrid();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja Iniciar?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    carregarDataXML();
                }
                catch
                {
                    //MessageBox.Show("Geral 3: " + Err.Message);
                }
                LoadDataGrid();
            }
        }
        //richTextBox1.AppendText(el.GetAttribute("type").Equals("span") + Environment.NewLine);
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja Voltar?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frm_Main frm_Main = new frm_Main();
                frm_Main.Show();
                this.Visible = false;
            }
        }
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox.Checked)
            {
                TempoPesquisa.Enabled = true;
                ConsultaNotasFiscaisBD();
            }
            else
            {
                TempoPesquisa.Enabled = false;
                ConsultaNotasFiscaisBD();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            var sw = new Stopwatch();
            sw.Start();
            lblTempo.Text = sw.Elapsed.ToString(@"hh\:mm\:ss");
            if (MessageBox.Show("Deseja Iniciar a consulta no SAP? \nO SistemaGSG ficará indisponivel por alguns minutos", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //Pega a tela de execução do Windows
                CSapROTWrapper sapROTWrapper = new CSapROTWrapper();
                //Pega a entrada ROT para o SAP Gui para conectar-se
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
                Session.SendCommand("/NJ1BNFE");
                int countg = dataGridViewSefaz.RowCount;

                //Tecla Enter
                guiWindow.SendVKey(0);


                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtDATE0-LOW")).Text = "";
                string EMPRESA;
                string NOTAFISCAL;
                string Lancamento;
                string Protocolo;
                string User;
                string AcaoEtapa;
                string ValorNFe;
                int Chave = 0;
                int ProgressBarra = 1;
                while (Chave < countg)
                {
                    int countgXML = dataGridViewRestante.RowCount;
                    if (dataGridViewSefaz.Rows[Chave].Cells["Column10"].Value.ToString() == "LANÇADA")
                    {

                    }
                    else
                    {
                        try
                        {
                            ProgBar.Value = ProgressBarra;
                            ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtBUKRS-LOW")).Text = "USGA";
                            ((GuiTextField)Session.FindById("wnd[0]/usr/txtR_ACCKEY-LOW")).Text = dataGridViewSefaz.Rows[Chave].Cells["Column2"].Value.ToString();
                            ((GuiButton)Session.FindById("wnd[0]/tbar[1]/btn[8]")).Press();
                            AcaoEtapa = ((GuiGridView)Session.FindById("wnd[0]/usr/cntlNFE_CONTAINER/shellcont/shell")).GetCellValue(0, "ACTION_REQU");
                            ((GuiGridView)Session.FindById("wnd[0]/usr/cntlNFE_CONTAINER/shellcont/shell")).CurrentCellColumn = "";
                            ((GuiGridView)Session.FindById("wnd[0]/usr/cntlNFE_CONTAINER/shellcont/shell")).SelectedRows = "0";
                            ((GuiGridView)Session.FindById("wnd[0]/usr/cntlNFE_CONTAINER/shellcont/shell")).CurrentCellColumn = "NFNUM9";
                            ((GuiGridView)Session.FindById("wnd[0]/usr/cntlNFE_CONTAINER/shellcont/shell")).ClickCurrentCell();
                            EMPRESA = ((GuiTextField)Session.FindById("wnd[0]/usr/subMAIN_PARTNER:SAPLJ1BB2:5250/txtJ_1BDYMPA-MAINNAME1")).Text;
                            NOTAFISCAL = ((GuiTextField)Session.FindById("wnd[0]/usr/subNF_NUMBER:SAPLJ1BB2:2002/txtJ_1BDYDOC-NFENUM")).Text;
                            Lancamento = ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtJ_1BDYDOC-PSTDAT")).Text;
                            ((GuiTab)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB8")).Select();
                            Protocolo = ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB8/ssubHEADER_TAB:SAPLJ1BB2:2800/subTIMESTAMP:SAPLJ1BB2:2803/txtJ_1BDYDOC-AUTHCOD")).Text;
                            ((GuiTab)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB7")).Select();
                            User = ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB7/ssubHEADER_TAB:SAPLJ1BB2:2700/txtJ_1BDYDOC-CRENAM")).Text;
                            ((GuiTab)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB2")).Select();
                            ValorNFe = ((GuiTextField)Session.FindById("wnd[0]/usr/tabsTABSTRIP1/tabpTAB2/ssubHEADER_TAB:SAPLJ1BB2:2200/txtJ_1BDYDOC-NFTOT")).Text;
                            ((GuiButton)Session.FindById("wnd[0]/tbar[0]/btn[3]")).Press();
                            ((GuiButton)Session.FindById("wnd[0]/tbar[0]/btn[3]")).Press();
                            if (AcaoEtapa == "C")
                            {
                                AcaoEtapa = "2";
                            }
                            if (string.IsNullOrEmpty(AcaoEtapa))
                            {
                                AcaoEtapa = "1";
                            }
                            if (AcaoEtapa == "A")
                            {
                                AcaoEtapa = "13";
                            }
                            if (AcaoEtapa == "B")
                            {
                                AcaoEtapa = "14";
                            }
                            if (AcaoEtapa == "D")
                            {
                                AcaoEtapa = "15";
                            }
                            dateTimePicker2.Text = Lancamento;
                            dateTimePicker2.Format = DateTimePickerFormat.Custom;
                            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
                            try
                            {
                                MySqlCommand prompt_cmd = new MySqlCommand("UPDATE `tb_chave` SET empresa='" + EMPRESA + "' , n_nfe='" + NOTAFISCAL + "', lancamento_sap='" + this.dateTimePicker2.Text + "', protocolo='" + Protocolo + "',  user_sap='" + User + "', status='LANÇADA', vNF='R$: " + ValorNFe + "', col_Downl='2' ,  ACTION_REQU='" + AcaoEtapa + "' WHERE col_chave='" + dataGridViewSefaz.Rows[Chave].Cells["Column2"].Value.ToString() + "'", ConexaoDados.GetConnectionXML());
                                prompt_cmd.ExecuteNonQuery();
                                ConexaoDados.GetConnectionXML().Close();
                            }
                            catch (MySqlException ErrMy)
                            {
                                MessageBox.Show("Erro no Banco de Dados! - \n" + ErrMy.Message);
                            }
                            catch (Exception Err)
                            {
                                MessageBox.Show(Err.Message);
                            }
                        }
                        catch
                        {
                            try
                            {
                                MySqlCommand prompt_cmd = new MySqlCommand("UPDATE `tb_chave` SET status='NÃO LANÇADA' WHERE col_chave='" + dataGridViewSefaz.Rows[Chave].Cells["Column2"].Value.ToString() + "'", ConexaoDados.GetConnectionXML());
                                prompt_cmd.ExecuteNonQuery();
                                ConexaoDados.GetConnectionXML().Close();
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.Message);
                            }
                            ((GuiButton)Session.FindById("wnd[0]/tbar[0]/btn[3]")).Press();
                        }
                    }
                    label10.Text = Chave.ToString();
                    Chave++;
                    ProgressBarra++;
                    LoadDataGridPress();
                    try
                    {
                        double TotaldeLinhas = countg;
                        double percentual = (Chave / TotaldeLinhas) * 100.0;
                        double percent = 100.0 - percentual;
                        string percentualFormatado = percentual.ToString("0.00") + " %";
                        lblPorcentagem.Text = percentualFormatado;
                    }
                    catch
                    {
                        // MessageBox.Show("Erro com a porcentagem!" +Error.Message);
                    }
                }
                LoadDataGrid();
            }
            sw.Stop();
            lblTempo.Text = sw.Elapsed.ToString(@"hh\:mm\:ss");
            if(MessageBox.Show("Enviar E-mail's ?","Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
            {
                SenderEmailReg();
            }
            MessageBox.Show("Processo, Finalizado com Sucesso!", "Conclusão", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        private void TempoEspera_Tick(object sender, EventArgs e)
        {
            HtmlElementCollection Botao = this.webBrowser.Document.GetElementsByTagName("input");
            foreach (HtmlElement Funcao in Botao)
            {
                if (Funcao.GetAttribute("id").Equals("ctl00_ContentPlaceHolder1_btnPesquisar"))
                {
                    Funcao.InvokeMember("Click");
                }
            }
            checkBox.Checked = true;
            TempoEspera.Enabled = false;
        }
        private void dataGridViewSefaz_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var url = dataGridViewSefaz.SelectedRows[0].Cells["Column12"].Value;
            if (string.IsNullOrEmpty(url.ToString()))
            {

            }
            else
            {
                try
                {
                    var AbrirNavegador = new Navegador(url.ToString());
                    AbrirNavegador.Show();
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message);
                }
            }
        }
        private void maskFiltro_MaskChanged(object sender, EventArgs e)
        {
        }
        private void maskFiltro_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string MES;
                string ANO;
                MES = maskFiltro.Text.Substring(0, 2);
                ANO = maskFiltro.Text.Substring(3, 4);

                MySqlDataAdapter ADAP = new MySqlDataAdapter("SELECT TipoDoc.col_desc_NFe, TbChave.* FROM `tb_chave` AS TbChave LEFT JOIN tb_tipo_nfe AS TipoDoc ON TbChave.tpNF=TipoDoc.col_id WHERE status!='LANÇADA' AND MONTH(emisao)='" + MES + "' AND  YEAR(emisao)='" + ANO + "' ORDER BY col_nsu DESC", ConexaoDados.GetConnectionXML());
                DataTable SS = new DataTable();
                ADAP.Fill(SS);
                dataGridViewSefaz.DataSource = SS;
                ConexaoDados.GetConnectionXML().Close();
                int countg = dataGridViewSefaz.RowCount;
                label6.Text = countg.ToString();
            }
            if (string.IsNullOrEmpty(maskFiltro.Text))
            {
                LoadDataGrid();
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja ir para o Relatório Geral de Danf's?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FormRelacao frm_Main = new FormRelacao();
                frm_Main.Show();
                Close();
            }
        }
    }
}

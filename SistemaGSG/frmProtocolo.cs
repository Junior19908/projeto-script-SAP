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
using sun.misc;
using SIGT.Mensagem;
using java.sql;
using System.Data.OleDb;
using System.Timers;
using System.Threading;

namespace SistemaGSG
{
    public partial class frmProtocolo : MetroFramework.Forms.MetroForm
    {
        private void emailsConsulta()
        {
            string tb_user = "SELECT * FROM tb_email WHERE col_status = @statusEmail";
            MySqlCommand cmd;
            MySqlDataReader dr;
            cmd = new MySqlCommand(tb_user, ConexaoDados.GetConnectionXML());
            cmd.Parameters.Add(new MySqlParameter("@statusEmail", "5"));
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dr.Read())
            {
                string email = dr["col_email"].ToString();
                destinatarios.Add(email);
            }
        }
        private const string Texto = "Duplicidade!, Esta Chave já existe no Banco de Dados.";
        string usuarioLogado = dados.Usuario;
        List<string> destinatarios = new List<string>();
        //System.Threading.Thread thread = new System.Threading.Thread(ConsultaNotasFiscaisBD());
        private void ConsultaNotasFiscaisBD()
        {
            ConsultaNotasFiscais consultaNotas = new ConsultaNotasFiscais();
            txtNotasOmissas.Text = consultaNotas.OmissasNotasFiscais().ToString();
            txtNotasRegistradas.Text = consultaNotas.RegistradasNotasFiscais().ToString();
            txtTotalNotas.Text = consultaNotas.TotalNotasFiscais().ToString();
            txtNotasCanceladas.Text = consultaNotas.CanceladaNotasFiscais().ToString();
            txtNotasEntrada.Text = consultaNotas.EntradaNotasFiscais().ToString();
            DateTime dataConsulta = consultaNotas.DataConsulta().Date;
            DateTime dataHoje = DateTime.Today;

            TimeSpan diff = dataConsulta.Subtract(dataHoje);
            int dias = System.Math.Abs(diff.Days);

            lblDias.Text = dias.ToString("0");
        }
        public frmProtocolo()
        {
            //thread.Start();
            InitializeComponent();
            VerifyVersion(webBrowser);
            webBrowser.Navigate("https://www.nfe.fazenda.gov.br/portal/manifestacaoDestinatario.aspx?tipoConteudo=o9MkXc+hmKs=");
            //webBrowser.Navigate("http://127.0.0.1/teste/index_sefaz.php");
            webBrowser.ScriptErrorsSuppressed = true;
            ConsultaNotasFiscaisBD();
            emailsConsulta();
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
        }
        private void UpdateNotaFiscalCancelada(string chaveCancelada)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE `tb_chave` SET `status` = @canceladaNotaFiscal, `col_Downl` = @concluidoDown WHERE `col_chave` = @chaveNotaFiscal;", ConexaoDados.GetConnectionXML());
                cmd.Parameters.AddWithValue("@canceladaNotaFiscal", "CANCELADA");
                cmd.Parameters.AddWithValue("@concluidoDown", "2");
                //cmd.Parameters.AddWithValue("@envioEmail", "4");
                cmd.Parameters.AddWithValue("@chaveNotaFiscal", chaveCancelada);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                log.WriteLog("Erro Update Chave : "+ex.Message);
            }
        }
        private void UpdateNotaFiscalEnviada(string emailEnviadoChave)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE `tb_chave` SET `col_envioEmail` = @emailEnviado WHERE `col_chave` = @chaveNotaFiscal;", ConexaoDados.GetConnectionXML());
                cmd.Parameters.AddWithValue("@emailEnviado", "3");
                cmd.Parameters.AddWithValue("@chaveNotaFiscal", emailEnviadoChave);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                log.WriteLog("Erro Update Chave : " + ex.Message);
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
                lblNFeQtdDataGridView.Text = countg.ToString();
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
                                    }
                                    txtNFE.Text = txtChave.Text.Substring(25, 9);
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
                List<string> chavesCanceladas = new List<string>();

                HtmlElementCollection trElements = this.webBrowser.Document.GetElementsByTagName("tr");
                int checkChave = 0;
                foreach (HtmlElement trElement in trElements)
                {
                    if (trElement.GetAttribute("className") == "linhaImparCentralizada" || trElement.GetAttribute("className") == "linhaParCentralizada")
                    {
                        HtmlElementCollection tdElements = trElement.GetElementsByTagName("td");

                        if (tdElements.Count > 3 && tdElements[3].InnerText.Trim() == "Cancelada")
                        {
                            // Obtém a chave de acesso e adiciona à lista
                            if (tdElements.Count > 2)
                            {
                                string chaveAcesso = tdElements[2].InnerText.Trim();
                                chavesCanceladas.Add(chaveAcesso.Substring(0,44));
                                UpdateNotaFiscalCancelada(chavesCanceladas[checkChave]);
                                checkChave++;
                            }
                        }
                    }
                }


                HtmlElementCollection manifestacaoElements = this.webBrowser.Document.GetElementsByTagName("tr");
                int checkmaniChave = 0;
                foreach (HtmlElement manifestElement in manifestacaoElements)
                {
                    if (manifestElement.GetAttribute("className") == "linhaImparCentralizada" || manifestElement.GetAttribute("className") == "linhaParCentralizada")
                    {
                        HtmlElementCollection tdElements = manifestElement.GetElementsByTagName("td");

                        if (tdElements.Count > 3 && tdElements[4].InnerText.Trim() == "Operação não Realizada")
                        {
                            // Obtém a chave de acesso e adiciona à lista
                            if (tdElements.Count > 2)
                            {
                                string chaveAcesso = tdElements[2].InnerText.Trim();
                                chavesCanceladas.Add(chaveAcesso.Substring(0, 44));
                                UpdateNotaFiscalCancelada(chavesCanceladas[checkmaniChave]);
                                checkmaniChave++;
                            }
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
                chavesCanceladas.Clear();
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
        string envioEmail;
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
                            if (txtChave.Text.Contains("12706289000148"))
                            {

                            }
                            else
                            {
                                MySqlCommand prompt_cmd = new MySqlCommand("INSERT INTO tb_chave (col_chave,empresa,status,col_Downl,col_dataHoraCriacao,emisao,tpNF,vNF,n_nfe,col_envioEmail) VALUES (@chaveAcesso, @razaoSocialEmpresa, @statusNF, @downloadXML, @dataHoraData, @dataData, @tipoOperador, @valorNF, @numeroNF, @envioEmail)", ConexaoDados.GetConnectionXML());
                                prompt_cmd.Parameters.AddWithValue("@chaveAcesso", txtChave.Text.Trim());
                                prompt_cmd.Parameters.AddWithValue("@razaoSocialEmpresa", razaoSocial);
                                prompt_cmd.Parameters.AddWithValue("@statusNF", ".");
                                prompt_cmd.Parameters.AddWithValue("@downloadXML", "1");
                                prompt_cmd.Parameters.AddWithValue("@dataHoraData", dataHora.Data.ToString("yyyy-MM-dd") + " " + dataHora.Hora.ToString("HH:mm:ss"));
                                prompt_cmd.Parameters.AddWithValue("@dataData", dataHora.Data.ToString("yyyy-MM-dd"));
                                prompt_cmd.Parameters.AddWithValue("@tipoOperador", operadorTipo);
                                prompt_cmd.Parameters.AddWithValue("@valorNF", valorNotaFiscalDec.ToString("C").Replace("R$ ", ""));
                                prompt_cmd.Parameters.AddWithValue("@numeroNF", txtNFE.Text);
                                prompt_cmd.Parameters.AddWithValue("@envioEmail", "4");
                                prompt_cmd.ExecuteNonQuery();
                                ConexaoDados.GetConnectionXML().Close();
                            }
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

        /*
         *Consulta o Banco de Dados quais notas fiscais
         *não foram enviadas por email
         */
        List<string> empresaNFe = new List<string>();
        List<DateTime> emissaoNFe = new List<DateTime>();
        List<DateTime> dataEmissaoNFe = new List<DateTime>();
        List<TimeSpan> horaEmissaoNFe = new List<TimeSpan>();
        List<string> chaveNFe = new List<string>();
        List<decimal> valorNFe = new List<decimal>();
        List<Int16> tpOperacaoNFe = new List<Int16>();
        List<string> canceladaNFe = new List<string>();
        private void BancoVerifica()
        {
            try
            {
                MySqlCommand conn = new MySqlCommand("SELECT *  FROM `tb_chave` WHERE `col_Downl` = 2 AND `col_envioEmail` = 4 ORDER BY col_dataHoraCriacao ASC", ConexaoDados.GetConnectionXML());
                conn.ExecuteNonQuery();
                MySqlDataReader leituraBanco = conn.ExecuteReader();
                while (leituraBanco.Read())
                {
                    empresaNFe.Add(leituraBanco.GetString("empresa"));
                    emissaoNFe.Add(leituraBanco.GetDateTime("col_dataHoraCriacao"));
                    chaveNFe.Add(leituraBanco.GetString("col_chave"));
                    valorNFe.Add(leituraBanco.GetDecimal("vNF"));
                    tpOperacaoNFe.Add(leituraBanco.GetInt16("tpNF"));
                    canceladaNFe.Add(leituraBanco.GetString("status"));
                }
                leituraBanco.Close();
                ConexaoDados.GetConnectionXML().Close();
                SenderEmail_();
            }
            catch (Exception Err)
            {
                MessageBox.Show("Erro: " + Err.Message);
            }
        }
        int delete;
        private void SenderEmail_()
        {
            lblEmailEnviado.Visible = false;
            EmailSender emailSender = new EmailSender();
            int s = empresaNFe.Count;
            
            foreach (string destinatario in destinatarios)
            {
                for (int i = 0; i < s; i++)
                {
                    emailSender.SendEmail(destinatario, emissaoNFe[i], emissaoNFe[i], chaveNFe[i], empresaNFe[i], valorNFe[i], tpOperacaoNFe[i], canceladaNFe[i]);
                    Log.log.WriteLog("Info : Chave de Acesso " + chaveNFe[i] + " enviada!");
                    UpdateNotaFiscalEnviada(chaveNFe[i]);
                    delete = i;
                }
            }
            MensagemClasseDiag mensagem = new MensagemClasseDiag();
            mensagem.MensagemEnvioEmail();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            ConsultaChaveSefaz();
        }
        private void SenderEmailReg()
        {
            lblEmailEnviado.Visible = false;
            EmailSender emailSender = new EmailSender();
            foreach (string destinatario in destinatarios)
            {
                emailSender.SendEmailRegistros(destinatario);
            }
            lblEmailEnviado.Visible = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
           if (MessageBox.Show("Deseja Baixar XML's?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
           {
               FormDownloadXML frm_Main = new FormDownloadXML();
               frm_Main.Show();
           }
        }
        private void LoadTudo()
        {
            LoadDataGrid();
            LoadDataGridPress();
            txtUrl.Text = @"C:\ArquivosSAP\xml\";
            label7.Text = @"C:\ArquivosSAP\xml\";
            lblChaveDuplicidade.Visible = false;
            ConsultaNotasFiscaisBD();
        }
        private void frmProtocolo_Load(object sender, EventArgs e)
        {
            DateTime dataInicioAvaliacao = DateTime.ParseExact("02/09/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dataAtual = DateTime.Now;
            TimeSpan duracaoAvaliacao = dataAtual - dataInicioAvaliacao;
            TimeSpan duracaoAvaliacaolbl = dataInicioAvaliacao.AddDays(1) - DateTime.Now;
            if (duracaoAvaliacaolbl.TotalDays > 0)
            {
                lblperiodoAvaliacao.Text = duracaoAvaliacaolbl.Days + " dias restantes";
                LoadTudo();
            }
            else
            {
                lblperiodoAvaliacao.Text = "Ativo!";
                LoadTudo();
            }
        }
        private void LoadDataGrid()
        {
            MySqlDataAdapter ADAP = new MySqlDataAdapter("SELECT TipoDoc.col_desc_NFe, TbChave.* FROM `tb_chave` AS TbChave LEFT JOIN tb_tipo_nfe AS TipoDoc ON TbChave.tpNF=TipoDoc.col_id WHERE status NOT IN('CANCELADA','REGISTRADA') AND tpNF NOT IN('0') ORDER BY col_dataHoraCriacao DESC", ConexaoDados.GetConnectionXML());
            DataTable SS = new DataTable();
            ADAP.Fill(SS);
            dataGridViewSefaz.DataSource = SS;
            ConexaoDados.GetConnectionXML().Close();
            int countg = dataGridViewSefaz.RowCount;
            lblNFeQtdDataGridView.Text = countg.ToString();
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
            lblNFeQtdDataGridView.Text = countg.ToString();
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
                                MySqlCommand prompt_cmd = new MySqlCommand("INSERT INTO `tb_chave` (`col_chave`, `col_nsu`, `empresa`, `n_nfe`, `emisao`, `lancamento_sap`, `protocolo`, `user_sap`, `status`, `col_Downl`, `col_link`, `tpNF`, `vNF`) VALUES " +
                                    "('" + str[l].Replace("-procNfe.xml", "") + "', NULL, '" + txtEmpresa.Text.Trim() + "', '" + txtNFE.Text.Trim() + "', '" + txtdate.Text.Trim() + "', NULL, NULL, NULL, '.', NULL, NULL, NULL, '" + vNF.Text + "')", ConexaoDados.GetConnectionXML());

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
                            File.Delete(@"C:\ArquivosSAP\xml\" + str[l]);
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
                            lblNFeConsultadas.Text = XMLCont.ToString();
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
                frmXML xML = new frmXML();
                xML.Show();

                LoadDataGrid();
            }
        }
        //richTextBox1.AppendText(el.GetAttribute("type").Equals("span") + Environment.NewLine);
        private void button3_Click(object sender, EventArgs e)
        {
            /*if (MessageBox.Show("Deseja Voltar?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frm_Main frm_Main = new frm_Main();
                frm_Main.Show();
                this.Visible = false;
            }*/
            BancoVerifica();
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
                    MySqlCommand mySqlCommand = new MySqlCommand("INSERT INTO `tb_dataverificacao` (`col_dataUltVerif`) VALUES (@NOW)", ConexaoDados.GetConnectionXML());
                    mySqlCommand.Parameters.AddWithValue("@NOW", DateTime.Now);
                    mySqlCommand.ExecuteNonQuery();
                    CSapROTWrapper sapROTWrapper = new CSapROTWrapper();
                    object SapGuilRot = sapROTWrapper.GetROTEntry("SAPGUI");
                    object engine = SapGuilRot.GetType().InvokeMember("GetScriptingEngine", System.Reflection.BindingFlags.InvokeMethod, null, SapGuilRot, null);
                    GuiApplication GuiApp = (GuiApplication)engine;
                    GuiConnection connection = (GuiConnection)GuiApp.Connections.ElementAt(0);
                    GuiSession Session = (GuiSession)connection.Children.ElementAt(0);
                    GuiFrameWindow guiWindow = Session.ActiveWindow;
                    guiWindow.Maximize();
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
                    string ValorNFe;
                    int Chave = 0;
                    int LancadasCh = 1;
                    int ProgressBarra = 1;
                    while (Chave < countg)
                    {
                        int countgXML = dataGridViewRestante.RowCount;
                        if (dataGridViewSefaz.Rows[Chave].Cells["Column10"].Value.ToString() == "REGISTRADA")
                        {

                        }
                        else
                        {
                            try
                            {
                                //ProgBar.Value = ProgressBarra;
                                ((GuiTextField)Session.FindById("wnd[0]/usr/ctxtBUKRS-LOW")).Text = "USGA";
                                ((GuiTextField)Session.FindById("wnd[0]/usr/txtR_ACCKEY-LOW")).Text = dataGridViewSefaz.Rows[Chave].Cells["Column2"].Value.ToString();
                                ((GuiButton)Session.FindById("wnd[0]/tbar[1]/btn[8]")).Press();
                                ((GuiGridView)Session.FindById("wnd[0]/usr/cntlNFE_CONTAINER/shellcont/shell")).CurrentCellColumn = "";
                                ((GuiGridView)Session.FindById("wnd[0]/usr/cntlNFE_CONTAINER/shellcont/shell")).SelectedRows = "0";
                                ((GuiGridView)Session.FindById("wnd[0]/usr/cntlNFE_CONTAINER/shellcont/shell")).CurrentCellColumn = "NFNUM9";
                                ((GuiGridView)Session.FindById("wnd[0]/usr/cntlNFE_CONTAINER/shellcont/shell")).ClickCurrentCell();
                                try
                                {
                                    EMPRESA = ((GuiTextField)Session.FindById("wnd[0]/usr/subMAIN_PARTNER:SAPLJ1BB2:5250/txtJ_1BDYMPA-MAINNAME1")).Text;
                                }
                                catch (Exception ErroEmpresa)
                                {
                                    EMPRESA = ((GuiTextField)Session.FindById("wnd[0]/usr/subMAIN_PARTNER:SAPLJ1BB2:5200/txtJ_1BDYMPA-MAINNAME1")).Text;
                                }
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
                                dateTimePicker2.Text = Lancamento;
                                dateTimePicker2.Format = DateTimePickerFormat.Custom;
                                dateTimePicker2.CustomFormat = "yyyy-MM-dd";
                                try
                                {
                                    MySqlCommand prompt_cmd = new MySqlCommand("UPDATE `tb_chave` SET empresa='" + EMPRESA + "' , n_nfe='" + NOTAFISCAL + "', lancamento_sap='" + this.dateTimePicker2.Text + "', protocolo='" + Protocolo + "',  user_sap='" + User + "', status='REGISTRADA', vNF='R$: " + ValorNFe + "', col_Downl='2' WHERE col_chave='" + dataGridViewSefaz.Rows[Chave].Cells["Column2"].Value.ToString() + "'", ConexaoDados.GetConnectionXML());
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
                                    MySqlCommand prompt_cmd = new MySqlCommand("UPDATE `tb_chave` SET status='NÃO REGISTRADA' WHERE col_chave='" + dataGridViewSefaz.Rows[Chave].Cells["Column2"].Value.ToString() + "'", ConexaoDados.GetConnectionXML());
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
                        lblNFeConsultadas.Text = LancadasCh.ToString();
                        Chave++;
                        ProgressBarra++;
                        LancadasCh++;
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
            
            MensagemClasseDiag mensagem = new MensagemClasseDiag();
            mensagem.MensagemConclusão(lblNFeConsultadas.Text, lblNFeQtdDataGridView.Text);
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
            //TempoEspera.Enabled = false;
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
                if(maskFiltro.Text == "00/0000")
                {
                    LoadDataGrid();
                }
                else
                {
                    string MES;
                    string ANO;
                    MES = maskFiltro.Text.Substring(0, 2).Trim();
                    ANO = maskFiltro.Text.Substring(3, 4).Trim();

                    MySqlDataAdapter ADAP = new MySqlDataAdapter("SELECT TipoDoc.col_desc_NFe, TbChave.* FROM `tb_chave` AS TbChave LEFT JOIN tb_tipo_nfe AS TipoDoc ON TbChave.tpNF=TipoDoc.col_id WHERE status NOT IN ('REGISTRADA','CANCELADA') AND tpNf!=0 AND MONTH(emisao)='" + MES + "' AND  YEAR(emisao)='" + ANO + "' ORDER BY `emisao` ASC", ConexaoDados.GetConnectionXML());
                    //MySqlDataAdapter ADAP = new MySqlDataAdapter("SELECT * FROM `tb_chave`  WHERE `emisao` BETWEEN '2023-06-01' AND '2023-06-30' AND status NOT IN('REGISTRADA','CANCELADA')", ConexaoDados.GetConnectionXML());
                    DataTable SS = new DataTable();
                    ADAP.Fill(SS);
                    dataGridViewSefaz.DataSource = SS;
                    ConexaoDados.GetConnectionXML().Close();
                    int countg = dataGridViewSefaz.RowCount;
                    lblNFeQtdDataGridView.Text = countg.ToString();
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja ir para o Relatório Geral de Danf's?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FormRelacao frm_Main = new FormRelacao();
                frm_Main.Show();
                //Close();
            }
        }

        private void dataGridViewSefaz_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {

        }
    }
}

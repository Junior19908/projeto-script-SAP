using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;
using SistemaGSG.Log;
using sun.util.calendar;

namespace SistemaGSG
{
    public partial class frmLogin : MetroFramework.Forms.MetroForm
    {
        int attempt = 1;
        public frmLogin()
        {
            InitializeComponent();
            label3.Text = version;
        }
        string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public bool FMP = false;

        public void logar()
        {
            try
            {
                string tb_user = "SELECT * FROM DBSGSG_Login WHERE col_userLogin = @usuario";
                OleDbCommand cmd;
                OleDbDataReader dr;
                cmd = new OleDbCommand(tb_user, ConexaoBancoDeDadosOffline.DBSGSG_Conex());
                //cmd.Connection.Open();
                //Verificar Usuário//
                cmd.Parameters.Add(new OleDbParameter("@usuario", txtUser.Text));

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    dados.NomeCompleto = Convert.ToString(dr["col_nameFull"]);
                    dados.Usuario = Convert.ToString(dr["col_userLogin"]);
                    dados.senha = Convert.ToString(dr["col_passwordUser"]);
                    dados.nivel = Convert.ToInt32(dr["col_levelUser"]);
                    dados.IdUser = Convert.ToInt32(dr["col_codigo"]);
                }
                if (dados.senha == txtSenha.Text)
                {
                    if (dados.nivel == 3)
                    {
                        log.WriteLog($"Info : Login de Usuário Padrão efetuado com sucesso! - {txtUser.Text}");
                        FormRelacao AbrirForm = new FormRelacao();
                        AbrirForm.Show();
                    }
                    if (dados.nivel == 1)
                    {
                        log.WriteLog($"Info : Login de Administrador efetuado com sucesso! - {txtUser.Text}");
                        FMP = true;
                        this.Dispose();
                    }
                }
                else
                {
                    int cont = 3;
                    int Menos = cont - attempt;
                    attempt++;
                    label5.Visible = true;
                    label5.Text = "Erro você ainda tem " + Menos + " chances.";
                    label5.ForeColor = Color.Red;
                    FMP = false;
                    log.WriteLog($"Info : Senha incorreta contagem {Menos} - Usuário - {txtUser.Text}");
                    txtUser.Text = "";
                    txtSenha.Text = "";
                }
                if (attempt == 4)
                {
                    label6.Visible = true;
                    label6.Text = "Você teve 3 de 3 tentativas, Feche o programa e tente novamente.";
                    label6.ForeColor = Color.Blue;
                    txtUser.Visible = false;
                    label5.Visible = false;
                    txtSenha.Visible = false;
                    btnEntrar.Visible = false;
                    lblUsuario.Visible = false;
                    lblSenha.Visible = false;
                    log.WriteLog("Info : Excesso de tentativas!");
                }
            }
            catch (OleDbException ErrO)
            {
                MessageBox.Show("Erro no Banco de Dados! - \n Não Foi Possível Conectar!");
                log.WriteLog("Warning : " + ErrO.Message);
                log.WriteLog("Info : Erro no Banco de Dados - Não Foi Possível Conectar!");

                if (MessageBox.Show("Deseja encerrar a aplicação ?" + ErrO.HelpLink, "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    log.WriteLog("Info : Programa encerrado pelo usuário!");
                    Application.Exit();
                }
                else
                {
                    if (MessageBox.Show("Deseja entrar no modo offline?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        dados.nivel = Convert.ToInt32("1");
                        log.WriteLog("Info : Programa executado em modo offline pelo usuário!");
                        FMP = true;
                        this.Dispose();

                    }
                }
            }
            catch (Exception Err)
            {
                log.WriteLog("Warning : " + Err.Message);
                MessageBox.Show(Err.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (attempt < 4)
            {
                logar();
            }
            else
            {

            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            log.WriteLog("Info : Programa fechado!");
            Application.Exit();
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = ConexaoBancoDeDadosOffline.DBSGSG_Conex();

            }
            catch (Exception ERROR)
            {
                log.WriteLog("Warning : " + ERROR.Message);
                MessageBox.Show(ERROR.Message);
            }
            try
            {

                if (ConexaoBancoDeDadosOffline.DBSGSG_Conex().State == System.Data.ConnectionState.Open)
                {
                    label1.ForeColor = Color.Lime;
                    label1.Text = "Conectado...";
                    ConexaoBancoDeDadosOffline.DBSGSG_Conex().Close();
                }
                else
                {
                    label1.ForeColor = Color.Red;
                    label1.Text = "Não Conectado...";
                }
            }
            catch (OleDbException MysqlErr)
            {
                MessageBox.Show("Erro no Banco de Dados! -\nNão Foi Possivel Conectar!");
                log.WriteLog("Warning : " + MysqlErr.Message);
                log.WriteLog("Info : Erro no Banco de Dados - Não Foi Possível Conectar!");

                if (MessageBox.Show("Localizar banco de dados ?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    log.WriteLog("Info : Busca por banco de dados iniciada!");

                    ConfigConexao configConexao = new ConfigConexao();
                    configConexao.ShowDialog();
                }
                else
                {
                    Application.Exit();
                }
                label1.Text = "Não Conectado...";
            }
            catch (Exception Err)
            {
                log.WriteLog("Warning : " + Err.Message);
                MessageBox.Show(Err.Message);
            }
        }
    }
}

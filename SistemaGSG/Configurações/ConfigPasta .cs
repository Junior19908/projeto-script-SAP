using SistemaGSG.Log;
using sun.util.calendar;
using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace SistemaGSG
{
    public partial class ConfigPasta : MetroFramework.Forms.MetroForm
    {
        public ConfigPasta()
        {
            InitializeComponent();
        }
        //VariaveisDeclaradas

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCaminho.Text))
                {

                }
                else
                {
                    OleDbCommand command = new OleDbCommand("INSERT INTO DBSGSG_Pasta(`col_UrlPasta`) VALUES ('"+txtCaminho.Text+"')", ConexaoBancoDeDadosOffline.DBSGSG_Conex());
                    command.ExecuteNonQuery();
                    MessageBox.Show("Cadastrado!");
                    log.WriteLog("Info : Pasta Cadastrada.");
                }
            }
            catch (OleDbException eRR)
            {
                MessageBox.Show(eRR.Message);
                log.WriteLog("Warning : " + eRR.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.WriteLog("Warning : " + ex.Message);
            }
        }

        private void ConfigPasta_Load(object sender, EventArgs e)
        {

        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

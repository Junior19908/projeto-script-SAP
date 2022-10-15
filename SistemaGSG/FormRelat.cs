using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaGSG
{
    public partial class FormRelat : MetroFramework.Forms.MetroForm
    {
        DateTime dateTime = DateTime.Now;
        public FormRelat()
        {
            InitializeComponent();
        }

        private void GerarRelatorio()
        {
            DataTable dt = new DataTable();
            OleDbCommand cm = new OleDbCommand("SELECT col_cidadeDest,col_unidade,col_codReceb,Sum(col_quantidade) AS col_quantidade,Sum(col_vlBruto) AS col_vlBruto FROM DBSGSG_SaidaSemana WHERE col_dataEmissao BETWEEN #" + dtFrom.Value.ToString("dd/MM/yyyy") +"# AND #"+ dtToDate.Value.ToString("dd/MM/yyyy") + "# AND col_grupoMerc = 9410 AND col_cfopDest NOT IN('6118/AA') AND col_tipoOrdem NOT IN('ZVVF') AND col_estadoDest NOT BETWEEN 'EX' AND 'EX....'  AND col_tipoFat NOT IN('ZFCO') AND col_status NOT IN(5) GROUP BY col_cidadeDest,col_unidade,col_codReceb ORDER BY col_codReceb ASC", ConexaoBancoDeDadosOffline.DBSGSG_Conex());
            OleDbDataReader oleDbDataReader = cm.ExecuteReader();

            dt.Load(oleDbDataReader);
            ReportDataSource rds = new ReportDataSource("TabelaSaidaSemana", dt);
            ReportParameterCollection reportParameters = new ReportParameterCollection();
            reportParameters.Add(new ReportParameter("fromDate", dtFrom.Text));
            reportParameters.Add(new ReportParameter("toDate", dtToDate.Text));
            reportParameters.Add(new ReportParameter("dateToday", dateTime.ToString()));
            reportParameters.Add(new ReportParameter("userName", "SISTEMA DE TESTE"));
            //reportParameters.Add(new ReportParameter("userName", dados.NomeCompleto));
            
            reportViewer1.LocalReport.SetParameters(reportParameters);
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(rds);
            this.reportViewer1.RefreshReport();
            cm.Connection.Close();
        }

        private void FormRelat_Load(object sender, EventArgs e)
        {
            //GerarRelatorio();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GerarRelatorio();
        }
    }
}

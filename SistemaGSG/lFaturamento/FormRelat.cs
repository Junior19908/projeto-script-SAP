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
        //Salvar
        private void GerarRelatorio()
        {
            int _Segunda = 0;
            int _Terca = 0;
            int _Quarta = 0;
            int _Quinta = 0;
            int _Sexta = 0;
            int _Sabado = 0;
            int _Domingo = 0;

            DataTable dt = new DataTable();
            OleDbCommand cm = new OleDbCommand("SELECT col_cidadeDest,col_unidade,col_codReceb,Sum(col_quantidade) AS col_quantidade,Sum(col_vlBruto) AS col_vlBruto FROM DBSGSG_SaidaSemana WHERE col_dataEmissao BETWEEN #" + dtFrom.Value.ToString("dd/MM/yyyy") + "# AND #" + dtToDate.Value.ToString("dd/MM/yyyy") + "# AND col_grupoMerc = 9410 AND col_cfopDest NOT IN('6118/AA') AND col_tipoOrdem NOT IN('ZVVF','ZVRA') AND col_estadoDest NOT BETWEEN 'EX' AND 'EX....'  AND col_tipoFat NOT IN('ZFCO') AND col_status NOT IN(5) GROUP BY col_cidadeDest,col_unidade,col_codReceb ORDER BY col_codReceb ASC", ConexaoBancoDeDadosOffline.DBSGSG_Conex());
            OleDbDataReader oleDbDataReader = cm.ExecuteReader();

            DataTable dr = new DataTable();
            OleDbCommand cr = new OleDbCommand("SELECT COUNT(col_tipoEmb) AS col_tipoEmb FROM `DBSGSG_SaidaSemana` WHERE col_dataEmissao = #" + dtFrom.Value.ToString("dd/MM/yyyy") + "# AND col_tipoEmb NOT IN('')", ConexaoBancoDeDadosOffline.DBSGSG_Conex());
            OleDbDataReader oleDbDataReaderr = cr.ExecuteReader();

            while (oleDbDataReaderr.Read())
            {
                _Segunda = Convert.ToInt16(oleDbDataReaderr["col_tipoEmb"].ToString());
                break;
            }


            DataTable drT = new DataTable();
            OleDbCommand crT = new OleDbCommand("SELECT COUNT(col_tipoEmb) AS col_tipoEmb FROM `DBSGSG_SaidaSemana` WHERE col_dataEmissao = DateAdd('d',1, #" + dtFrom.Value.ToString("dd/MM/yyyy") + "#) AND col_tipoEmb NOT IN('')", ConexaoBancoDeDadosOffline.DBSGSG_Conex());
            OleDbDataReader oleDbDataReaderrT = crT.ExecuteReader();

            while (oleDbDataReaderrT.Read())
            {
                _Terca = Convert.ToInt16(oleDbDataReaderrT["col_tipoEmb"].ToString());
                break;
            }

            OleDbCommand crQ = new OleDbCommand("SELECT COUNT(col_tipoEmb) AS col_tipoEmb FROM `DBSGSG_SaidaSemana` WHERE col_dataEmissao = DateAdd('d',2, #" + dtFrom.Value.ToString("dd/MM/yyyy") + "#) AND col_tipoEmb NOT IN('')", ConexaoBancoDeDadosOffline.DBSGSG_Conex());
            OleDbDataReader oleDbDataReaderrQ = crQ.ExecuteReader();

            while (oleDbDataReaderrQ.Read())
            {
                _Quarta = Convert.ToInt16(oleDbDataReaderrQ["col_tipoEmb"].ToString());
                break;
            }

            OleDbCommand crQu = new OleDbCommand("SELECT COUNT(col_tipoEmb) AS col_tipoEmb FROM `DBSGSG_SaidaSemana` WHERE col_dataEmissao = DateAdd('d',3, #" + dtFrom.Value.ToString("dd/MM/yyyy") + "#) AND col_tipoEmb NOT IN('')", ConexaoBancoDeDadosOffline.DBSGSG_Conex());
            OleDbDataReader oleDbDataReaderrQu = crQu.ExecuteReader();

            while (oleDbDataReaderrQu.Read())
            {
                _Quinta = Convert.ToInt16(oleDbDataReaderrQu["col_tipoEmb"].ToString());
                break;
            }

            DataTable drS = new DataTable();
            OleDbCommand crS = new OleDbCommand("SELECT COUNT(col_tipoEmb) AS col_tipoEmb FROM `DBSGSG_SaidaSemana` WHERE col_dataEmissao = DateAdd('d',4, #" + dtFrom.Value.ToString("dd/MM/yyyy") + "#) AND col_tipoEmb NOT IN('')", ConexaoBancoDeDadosOffline.DBSGSG_Conex());
            OleDbDataReader oleDbDataReaderrS = crS.ExecuteReader();

            while (oleDbDataReaderrS.Read())
            {
                _Sexta = Convert.ToInt16(oleDbDataReaderrS["col_tipoEmb"].ToString());
                break;
            }

            DataTable drSa = new DataTable();
            OleDbCommand crSa = new OleDbCommand("SELECT COUNT(col_tipoEmb) AS col_tipoEmb FROM `DBSGSG_SaidaSemana` WHERE col_dataEmissao = DateAdd('d',5, #" + dtFrom.Value.ToString("dd/MM/yyyy") + "#) AND col_tipoEmb NOT IN('')", ConexaoBancoDeDadosOffline.DBSGSG_Conex());
            OleDbDataReader oleDbDataReaderrSa = crSa.ExecuteReader();

            while (oleDbDataReaderrSa.Read())
            {
                _Sabado = Convert.ToInt16(oleDbDataReaderrSa["col_tipoEmb"].ToString());
                break;
            }


            DataTable drDo = new DataTable();
            OleDbCommand crDo = new OleDbCommand("SELECT COUNT(col_tipoEmb) AS col_tipoEmb FROM `DBSGSG_SaidaSemana` WHERE col_dataEmissao = DateAdd('d',6, #" + dtFrom.Value.ToString("dd/MM/yyyy") + "#) AND col_tipoEmb NOT IN('')", ConexaoBancoDeDadosOffline.DBSGSG_Conex());
            OleDbDataReader oleDbDataReaderrDo = crDo.ExecuteReader();

            while (oleDbDataReaderrDo.Read())
            {
                _Domingo = Convert.ToInt16(oleDbDataReaderrDo["col_tipoEmb"].ToString());
                break;
            }

            int somaCarretas = _Segunda + _Terca + _Quarta + _Quinta + _Sexta + _Sabado + _Domingo;





            dt.Load(oleDbDataReader);



            ReportDataSource rds = new ReportDataSource("TabelaSaidaSemana", dt);

            ReportParameterCollection reportParameters = new ReportParameterCollection();
            reportParameters.Add(new ReportParameter("somaCarretas", somaCarretas.ToString()));
            reportParameters.Add(new ReportParameter("segundaFeira", _Segunda.ToString()));
            reportParameters.Add(new ReportParameter("tercaFeira", _Terca.ToString()));
            reportParameters.Add(new ReportParameter("quartaFeira", _Quarta.ToString()));
            reportParameters.Add(new ReportParameter("quintaFeira", _Quinta.ToString()));
            reportParameters.Add(new ReportParameter("sextaFeira", _Sexta.ToString()));
            reportParameters.Add(new ReportParameter("sabado", _Sabado.ToString()));
            reportParameters.Add(new ReportParameter("domingo", _Domingo.ToString()));
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

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace SistemaGSG.NotasFiscais
{
    internal class ConsultaNotasFiscais
    {
        public int TotalNotasFiscais()
        {
                int total = 0;
                string query = "SELECT COUNT(*) FROM tb_chave";
                MySqlCommand command = new MySqlCommand(query, ConexaoDados.GetConnectionXML());
                total = Convert.ToInt32(command.ExecuteScalar());
                return total; 
        }
        public int OmissasNotasFiscais()
        {
            int omissas = 0;
            string query = "SELECT COUNT(*) FROM tb_chave WHERE status NOT IN('CANCELADA','REGISTRADA') AND tpNF NOT IN('0')";
            MySqlCommand command = new MySqlCommand(query, ConexaoDados.GetConnectionXML());
            omissas = Convert.ToInt32(command.ExecuteScalar());
            return omissas;
        }
        public int CanceladaNotasFiscais()
        {
            int cancelada = 0;
            string query = "SELECT COUNT(*) FROM tb_chave WHERE status = 'CANCELADA'";
            MySqlCommand command = new MySqlCommand(query, ConexaoDados.GetConnectionXML());
            cancelada = Convert.ToInt32(command.ExecuteScalar());
            return cancelada;
        }
        public int EntradaNotasFiscais()
        {
            int entrada = 0;
            string query = "SELECT COUNT(*) FROM tb_chave WHERE tpNF = 0";
            MySqlCommand command = new MySqlCommand(query, ConexaoDados.GetConnectionXML());
            entrada = Convert.ToInt32(command.ExecuteScalar());
            return entrada;
        }
        public int RegistradasNotasFiscais()
        {
            int registradas = 0;
            string query = "SELECT COUNT(*) FROM tb_chave WHERE status NOT IN('CANCELADA','NÃO REGISTRADA') AND tpNF = 1";
            MySqlCommand command = new MySqlCommand(query, ConexaoDados.GetConnectionXML());
            registradas = Convert.ToInt32(command.ExecuteScalar());
            return registradas;
        }
        public DateTime DataConsulta()
        {
            DateTime dataConsulta = DateTime.Now;

            string query = "SELECT col_dataHoraCriacao FROM tb_chave ORDER BY `tb_chave`.`col_dataHoraCriacao` DESC";
            MySqlCommand command = new MySqlCommand(query, ConexaoDados.GetConnectionXML());
            dataConsulta = (DateTime)command.ExecuteScalar();

            return dataConsulta;
        }
    }
}
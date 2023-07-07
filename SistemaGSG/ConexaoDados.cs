using MySql.Data.MySqlClient;
using System;

namespace SistemaGSG
{
    public class ConexaoDados
    {
        private static readonly string server = "10.2.1.4";
        private static readonly string username = "xml";
        private static readonly string password = "02984646#Lua";
        private static readonly string sslMode = "none";

        public static MySqlConnection GetConnectionXML()
        {
            string database = "sistemagsgxml";
            return CreateConnection(database);
        }

        public static MySqlConnection GetConnectionAlmoxarifado()
        {
            string database = "sistemagsgalmoxarifado";
            return CreateConnection(database);
        }

        public static string ACESSO()
        {
            return "http://10.2.1.4/sistemagsgv2.0/template/dashboard/pages/relatorios/faturamento/acesso/RelatorioAcesso.php";
        }

        public static string CHECKLIST()
        {
            return "http://10.2.1.4/sistemagsgv2.0/template/dashboard/pages/relatorios/faturamento/acesso/RelatorioCheckList.php";
        }

        public static MySqlConnection GetConnectionFaturameto()
        {
            string database = "sistemagsgfaturamento";
            return CreateConnection(database);
        }

        public static MySqlConnection GetConnectionFornecedor()
        {
            string database = "sistemagsgfornecedor";
            return CreateConnection(database);
        }

        public static MySqlConnection GetConnectionEquatorial()
        {
            string database = "sistemagsgequatorial";
            return CreateConnection(database);
        }

        public static MySqlConnection GetConnectionPosto()
        {
            string database = "sistemagsgposto";
            return CreateConnection(database);
        }

        private static MySqlConnection CreateConnection(string database)
        {
            MySqlConnection connection = new MySqlConnection();
            connection.ConnectionString = $"server={server};database={database};Uid={username};Pwd={password};SslMode={sslMode};";
            connection.Open();
            return connection;
        }
    }
}

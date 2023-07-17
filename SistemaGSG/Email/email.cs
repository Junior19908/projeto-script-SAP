using MySql.Data.MySqlClient;
using org.junit.@internal.runners.statements;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Windows;
using static sun.awt.geom.AreaOp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace SistemaGSG.Email
{
    public class EmailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly string _emailNome = "SIGTI - SISTEMAS INTEGRADOS TI";
        private readonly string _email = "sigtisistemasintegrados@gmail.com";
        public EmailSender()
        {
            _host = "smtp.gmail.com";
            _port = 587;
            _username = "carlosjunyoor@gmail.com";
            _password = "bcnkjvyotwfkrdlm";
        }
        public void SendEmail(string destinatario, DateTime date, DateTime hora, string chaveAcesso, string razaoSocial, decimal valorNotaFiscalDec, string tipoOperacao)
        {
            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(_email, _emailNome);
            mensagem.To.Add(new MailAddress(destinatario));
            mensagem.Subject = "Nova Emissão de Nota Fiscal contra seu CNPJ";
            string corpo = $@"
                                                <html lang=""pt-br"">
                                                <head>
                                                    <meta charset=""UTF-8"">
                                                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                                    <title>Nota Fiscal - Apresentação ao Cliente</title>
                                                    <style>
                                                        /* Estilos CSS */
                                                        body {{
                                                            font-family: Arial, sans-serif;
                                                            color: #333;
                                                            margin: 0;
                                                            padding: 0;
                                                            background-color: #f9f9f9;
                                                        }}
                                                        .container {{
                                                            max-width: 600px;
                                                            margin: 0 auto;
                                                            padding: 20px;
                                                            background-color: #f6f6f6;
                                                            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                                                        }}
                                                        header {{
                                                            text-align: center;
                                                            margin-bottom: 20px;
                                                        }}
                                                        .logo {{
                                                            max-width: 200px;
                                                            margin: 0 auto;
                                                        }}
                                                        h1 {{
                                                            color: #333;
                                                            font-size: 24px;
                                                            margin: 0;
                                                            padding: 10px 0;
                                                        }}
                                                        table {{
                                                            width: 100%;
                                                            border-collapse: collapse;
                                                            margin-bottom: 20px;
                                                        }}
                                                        td {{
                                                            padding: 10px;
                                                            border-bottom: 1px solid #ccc;
                                                        }}
                                                        td:first-child {{
                                                            font-weight: bold;
                                                            width: 30%;
                                                        }}
                                                        .note {{
                                                            margin-bottom: 20px;
                                                            background-color: #fff;
                                                            padding: 20px;
                                                            border-radius: 5px;
                                                            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                                                        }}
                                                        .note p {{
                                                            margin: 0;
                                                            line-height: 1.5;
                                                        }}
                                                        .signature {{
                                                            text-align: right;
                                                            font-style: italic;
                                                        }}
                                                        .footer-note {{
                                                            font-size: 12px;
                                                            color: #888;
                                                            text-align: center;
                                                            margin-top: 40px;
                                                        }}
                                                        .sap-yes {{
                                                            background-color: #ccffcc;
                                                            color: #006600;
                                                        }}
                                                        .sap-no {{
                                                            background-color: #ffcccc;
                                                            color: #990000;
                                                        }}
                                                    </style>
                                                </head>
                                                <body>
                                                    <div class=""container"">
                                                        <header>
                                                            <img src=""https://i.ibb.co/rsTy1xZ/USGA-01.png"" alt=""Logo da Empresa"" class=""logo"">
                                                            <h1 style=""color: #0066cc;"">Nova Emissão de Nota Fiscal contra seu CNPJ</h1>
                                                        </header>
                                                        <table>
                                                            <tr>
                                                                <td style=""background-color: #f6f6f6; color: #999;"">Data:</td>
                                                                <td style=""background-color: #fff; color: #333;"">{date.ToString("dd/MM/yyyy")}</td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""background-color: #f6f6f6; color: #999;"">Hora:</td>
                                                                <td style=""background-color: #fff; color: #333;"">{hora.ToString("HH:mm:ss")}</td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""background-color: #f6f6f6; color: #999;"">Chave de Acesso:</td>
                                                                <td style=""background-color: #fff; color: #333;"">{chaveAcesso}</td>
                                                            </tr>
                                                        </table>
                                                        <div class=""note"">
                                                            <h2 style=""color: #0066cc;"">Detalhes da Nota Fiscal</h2>
                                                            <p>Prezado(a) USINA SERRA GRANDE S/A,</p>
                                                            <p>É com imenso prazer que compartilhamos com você os detalhes completos da mais recente nota fiscal registrada em nosso sistema.</p>
                                                            <p>Acompanhe abaixo todas as informações relevantes:</p>
                                                            <ul>
                                                                <li><strong>Emissor:</strong> {razaoSocial}</li>
                                                                <li><strong>Valor:</strong> {valorNotaFiscalDec.ToString("C")}</li>
                                                                <li><strong>Data de Emissão:</strong> {date.ToString("dd/MM/yyyy")}</li>
                                                                <li><strong>Tipo de Operação:</strong> {tipoOperacao}</li>
                                                            </ul>
                                                            <p>Estamos à sua disposição para fornecer qualquer informação adicional que você necessitar ou auxiliá-lo(a) com todas as providências necessárias.</p>
                                                            <p>Atenciosamente,</p>
                                                        </div>
                                                        <footer>
                                                            <p class=""signature"">SIGTI</p>
                                                        </footer>
                                                        <p class=""footer-note"">Este e-mail é apenas para fins informativos. Favor não responder.</p>
                                                    </div>
                                                </body>
                                                </html>
                                                ";
            mensagem.Body = corpo;
            mensagem.IsBodyHtml = true;
            SmtpClient clienteSmtp = new SmtpClient(_host, _port);
            clienteSmtp.EnableSsl = true;
            clienteSmtp.Credentials = new NetworkCredential(_username, _password);
            try
            {
                clienteSmtp.Send(mensagem);
                Log.log.WriteLog("Info : E-mail enviado! " + _username);
            }catch(Exception ex)
            {
                Log.log.WriteLog("Warning :"+ ex.Message);
            }
        }
        public class DadosAcessoEmpresa
        {
            public List<string> ChavesDeAcesso { get; set; }
            public List<string> RazoesSociais { get; set; }
        }
        public DadosAcessoEmpresa ObterChavesDeAcessoDoBancoDeDados()
        {
            // Aqui você deve implementar o código para obter as chaves de acesso do banco de dados
            // Neste exemplo, estou retornando algumas chaves de acesso de forma estática
            MySqlCommand prompt = new MySqlCommand("SELECT col_chave,empresa FROM tb_chave WHERE status NOT IN(@lancada,@cancelada) ORDER BY `empresa` ASC;", ConexaoDados.GetConnectionXML());
            prompt.Parameters.AddWithValue("@lancada", "LANÇADA");
            prompt.Parameters.AddWithValue("@cancelada", "CANCELADA");
            List<string> chavesDeAcesso = new List<string>();
            List<string> razaoSocial = new List<string>();
            MySqlDataReader reader = prompt.ExecuteReader();
            while (reader.Read())
            {
                string chave = reader.GetString("col_chave");
                string razao = reader.GetString("empresa");
                chavesDeAcesso.Add(chave);
                razaoSocial.Add(razao);
            }
            reader.Close();
            DadosAcessoEmpresa dadosAcessoEmpresa = new DadosAcessoEmpresa()
            {
                ChavesDeAcesso = chavesDeAcesso,
                RazoesSociais = razaoSocial
            };
            return dadosAcessoEmpresa;
        }
        public void SendEmailRegistros(string destinatario)
        {
            List<string> chavesDeAcesso = ObterChavesDeAcessoDoBancoDeDados().ChavesDeAcesso;
            List<string> razaoSocial = ObterChavesDeAcessoDoBancoDeDados().RazoesSociais;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_email, _emailNome);
            mailMessage.To.Add(new MailAddress(destinatario));
            mailMessage.Subject = "Aviso de notas fiscais pendentes";
            string corpo = $@"
                    <html lang=""pt-br"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Nota Fiscal - Apresentação ao Cliente</title>
                        <style>
                            /* Estilos CSS */
                            body {{
                                font-family: Arial, sans-serif;
                                color: #333;
                                margin: 0;
                                padding: 0;
                                background-color: #f9f9f9;
                            }}
                            .container {{
                                max-width: 600px;
                                margin: 0 auto;
                                padding: 20px;
                                background-color: #f6f6f6;
                                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                            }}
                            header {{
                                text-align: center;
                                margin-bottom: 20px;
                            }}
                            .logo {{
                                max-width: 200px;
                                margin: 0 auto;
                            }}
                            h1 {{
                                color: #333;
                                font-size: 24px;
                                margin: 0;
                                padding: 10px 0;
                            }}
                            table {{
                                width: 100%;
                                border-collapse: collapse;
                                margin-bottom: 20px;
                            }}
                            td {{
                                padding: 10px;
                                border-bottom: 1px solid #ccc;
                            }}
                            td:first-child {{
                                font-weight: bold;
                                width: 30%;
                            }}
                            .note {{
                                margin-bottom: 20px;
                                background-color: #fff;
                                padding: 20px;
                                border-radius: 5px;
                                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                            }}
                            .note p {{
                                margin: 0;
                                line-height: 1.5;
                            }}
                            .signature {{
                                text-align: right;
                                font-style: italic;
                            }}
                            .footer-note {{
                                font-size: 12px;
                                color: #888;
                                text-align: center;
                                margin-top: 40px;
                            }}
                            .sap-yes {{
                                font-size: 10px;
                                background-color: #ccffcc;
                                color: #006600;
                            }}
                            .sap-no {{
                                background-color: #ffcccc;
                                color: #990000;
                            }}
                            .chave{{
                                        font-size: 12px;
                                    }}
                            .update-button {{
                            display: flex;
                            justify-content: flex-end;
                            }}
                            .update-button button {{
                                background-color: #006600;
                                color: #fff;
                                padding: 10px 20px;
                                border: none;
                                cursor: pointer;
                                border-radius: 5px;
                            }}
                            .styled-table {{
                                border-collapse: collapse;
                                margin: 25px 0;
                                font-size: 0.9em;
                                font-family: sans-serif;
                                min-width: 400px;
                                box-shadow: 0 0 20px rgba(0, 0, 0, 0.15);
                            }}
                            .styled-table thead tr {{
                                background-color: #0066cc;
                                color: #ffffff;
                                text-align: left;
                            }}
                            .styled-table th,
                            .styled-table td {{
                                padding: 12px 15px;
                            }}
                            .styled-table tbody tr {{
                                border-bottom: 1px solid #dddddd;
                            }}
                            .styled-table tbody tr:nth-of-type(even) {{
                                background-color: #f3f3f3;
                            }}
                            .styled-table tbody tr:last-of-type {{
                                border-bottom: 2px solid #0066cc;
                            }}
                            .styled-table tbody tr.active-row {{
                                font-weight: bold;
                                color: #009879;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                            <header>
                               <img src=""https://i.ibb.co/rsTy1xZ/USGA-01.png"" alt=""Logo da Empresa"" class=""logo"" width=""300"" height=""200"">
                                <h1 style=""color: #0066cc;"">Notas Fiscais não registradas <br> no <br> SAP ERP S/4 HANA</h1>
                            </header>
                            <div class=""note"">
                                <h2 style=""color: #0066cc;"">Aviso de notas fiscais pendentes</h2>";
                                int tamanhoLista = Math.Min(chavesDeAcesso.Count, razaoSocial.Count);
            corpo += $@"
                                <p>Prezado(a) <strong> USINA SERRA GRANDE S/A, </strong> gostaríamos de informar que identificamos {tamanhoLista} notas fiscais em nosso sistema que não foram devidamente registradas em seu sistema SAP ERP S/4 HANA. Solicitamos gentilmente que verifiquem prontamente essa situação, a fim de evitar possíveis problemas futuros com a SEFAZ e o registro na malha fiscal associado ao CNPJ de sua empresa.</p>
                                <br>
                                <p>A seguir, apresentamos todas as chaves de acesso:</p>
                                <!-- Informação da tabela-->
                                <table class=""styled-table"">
                                    <thead>
                                        <tr>
                                            <th><center>Razão Social</center></th>
                                            <th><center>Chave de Acesso</center></th>
                                        </tr>
                                    </thead>
                                    <tbody>";
                            for (int i = 0; i < tamanhoLista; i++)
                            {
                                string razao = razaoSocial[i];
                                string chave = chavesDeAcesso[i];
                                corpo += $@"<tr><td class=""sap-yes""><center>{razao}</center></td><td class=""chave""><center>{chave}</center></td></tr>";
                            }
            corpo += $@"</tbody>
                                </table>
                                <!-- Fim da Tabela-->
                                <p>Estamos à sua disposição para fornecer qualquer informação adicional que você necessitar ou auxiliá-lo(a) com todas as providências necessárias.</p>
                                <p>Atenciosamente,</p>
                            </div>
                            <footer>
                                <p class=""signature"">SIGTI</p>
                            </footer>
                            <p class=""footer-note"">Este e-mail é apenas para fins informativos. Favor não responder.</p>
                        </div>
                    </body>
                    </html>";
            mailMessage.Body = corpo;
            mailMessage.IsBodyHtml = true;
            SmtpClient clienteSmtp = new SmtpClient(_host, _port);
            clienteSmtp.EnableSsl = true;
            clienteSmtp.Credentials = new NetworkCredential(_username, _password);
            try
            {
                clienteSmtp.Send(mailMessage);
                Log.log.WriteLog("Info : E-mail enviado! " + _username);
            }
            catch (Exception ex)
            {
                Log.log.WriteLog("Warning :" + ex.Message);
            }
        }
    }
}

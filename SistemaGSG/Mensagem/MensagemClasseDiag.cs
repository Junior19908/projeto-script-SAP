using SistemaGSG;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGT.Mensagem
{
    internal class MensagemClasseDiag
    {
        Bitmap iconeLogo = Properties.Resources.SIGT_LOGO_OFC;
        Bitmap marcaLogo = Properties.Resources.document;

        public void MensagemConclusão(string lancadas, string nlancadas)
        {
            int valor;
            valor = Convert.ToInt32(lancadas) - Convert.ToInt32(nlancadas);

            Icon seuIcone = Icon.FromHandle(iconeLogo.GetHicon());
            Bitmap marcaLogo = Properties.Resources.document;
            Image suaLogo = marcaLogo;
            string mensagem = "Prezado Usuário,\n\n"
                             + "Concluímos o processo de verificação das notas fiscais emitidas e registradas no sistema SAP S/4 Hana.\n"
                             + "Durante a verificação, identificamos as seguintes situações:\n\n"
                             + "Notas fiscais devidamente lançadas no SAP S/4 Hana: " + valor +"\n"
                             + "Notas fiscais não lançadas no SAP S/4 Hana: " + nlancadas +"\n\n"
                             + "Solicitamos que tome as devidas providências com os departamentos responsáveis para garantir a integração adequada das notas fiscais não lançadas.\n\n"
                             + "Caso necessite de assistência, esclarecimentos adicionais ou precise reportar qualquer problema, nossa equipe está à disposição para prestar suporte.\n\n"
                             + "Agradecemos pela colaboração e pela sua atenção a este processo.\n\n"
                             + "Atenciosamente,\n"
                             + "[SIGT - Sistemas Integrados TI]";
            string titulo = "SIGT - Processo, Finalizado com Sucesso";
            MensagemClasse customMessageBox = new MensagemClasse(mensagem, titulo, suaLogo, seuIcone);
            customMessageBox.ShowDialog();
        }
        public void MensagemEnvioEmail(string lancadas, string nlancadas)
        {
            int valor;
            valor = Convert.ToInt32(lancadas) - Convert.ToInt32(nlancadas);

            Icon seuIcone = Icon.FromHandle(iconeLogo.GetHicon());
            Bitmap marcaLogo = Properties.Resources.document;
            Image suaLogo = marcaLogo;
            string mensagem = "Prezado Usuário,\n\n"
                             + "Concluímos o processo de envio dos email's.\n"
                             + "Durante a verificação, identificamos as seguintes situações:\n\n"
                             + "Notas fiscais devidamente lançadas no SAP S/4 Hana: " + valor + "\n"
                             + "Notas fiscais não lançadas no SAP S/4 Hana: " + nlancadas + "\n\n"
                             + "Solicitamos que tome as devidas providências com os departamentos responsáveis para garantir a integração adequada das notas fiscais não lançadas.\n\n"
                             + "Caso necessite de assistência, esclarecimentos adicionais ou precise reportar qualquer problema, nossa equipe está à disposição para prestar suporte.\n\n"
                             + "Agradecemos pela colaboração e pela sua atenção a este processo.\n\n"
                             + "Atenciosamente,\n"
                             + "[SIGT - Sistemas Integrados TI]";
            string titulo = "SIGT - Processo, Finalizado com Sucesso";
            MensagemClasse customMessageBox = new MensagemClasse(mensagem, titulo, suaLogo, seuIcone);
            customMessageBox.ShowDialog();
        }
        public void MostrarMensagemPersonalizadaErro(string form)
        {
            Icon seuIcone = Icon.FromHandle(iconeLogo.GetHicon());
            Image suaLogo = marcaLogo;
            string mensagem = "Prezado Usuário,\n\n"
                             + "Detectamos que já existe uma nota fiscal semelhante registrada em nosso sistema, impossibilitando a inserção de " + form + ".\n"
                             + "Por favor, verifique os dados fornecidos e tente novamente. Caso necessite de assistência ou tenha alguma dúvida, nossa equipe está pronta para oferecer suporte.\n\n"
                             + "Agradecemos pela compreensão e colaboração.\n\n"
                             + "Atenciosamente,\n"
                             + "[SIGT - Sistemas Integrados TI]";
            string titulo = "SIGT - Erro na Inserção do Equipamento";
            MensagemClasse customMessageBox = new MensagemClasse(mensagem, titulo, suaLogo, seuIcone);
            customMessageBox.ShowDialog();
        }
    }
}

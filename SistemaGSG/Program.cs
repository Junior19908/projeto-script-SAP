﻿using SistemaGSG.Almoxarifado;
using SistemaGSG.Log;
using System;
using System.Windows.Forms;

namespace SistemaGSG
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log.WriteLog("Info : Programa iniciado!");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmProtocolo frm_Main = new frmProtocolo();
            frm_Main.ShowDialog();      
            //frm_Main frm_Main = new frm_Main();
            //frm_Main.ShowDialog();


           //Splash fmr = new Splash();
           //fmr.ShowDialog();
           //frmLogin fml = new frmLogin();
           //fml.ShowDialog();
           //if (fml.FMP == true)
           //{
           //  Application.Run(new frm_Main());
           //}
        }
    }
}

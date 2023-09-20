using SAPFEWSELib;
using SapROTWr;
using System;
using System.Linq;

namespace SIGT.Configurações
{
    public class SAPClasse
    {
        public class SAP
        {
            private GuiApplication GuiApp;
            private GuiSession Session;
            public SAP()
            {
                InicialiseSapGui();
            }
            private void InicialiseSapGui()
            {
                try
                {
                    CSapROTWrapper sapROTWrapper = new CSapROTWrapper();
                    object SapGuilRot = sapROTWrapper.GetROTEntry("SAPGUI");
                    object engine = SapGuilRot.GetType().InvokeMember("GetScriptingEngine", System.Reflection.BindingFlags.InvokeMethod, null, SapGuilRot, null);
                    GuiApp = (GuiApplication)engine;
                    GuiConnection connection = (GuiConnection)GuiApp.Connections.ElementAt(0);
                    Session = (GuiSession)connection.Children.ElementAt(0);
                }
                catch (Exception ex)
                {
                    throw new Exception("Falha na inicialização do SAP GUI: " + ex.Message);
                }
            }
            public void MaximizeMainWindow()
            {
                try
                {
                    GuiFrameWindow guiWindow = Session.ActiveWindow;
                    guiWindow.Maximize();
                }
                catch (Exception ex)
                {
                    throw new Exception("Falha na maximização da janela no SAP GUI: " + ex.Message);
                }
            }
            public void SendSapCommand(string command)
            {
                try
                {
                    Session.SendCommand(command);
                }
                catch (Exception ex)
                {
                    throw new Exception("Falha ao enviar transação no SAP GUI: " + ex.Message);
                }
            }
        }
    }
}

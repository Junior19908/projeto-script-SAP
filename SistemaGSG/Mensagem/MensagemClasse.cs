using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIGT.Mensagem
{
    public partial class MensagemClasse : MetroFramework.Forms.MetroForm
    {
        private MetroFramework.Components.MetroStyleManager metroStyleManagerMensagemClasse;
        private System.ComponentModel.IContainer components;

        public MensagemClasse(string message, string title, Image logo, Icon icon)
        {
            InitializeComponent();
            //TemaGeralPrograma.SetPurpleStyle(metroStyleManagerMensagemClasse);

            // Configurar a janela
            //Text = title;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            Icon = icon;
            ControlBox = false;
            // Configurar a logo
            PictureBox logoPictureBox = new PictureBox();
            logoPictureBox.Image = logo;
            logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            logoPictureBox.Location = new Point(12, 12);
            logoPictureBox.Size = new Size(100, 100);

            // Configurar o texto
            Label messageLabel = new Label();
            messageLabel.Text = message;
            messageLabel.AutoSize = true;
            messageLabel.Location = new Point(120, 12);

            // Configurar o botão OK
            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new Point(25, Math.Max(logoPictureBox.Bottom, messageLabel.Bottom) + 10);
            okButton.Size = new Size(75, 20);

            // Adicionar controles ao formulário
            Controls.Add(logoPictureBox);
            Controls.Add(okButton);
            Controls.Add(messageLabel);

            // Definir tamanho do formulário
            int maxWidth = Math.Max(logoPictureBox.Right, messageLabel.Right) + 20;
            int totalHeight = Math.Max(logoPictureBox.Height, messageLabel.Height) + 50;
            Size = new Size(maxWidth, totalHeight);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.metroStyleManagerMensagemClasse = new MetroFramework.Components.MetroStyleManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManagerMensagemClasse)).BeginInit();
            this.SuspendLayout();
            // 
            // metroStyleManagerMensagemClasse
            // 
            this.metroStyleManagerMensagemClasse.Owner = this;
            // 
            // MensagemClasse
            // 
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Name = "MensagemClasse";
            this.Style = MetroFramework.MetroColorStyle.Green;
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManagerMensagemClasse)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

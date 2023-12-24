using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dot.Game
{
    public partial class Form1 : Form
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Colored Points";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            txtUserName = new TextBox(); 
            txtUserName.Location = new System.Drawing.Point(10, 10);
            txtUserName.Size = new System.Drawing.Size(150, 20);
            this.Controls.Add(txtUserName);
            
            Button btnConnect = new Button();
            btnConnect.Text = "Connect";
            btnConnect.Location = new System.Drawing.Point(170, 10);
            btnConnect.Click += new EventHandler(this.btnConnect_Click);
            this.Controls.Add(btnConnect);
        }
        public Form1()
        {
            InitializeComponent();
        }
    }
}
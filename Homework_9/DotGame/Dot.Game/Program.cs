using System;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Dot.Game
{
    static class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GUI_GT
{
    static class Program
    {

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //for (int i = 0; i < args.Length; i++)
            //{
            //    string s = args[i];
            //    MessageBox.Show(s);
            //}
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0)
            {
                Application.Run(new FormPrincipal(args[0]));
            }
            else
            {
                Application.Run(new FormPrincipal());
            }
            
        }
    }
}

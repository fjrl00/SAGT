using System;
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

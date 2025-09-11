/* 
 * Proyecto: SOFTWARE PARA LA APLICACIÓN DE LA TEORÍA DE LA GENERALIZABILIDAD
 * Nº de orden: 4778
 * 
 * Alumno:   Francisco Jesús Ramos Pérez
 * 
 * Directores de Proyecto:
 *          Dr. Don José Luis Pastrana Brincones
 *          Dr. Don Antonio Hernández Mendo                        
 * 
 * Descripción:
 *      Ventana de inicio con el titulo y la versión.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI_GT
{
    public partial class FormSplashScreen : Form
    {
        public FormSplashScreen()
        {
            InitializeComponent();
        }

        public FormSplashScreen(string version)
            :this()
        {
            int pos = version.LastIndexOf('.');
            string textVersion = version.Remove(pos);
            pos = textVersion.LastIndexOf('.');
            textVersion = textVersion.Remove(pos);
            textVersion = textVersion.Replace("build", "");

            char[] charDelimiters = { ' ' };
            string[] arrayWords = textVersion.Split(charDelimiters, StringSplitOptions.RemoveEmptyEntries);
            this.lbVersion.Text = this.lbVersion.Text + "1.0." + arrayWords[1];
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}

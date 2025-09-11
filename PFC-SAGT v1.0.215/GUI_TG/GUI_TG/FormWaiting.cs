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
 * Fecha de revisión: 29/Abr/2010
 * 
 * Descripción:
 *  Muestra una ventana con un mesage mientras el usuario espera y el programa principal completa
 *  una operación.
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
    public partial class FormWaiting : Form
    {
        /************************************************************************************************
         * CONSTRUCTORES
         ************************************************************************************************/

        /* Descripción: 
         *  Constructor por defecto de la clase.
         */
        public FormWaiting()
        {
            InitializeComponent();
        }

        /* Descripción: 
         *  Constructor de la clase.
         * Parámetros:
         *      string msg: El mensage que se mostrará por pantalla mientras se espera.
         */
        public FormWaiting(string msg)
        {
            InitializeComponent();
            this.lbMessage.Text = msg;
        }

    }// public partial class FormWaiting : Form
}// namespace GUI_TG

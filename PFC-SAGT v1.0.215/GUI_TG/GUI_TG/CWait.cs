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
 *  Muestra una ventana (FormWaiting) con un mesage mientras el usuario espera y el programa principal completa
 *  una operación.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUI_GT
{
    class CWait
    {
        private FormWaiting fw;

        public CWait()
        {
            fw = new FormWaiting();
        }

        public CWait(string msg)
        {
            fw = new FormWaiting(msg);
        }

        public void CWaitShowDialog()
        {
            fw.ShowDialog();
        }

        public void Show()
        {
            fw.Show();
        }

        public void Close()
        {
            fw.Close();
        }
    }
}

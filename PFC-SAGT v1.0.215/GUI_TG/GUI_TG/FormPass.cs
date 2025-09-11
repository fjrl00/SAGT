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
 * Fecha de revisión: 13/Oct/2011                           
 * 
 * Descripción:
 *  Introducción de la clave para la conexión por parte del usuario.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using ConfigCFG;

namespace GUI_GT
{
    public partial class FormPass : Form
    {
        /*=========================================================================================
         *  Constantes
         *=========================================================================================*/
        const string STRING_TEXT = "formPass.txt";
        const string LANG_PATH = "\\lang\\";

        /*=========================================================================================
         *  Variables
         *=========================================================================================*/
        PassUsers dtUser;
        private string file_user_pass; // Path donde se guarda el archivo con las claves


        /*=========================================================================================
         *  Constructores
         *=========================================================================================*/
        public FormPass()
        {
            InitializeComponent();
        }

        public FormPass(TransLibrary.Language lang, string file_user_pass)
            :this()
        {
            traslationElements(lang, Application.StartupPath + LANG_PATH + STRING_TEXT);
            this.file_user_pass = file_user_pass;
        }



        /*=========================================================================================
         *  Métodos de consulta
         *=========================================================================================*/

        /* Descripción:
         *  Devuelve el nombre de usuario
         */
        public string UserId()
        {
            return tbUserId.Text;
        }


        /* Descripción:
         *  Devuelve la contraseña del usuario.
         */
        public string UserPass()
        {
            return m_tbPassword.Text;
        }


        /* Descripción:
         *  Devuelve el valor true si se ha seleccionado guardar contraseña false en otro caso.
         */
        public bool SavePass()
        {
            return this.checkBoxSavePassword.Checked;
        }


        #region Traducción de la ventana
        /*======================================================================================
         * Traducción de la ventana
         *======================================================================================*/

        /*
         * Descripción:
         *  Traduce todos los textos de la ventana al idioma que se encuentre activo
         */
        private void traslationElements(TransLibrary.Language lang, string pathFileTrans)
        {
            TransLibrary.ReadFileTrans dic = new TransLibrary.ReadFileTrans(pathFileTrans);
            string name = "";
            try
            {
                // Traducimos los Textos de la ventana
                //====================================

                // Traducimos el título de la ventana
                name = this.Name.ToString();
                this.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el botón aceptar
                name = this.btOk.Name.ToString();
                this.btOk.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el botón cancelar
                name = this.btCancel.Name.ToString();
                this.btCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos el checkBox
                name = this.checkBoxSavePassword.Name.ToString();
                this.checkBoxSavePassword.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // Traducimos etiquetas
                name = this.lbInfo.Name.ToString();
                this.lbInfo.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbUserId.Name.ToString();
                this.lbUserId.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbPassword.Name.ToString();
                this.lbPassword.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " " + "Error al traducir:" + " " + name);
            }
        }
        #endregion Traducción de la ventana


        /* Descripción:
         *  Realiza la conexión con la plataforma Web MenPas
         */
        private void btOk_Click(object sender, EventArgs e)
        {
            // WS_MenPas.WS_Modrian prueba = new WS_MenPas.WS_Modrian();
            // bool usuario = prueba.EstaRegistrado(this.tbUserId.Text.ToLower(), this.m_tbPassword.Text.ToLower()); 

            // Variable para pruebas 
            //bool usuario = EstaRegistrado(this.tbUserId.Text.ToLower(), this.m_tbPassword.Text.ToLower());

            //if (usuario)
            //{
            //    MessageBox.Show("Esta registrado");
            //}
            //else
            //{
            //    MessageBox.Show("No esta registrado");
            //}
        }

        /**************************************************************************************************
         * ================================================================================================
         * MÉTODO PARA PRUEBAS
         * ================================================================================================
         **************************************************************************************************/
        //private bool EstaRegistrado(string user, string pass)
        //{
        //    string[] arrayNames = { "Francis", "María", "Fernado" };
        //    string[] arrayPass = { "windows7", "macos", "ubuntu" };

        //    int l = arrayNames.Length;
        //    bool found = false;
        //    for (int i = 0; i < l && !found; i++)
        //    {
        //        found = arrayNames[i].Equals(user) && arrayPass[i].Equals(pass);
        //    }
        //    return found;
        //}

         
        /* Descripción:
         *  Ejecuta el explorador de internet asignado por defecto con la página de recuperación 
         *  de contraseña indicada en la plataforma MenPas
         */
        private void linkLbForgottenPass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.menpas.com/Usuarios/RecuperarUsuarioClave.aspx");
        }


        private void FormPass_Load(object sender, EventArgs e)
        {
            AuxAutoComplete();
        }


        public void AuxAutoComplete()
        {
            //
            // cargo la lista de items para el autocomplete
            //
            this.tbUserId.AutoCompleteCustomSource = LoadAutoComplete();
            this.tbUserId.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.tbUserId.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }


        public AutoCompleteStringCollection LoadAutoComplete()
        {

            if (File.Exists(file_user_pass))
            {
                dtUser = PassUsers.LoadDataTable(file_user_pass);
            }
            else
            {
                dtUser = new PassUsers();
            }

            DataTable dt = dtUser.DataTable();

            AutoCompleteStringCollection stringCol = new AutoCompleteStringCollection();

            foreach (DataRow row in dt.Rows)
            {
                stringCol.Add(Convert.ToString(row[PassUsers.COLUMN_USER_NAME]));
            }

            return stringCol;
        }


        private void tbUserId_TextChanged(object sender, EventArgs e)
        {
            this.m_tbPassword.Text = dtUser.ReturnPass(this.tbUserId.Text);
        }

        

    }// end public partial class FormPass : Form
}// end namespace GUI_TG

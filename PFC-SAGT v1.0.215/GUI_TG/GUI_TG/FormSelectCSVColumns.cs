using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GUI_GT
{
    public partial class FormSelectCSVColumns : Form
    {
        // nombre del archivo que contiene las traducciones
        const string STRING_TEXT = "formSelectCSVColumns.txt";
        const string LANG_PATH = "\\lang\\";

        // The caller passes its own List<string> for facets — we populate it in-place on OK.
        // For the dependent variable, callers read the SelectedDependent property after ShowDialog().
        private readonly List<string> _facets;
        public string SelectedDependent { get; private set; } = string.Empty;

        /*
         * Descripción:
         *  Inicializa el form.
         *  
         *      allColumns: All columns loaded from the imported table.
         *      facets: Caller-supplied list that will be filled with selected facets on OK.
         */
        public FormSelectCSVColumns(List<string> allColumns, List<string> facets, TransLibrary.Language lang)
        {
            InitializeComponent();
            this.traslationElements(lang, Application.StartupPath + LANG_PATH + STRING_TEXT);

            _facets = facets;

            // Populate the excluded list with every column initially.
            foreach (string col in allColumns)
                listBoxExcludedColumns.Items.Add(col);

            // Wire up the handlers that the designer left empty.
            listBoxFacets.SelectedIndexChanged += listBoxFacets_SelectedIndexChanged;
            btExcludedToDependent.Click += btExcludedToDependent_Click;
            btDependentToExcluded.Click += btDependentToExcluded_Click;

            // Nothing is selected yet — all transfer buttons start disabled.
            RefreshButtonStates();

            // OK must be disabled until the user picks a dependent variable.
            btOK.Enabled = false;
        }

        // -------------------------------------------------------------------------
        // Selection-changed handlers — drive button enable/disable state.
        // -------------------------------------------------------------------------

        private void listBoxSourceDiff_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshButtonStates();
        }

        private void listBoxFacets_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshButtonStates();
        }

        // -------------------------------------------------------------------------
        // Transfer button handlers.
        // -------------------------------------------------------------------------

        // Excluded → Facets
        private void btMoveRight_Click(object sender, EventArgs e)
        {
            if (listBoxExcludedColumns.SelectedItem is string selected)
            {
                listBoxExcludedColumns.Items.Remove(selected);
                listBoxFacets.Items.Add(selected);

                if (listBoxExcludedColumns.Items.Count > 0)
                {
                    // mientras haya elementos en el listBox selecionaremos el primero
                    this.listBoxExcludedColumns.SetSelected(0, true);
                }
                else
                {
                    // si no hay elementos en el listBox seleccionaremos los del otro listBox
                    this.listBoxFacets.SetSelected(0, true);
                }
            }

            RefreshButtonStates();
        }

        // Facets → Excluded
        private void btMoveLeft_Click(object sender, EventArgs e)
        {
            if (listBoxFacets.SelectedItem is string selected)
            {
                listBoxFacets.Items.Remove(selected);
                listBoxExcludedColumns.Items.Add(selected);

                if (listBoxFacets.Items.Count > 0)
                {
                    // mientras haya elementos en el listBox selecionaremos el primero
                    this.listBoxFacets.SetSelected(0, true);
                }
                else
                {
                    // si no hay elementos en el listBox seleccionaremos los del otro listBox
                    this.listBoxExcludedColumns.SetSelected(0, true);
                }
            }

            RefreshButtonStates();
        }

        // Excluded → Dependent
        private void btExcludedToDependent_Click(object sender, EventArgs e)
        {
            if (listBoxExcludedColumns.SelectedItem is string selected)
            {
                // If a dependent variable is already set, return it to the excluded list first.
                if (!string.IsNullOrEmpty(tbDependent.Text))
                    listBoxExcludedColumns.Items.Add(tbDependent.Text);

                listBoxExcludedColumns.Items.Remove(selected);
                tbDependent.Text = selected;

                if (listBoxExcludedColumns.Items.Count > 0)
                {
                    // mientras haya elementos en el listBox selecionaremos el primero
                    this.listBoxExcludedColumns.SetSelected(0, true);
                }
                
                listBoxExcludedColumns.Focus();

                btOK.Enabled = true;
            }

            RefreshButtonStates();
        }

        // Dependent → Excluded
        private void btDependentToExcluded_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbDependent.Text))
            {
                listBoxExcludedColumns.Items.Add(tbDependent.Text);
                tbDependent.Text = string.Empty;

                this.listBoxExcludedColumns.SetSelected(0, true);
                
                btOK.Enabled = false;
            }

            RefreshButtonStates();
        }

        // -------------------------------------------------------------------------
        // OK / Cancel.
        // -------------------------------------------------------------------------

        private void btOK_Click(object sender, EventArgs e)
        {
            // Write results back to the caller's references.
            SelectedDependent = tbDependent.Text;

            _facets.Clear();
            foreach (object item in listBoxFacets.Items)
                _facets.Add(item.ToString());
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            // DialogResult is already set to Cancel in the designer — nothing else needed.
        }

        // -------------------------------------------------------------------------
        // Helpers.
        // -------------------------------------------------------------------------

        private void RefreshButtonStates()
        {
            bool excludedSelected = listBoxExcludedColumns.SelectedIndex != -1;
            bool facetSelected = listBoxFacets.SelectedIndex != -1;
            bool hasDependant = !string.IsNullOrEmpty(tbDependent.Text);

            btExcludedToFacets.Enabled = excludedSelected;
            btExcludedToDependent.Enabled = excludedSelected;
            btFacetsToExcluded.Enabled = facetSelected;
            btDependentToExcluded.Enabled = hasDependant;
        }

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
                // Traducimos el boton aceptar
                name = this.btOK.Name.ToString();
                this.btOK.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                // Traducimos el boton cancelar
                name = this.btCancel.Name.ToString();
                this.btCancel.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();

                // traducimos las etiquetas
                name = this.labelExcludedColumns.Name.ToString();
                this.labelExcludedColumns.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.labelFacets.Name.ToString();
                this.labelFacets.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
                name = this.lbDependent.Name.ToString();
                this.lbDependent.Text = dic.labelTraslation(name).GetTranslation(lang).ToString();
            }
            catch (TransLibrary.LabelTranslationException lEx)
            {
                MessageBox.Show(lEx.Message + " Se produjo un error al traducir " + name);
            }
        }
    }
}

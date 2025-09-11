namespace GUI_GT
{
    partial class FormShowCharts2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormShowCharts2));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageChart = new System.Windows.Forms.TabPage();
            this.btSaveChartImage = new System.Windows.Forms.Button();
            this.chartCoef_G = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPageResum = new System.Windows.Forms.TabPage();
            this.dgvExOptDatas = new DataGridViewEx.DataGridViewEx();
            this.cBoxSelectFacet = new System.Windows.Forms.ComboBox();
            this.lbSelectFacet = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPageChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartCoef_G)).BeginInit();
            this.tabPageResum.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExOptDatas)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageChart);
            this.tabControl1.Controls.Add(this.tabPageResum);
            this.tabControl1.Location = new System.Drawing.Point(3, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(690, 437);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageChart
            // 
            this.tabPageChart.BackgroundImage = global::GUI_GT.Properties.Resources.Fondo;
            this.tabPageChart.Controls.Add(this.btSaveChartImage);
            this.tabPageChart.Controls.Add(this.chartCoef_G);
            this.tabPageChart.Location = new System.Drawing.Point(4, 22);
            this.tabPageChart.Name = "tabPageChart";
            this.tabPageChart.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageChart.Size = new System.Drawing.Size(682, 411);
            this.tabPageChart.TabIndex = 0;
            this.tabPageChart.Text = "Gráfico";
            this.tabPageChart.UseVisualStyleBackColor = true;
            // 
            // btSaveChartImage
            // 
            this.btSaveChartImage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btSaveChartImage.Image = global::GUI_GT.Properties.Resources.document_and_charts_22x22;
            this.btSaveChartImage.Location = new System.Drawing.Point(279, 374);
            this.btSaveChartImage.Name = "btSaveChartImage";
            this.btSaveChartImage.Size = new System.Drawing.Size(125, 32);
            this.btSaveChartImage.TabIndex = 1;
            this.btSaveChartImage.Text = "Guardar imagen";
            this.btSaveChartImage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btSaveChartImage.UseVisualStyleBackColor = true;
            this.btSaveChartImage.Click += new System.EventHandler(this.btSaveChartImage_Click);
            // 
            // chartCoef_G
            // 
            this.chartCoef_G.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chartCoef_G.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartCoef_G.Legends.Add(legend1);
            this.chartCoef_G.Location = new System.Drawing.Point(7, 7);
            this.chartCoef_G.Name = "chartCoef_G";
            this.chartCoef_G.Size = new System.Drawing.Size(655, 360);
            this.chartCoef_G.TabIndex = 0;
            this.chartCoef_G.Text = "chart1";
            // 
            // tabPageResum
            // 
            this.tabPageResum.BackgroundImage = global::GUI_GT.Properties.Resources.Fondo;
            this.tabPageResum.Controls.Add(this.lbSelectFacet);
            this.tabPageResum.Controls.Add(this.cBoxSelectFacet);
            this.tabPageResum.Controls.Add(this.dgvExOptDatas);
            this.tabPageResum.Location = new System.Drawing.Point(4, 22);
            this.tabPageResum.Name = "tabPageResum";
            this.tabPageResum.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageResum.Size = new System.Drawing.Size(682, 411);
            this.tabPageResum.TabIndex = 1;
            this.tabPageResum.Text = "Datos";
            this.tabPageResum.UseVisualStyleBackColor = true;
            // 
            // dgvExOptDatas
            // 
            this.dgvExOptDatas.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgvExOptDatas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvExOptDatas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvExOptDatas.BackgroundColor = System.Drawing.Color.White;
            this.dgvExOptDatas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvExOptDatas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.MenuText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvExOptDatas.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvExOptDatas.Location = new System.Drawing.Point(7, 18);
            this.dgvExOptDatas.MinNumeracionFilas = 0;
            this.dgvExOptDatas.Name = "dgvExOptDatas";
            this.dgvExOptDatas.NumeracionFilas = true;
            this.dgvExOptDatas.NumeroColumnas = 0;
            this.dgvExOptDatas.NumeroFilas = 0;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomRight;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvExOptDatas.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvExOptDatas.RowHeadersVisible = false;
            this.dgvExOptDatas.RowHeadersWidth = 50;
            this.dgvExOptDatas.Size = new System.Drawing.Size(667, 325);
            this.dgvExOptDatas.TabIndex = 0;
            // 
            // cBoxSelectFacet
            // 
            this.cBoxSelectFacet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxSelectFacet.FormattingEnabled = true;
            this.cBoxSelectFacet.Location = new System.Drawing.Point(240, 357);
            this.cBoxSelectFacet.Name = "cBoxSelectFacet";
            this.cBoxSelectFacet.Size = new System.Drawing.Size(271, 21);
            this.cBoxSelectFacet.TabIndex = 1;
            this.cBoxSelectFacet.SelectedIndexChanged += new System.EventHandler(this.cBoxSelectFacet_SelectedIndexChanged);
            // 
            // lbSelectFacet
            // 
            this.lbSelectFacet.AutoSize = true;
            this.lbSelectFacet.Location = new System.Drawing.Point(172, 361);
            this.lbSelectFacet.Name = "lbSelectFacet";
            this.lbSelectFacet.Size = new System.Drawing.Size(40, 13);
            this.lbSelectFacet.TabIndex = 2;
            this.lbSelectFacet.Text = "Faceta";
            // 
            // FormShowCharts2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 437);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "FormShowCharts2";
            this.Text = "Gráfico tipo 2";
            this.tabControl1.ResumeLayout(false);
            this.tabPageChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartCoef_G)).EndInit();
            this.tabPageResum.ResumeLayout(false);
            this.tabPageResum.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExOptDatas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCoef_G;
        private System.Windows.Forms.TabPage tabPageResum;
        private DataGridViewEx.DataGridViewEx dgvExOptDatas;
        private System.Windows.Forms.Button btSaveChartImage;
        private System.Windows.Forms.Label lbSelectFacet;
        private System.Windows.Forms.ComboBox cBoxSelectFacet;
    }
}
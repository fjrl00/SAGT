namespace GUI_GT
{
    partial class FormShowCharts
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
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormShowCharts));
            this.chartG_Parameters = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btSaveChartImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chartG_Parameters)).BeginInit();
            this.SuspendLayout();
            // 
            // chartG_Parameters
            // 
            this.chartG_Parameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AlignWithChartArea = "ChartAreaG_Parameters";
            chartArea1.Name = "ChartAreaG_Parameters";
            this.chartG_Parameters.ChartAreas.Add(chartArea1);
            legend1.DockedToChartArea = "ChartAreaG_Parameters";
            legend1.IsDockedInsideChartArea = false;
            legend1.Name = "Legend_coefG_Rel";
            this.chartG_Parameters.Legends.Add(legend1);
            this.chartG_Parameters.Location = new System.Drawing.Point(16, 13);
            this.chartG_Parameters.Name = "chartG_Parameters";
            this.chartG_Parameters.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            series1.ChartArea = "ChartAreaG_Parameters";
            series1.Color = System.Drawing.Color.Red;
            series1.EmptyPointStyle.Label = "h";
            series1.EmptyPointStyle.LegendText = "n";
            series1.Legend = "Legend_coefG_Rel";
            series1.LegendText = "Coeficiente G Rel.";
            series1.MarkerColor = System.Drawing.Color.Red;
            series1.Name = "Series_coefG_Rel";
            series2.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.TopRight;
            series2.ChartArea = "ChartAreaG_Parameters";
            series2.Legend = "Legend_coefG_Rel";
            series2.LegendText = "Coeficiente G Abs.";
            series2.Name = "Series_coefG_Abs";
            series3.ChartArea = "ChartAreaG_Parameters";
            series3.Legend = "Legend_coefG_Rel";
            series3.LegendText = "Varianza del Error Realtivo";
            series3.Name = "SeriesTotalRelErrorVar";
            series4.ChartArea = "ChartAreaG_Parameters";
            series4.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            series4.Legend = "Legend_coefG_Rel";
            series4.LegendText = "Varianza del Error Absoluto";
            series4.Name = "SeriesTotalAbsErrorVar";
            series5.ChartArea = "ChartAreaG_Parameters";
            series5.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            series5.Legend = "Legend_coefG_Rel";
            series5.LegendText = "Desv. Típica Relativa";
            series5.Name = "SeriesErrorRelStandDev";
            series6.ChartArea = "ChartAreaG_Parameters";
            series6.Color = System.Drawing.Color.Silver;
            series6.Legend = "Legend_coefG_Rel";
            series6.LegendText = "Desv. Típica Absoluta";
            series6.Name = "SeriesErrorAbsStandDev";
            this.chartG_Parameters.Series.Add(series1);
            this.chartG_Parameters.Series.Add(series2);
            this.chartG_Parameters.Series.Add(series3);
            this.chartG_Parameters.Series.Add(series4);
            this.chartG_Parameters.Series.Add(series5);
            this.chartG_Parameters.Series.Add(series6);
            this.chartG_Parameters.Size = new System.Drawing.Size(593, 390);
            this.chartG_Parameters.TabIndex = 0;
            this.chartG_Parameters.Text = "Resumen de los G Parametros";
            // 
            // btSaveChartImage
            // 
            this.btSaveChartImage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btSaveChartImage.Image = global::GUI_GT.Properties.Resources.document_and_charts_22x22;
            this.btSaveChartImage.Location = new System.Drawing.Point(250, 409);
            this.btSaveChartImage.Name = "btSaveChartImage";
            this.btSaveChartImage.Size = new System.Drawing.Size(125, 32);
            this.btSaveChartImage.TabIndex = 1;
            this.btSaveChartImage.Text = "Guardar imagen";
            this.btSaveChartImage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btSaveChartImage.UseVisualStyleBackColor = true;
            this.btSaveChartImage.Click += new System.EventHandler(this.btSaveChartImage_Click);
            // 
            // FormShowCharts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 444);
            this.Controls.Add(this.btSaveChartImage);
            this.Controls.Add(this.chartG_Parameters);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "FormShowCharts";
            this.Text = "Gráficas";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormShowCharstClosed);
            ((System.ComponentModel.ISupportInitialize)(this.chartG_Parameters)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartG_Parameters;
        private System.Windows.Forms.Button btSaveChartImage;

    }
}
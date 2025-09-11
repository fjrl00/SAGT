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
 * Fecha de revisión: 02/Jul/2012 
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using ProjectMeans;
using TransLibrary;
using System.IO;

namespace ConfigCFG
{
    public class ConfigCFG
    {
        /*-------------------------------------------------------------------------------------
         * Constantes
         *-------------------------------------------------------------------------------------*/
        // CONSTANTES (separador decimal)
        public const string DECIMAL_SEPARATOR_PERIOD = ".";
        public const string DECIMAL_SEPARATOR_COMMA = ",";

        // CONSTANTES (Valores por defecto)
        const string NAME_FILE_CONFIG = "config.cfg";
        const string STRING_LANGUAGE = "Language";
        const string STRING_NUM_OF_DECIMALS = "Decimals";
        const string STRING_DECIMAL_SEPARATOR = "Separator";
        const int DEFAULT_DECIMAL = 3; // Número de decimales por defecto
        const string STRING_NULL_TO_ZERO = "Null_to_zero";  
        const string DEFAULT_DECIMAL_SEPARATOR = DECIMAL_SEPARATOR_PERIOD;
        const string WORKSPACE_FOLDER = "Workspace_folder";

        const string TITLE_REPORTS = "[REPORTS]";
        const string STRING_TYPE_OF_TABLE_MEANS = "Type_of_Table_Means";
        const string STRING_SHADING_ROWS = "Shading_Rows";
        const string STRING_TABLE_FONT_SIZE = "Table_Font_Size";
        const string STRING_TABLE_FONT_FAMILY = "Table_Font_family";
        const string STRING_TEXT_FONT_SIZE = "Text_Font_Size";
        const string STRING_TEXT_FONT_FAMILY = "Text_Font_family";
        // Constantes refentes a la clase TypeOfTableMeans
        const string TABLE_MEANS_DEFAULT = "Default";
        const string TABLE_MEANS_DIF = "TableMeansDif";
        const string TABLE_MEANS_TYPE_POINT = "TableMeansTipPoint";

        const string TITLE_CHARTS = "[CHARTS]";
        // Coeficiente G relativo
        const string STRING_CHECK_COEFG_REL = "Check_coefG_Rel";
        const string STRING_COLOR_COEFG_REL = "Color_coefG_Rel";
        // Coefiente G Absoluto
        const string STRING_CHECK_COEFG_ABS = "Check_coefG_Abs";
        const string STRING_COLOR_COEFG_ABS = "Color_coefG_Abs";
        // Varianza de error relativo
        const string STRING_CHECK_TOTAL_REL_ERROR_VAR = "Check_Total_Rel_Error_Var";
        const string STRING_COLOR_TOTAL_REL_ERROR_VAR = "Color_Total_Rel_Error_Var";
        // Varianza de error absoluto
        const string STRING_CHECK_TOTAL_ABS_ERROR_VAR = "Check_Total_Abs_Error_Var";
        const string STRING_COLOR_TOTAL_ABS_ERROR_VAR = "Color_Total_Abs_Error_Var";
        // Error standar relativo
        const string STRING_CHECK_ERROR_REL_STAND_DEV = "Check_Error_Rel_Stand_Dev";
        const string STRING_COLOR_ERROR_REL_STAND_DEV = "Color_Error_Rel_Stand_Dev";
        // Error standar absoluto
        const string STRING_CHECK_ERROR_ABS_STAND_DEV = "Check_Error_Abs_Stand_Dev";
        const string STRING_COLOR_ERROR_ABS_STAND_DEV = "Color_Error_Abs_Stand_Dev";
        // Tipo de manera en la que se representarán las graficas de Coef G (lineal o splin)
        const string STRING_SERIES_CHART_TYPE = "SeriesChartType";
        // Posición donde se mostrará el punto
        const string STRING_POINT_LABEL = "PointLabel";
        // Simbolo con el que se representará el punto
        const string STRING_MARKER_STYLE = "MarkerStyle";


        /*-------------------------------------------------------------------------------------
         * Variables
         *-------------------------------------------------------------------------------------*/

        // Variables de clase con los valores por defecto.
        private TransLibrary.Language language; // idioma de la aplicación
        private int numOfDecimals; // Número de decimales con los que serán representados los datos
        private string decimalSeparator; // Separador decimal
        private bool null_to_zero; // True si los valores nulos se interpretan como zero
        private TypeOfTableMeans typeMeans; // tipo de tabla de medias
        private bool shadingRows; // si esta a true sobrea alternativamenta las filas de las tablas de los informes
        private int tableFontSize; // Tamaño de la fuente de la tabla
        private string tableFontFamily; // Nombre de la fuente de la tabla
        private int textFontSize; // Tamaño de la fuente del texto
        private string textFontFamily; // Nombre de la fuente del texto
        private string path_workspace; // Directorio de trabajo

        //
        // Coeficiente G relativo
        private bool check_coefG_Rel; // indica si se mostrará o no dicho valor en la gráfica
        private Color color_coefG_Rel; // indica el color que se mostrara del coeficiente relativo
        // Coeficiente G absoluto
        private bool check_coefG_Abs;
        private Color color_coefG_Abs; // indica el color que se mostrara del coeficiente absoluto
        // Varianza error relativo
        private bool checkTotalRelErrorVar;
        private Color colorTotalRelErrorVar; // indica el color que se mostrara el color varianza del error relativo
        // Varianza de error absoltuo
        private bool checkTotalAbsErrorVar;
        private Color colorTotalAbsErrorVar;
        // Error standar relativo
        private bool checkErrorRelStandDev;
        private Color colorErrorRelStandDev;
        // Error standar absoluto
        private bool checkErrorAbsStandDev;
        private Color colorErrorAbsStandDev;

        // Tipo de grafica para representar la serie: Lineal o splin
        private SeriesChartType serieChartType;
        // Posición donde se mostrará el dato del punto.
        private LabelAlignmentStyles labelAlignmentStyles; 
        // Marca de punto
        private MarkerStyle markerStyle;

        /*-------------------------------------------------------------------------------------
         * Constructores
         *-------------------------------------------------------------------------------------*/

        /*
         * Descripción:
         *  Constructor por defecto.
         *      Los valores por defecto son los siguientes:
         *          los valores null se interpretaran como cero (null_to_zero = true)
         *          language = spanish;
         */
        public ConfigCFG()
        {
            this.language = TransLibrary.Language.spanish;
            this.numOfDecimals = DEFAULT_DECIMAL;
            this.decimalSeparator = DEFAULT_DECIMAL_SEPARATOR;
            this.null_to_zero = true;
            this.path_workspace = "";

            
            // Coeficiente G relativo
            this.check_coefG_Rel = true;
            this.color_coefG_Rel = Color.Blue;
            // Coeficiente G absoluto
            this.check_coefG_Abs = true;
            this.color_coefG_Abs = Color.Red;
            // Varianza de error relativo
            this.checkTotalRelErrorVar = true;
            this.colorTotalRelErrorVar = Color.Green;
            // Varianza de error absoluto
            this.checkTotalAbsErrorVar = true;
            this.colorTotalAbsErrorVar = Color.Yellow;
            // Error estandar relativo
            this.checkErrorRelStandDev = true;
            this.colorErrorRelStandDev = Color.Violet;
            // Error estandar absoluto
            this.checkErrorAbsStandDev = true;
            this.colorErrorAbsStandDev = Color.Orange;
            // Estilo en el que se mostrarán los gráficos de Coeficiente G
            this.serieChartType = SeriesChartType.Spline; // por defecto lo representamos por splin
            // posición en la que se mostrarán el valor de los puntos.
            this.labelAlignmentStyles = LabelAlignmentStyles.None;
            // Marca de punto
            this.markerStyle = MarkerStyle.Circle; // por defecto se representa como punto.

            this.typeMeans = TypeOfTableMeans.Default;
            this.shadingRows = true;
            this.tableFontSize = 7; // tamaño por defecto de la fuente
            this.tableFontFamily = "Verdana";
            this.textFontSize = 12; // tamaño por defecto de la fuente
            this.textFontFamily = "Verdana";
            
        }// ConfigCFG


        /*-------------------------------------------------------------------------------------
         * Métodos de consulta
         *-------------------------------------------------------------------------------------*/

        /*
         * Descripción:
         *  Devuelve el nombre del fichero de configuración.
         */
        public string NameFileConfig()
        {
            return NAME_FILE_CONFIG;
        }


        /*
         * Descripción:
         *  Devuelve el idioma de la configuración actual.
         */
        public TransLibrary.Language GetConfigLanguage()
        {
            return language;
        }


        /* Descripción:
         *  Devuelve el número de decimales que usara para representar los datos
         */
        public int GetNumberOfDecimals()
        {
            return this.numOfDecimals;
        }


        /* Decripción:
         *  Devuel el caracter que se usara como separador decimal.
         */
        public string GetDecimalSeparator()
        {
            return this.decimalSeparator;
        }

        
        /* Descripción:
         *  Devuelve el valor de la variable booleana que indica si se interpretarán los valores nulos como
         *  cero.
         */
        public bool GetNull_to_Zero()
        {
            return this.null_to_zero;
        }


        /* Descripción:
         *  Devuelve el path del directorio de trabajo
         */
        public string Get_Path_Workspace()
        {
            return this.path_workspace;
        }


        /* Descripción:
         *  Devuelve el valor almacenado de el tipo de tabla de medias
         */
        public TypeOfTableMeans GetTypeOfTableMeans()
        {
            return this.typeMeans;
        }


        /* Descripción: 
         *  Devuelve el valor boleano asociado a la variable, si es true indica que si deben sombrearse
         *  las celdas alternativamente de las tablas de los informes.
         */
        public bool GetShadingRows()
        {
            return this.shadingRows;
        }


        /* Descripción:
         *  Devuelve el tamaño de la fuentes que se usara para las tablas.
         */
        public int GetTableFontSize()
        {
            return this.tableFontSize;
        }


        /* Descripción:
         *  Devuelve el string de la familia de fuentes asignada en la configuración para
         *  las tablas.
         */
        public string GetTableFontFamily()
        {
            return this.tableFontFamily;
        }

        /* Descripción:
         *  Devuelve el tamaño de la fuentes que se usara para las texto.
         */
        public int GetTextFontSize()
        {
            return this.textFontSize;
        }


        /* Descripción:
         *  Devuelve el string de la familia de fuentes asignada en la configuración para
         *  las texto.
         */
        public string GetTextFontFamily()
        {
            return this.textFontFamily;
        }


        /* 
         * ======================
         * Coeficiente G Relativo
         * ======================
         */

        /* Descripción:
         *  Devuelve el valor bool almacenado en la variable Coeficiente G Relativo.
         *  True si se debe representar el valor.
         */
        public bool GetCheck_coefG_Rel()
        {
            return this.check_coefG_Rel;
        }


        /* Descripción:
         *  Devuelve el valor int almacenado en la variable Coeficiente G Relativo que representa el color.
         */
        public Color GetColor_coefG_Rel()
        {
            return this.color_coefG_Rel;
        }


        /* 
         * ======================
         * Coeficiente G Absoluto
         * ======================
         */

        /* Descripción:
         *  Devuelve el valor bool almacenado en la variable Coeficiente G Absoluto.
         *  True si se debe representar el valor.
         */
        public bool GetCheck_coefG_Abs()
        {
            return this.check_coefG_Abs;
        }


        /* Descripción:
        *  Devuelve el color almacenado en la variable Coeficiente G Absoluto.
        */
        public Color GetColor_coefG_Abs()
        {
            return this.color_coefG_Abs;
        }


        /* 
         * ===========================
         * Varianza del Error Relativo
         * ===========================
         */

        /* Descripción:
         *  Devuelve el valor bool almacenado en la variable Varianza del Error Relativo.
         *  True si se debe representar el valor.
         */
        public bool GetCheckTotalRelErrorVar()
        {
            return this.checkTotalRelErrorVar;
        }


        /* Descripción:
         *  Devuelve el color almacenado en la variable Varianza del Error Relativo.
         */
        public Color GetColorTotalRelErrorVar()
        {
            return this.colorTotalRelErrorVar;
        }


        /* 
         * ===========================
         * Varianza del Error Absoluto
         * ===========================
         */

        /* Descripción:
         *  Devuelve el valor bool almacenado en la variable Varianza del Error Absoluto.
         *  True si se debe representar el valor.
         */
        public bool GetCheckTotalAbsErrorVar()
        {
            return this.checkTotalAbsErrorVar;
        }


        /* Descripción:
         *  Devuelve el color almacenado en la variable Varianza del Error Absoluto.
         */
        public Color GetColorTotalAbsErrorVar()
        {
            return this.colorTotalAbsErrorVar;
        }


        /* 
         * =======================
         * Error Estandar Relativo
         * =======================
         */

        /* Descripción:
         *  Devuelve el valor bool almacenado en la variable Desviación tipica del Error Relativo.
         *  True si se debe representar el valor.
         */
        public bool GetCheckErrorRelStandDev()
        {
            return this.checkErrorRelStandDev;
        }


        /* Descripción:
         *  Devuelve el color almacenado en la variable Desviación tipica del Error Relativo.
         */
        public Color GetColorErrorRelStandDev()
        {
            return this.colorErrorRelStandDev;
        }


        /* 
         * =======================
         * Error Estandar Absoluto
         * =======================
         */

        /* Descripción:
         *  Devuelve el valor bool almacenado en la variable Desviación tipica del Error Absoluto.
         *  True si se debe representar el valor.
         */
        public bool GetCheckErrorAbsStandDev()
        {
            return this.checkErrorAbsStandDev;
        }


        /* Descripción:
         *  Devuelve el color almacenado en la variable Desviación tipica del Error Absoluto.
         */
        public Color GetColorErrorAbsStandDev()
        {
            return this.colorErrorAbsStandDev;
        }


        /*
         * ===================================
         * TIPO DE GRAFICA
         * ===================================
         */

        /* Descripción:
         *  Devuelve el tipo de gráfica con la que se representa la serie.
         */
        public SeriesChartType GetSerieChartType()
        {
            return this.serieChartType;
        }


        /* Descripción:
         *  Devuelve la posición en la que se mostrará el valor del punto representado
         */
        public LabelAlignmentStyles GetLabelAlignmentStyles()
        {
            return this.labelAlignmentStyles;
        }


        /* Descripción:
         *  Devuelve el signo con el que se representará el punto en la gráfica.
         */
        public MarkerStyle GetMarkerStyle()
        {
            return this.markerStyle;
        }


        /*----------------------------------------------------------------------------------------
         * Método de instancia
         *---------------------------------------------------------------------------------------*/

        /*
         * Descripción:
         *  Cabia el valor actual de la configuración por aquel que se pasa como parámetro.
         */
        public void SetConfigLanguage(TransLibrary.Language lang)
        {
            this.language = lang;
        }


        /* Descipción:
         *  Establece el valor de decimales que se usaran para representar los datos
         */
        public void SetNumberOfDecimal(int n)
        {
            if (n < 0 || n > 10)
            {
                throw new ConfigCFGException("Error: el número de decimales ha de estar entre 0 y 10");
            }
            this.numOfDecimals = n;
        }


        /* Descripción:
         *  Asigna el separador decimal que solo puede ser '.' o ',' en otro caso lanzara una excepción.
         */
        public void SetDecimalSeparator(string separator)
        {
            if (!separator.Equals(DECIMAL_SEPARATOR_PERIOD) && !separator.Equals(DECIMAL_SEPARATOR_COMMA))
            {
                throw new ConfigCFGException("Error: No es un separador válido");
            }
            this.decimalSeparator = separator;
        }


        /* Descripción:
         *  Establece el valor decimal de la variable que indica si se tomarán los valores decimales como
         *  nulos.
         */
        public void SetNull_to_Zero(bool null_to_zero)
        {
            this.null_to_zero = null_to_zero;
        }


        /* Descripción:
         *  Devuelve el path del directorio de trabajo
         */
        public void Set_Path_Workspace(string path)
        {
            this.path_workspace = path;
        }


        /* Descripción:
         *  Asigna el valor del tipo de tabla de medias que se generará en la opción de medias
         */
        public void SetTypeTableOfMeans(TypeOfTableMeans typeMeans)
        {
            this.typeMeans = typeMeans;
        }


        /* Descripción:
         *  Asigna el valor booleano como parámetro a la varible que lo almacena
         */
        public void SetShadingRows(bool shading)
        {
            this.shadingRows = shading;
        }


        /* Descripción:
         *  Asigna el tamaño de la fuente de la tabla
         */
        public void SetTableFontSize(int size)
        {
            this.tableFontSize = size;
        }


        /* Descripción:
         *  Asigna el valor del string que se pasa como parametro como familia de fuentes para la tabla
         */
        public void SetTableFontFamily(string f)
        {
            this.tableFontFamily = f;
        }


        /* Descripción:
         *  Asigna el tamaño de la fuente de texto.
         */
        public void SetTextFontSize(int size)
        {
            this.textFontSize = size;
        }


        /* Descripción:
         *  Asigna el valor del string que se pasa como parámetro como familia de fuentes para la texto.
         */
        public void SetTextFontFamily(string f)
        {
            this.textFontFamily = f;
        }


        /* 
         * ======================
         * Coeficiente G Relativo
         * ======================
         */

        /* Descripción:
         *  Asigna el valor pasado como parámetro bool almacenado en la variable Coeficiente G Relativo.
         *  True si se debe representar el valor.
         */
        public void SetCheck_coefG_Rel(bool coefG_Rel)
        {
            this.check_coefG_Rel = coefG_Rel;
        }


        /* Descripción:
        *  Asigna el color pasado como parámetro y la asigna en la variable Color Coef. G Relativo.
        */
        public void SetColor_coefG_Rel(Color coefG_Rel)
        {
            this.color_coefG_Rel = coefG_Rel;
        }


        /* 
         * ======================
         * Coeficiente G Absoluto
         * ======================
         */

        /* Descripción:
         *  Asigna el valor pasado como parámetro bool almacenado en la variable Coeficiente G Absoluto.
         *  True si se debe representar el valor.
         */
        public void SetCheck_coefG_Abs(bool coefG_Abs)
        {
            this.check_coefG_Abs = coefG_Abs;
        }


        /* Descripción:
         *  Asigna el color pasado como parámetro en la variable Coeficiente G Absoluto.
         */
        public void SetColor_coefG_Abs(Color coefG_Abs)
        {
            this.color_coefG_Abs = coefG_Abs;
        }


        /* 
         * ===========================
         * Varianza del Error Relativo
         * ===========================
         */

        /* Descripción:
         *  Asigna el valor pasado como parámetro bool almacenado en la variable Varianza del Error Relativo.
         *  True si se debe representar el valor.
         */
        public void SetCheckTotalRelErrorVar(bool totalRelError)
        {
            this.checkTotalRelErrorVar = totalRelError;
        }


        /* Descripción:
         *  Asigna el color pasado como parámetro  en la variable Varianza del Error Relativo.
         */
        public void SetColorTotalRelErrorVar(Color totalRelError)
        {
            this.colorTotalRelErrorVar = totalRelError;
        }


        /* 
         * ===========================
         * Varianza del Error Absoluto
         * ===========================
         */

        /* Descripción:
         *  Asigna el valor pasado como parámetro bool almacenado en la variable Varianza del Error Absoluto.
         *  True si se debe representar el valor.
         */
        public void SetCheckTotalAbsErrorVar(bool totalAbsError)
        {
            this.checkTotalAbsErrorVar = totalAbsError;
        }


        /* Descripción:
         *  Asigna el color pasado como parámetro en la variable Varianza del Error Absoluto.
         */
        public void SetColorTotalAbsErrorVar(Color totalAbsError)
        {
            this.colorTotalAbsErrorVar = totalAbsError;
        }


        /* 
         * =======================
         * Error Estandar Relativo
         * =======================
         */

        /* Descripción:
         *  Asigna el valor pasado como parámetro bool almacenado en la variable Desviación tipica del Error Relativo.
         *  True si se debe representar el valor.
         */
        public void SetCheckErrorRelStandDev(bool errorRelStand)
        {
            this.checkErrorRelStandDev = errorRelStand;
        }

        /* Descripción:
         *  Asigna el color pasado como parámetro en la variable Desviación tipica del Error Relativo.
         */
        public void SetColorErrorRelStandDev(Color errorRelStand)
        {
            this.colorErrorRelStandDev = errorRelStand;
        }


        /* 
         * =======================
         * Error Estandar Absoluto
         * =======================
         */

        /* Descripción:
         *  Asigna el valor pasado como parámetro bool almacenado en la variable Desviación tipica del Error Absoluto.
         *  True si se debe representar el valor.
         */
        public void SetCheckErrorAbsStandDev(bool errorAbsStand)
        {
            this.checkErrorAbsStandDev = errorAbsStand;
        }


        /* Descripción:
         *  Asigna el color pasado como parámetro en la variable Desviación Típica del Error Absoluto.
         */
        public void SetColorErrorAbsStandDev(Color errorAbsStand)
        {
            this.colorErrorAbsStandDev = errorAbsStand;
        }


        /*
         * ===================================
         * TIPO DE GRÁFICA
         * ===================================
         */

        /* Descripción:
         *  Asigna el tipo de gráfica con la que se representa la serie.
         */
        public void SetSerieChartType(SeriesChartType type)
        {
            this.serieChartType = type;
        }


        /* Descripción:
         *  Posición en la que se mostrarán los datos respecto al punto de la serie
         */
        public void SetLabelAlignmentStyles(LabelAlignmentStyles position)
        {
            this.labelAlignmentStyles = position;
        }

        /* Descripción:
         *  Asigna el signo con el que se representará el punto en la gráfica.
         */
        public void SetMarkerStyle(MarkerStyle market)
        {
            this.markerStyle = market;
        }


        /*-------------------------------------------------------------------------------------
         * Escritura y lectura de ficheros de configuración
         *-------------------------------------------------------------------------------------*/

        /*
         * Descripción:
         *  Escribe el archivo de configuración.
         */
        public void WriteFileConfig(string path)
        {
            string pathFile = path + "\\" + NAME_FILE_CONFIG;
            using (StreamWriter writer = new StreamWriter(pathFile))
            {
                /*
                 * ======================
                 * Parámetros de Informes
                 * ======================
                 */
                writer.WriteLine(TITLE_REPORTS);
                writer.WriteLine();
                writer.WriteLine(STRING_LANGUAGE + "=" + this.GetConfigLanguage().ToString());
                writer.WriteLine(STRING_NUM_OF_DECIMALS + "=" + this.GetNumberOfDecimals().ToString());
                writer.WriteLine(STRING_DECIMAL_SEPARATOR + "=" + this.GetDecimalSeparator());
                writer.WriteLine(STRING_NULL_TO_ZERO + "=" + this.GetNull_to_Zero());
                writer.WriteLine(STRING_TYPE_OF_TABLE_MEANS + "=" + this.GetTypeOfTableMeans().ToString());
                writer.WriteLine(STRING_SHADING_ROWS + "=" + this.GetShadingRows().ToString());
                writer.WriteLine(STRING_TABLE_FONT_SIZE + "=" + this.GetTableFontSize().ToString());
                writer.WriteLine(STRING_TABLE_FONT_FAMILY + "=" + this.GetTableFontFamily());
                writer.WriteLine(STRING_TEXT_FONT_SIZE + "=" + this.GetTextFontSize().ToString());
                writer.WriteLine(STRING_TEXT_FONT_FAMILY + "=" + this.GetTextFontFamily());
                writer.WriteLine(WORKSPACE_FOLDER + "=" + this.Get_Path_Workspace());
                writer.WriteLine();
                writer.WriteLine();
                
                /*
                 * ======================
                 * Parámetros de Gráficos
                 * ======================
                 */
                writer.WriteLine(TITLE_CHARTS);
                writer.WriteLine();
                // Coeficiente G absoluto
                writer.WriteLine(STRING_CHECK_COEFG_ABS + "=" + this.GetCheck_coefG_Abs().ToString());
                writer.WriteLine(STRING_COLOR_COEFG_ABS + "=" + this.GetColor_coefG_Abs().ToArgb().ToString());
                // Coeficiente G Relativo
                writer.WriteLine(STRING_CHECK_COEFG_REL + "=" + this.GetCheck_coefG_Rel().ToString());
                writer.WriteLine(STRING_COLOR_COEFG_REL + "=" + this.GetColor_coefG_Rel().ToArgb().ToString());
                // Error standar absoluto
                writer.WriteLine(STRING_CHECK_ERROR_ABS_STAND_DEV + "=" + this.GetCheckErrorAbsStandDev().ToString());
                writer.WriteLine(STRING_COLOR_ERROR_ABS_STAND_DEV + "=" + this.GetColorErrorAbsStandDev().ToArgb().ToString());
                // Error standar relativo
                writer.WriteLine(STRING_CHECK_ERROR_REL_STAND_DEV + "=" + this.GetCheckErrorRelStandDev().ToString());
                writer.WriteLine(STRING_COLOR_ERROR_REL_STAND_DEV + "=" + this.GetColorErrorRelStandDev().ToArgb().ToString());
                // Varianza de error absoluta
                writer.WriteLine(STRING_CHECK_TOTAL_ABS_ERROR_VAR + "=" + this.GetCheckTotalAbsErrorVar().ToString());
                writer.WriteLine(STRING_COLOR_TOTAL_ABS_ERROR_VAR + "=" + this.GetColorTotalAbsErrorVar().ToArgb().ToString());
                // Varianza de error relativa
                writer.WriteLine(STRING_CHECK_TOTAL_REL_ERROR_VAR + "=" + this.GetCheckTotalRelErrorVar().ToString());
                writer.WriteLine(STRING_COLOR_TOTAL_REL_ERROR_VAR + "=" + this.GetColorTotalRelErrorVar().ToArgb().ToString());
                /*
                 * ===================================
                 * Tipo de gráfica
                 * ===================================
                 */
                // Tipo de grafica
                writer.WriteLine(STRING_SERIES_CHART_TYPE + "=" + this.serieChartType.ToString());
                // Posición del valor del punto
                writer.WriteLine(STRING_POINT_LABEL + "=" + this.labelAlignmentStyles.ToString());
                // Marca de representación del punto
                writer.WriteLine(STRING_MARKER_STYLE + "=" + this.markerStyle.ToString());
            }
        }// end WriteFileConfig

        
        /*
         * Descripción:
         *  Lee el contenido de el archivo config.cfg y lo carga en la clase
         */
        public void ReadFileConfig(string path)
        {
            string pathFile = path + "\\" + NAME_FILE_CONFIG;
            using (StreamReader reader = new StreamReader(pathFile))
            {
                string line = "";
                int nLine = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    this.AssigValueCfg(line, nLine);
                    nLine++;
                }
            }
        }

        
        /*
         * Descripción:
         *  Toma los datos de la linea que se pasa como parametró los interpreta y los carga en la
         *  variable.
         * Parámetros:
         *      string line: Linea leida del fichero.
         *      int nLine: número de línea
         * Excepciones:
         *      Lanza una excepción ConfigCFGException (se indica la línea donde se produjo el error) 
         *      en el caso de que no se pueda interpretar la linea leida, o no se reconozca el idioma.
         */
        private void AssigValueCfg(string line, int nLine)
        {
            int pos = line.IndexOf("=");
            if (pos != (-1))
            {
                string w = line.Substring(0, pos);
                string l = line.Substring(pos + 1).Trim();
                try
                {
                    switch (w)
                    {
                        case (STRING_LANGUAGE):
                            TransLibrary.Language lang = (TransLibrary.Language)Enum.Parse(typeof(TransLibrary.Language), l);
                            this.SetConfigLanguage(lang);
                            break;
                        case (STRING_NUM_OF_DECIMALS):
                            int n = int.Parse(l);
                            this.SetNumberOfDecimal(n);
                            break;
                        case (STRING_DECIMAL_SEPARATOR):
                            this.SetDecimalSeparator(l);
                            break;
                        case (STRING_NULL_TO_ZERO):
                            this.SetNull_to_Zero(this.StringToBool(l));
                            break;
                        case (WORKSPACE_FOLDER):
                            this.Set_Path_Workspace(l);
                            break;
                        /*
                         * ===========================================
                         * Parámetros gráficos
                         * ===========================================
                         */
                        // Coeficiente G Absoluto
                        case (STRING_CHECK_COEFG_ABS):
                            this.SetCheck_coefG_Abs(this.StringToBool(l));
                            break;
                        case (STRING_COLOR_COEFG_ABS):
                            Color colorCoefG_Abs = Color.FromArgb(int.Parse(l));
                            this.SetColor_coefG_Abs(colorCoefG_Abs);
                            break;
                        // Coeficiente G Relativo
                        case (STRING_CHECK_COEFG_REL):
                            this.SetCheck_coefG_Rel(this.StringToBool(l));
                            break;
                        case (STRING_COLOR_COEFG_REL):
                            Color colorCoefG_Rel = Color.FromArgb(int.Parse(l));
                            this.SetColor_coefG_Rel(colorCoefG_Rel);
                            break;
                        // Error Estandar Absoluto
                        case (STRING_CHECK_ERROR_ABS_STAND_DEV):
                            this.SetCheckErrorAbsStandDev(this.StringToBool(l));
                            break;
                        case (STRING_COLOR_ERROR_ABS_STAND_DEV):
                            Color colorError_Abs_stand = Color.FromArgb(int.Parse(l));
                            this.SetColorErrorAbsStandDev(colorError_Abs_stand);
                            break;
                        // Error Estandar Relativo
                        case (STRING_CHECK_ERROR_REL_STAND_DEV):
                            this.SetCheckErrorRelStandDev(this.StringToBool(l));
                            break;
                        case (STRING_COLOR_ERROR_REL_STAND_DEV):
                            Color colorError_Rel_stand = Color.FromArgb(int.Parse(l));
                            this.SetColorErrorRelStandDev(colorError_Rel_stand);
                            break;
                        // Varianza de Error Absoluto
                        case (STRING_CHECK_TOTAL_ABS_ERROR_VAR):
                            this.SetCheckTotalAbsErrorVar(this.StringToBool(l));
                            break;
                        case (STRING_COLOR_TOTAL_ABS_ERROR_VAR):
                            Color colorTotal_Abs_Error = Color.FromArgb(int.Parse(l));
                            this.SetColorTotalAbsErrorVar(colorTotal_Abs_Error);
                            break;
                        // Varianza de Error Relativo
                        case (STRING_CHECK_TOTAL_REL_ERROR_VAR):
                            this.SetCheckTotalRelErrorVar(this.StringToBool(l));
                            break;
                        case (STRING_COLOR_TOTAL_REL_ERROR_VAR):
                            Color colorTotal_Rel_Error = Color.FromArgb(int.Parse(l));
                            this.SetColorTotalRelErrorVar(colorTotal_Rel_Error);
                            break;
                        /*
                         * ===================================
                         * Tipo de gráfica
                         * ===================================
                         */
                        case (STRING_SERIES_CHART_TYPE):
                            this.SetSerieChartType(StringToSeriesCharType(l));
                            break;
                        case (STRING_POINT_LABEL):
                            this.SetLabelAlignmentStyles((LabelAlignmentStyles)Enum.Parse(typeof(LabelAlignmentStyles), l));
                            break;
                        case (STRING_MARKER_STYLE):
                            this.SetMarkerStyle((MarkerStyle)Enum.Parse(typeof(MarkerStyle), l));
                            break;
                        /*
                         * =================================================
                         * Tipo de tabla de medias
                         * =================================================
                         */
                        case (STRING_TYPE_OF_TABLE_MEANS):
                            string sringTypeMeans = l;
                            switch (l)
                            {
                                case (TABLE_MEANS_DEFAULT):
                                    this.SetTypeTableOfMeans(TypeOfTableMeans.Default);
                                    break;
                                case (TABLE_MEANS_DIF):
                                    this.SetTypeTableOfMeans(TypeOfTableMeans.TableMeansDif);
                                    break;
                                case (TABLE_MEANS_TYPE_POINT):
                                    this.SetTypeTableOfMeans(TypeOfTableMeans.TableMeansTipPoint);
                                    break;
                                default:
                                    throw new ConfigCFGException();
                            }
                            break;
                        case (STRING_SHADING_ROWS):
                            this.SetShadingRows(this.StringToBool(l));
                            break;

                        case (STRING_TABLE_FONT_SIZE):
                            this.SetTableFontSize(int.Parse(l));
                            break;
                        case (STRING_TABLE_FONT_FAMILY):
                            this.SetTableFontFamily(l);
                            break;
                        case (STRING_TEXT_FONT_SIZE):
                            this.SetTextFontSize(int.Parse(l));
                            break;
                        case (STRING_TEXT_FONT_FAMILY):
                            this.SetTextFontFamily(l);
                            break;
                        default:
                            // lanzamos una excepcion
                            throw new ConfigCFGException("Error al leer la linea " + nLine.ToString());
                    }
                }
                catch (FormatException)
                {
                    throw new ConfigCFGException("Error al leer la linea " + nLine.ToString() + ". Tamaño de fuente no valida.");
                }
                catch (ConfigCFGException)
                {
                    throw new ConfigCFGException("Error al leer la linea " + nLine.ToString());
                }
                
            }
        }// end AssigValueCfg
        
        
        /* Descripción:
         *  Operación auxiliar. Devuelve true si el string que se pasa como parámetro se corresponde con la cadena
         *  "true", false en el caso que la cadena sea "false" y lanzará una excepción en otro caso.
         */
        private bool StringToBool(string b)
        {
            bool retVal = false;
            switch (b.ToLower())
            {
                case("true"):
                    retVal = true;
                    break;
                case("false"):
                    break;
                default:
                    throw new ConfigCFGException("No es un booleano valido");
            }
            return retVal;
        }


        #region Métodos redefinidos: ToString, Equals y GetHashCode
        /*----------------------------------------------------------------------------
         * Metodos redefinidos
         *---------------------------------------------------------------------------*/

        /*
         * Descripción:
         *  Redefinición del método ToString().
         */
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();

            res.Append(TITLE_REPORTS + "/n");
            res.Append(STRING_LANGUAGE + " = " + this.language.ToString() + "/n");
            res.Append(STRING_NUM_OF_DECIMALS + " = " + this.numOfDecimals.ToString()+"/n");
            res.Append(STRING_DECIMAL_SEPARATOR + " = " + this.decimalSeparator + "/n");
            res.Append(STRING_NULL_TO_ZERO + " = " + this.null_to_zero + "/n");
            res.Append(STRING_TYPE_OF_TABLE_MEANS + " = " + this.typeMeans.ToString() + "/n");
            res.Append(STRING_SHADING_ROWS + " = " + this.shadingRows + "/n");
            res.Append(STRING_TABLE_FONT_SIZE + " = " + this.tableFontSize + "/n");
            res.Append(STRING_TABLE_FONT_FAMILY + " = " + this.tableFontFamily + "/n");
            res.Append(STRING_TEXT_FONT_SIZE + " = " + this.textFontSize + "/n");
            res.Append(STRING_TEXT_FONT_FAMILY + " = " + this.textFontFamily + "/n");
            res.Append(WORKSPACE_FOLDER + " = " + this.path_workspace + "/n");
            res.Append("/n"); // Salto de línea

            res.Append(TITLE_CHARTS + "/n");
            // Coeficiente G absoluto
            res.Append(STRING_CHECK_COEFG_ABS + " = " + this.check_coefG_Abs + "/n");
            res.Append(STRING_COLOR_COEFG_ABS + " = " + Color2String(this.color_coefG_Abs) + "/n");
            // Coeficiente G Relativo
            res.Append(STRING_CHECK_COEFG_REL + " = " + this.check_coefG_Rel + "/n");
            res.Append(STRING_COLOR_COEFG_REL + " = " + Color2String(this.color_coefG_Rel) + "/n");
            // Error absoluto standar
            res.Append(STRING_CHECK_ERROR_ABS_STAND_DEV + " = " + this.checkErrorAbsStandDev + "/n");
            res.Append(STRING_COLOR_ERROR_ABS_STAND_DEV + " = " + Color2String(this.colorErrorAbsStandDev) + "/n");
            // Error relativo standar
            res.Append(STRING_CHECK_ERROR_REL_STAND_DEV + " = " + this.checkErrorRelStandDev + "/n");
            res.Append(STRING_COLOR_ERROR_REL_STAND_DEV + " = " + Color2String(this.colorErrorRelStandDev) + "/n");
            // Varianza de error absoluto
            res.Append(STRING_CHECK_TOTAL_ABS_ERROR_VAR + " = " + this.checkTotalAbsErrorVar + "/n");
            res.Append(STRING_COLOR_TOTAL_ABS_ERROR_VAR + " = " + Color2String(this.colorTotalAbsErrorVar) + "/n");
            // Varianza de error relativo
            res.Append(STRING_CHECK_TOTAL_REL_ERROR_VAR + " = " + this.checkTotalRelErrorVar + "/n");
            res.Append(STRING_COLOR_TOTAL_REL_ERROR_VAR + " = " + Color2String(this.colorTotalRelErrorVar) );
            // Tipo de gráfica
            res.Append(STRING_SERIES_CHART_TYPE + " = " + this.serieChartType.ToString());
            // Posición de los valores
            res.Append(STRING_POINT_LABEL + " = " + this.labelAlignmentStyles.ToString());
            // Tipo de signo con el que se representará el punto
            res.Append(STRING_MARKER_STYLE + " = " + this.markerStyle.ToString());

            return res.ToString();
        }// end ToString


        /* Descripción:
         *  Operación auxiliar para pasar a string un color.
         */
        private string Color2String(Color c)
        {
            string retVal = c.ToArgb().ToString();
            if (c.IsKnownColor)
            {
                retVal = c.Name.ToString();
            }
            return retVal;
        }


        /* Descripción:
         *  Devuelve el tipo de gráfico que se corresponde con el string que se pasa como parámetro.
         */
        private SeriesChartType StringToSeriesCharType(string type)
        {
            SeriesChartType  retVal = SeriesChartType.Spline;
            switch (type)
            {
                case("Spline"):
                    retVal = SeriesChartType.Spline;
                    break;
                case("Line"):
                    retVal = SeriesChartType.Line;
                    break;
                default:
                    throw new ConfigCFGException("Error con el tipo de gráfica");
            }
            return retVal;
        }


        /*
         * Descripción:
         *  Redefinición del método Equals.
         */
        public override bool Equals(object obj)
        {
            bool retVal = false;
            if (!(obj == null || GetType() != obj.GetType()))
            {
                ConfigCFG cfg = (ConfigCFG)obj;
                retVal = this.language.Equals(cfg.language)
                    && this.numOfDecimals.Equals(cfg.numOfDecimals)
                    && this.decimalSeparator.Equals(cfg.decimalSeparator)
                    && this.null_to_zero.Equals(cfg.null_to_zero)
                    && this.path_workspace.Equals(cfg.path_workspace)
                    && this.check_coefG_Abs.Equals(cfg.check_coefG_Abs)
                    && this.color_coefG_Abs.Equals(cfg.color_coefG_Abs)
                    && this.check_coefG_Rel.Equals(cfg.check_coefG_Rel)
                    && this.color_coefG_Rel.Equals(cfg.color_coefG_Rel)
                    && this.checkErrorAbsStandDev.Equals(cfg.checkErrorAbsStandDev)
                    && this.colorErrorAbsStandDev.Equals(cfg.colorErrorAbsStandDev)
                    && this.checkErrorRelStandDev.Equals(cfg.checkErrorRelStandDev)
                    && this.colorErrorRelStandDev.Equals(cfg.colorErrorRelStandDev)
                    && this.checkTotalAbsErrorVar.Equals(cfg.checkTotalAbsErrorVar)
                    && this.colorTotalAbsErrorVar.Equals(cfg.colorTotalAbsErrorVar)
                    && this.checkTotalRelErrorVar.Equals(cfg.checkTotalRelErrorVar)
                    && this.colorTotalRelErrorVar.Equals(cfg.colorTotalRelErrorVar)
                    && this.typeMeans.Equals(cfg.typeMeans)
                    && this.shadingRows.Equals(cfg.shadingRows)
                    && this.tableFontSize == cfg.tableFontSize
                    && this.tableFontFamily.Equals(cfg.tableFontFamily)
                    && this.textFontSize == cfg.textFontSize
                    && this.textFontFamily.Equals(cfg.textFontFamily)
                    && this.serieChartType.Equals(cfg.serieChartType)
                    && this.labelAlignmentStyles.Equals(cfg.labelAlignmentStyles)
                    && this.markerStyle.Equals(cfg.markerStyle);

            }
            return retVal;
        }


        /*
         * Descripción:
         *  Redefinición del método Equals.
         */
        public override int GetHashCode()
        {
            return (this.language.GetHashCode() + this.numOfDecimals.GetHashCode()
                + this.decimalSeparator.GetHashCode() + this.null_to_zero.GetHashCode()
                + this.path_workspace.GetHashCode()
                + this.check_coefG_Abs.GetHashCode() + this.color_coefG_Abs.GetHashCode()
                + this.check_coefG_Rel.GetHashCode() + this.color_coefG_Rel.GetHashCode()
                + this.checkErrorAbsStandDev.GetHashCode() + this.colorErrorAbsStandDev.GetHashCode()
                + this.checkErrorRelStandDev.GetHashCode() + this.colorErrorRelStandDev.GetHashCode()
                + this.checkTotalAbsErrorVar.GetHashCode() + this.colorTotalAbsErrorVar.GetHashCode()
                + this.checkTotalRelErrorVar.GetHashCode() + this.colorTotalRelErrorVar.GetHashCode()
                + this.serieChartType.GetHashCode() + this.labelAlignmentStyles.GetHashCode()
                + this.markerStyle.GetHashCode()
                + this.typeMeans.GetHashCode() + this.shadingRows.GetHashCode()
                + this.tableFontSize.GetHashCode() + this.tableFontFamily.GetHashCode()
                + this.textFontSize.GetHashCode() + this.textFontFamily.GetHashCode())/3;
        }

        #endregion Métodos redefinidos: ToString, Equals y GetHashCode


    }// end public class ConfigCFG
}// end namespace ConfigCFG

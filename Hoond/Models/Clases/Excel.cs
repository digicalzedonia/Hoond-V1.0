using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Excel;
using System.Data;
using System.IO;
using Excelensam = Microsoft.Office.Interop.Excel;

namespace Excel
{
    public class ExcelData
    {
        string _path;

        public ExcelData(string path)
        {
            _path = path;

        }


 public string fncExcelExport(DataSet DataS, string sRuta, string sHoja, int sTipo, bool sVista) {
        int intColumn;
        int intRow;
        int intColumnValue;
        try {
            Excelensam.Application Excel_1 = new Excelensam.Application();
            //string strExcelFile;
            //string strFileName;
            string encabezado;
           // cls_formato tfotmato = new cls_formato();
            string formato = "@";
           // VBIDE.VBComponent oModule1;
           // Excelensam.Workbook obj;
            
            string auxNombreHoja;
            // With...
              Excel_1.SheetsInNewWorkbook = DataS.Tables.Count;
                Excel_1.Workbooks.Add();

            for (int tablas = 1; (tablas <= DataS.Tables.Count); tablas++) {
                // If tablas > 1 Then
                // End If
                Excel_1.Visible = sVista;
                Excel_1.Worksheets[tablas].Select();

                if ((DataS.Tables.Count == 1)) {
                    if ((sHoja == "")) {
                        //DataS.Tables[(tablas - 1)].TableName.ToString.Worksheets(tablas).Name = auxNombreHoja;
                        //auxNombreHoja = auxNombreHoja;

                         auxNombreHoja = DataS.Tables[tablas - 1].TableName.ToString();
                         Excel_1.Worksheets[tablas].Name = auxNombreHoja;
                    }
                    else {
                          Excel_1.Worksheets[tablas].Name = sHoja;
                    }
                    
                }
                else {
                    Excel_1.Worksheets[tablas].Name = DataS.Tables[(tablas - 1)].TableName.ToString();
                }
                
                // Agrega Encabezados
                switch (sTipo) {
                    case 0:
                        for (intColumn = 0; (intColumn 
                                    <= (DataS.Tables[(tablas - 1)].Columns.Count - 1)); intColumn++) {
                            encabezado = DataS.Tables[(tablas - 1)].Columns[intColumn].ColumnName.ToString();
                            // da formato a las columnas
                           // tfotmato.titulo = encabezado;
                           // formato = tfotmato.TraeFormato;
                            if ((formato != ".")) {
                                Excel_1.Worksheets[tablas].columns[(intColumn + 1)].numberformat = formato;
                            }
                            
                            Excel_1.Cells[1, (intColumn + 1)].Value = encabezado;
                        }
                        
                        for (intRow = 0; (intRow  <= (DataS.Tables[(tablas - 1)].Rows.Count - 1)); intRow++) 
                        {
                            for (intColumnValue = 0; intColumnValue <=(DataS.Tables[(tablas - 1)].Columns.Count - 1); intColumnValue++)
                            {
                                Excel_1.Cells[intRow + 2, intColumnValue + 1].Value = DataS.Tables[tablas - 1].Rows[intRow].ItemArray[intColumnValue].ToString();
                            }
                            
                        }
                        

                      

                        break;
                    case 1:
                        for (intRow = 0; (intRow 
                                    <= (DataS.Tables[(tablas - 1)].Rows.Count - 1)); intRow++) {
                            for (intColumnValue = 0; (intColumnValue <= (DataS.Tables[(tablas - 1)].Columns.Count - 1)); intColumnValue++)
                            {
                                 Excel_1.Cells[intRow + 2, intColumnValue + 1].Value = DataS.Tables[tablas - 1].Rows[intRow].ItemArray[intColumnValue].ToString();
                            }
                            
                        }
                        
                        break;
                }
            }
            
            switch (sTipo) {
                case 0:
                Excel_1.ActiveWorkbook.SaveAs(sRuta, -4143, "", "", false, false);
                    if ((sVista == false)) {
                        Excel_1.ActiveWorkbook.Close();
                        // TODO: Labeled Arguments not supported. Argument: 2 := 'FileFormat'
                        // TODO: Labeled Arguments not supported. Argument: 3 := 'Password'
                        // TODO: Labeled Arguments not supported. Argument: 4 := 'WriteResPassword'
                        // TODO: Labeled Arguments not supported. Argument: 5 := 'ReadOnlyRecommended'
                        // TODO: Labeled Arguments not supported. Argument: 6 := 'CreateBackup'
                    }
                    
                    break;
                case 1:
                    Excel_1.ActiveWorkbook.SaveAs(sRuta);
                       Excel_1.ActiveWorkbook.Close();
                    break;
            }
            // Excel.Quit()
            // Excel = Nothing
            GC.Collect();
            return "Se exporto el archivo correctamente.";
        }
        catch (Exception ex) {
            return ex.Message.ToString();
        }
        
    }

 public DataTable getDataDT(string sheet, bool firstRowIsColumnNames = true)
 {
     var reader = this.getExcelReader();
     reader.IsFirstRowAsColumnNames = firstRowIsColumnNames;
     return reader.AsDataSet().Tables[sheet];
 }

        public IEnumerable<DataRow> getData(string sheet, bool firstRowIsColumnNames = true)
        {
            var reader = this.getExcelReader();
            reader.IsFirstRowAsColumnNames = firstRowIsColumnNames;
            var workSheet = reader.AsDataSet().Tables[sheet];
            var rows = from DataRow a in workSheet.Rows select a;
            return rows;
        }

        public IEnumerable<string> getWorksheetExiste(string hoja)
        {
            var reader = this.getExcelReader();
            var workbook = reader.AsDataSet();
            var sheets = from DataTable sheet in workbook.Tables
                         where sheet.TableName == hoja
                         select sheet.TableName;
            return sheets;
        }

        public IEnumerable<string> getWorksheetNames()
        {
            var reader = this.getExcelReader();
            var workbook = reader.AsDataSet();
            var sheets = from DataTable sheet in workbook.Tables select sheet.TableName;
            return sheets;
        }
        public IExcelDataReader getExcelReader()
        {
            // ExcelDataReader works with the binary Excel file, so it needs a FileStream
            // to get started. This is how we avoid dependencies on ACE or Interop:
            FileStream stream = File.Open(_path, FileMode.Open, FileAccess.Read);

            // We return the interface, so that 
            IExcelDataReader reader = null;
            try
            {
                if (_path.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                if (_path.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Net;
using System.IO;
using Ionic.Zip;
using System.Data;
using System.Collections;
using System.Xml.Linq;
using System.Reflection;

namespace Archivos
{
    public class ArchivoData
    {


       


        public string CreateTextDelimiterFile(string fileName, DataTable dt, string separatorChar, bool hdr, bool textDelimiter)
        {

            // Si no se ha especificado un nombre de archivo,
            // o el objeto DataTable no es válido, provocamos
            // una excepción de argumentos no válidos.
            //
            if ((fileName == string.Empty) || (dt == null))
                throw new System.ArgumentException("Argumentos no válidos.");

            // Si el archivo existe, solicito confirmación para sobreescribirlo.
            //
            if ((System.IO.File.Exists(fileName)))
            {
                return "0";
            }

            System.IO.StreamWriter sw = default(System.IO.StreamWriter);

            try
            {
                int col = 0;
                string value = string.Empty;

                // Creamos el archivo de texto con la codificación por defecto.
                //
                sw = new System.IO.StreamWriter(fileName, false, System.Text.Encoding.Default);

                if ((hdr))
                {
                    // La primera línea del archivo de texto contiene
                    // el nombre de los campos.
                    foreach (DataColumn dc in dt.Columns)
                    {
                        value += dc.ColumnName + separatorChar;
                    }

                    sw.WriteLine(value.Remove(value.Length - 1, 1));
                    value = string.Empty;

                }

                // Recorremos todas las filas del objeto DataTable
                // incluido en el conjunto de datos.
                //

                foreach (DataRow dr in dt.Rows)
                {

                    foreach (DataColumn dc in dt.Columns)
                    {

                        if (((object.ReferenceEquals(dc.DataType, System.Type.GetType("System.String"))) & (textDelimiter == true)))
                        {
                            // Incluimos el dato alfanumérico entre el caracter
                            // delimitador de texto especificado.
                            //
                            value +=   "\"" + dr[col].ToString() + "\"" + separatorChar;

                        }
                        else
                        {
                            // No se incluye caracter delimitador de texto alguno
                            //
                            value += dr[col].ToString() + separatorChar;

                        }

                        // Siguiente columna
                        col += 1;

                    }

                    // Al escribir los datos en el archivo, elimino el
                    // último carácter delimitador de la fila.
                    //
                    sw.WriteLine(value.Remove(value.Length - 1, 1));
                    value = string.Empty;
                    col = 0;

                }
                // Siguiente fila

                // Nos aseguramos de cerrar el archivo
                //
                sw.Close();

                // Se ha creado con éxito el archivo de texto.
                //
                return "1";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                sw = null;
            }
        }


        public DataTable LINQResultToDataTable<T>(IEnumerable<T> collection)
        {
            DataTable dt = new DataTable();

            Type t = typeof(T);

            PropertyInfo[] pia = t.GetProperties();

            //Create the columns in the DataTable

            foreach (PropertyInfo pi in pia)
            {

                dt.Columns.Add(pi.Name, pi.PropertyType);

            }

            //Populate the table

            foreach (T item in collection)
            {

                DataRow dr = dt.NewRow();

                dr.BeginEdit();

                foreach (PropertyInfo pi in pia)
                {

                    dr[pi.Name] = pi.GetValue(item, null);

                }

                dr.EndEdit();

                dt.Rows.Add(dr);

            }

            return dt;
        }

        public void descomprimir(string file_descomprimir, string ruta_destino)
        {
            try
            {
                if (File.Exists(file_descomprimir))
                {
                    ZipFile s = ZipFile.Read(file_descomprimir);
                    s.ExtractAll(ruta_destino);
                    s.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void ComprimeFile(string ruta, string NombreArchivoZip)
        {
            ZipFile zip = new ZipFile();
            zip.AddFile(ruta, "");
            zip.Save(NombreArchivoZip);
        }

        public void Comprime(string ruta, string NombreArchivoZip)
        {
            ZipFile zip = new ZipFile();
            zip.AddDirectory(ruta);
            zip.Save(NombreArchivoZip);
        }

        //lee un archivo txt o cvs y regresa un dt
        public DataTable leer_archivo_db(string archivo, string direccion)
        {
            System.Data.DataTable dt = lee_archivo_stream_separador(direccion + archivo);
            return dt;
        }

        //lee todo el contenido de un archivo y regresa un string
        public string Lee_Archivo_RString(string Archivo, int replaceGo)
        {
            string str = "";
            try
            {

                string s = "";
                using (StreamReader reader = new StreamReader(Archivo, System.Text.Encoding.UTF8))
                {
                    s = reader.ReadToEnd();
                }


                if (replaceGo == 1)
                {
                    str = s.ToString().Replace(Environment.NewLine + "GO" + Environment.NewLine, "");
                }
                else
                {
                    str = s.ToString();
                }
            }
            catch (Exception ex)
            {

            }

            return str;
        }


        public DataSet LeerXML(string direcc_archivo)  
         {
             string xmlData = direcc_archivo;
             //string xmlData = HttpContext.Current.Server.MapPath(direcc_archivo);
               DataSet ds = new DataSet();  
               ds.ReadXml(xmlData);  
               return ds;
      }  

        //  lee archivo txt o cvs con separador
        public DataTable lee_archivo_stream_separador(string direcc_archivo, char separador = ',', bool columnas = true)
        {
            System.IO.StreamReader objReader = new System.IO.StreamReader(direcc_archivo);
            DataTable dt = new DataTable("TablaInfo");
            try
            {
                DataRow row_dt = default(DataRow);

                //sacamos las columnas

                string[] column = objReader.ReadLine().ToString().Split(separador);

                for (int c = 0; c <= column.Length - 1; c++)
                {
                    string nombreColumna = column[c];
                    if (columnas == false)
                    {
                        nombreColumna = "Columna" + c;
                    }

                    if (nombreColumna.IndexOf("fecha") >= 0)
                    {
                        dt.Columns.Add(new DataColumn(nombreColumna, typeof(System.DateTime)));
                    }
                    else if (nombreColumna.IndexOf("id_") >= 0)
                    {
                        dt.Columns.Add(new DataColumn(nombreColumna, typeof(int)));
                    }
                    else
                    {
                        dt.Columns.Add(new DataColumn(nombreColumna));
                    }
                }

                while (objReader.Peek() >= 0)
                {
                    string[] sLine = objReader.ReadLine().Split(separador);
                    row_dt = dt.NewRow();
                    for (int r = 0; r <= sLine.Length - 1; r++)
                    {
                        string valor = sLine[r];

                        dynamic tipo = dt.Columns[r].DataType;

                        if (!string.IsNullOrEmpty(valor))
                        {
                            row_dt[r] = valor;
                        }
                    }
                    dt.Rows.Add(row_dt);
                }

            }
            catch (Exception ex)
            {
                dt = null;
            }
            objReader.Close();
            objReader.Dispose();
            return dt;
        }

        //lee un archivo txt o cvs
        public DataTable LeeArchivo_dt(string direcc_archivo)
        {
            StreamReader objReader = new StreamReader(direcc_archivo, System.Text.Encoding.Default);
            DataTable dt = new DataTable("TablaInfo");

            try
            {
                string sLine = "";

                dt.Columns.Add(new DataColumn("linea", typeof(string)));
                DataRow dr_row = default(DataRow);

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                    {
                        dr_row = dt.NewRow();
                        dr_row["linea"] = sLine;
                        dt.Rows.Add(dr_row);
                    }
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }
            objReader.Close();
            objReader.Dispose();
            return dt;

        }

        //lee un archivo txt o cvs
        public ArrayList LeeArchivo_Arraylist(string direcc_archivo)
        {
            StreamReader objReader = new StreamReader(direcc_archivo, System.Text.Encoding.Default);
            ArrayList arrText = new ArrayList();

            try
            {
                string sLine = "";
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                    {
                        arrText.Add(sLine);
                    }
                }


            }
            catch (Exception ex)
            {
                arrText = null;
            }
            objReader.Close();
            objReader.Dispose();
            return arrText;
        }





    }
}
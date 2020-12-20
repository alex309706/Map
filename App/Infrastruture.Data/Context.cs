using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Data.OleDb;
using Domain.Core;

namespace Infrastructure.Data
{
    public class Context
    {
        //Считываемые строки
        public List<Dictionary<string, string>> List_of_strings { get; set; }
        //Координаты из второго листа
        public Dictionary<string,Coordinates> Coordinates_of_city { get; set; }

        public Context(string path_to_file)
        {
            List_of_strings = new List<Dictionary<string, string>>();
            Coordinates_of_city = new Dictionary<string, Coordinates>();
            FileInfo file = new FileInfo(path_to_file);
            Fill_Data(file);
        }

        //Строка подключения к файлу
        string GetConnectionString(FileInfo _file)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();
            // XLSX - Excel 2007, 2010, 2012, 2013
            if (_file.Extension == ".xlsx")
            {
                props["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
                props["Extended Properties"] = "Excel 12.0 XML";
                props["Data Source"] = _file.FullName;
            }
            else if (_file.Extension == ".xls")
            {
                props["Provider"] = "Microsoft.Jet.OLEDB.4.0";
                props["Extended Properties"] = "Excel 8.0";
            }
            else throw new Exception("Неизвестное расширение файла!");

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            return sb.ToString();
        }
        //считывание строк (наполнение данными объекты с#)
        private void Fill_Data(FileInfo file)
        {
            DataSet ds = new DataSet();
            string connectionString = GetConnectionString(file);

            using (System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(connectionString))
            {
                conn.Open();
                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
                cmd.Connection = conn;

                // Get all Sheets in Excel File
                DataTable dtSheet = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                
                // Loop through all Sheets to get data
                foreach (DataRow dr in dtSheet.Rows)
                {
                    string sheetName = dr["TABLE_NAME"].ToString();

                    // Get all rows from the Sheet
                    cmd.CommandText = "SELECT * FROM [" + sheetName + "]";

                    DataTable dt = new DataTable();
                    dt.TableName = sheetName;

                    System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(cmd);
                    da.Fill(dt);

                    ds.Tables.Add(dt);
                }
                foreach (DataTable dt in ds.Tables)
                {
                    if (dt.TableName=="'Общие данные$'")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            //получаем все ячейки строки
                            var cells = row.ItemArray;
                            Dictionary<string, string> new_row = new Dictionary<string, string>();

                            string key = cells[1].ToString() + ":";
                            string value = string.Empty;
                            for (int i = 2; i < cells.Length; i++)
                            {
                                value += cells[i].ToString() + " ";
                            }
                            if (key == ":")
                            {
                                continue;
                            }

                            new_row.Add(key, RemoveSpaces(value));

                            List_of_strings.Add(new_row);
                        }
                    }
                    if (dt.TableName=="Города_Yandex$")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            var cells = row.ItemArray;
                            string name_of_city = cells[0].ToString();
                            string x = cells[1].ToString() == "" ? "0" : cells[1].ToString();
                            string y = cells[2].ToString()==""? "0" : cells[2].ToString();

                            Domain.Core.Coordinates coord = new Domain.Core.Coordinates(double.Parse(x),double.Parse(y));
                            if (!Coordinates_of_city.ContainsKey(name_of_city))
                            {
                                Coordinates_of_city.Add(name_of_city, coord);
                            }
                        }
                    }
                   
                }
            }
        }
        private string RemoveSpaces(string str)
        {
            string new_str = string.Empty;
            for (int i = 0; i < str.Length-1; i++)
            {
                char letter = str[i];
                char next_letter = str[i + 1];
                string two_letters = letter.ToString() + next_letter.ToString();
                if (two_letters=="  ")
                {
                    continue;
                }
                new_str += letter;
            }
            return new_str;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FManager
{
    public class DB
    {
        private string connectionDB
        {
            get
            {
                return @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=" + set.locationWorkFolder + "\\stats.mdf;" +
                    "Integrated Security=True;Connect Timeout=30";
            }
        }

        private Assistant set = new Assistant();
        private SqlConnection connection;
        private SqlCommand cmd;
        private SqlDataReader reader;

        public enum Tables
        {
            She,
            SheBig,
            He,
            HeGifts,
            HeBig,
            AbstractCASUAL,
            AbstractBIG
        }

        public DB()
        {
            connection = new SqlConnection(connectionDB);
        }

        /// <summary>
        /// Создает выбранную таблицу
        /// </summary>
        public void CreateTable(Tables table)
        {
            Open();
            if (table == Tables.He)
            {
                cmd.CommandText = "create table He(" +
                                    "id integer primary key identity, " +
                                     "date_expense date not null," +
                                     "event_type nvarchar(2) not null, " +
                                     "count int not null, " +
                                     "count_expenses int not null, " +
                                     "description nvarchar(255) default null," +
                                     "type nvarchar(3) default null, " +
                                     "full_line nvarchar(255) not null);\n";
            }
            if (table == Tables.She)
            {
                cmd.CommandText += "create table She(" +
                                    "id integer primary key identity, " +
                                     "date_expense date not null," +
                                     "event_type nvarchar(2) not null, " +
                                     "count int not null, " +
                                      "count_expenses int not null, " +
                                     "description nvarchar(255) default null," +
                                     "type nvarchar(3) default null, " +
                                     "full_line nvarchar(255) not null);\n";
            }
            if (table == Tables.HeGifts)
            {
                cmd.CommandText += "create table HeGifts(" +
                                     "id integer primary key identity, " +
                                     "date_expense date not null," +
                                     "description nvarchar(255) not null," +
                                      "expenses nvarchar(100) not null," +
                                       "type nvarchar(3) not null," +
                                       "param nvarchar(1)," +
                                       "full_line nvarchar(100));\n";
            }
            if (table == Tables.HeBig)
            {
                cmd.CommandText += "create table HeBig(" +
                                     "id integer primary key identity, " +
                                     "date_expense date not null," +
                                     "description nvarchar(255) not null," +
                                     "expenses nvarchar(100) not null," +
                                       "type nvarchar(3) not null," +
                                       "full_line nvarchar(100));\n";
            }
            if (table == Tables.SheBig)
            {
                cmd.CommandText += "create table SheBig(" +
                                     "id integer primary key identity, " +
                                     "date_expense date not null," +
                                     "description nvarchar(255) not null," +
                                     "expenses nvarchar(100) not null," +
                                       "type nvarchar(3) not null," +
                                       "full_line nvarchar(100));\n";
            }
            try
            {
                cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка из функции: DB.CreateTable. Ошибка: " + ex.Message);
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Удаляет выбранную таблицу
        /// </summary>
        /// <param name="table"></param>
        public void DeleteTables(Tables table)
        {
            try
            {
                string nameTable = string.Empty;
                if (table == Tables.He)
                    nameTable = "He";
                if (table == Tables.HeBig)
                    nameTable = "HeBig";
                if (table == Tables.HeGifts)
                    nameTable = "HeGifts";
                if (table == Tables.She)
                    nameTable = "She";
                if (table == Tables.SheBig)
                    nameTable = "SheBig";
                Open();
                cmd.CommandText = "drop table dbo." + nameTable;
                cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка из функции: DB.DeleteTables. Ошибка: " + ex.Message);
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Выполняет запись в таблицу She или He
        /// </summary>
        /// <param name="gender"></param>
        /// <param name="date_expense"></param>
        /// <param name="event_type"></param>
        /// <param name="count"></param>
        /// <param name="count_expenses"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        /// <param name="full_line"></param>
        public void InsertIntoCasual(Tables table, string date_expense, string event_type,
            string count, string count_expenses, string description, string type, string full_line)
        {
            try
            {
                Open();
                string nameTable = "";
                if (table == Tables.He)
                    nameTable = "He";
                else if (table == Tables.She)
                    nameTable = "She";
                else
                {
                    MessageBox.Show("Выбранная таблциа не поддерживется для записи из этой функции.", "Ошибкочка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cmd.CommandText = string.Format("insert into dbo." + nameTable +
                    "(date_expense, event_type, count, count_expenses, description, type, full_line) values('{0}', N'{1}', {2}, N'{3}', N'{4}', N'{5}', N'{6}')",
                    date_expense, event_type, count, count_expenses, description, type, full_line);
                cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка из функции: DB.InsertIntoCasult. Ошибка: " + ex.Message);
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Выполняет запись в таблицу HeBig или heGifts
        /// </summary>
        /// <param name="table"></param>
        /// <param name="date_expenses"></param>
        /// <param name="description"></param>
        /// <param name="expenses"></param>
        /// <param name="type"></param>
        public void InsertIntoBigTables(Tables table, string date_expenses, string description, string expenses, string type, string param, string full_line)
        {
            try
            {
                Open();
                string nameTable = "";
                if (table == Tables.HeBig)
                    nameTable = "HeBig";
                else if (table == Tables.HeGifts)
                    nameTable = "HeGifts";
                else if (table == Tables.SheBig)
                    nameTable = "SheBig";
                else
                {
                    MessageBox.Show("Выбранная табоица не поддерживается для записи из этой функции.", "Ошибочка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (table == Tables.HeBig || table == Tables.SheBig)
                    cmd.CommandText += string.Format("insert into dbo." + nameTable + "(date_expense, description, expenses, type, full_line)" +
                    " values ('{0}', N'{1}', N'{2}', N'{3}', N'{4}')",
                    date_expenses, description, expenses, type, full_line);
                else
                    cmd.CommandText += string.Format("insert into dbo." + nameTable + "(date_expense, description, expenses, type, param, full_line)" +
                        " values ('{0}', N'{1}', N'{2}', N'{3}', N'{4}', N'{5}')",
                        date_expenses, description, expenses, type, param, full_line);
                cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка из функции: DB.InsertIntoBigTables. Ошибка: " + ex.Message);
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Полностью очищаает выбранную таблицу
        /// </summary>
        /// <param name="table"></param>
        public void ClearTable(Tables table)
        {
            string nameTable = string.Empty;
            if (table == Tables.She)
                nameTable = "She";
            if (table == Tables.He)
                nameTable = "He";
            if (table == Tables.HeGifts)
                nameTable = "HeGifts";
            if (table == Tables.HeBig)
                nameTable = "HeBig";
            if (table == Tables.SheBig)
                nameTable = "SheBig";
            try
            {
                Open();
                cmd.CommandText = "delete from dbo." + nameTable;
                cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка из функции: DB.ClearTables. Ошибка: " + ex.Message);
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Возвращает список существующий таблиц
        /// </summary>
        /// <returns></returns>
        public List<Tables> GetExistsTables()
        {
            List<string> names = new List<string>();
            List<Tables> tables = new List<Tables>();
            try
            {
                Open();
                cmd.CommandText = "select name from sys.objects where type in (N'U');";
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                        names.Add(reader[i].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка из функции: DB.GetExistsTables. Ошибка: " + ex.Message);
            }
            finally
            {
                Close();
            }
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i] == "He")
                    tables.Add(Tables.He);
                if (names[i] == "HeBig")
                    tables.Add(Tables.HeBig);
                if (names[i] == "HeGifts")
                    tables.Add(Tables.HeGifts);
                if (names[i] == "She")
                    tables.Add(Tables.She);
                if (names[i] == "SheBig")
                    tables.Add(Tables.SheBig);
            }
            return tables;
        }

        /// <summary>
        /// Выполняет запрос к таблице
        /// </summary>
        /// <param name="query"></param>
        public Dictionary<int, Dictionary<string, string>> ExecuteQuery(string query)
        {
            Dictionary<int, Dictionary<string, string>> response = new Dictionary<int, Dictionary<string, string>>();
            try
            {
                Open();
                cmd.CommandText = query;
                reader = cmd.ExecuteReader();
                int counter = 0;
                while (reader.Read())
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        dict.Add(reader.GetName(i), reader[i].ToString());
                    response.Add(counter, dict);
                    counter++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка из функции: DB.ExecuteQuery. Ошибка: " + ex.Message);
            }
            finally
            {
                Close();
            }
            return response;
        }

        /// <summary>
        /// Возвращает словарь всех записей с выбранной таблицы
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        public Dictionary<int, Dictionary<string, string>> GetData(Tables table)
        {
            Dictionary<int, Dictionary<string, string>> data = new Dictionary<int, Dictionary<string, string>>();
            try
            {
                Open();
                string tableName = "";
                if (table == Tables.He)
                    tableName = "He";
                else if (table == Tables.She)
                    tableName = "She";
                else if (table == Tables.HeGifts)
                    tableName = "HeGifts";
                else if (table == Tables.HeBig)
                    tableName = "HeBig";
                else if (table == Tables.SheBig)
                    tableName = "SheBig";
                cmd.CommandText = "select * from dbo." + tableName;
                reader = cmd.ExecuteReader();
                int counter = 0;
                while (reader.Read())
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        dict.Add(reader.GetName(i), reader[i].ToString());
                    data.Add(counter, dict);
                    counter++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("При запросе данных из БД выпала ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                Close();
            }
            return data;
        }

        /// <summary>
        /// Выводит в таблицу все записи выбранной таблицы
        /// </summary>
        /// <param name="gender"></param>
        /// <param name="view"></param>
        public void PrintDataFromCasual(Dictionary<int, Dictionary<string, string>> data, DataGridView view)
        {
            List<string> keys = new List<string>();
            foreach (var key in data[0].Keys)
                keys.Add(key);
            if (view.Rows[0].Cells[0].Value == null)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < keys.Count; j++)
                    {
                        if (keys[j] == "date_expense")
                        {
                            string date = data[i]["date_expense"];
                            int idx = date.IndexOf(' ');
                            view.Rows[i].Cells[j].Value = date.Remove(idx);

                        }
                        else
                            view.Rows[i].Cells[j].Value = data[i][keys[j]];
                    }
                }
            }
        }

        private void Open()
        {
            connection.Open();
            cmd = connection.CreateCommand();
        }

        private void Close()
        {
            cmd = null;
            connection.Close();
        }
    }
}

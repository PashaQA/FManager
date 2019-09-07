using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace Libs
{
    public class DB
    {
        public const string _he = "He";
        public const string _hebig = "HeBig";
        public const string _hegifts = "HeGifts";
        public const string _hecar = "HeCar";
        public const string _she = "She";
        public const string _shebig = "SheBig";

        public enum Tables
        {
            She,
            SheBig,
            He,
            HeGifts,
            HeBig,
            HeCar,
            AbstractCASUAL,
            AbstractBIG
        }

    }

    public class DB_mssql
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

        public DB_mssql()
        {
            connection = new SqlConnection(connectionDB);
        }

        /// <summary>
        /// Создает выбранную таблицу
        /// </summary>
        public void CreateTable(DB.Tables table)
        {
            Open();
            if (table == DB.Tables.He)
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
            if (table == DB.Tables.She)
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
            if (table == DB.Tables.HeGifts)
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
            if (table == DB.Tables.HeBig)
            {
                cmd.CommandText += "create table HeBig(" +
                                     "id integer primary key identity, " +
                                     "date_expense date not null," +
                                     "description nvarchar(255) not null," +
                                     "expenses nvarchar(100) not null," +
                                       "type nvarchar(3) not null," +
                                       "full_line nvarchar(100));\n";
            }
            if (table == DB.Tables.SheBig)
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
        public void DeleteTables(DB.Tables table)
        {
            try
            {
                string nameTable = string.Empty;
                if (table == DB.Tables.He)
                    nameTable = "He";
                if (table == DB.Tables.HeBig)
                    nameTable = "HeBig";
                if (table == DB.Tables.HeGifts)
                    nameTable = "HeGifts";
                if (table == DB.Tables.She)
                    nameTable = "She";
                if (table == DB.Tables.SheBig)
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
        public void InsertIntoCasual(DB.Tables table, string date_expense, string event_type,
            string count, string count_expenses, string description, string type, string full_line)
        {
            try
            {
                Open();
                string nameTable = "";
                if (table == DB.Tables.He)
                    nameTable = "He";
                else if (table == DB.Tables.She)
                    nameTable = "She";
                else
                {
                    MessageBox.Show("Выбранная таблциа не поддерживется для записи из этой функции.", "Ошибкочка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cmd.CommandText = string.Format("insert into dbo.'" + nameTable +
                    "'(date_expense, event_type, count, count_expenses, description, type, full_line) values('{0}', N'{1}', {2}, N'{3}', N'{4}', N'{5}', N'{6}')",
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
        public void InsertIntoBigTables(DB.Tables table, string date_expenses, string description, string expenses, string type, string param, string full_line)
        {
            try
            {
                Open();
                string nameTable = "";
                if (table == DB.Tables.HeBig)
                    nameTable = "HeBig";
                else if (table == DB.Tables.HeGifts)
                    nameTable = "HeGifts";
                else if (table == DB.Tables.SheBig)
                    nameTable = "SheBig";
                else
                {
                    MessageBox.Show("Выбранная табоица не поддерживается для записи из этой функции.", "Ошибочка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (table == DB.Tables.HeBig || table == DB.Tables.SheBig)
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
        public void ClearTable(DB.Tables table)
        {
            string nameTable = string.Empty;
            if (table == DB.Tables.She)
                nameTable = "She";
            if (table == DB.Tables.He)
                nameTable = "He";
            if (table == DB.Tables.HeGifts)
                nameTable = "HeGifts";
            if (table == DB.Tables.HeBig)
                nameTable = "HeBig";
            if (table == DB.Tables.SheBig)
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
        public List<DB.Tables> GetExistsTables()
        {
            List<string> names = new List<string>();
            List<DB.Tables> tables = new List<DB.Tables>();
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
                    tables.Add(DB.Tables.He);
                if (names[i] == "HeBig")
                    tables.Add(DB.Tables.HeBig);
                if (names[i] == "HeGifts")
                    tables.Add(DB.Tables.HeGifts);
                if (names[i] == "She")
                    tables.Add(DB.Tables.She);
                if (names[i] == "SheBig")
                    tables.Add(DB.Tables.SheBig);
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
        public Dictionary<int, Dictionary<string, string>> GetData(DB.Tables table)
        {
            Dictionary<int, Dictionary<string, string>> data = new Dictionary<int, Dictionary<string, string>>();
            try
            {
                Open();
                string tableName = "";
                if (table == DB.Tables.He)
                    tableName = "He";
                else if (table == DB.Tables.She)
                    tableName = "She";
                else if (table == DB.Tables.HeGifts)
                    tableName = "HeGifts";
                else if (table == DB.Tables.HeBig)
                    tableName = "HeBig";
                else if (table == DB.Tables.SheBig)
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

    public class DB_sqlite
    {
        private Assistant assistant = new Assistant();
        private SQLiteConnection connection;
        private SQLiteCommand cmd;
        private SQLiteDataReader reader;

        public DB_sqlite()
        {
            if (!File.Exists(assistant.locationSQLite)) SQLiteConnection.CreateFile(assistant.locationSQLite);
            connection = new SQLiteConnection("Data Source=" + assistant.locationSQLite);
        }

        /// <summary>
        /// Создает выбранную таблицу
        /// </summary>
        public void CreateTable(DB.Tables table)
        {
            Open();
            if (table == DB.Tables.He)
            {
                cmd.CommandText = string.Format("create table {0}(" +
                                    "id integer primary key autoincrement, " +
                                     "date_expense date not null," +
                                     "event_type nvarchar(2) not null, " +
                                     "count int not null, " +
                                     "count_expenses int, " +
                                     "description nvarchar(255) default null," +
                                     "type nvarchar(3) default null, " +
                                     "full_line nvarchar(255) not null);\n", DB._he);
            }
            if (table == DB.Tables.She)
            {
                cmd.CommandText += string.Format("create table {0}(" +
                                    "id integer primary key autoincrement, " +
                                     "date_expense date not null," +
                                     "event_type nvarchar(2) not null, " +
                                     "count int not null, " +
                                      "count_expenses int, " +
                                     "description nvarchar(255) default null," +
                                     "type nvarchar(3) default null, " +
                                     "full_line nvarchar(255) not null);\n", DB._she);
            }
            if (table == DB.Tables.HeGifts)
            {
                cmd.CommandText += string.Format("create table {0}(" +
                                     "id integer primary key autoincrement, " +
                                     "date_expense date not null," +
                                     "description nvarchar(255) not null," +
                                      "expenses nvarchar(100) not null," +
                                       "type nvarchar(3) not null," +
                                       "param nvarchar(1)," +
                                       "full_line nvarchar(100));\n", DB._hegifts);
            }
            if (table == DB.Tables.HeBig)
            {
                cmd.CommandText += string.Format("create table {0}(" +
                                     "id integer primary key autoincrement, " +
                                     "date_expense date not null," +
                                     "description nvarchar(255) not null," +
                                     "expenses nvarchar(100) not null," +
                                       "type nvarchar(3) not null," +
                                       "full_line nvarchar(100));\n", DB._hebig);
            }
            if (table == DB.Tables.HeCar)
            {
                cmd.CommandText = string.Format("create table {0}(" +
                                    "id integer primary key autoincrement, " +
                                     "date_expense date not null," +
                                     "event_type nvarchar(2) not null, " +
                                     "count real not null, " +
                                     "count_expenses int, " +
                                     "description nvarchar(255) default null," +
                                     "type nvarchar(3) default null, " +
                                     "full_line nvarchar(255) not null);\n", DB._hecar);
            }
            if (table == DB.Tables.SheBig)
            {
                cmd.CommandText += string.Format("create table {0}" +
                                     "id integer primary key autoincrement, " +
                                     "date_expense date not null," +
                                     "description nvarchar(255) not null," +
                                     "expenses nvarchar(100) not null," +
                                       "type nvarchar(3) not null," +
                                       "full_line nvarchar(100));\n", DB._shebig);
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
        public void DeleteTables(DB.Tables table)
        {
            try
            {
                string nameTable = string.Empty;
                if (table == DB.Tables.He)
                    nameTable = DB._he;
                if (table == DB.Tables.HeBig)
                    nameTable = DB._hebig;
                if (table == DB.Tables.HeGifts)
                    nameTable = DB._hegifts;
                if (table == DB.Tables.HeCar)
                    nameTable = DB._hecar;
                if (table == DB.Tables.She)
                    nameTable = DB._she;
                if (table == DB.Tables.SheBig)
                    nameTable = DB._shebig;
                Open();
                cmd.CommandText = "drop table " + nameTable;
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
        public void InsertIntoCasual(DB.Tables table, string date_expense, string event_type,
            string count, string count_expenses, string description, string type, string full_line)
        {
            if (count_expenses == "") count_expenses = "1";
            try
            {
                Open();
                string nameTable = "";
                if (table == DB.Tables.He)
                    nameTable = DB._he;
                else if (table == DB.Tables.She)
                    nameTable = DB._she;
                else if (table == DB.Tables.HeCar)
                    nameTable = DB._hecar;
                else
                {
                    MessageBox.Show("Выбранная таблциа не поддерживется для записи из этой функции.", "Ошибкочка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cmd.CommandText = string.Format("insert into {7} (date_expense, event_type, count, count_expenses, description, type, full_line) values('{0}', '{1}', {2}, {3}, '{4}', '{5}', '{6}')",
                    date_expense, event_type, count, count_expenses, description, type, full_line, nameTable);
                //cmd.CommandText = "insert into she(date_expense, event_type, count, count_expenses, description, type, full_line) values('2016-01-05', '-', 10, 2, 'asda', 'н', 'dsfsdfsfs');";
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
        public void InsertIntoBigTables(DB.Tables table, string date_expenses, string description, string expenses, string type, string param, string full_line)
        {
            try
            {
                Open();
                string nameTable = "";
                if (table == DB.Tables.HeBig)
                    nameTable = DB._hebig;
                else if (table == DB.Tables.HeGifts)
                    nameTable = DB._hegifts;
                else if (table == DB.Tables.SheBig)
                    nameTable = DB._shebig;
                else
                {
                    MessageBox.Show("Выбранная таблица не поддерживается для записи из этой функции.", "Ошибочка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (table != DB.Tables.HeGifts)
                    cmd.CommandText += string.Format("insert into {5}(date_expense, description, expenses, type, full_line)" +
                    " values ('{0}', '{1}', '{2}', '{3}', '{4}')",
                    date_expenses, description, expenses, type, full_line, nameTable);
                else
                    cmd.CommandText += string.Format("insert into {6}(date_expense, description, expenses, type, param, full_line)" +
                        " values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                        date_expenses, description, expenses, type, param, full_line, nameTable);
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
        public void ClearTable(DB.Tables table)
        {
            string nameTable = string.Empty;
            if (table == DB.Tables.She)
                nameTable = DB._she;
            if (table == DB.Tables.He)
                nameTable = DB._he;
            if (table == DB.Tables.HeGifts)
                nameTable = DB._hegifts;
            if (table == DB.Tables.HeBig)
                nameTable = DB._hebig;
            if (table == DB.Tables.HeCar)
                nameTable = DB._hecar;
            if (table == DB.Tables.SheBig)
                nameTable = DB._shebig;
            try
            {
                Open();
                cmd.CommandText = "delete from " + nameTable;
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
        public List<DB.Tables> GetExistsTables()
        {
            List<string> names = new List<string>();
            List<DB.Tables> tables = new List<DB.Tables>();
            try
            {
                Open();
                cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;";
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
                if (names[i] == DB._he)
                    tables.Add(DB.Tables.He);
                if (names[i] == DB._hebig)
                    tables.Add(DB.Tables.HeBig);
                if (names[i] == DB._hegifts)
                    tables.Add(DB.Tables.HeGifts);
                if (names[i] == DB._hecar)
                    tables.Add(DB.Tables.HeCar);
                if (names[i] == DB._she)
                    tables.Add(DB.Tables.She);
                if (names[i] == DB._shebig)
                    tables.Add(DB.Tables.SheBig);
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
        public Dictionary<int, Dictionary<string, string>> GetData(DB.Tables table)
        {
            Dictionary<int, Dictionary<string, string>> data = new Dictionary<int, Dictionary<string, string>>();
            try
            {
                Open();
                string tableName = "";
                if (table == DB.Tables.He)
                    tableName = DB._he;
                else if (table == DB.Tables.She)
                    tableName = DB._she;
                else if (table == DB.Tables.HeGifts)
                    tableName = DB._hegifts;
                else if (table == DB.Tables.HeBig)
                    tableName = DB._hebig;
                else if (table == DB.Tables.HeCar)
                    tableName = DB._hecar;
                else if (table == DB.Tables.SheBig)
                    tableName = DB._shebig;
                cmd.CommandText = "select * from " + tableName + ";";
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

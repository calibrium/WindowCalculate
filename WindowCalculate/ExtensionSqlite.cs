using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Microsoft.Data.Sqlite;
using System.ComponentModel;
using System.Security.Policy;

namespace WindowCalculate
{
    static partial class SqliteCommand
    {
        readonly static string _path;
        static SqliteCommand()
        {
            Path path = new Path();
            _path = path.GetPath() + "\\userdata.db";
        }
        public enum KeyDictSqlEnum
        {
            create_table,
            exist_table,
            add_day,
            find_pastday,
            array_all_day
        }
        
        private static string GetValueByKey(KeyDictSqlEnum key)
        {
            DataTimeTable dataNow = new DataTimeTable();
            Dictionary<KeyDictSqlEnum, string> command_sql = new Dictionary<KeyDictSqlEnum, string>()
            {
                {KeyDictSqlEnum.create_table, $"CREATE TABLE IF NOT EXISTS '{dataNow.Mounth_year}'(Day INTEGER, Transfer TEXT, Expenses TEXT, Rest TEXT); PRAGMA journal_mode = WAL" },
                {KeyDictSqlEnum.exist_table, $"SELECT name FROM sqlite_master WHERE type='table' AND name='{dataNow.Mounth_year}'" },
                {KeyDictSqlEnum.add_day, $"INSERT INTO {dataNow.Mounth_year} (Day) VALUES ('{dataNow.CurrentDay}')" },
                {KeyDictSqlEnum.find_pastday, $"SELECT MAX(Day) FROM {dataNow.Mounth_year}" },
                {KeyDictSqlEnum.array_all_day, $"SELECT Day, Transfer, Expenses FROM {dataNow.Mounth_year}" },

            };
            return command_sql[key];
        }
        private static string GetValueByKeyUser(string key, string money = null)
        {
            Dictionary<string, string> command_sql = new Dictionary<string, string>()
            {
                {"create_user", $"CREATE TABLE IF NOT EXISTS 'UserInfo'(Name TEXT, CountMoney TEXT, MoneyModel TEXT); PRAGMA journal_mode = WAL" },
                {"exist_table_info", $"SELECT name FROM sqlite_master WHERE type='table' AND name='UserInfo'" },
                {"update_count_money", $"UPDATE UserInfo SET CountMoney = '{money}'" },
                {"select_count_money", $"SELECT CountMoney FROM UserInfo" },
                {"add_count_money", $"INSERT INTO UserInfo (CountMoney) VALUES ('{money}')"}
            };
            return command_sql[key];
        }
        private static void ExecuteCommand(string sql)
        {
            using (var connection = new SQLiteConnection("Data Source=" + _path + "; FailIfMissing=false"))
            {
                connection.Open();
                SQLiteCommand commandexp = new SQLiteCommand(sql, connection);
                commandexp.ExecuteNonQuery();
                connection.Close();
            }
        }
        private static string ExecuteCommand(string sql, bool returnStringValue = true)
        {
            using (var connection = new SQLiteConnection("Data Source=" + _path + "; FailIfMissing=false"))
            {
                connection.Open();
               /*var t = connection.CreateCommand();
                t.CommandText = "PRAGMA synchronous = 1";*/
                SQLiteCommand commandexp = new SQLiteCommand(sql, connection);
                string max_day = commandexp.ExecuteScalar()?.ToString();
                connection.Close();
                return max_day;
            }
        }
        public static BindingList<TodoModel> ExecuteCommand(string sql, ref BindingList<TodoModel> returnListValue, string rest = "")
        {
            using (var connection = new SQLiteConnection("Data Source=" + _path + "; FailIfMissing=false"))
            {
                connection.Open();
                SQLiteCommand commandexp = new SQLiteCommand(sql, connection);
                using (SQLiteDataReader reader = commandexp.ExecuteReader())
                {
                    while (reader.Read())
                    {
                         TodoModel returnColumnValue = new TodoModel
                         {
                             Day = reader["Day"].ToString(),
                             Transfer = reader["Transfer"].ToString(),
                             Spend = reader["Expenses"].ToString(),
                         };
                         returnListValue.Add(returnColumnValue);
                        
                        //Возможно нужно использовать класс, а не лист...
                        
                        
                    }
                }
                connection.Close();
                return returnListValue;
            }
        }

    }
    static partial class SqliteCommand
    {
        
        public static void GetCommandSqlVoid(KeyDictSqlEnum keyDict)
        {
            //Добавить проверки значений
            ExecuteCommand(GetValueByKey(keyDict));
        }
        public static string GetCommandSqlString(KeyDictSqlEnum keyDict)
        {
            //Добавить проверки значений
            return ExecuteCommand(GetValueByKey(keyDict), returnStringValue: true);
        }
        public static BindingList<TodoModel> GetCommandSqlList(KeyDictSqlEnum keyDict, ref BindingList<TodoModel> returnListValue, string rest ="")
        {
            //Добавить проверки значений
            return ExecuteCommand(GetValueByKey(keyDict), ref returnListValue, rest);
        }
        public static void GetManyCommandSqlVoid(KeyDictSqlEnum keyDict, int primary_count_iterator = 1)
        {
            DataTimeTable dataNow = new DataTimeTable();
            int last_day_table = dataNow.CurrentDay;
            StringBuilder many_request = new StringBuilder();
            for (int i = primary_count_iterator; i <= last_day_table; i++)
            {
                //по другому нужно организовать
                //?DataTimeTable.Add_day = i;
                many_request.Append(GetValueByKey(keyDict));
                many_request.Append("; ");
            }
            ExecuteCommand(many_request.ToString());
        }
        public static void SaveData(string dataCells, string field, int dataDay)
        {
            DataTimeTable dataNow = new DataTimeTable();
            ExecuteCommand($"UPDATE {dataNow.Mounth_year} SET {field} = '{dataCells}' WHERE Day = '{dataDay.ToString()}'");
        }

    }
    static partial class SqliteCommand
    {
        public static void GetCommandSqlVoidUser()
        {
            ExecuteCommand(GetValueByKeyUser("create_user"));
        }
        public static void GetExistsTable()
        {
            ExecuteCommand(GetValueByKeyUser("exist_table_info"));
        }
        public static void AddCountMoney(string money)
        {
            if(ExecuteCommand(GetValueByKeyUser("select_count_money"), returnStringValue: true) == null)
            {
                ExecuteCommand(GetValueByKeyUser("add_count_money", money));
                return;
            }
            ExecuteCommand(GetValueByKeyUser("update_count_money", money));
        }
        public static void UserGetManyCommandSqlVoid(string money, int primary_count_iterator = 1)
        {
            //Не забудь обработать, если нет!! Иначе не хватит строк
            DataTimeTable dataNow = new DataTimeTable();
            int last_day_table = dataNow.CurrentDay;
            StringBuilder many_request = new StringBuilder();
            for (int i = primary_count_iterator; i <= last_day_table; i++)
            {
                many_request.Append($"UPDATE {dataNow.Mounth_year} Set Rest = '{money}'");
                many_request.Append("; ");
            }
            ExecuteCommand(many_request.ToString());
        }
    }
}

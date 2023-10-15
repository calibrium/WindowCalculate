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
    class ManageSqlCommand
    {
        readonly static string _path;
        static ManageSqlCommand()
        {
            Path path = new Path();
            _path = path.GetPath() + "\\userdata.db";
        }
        /// <summary>
        /// Создаёт таблицу, если не существует
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="keyValuePairs">Ключ создает название столбца, значение тип столбца</param>
        public void CreateTableNE(string tableName, Dictionary<string, string> keyValuePairs)
        {
            StringBuilder allField = new StringBuilder();
            allField.Append($"CREATE TABLE IF NOT EXISTS '{tableName}'(");
            foreach (var item in keyValuePairs.Keys)
            {
                if (keyValuePairs.Keys.Last() == item)
                {
                    allField.Append($"{item} {keyValuePairs[item]}); ");
                    break;
                };
                allField.Append($"{item} {keyValuePairs[item]}, ");
            }
            allField.Append($"PRAGMA journal_mode = WAL");
            Execute(allField.ToString());
        }
        /// <summary>
        /// Добавляет в ячейку таблицы значение
        /// </summary>
        /// <param name="field">Поле, где изменяем</param>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="addValue">Добавить значение</param>
        public void InsertInto(string field, string tableName, string addValue)
        {
            string sql = CreateInsert(field, tableName, addValue).ToString();
            Execute(sql);
        }
        public void InsertIntoManyDay(string field, string tableName, int beginDay, int currentDay)
        {
            StringBuilder many_request = new StringBuilder();
            for (int i = beginDay; i <= currentDay; i++)
            {
                //по другому нужно организовать
                //?DataTimeTable.Add_day = i;
                many_request.Append(CreateInsert(field, tableName, i.ToString()));
                many_request.Append("; ");
            }
            Execute(many_request.ToString());
        }
        // потом переделать данный метод, по-другому всё будет. Это трата на день
        public void UpdateManyDay(string money, int beginDay = 1)
        {
            //Не забудь обработать, если нет!! Иначе не хватит строк
            DataTimeTable dataNow = new DataTimeTable();
            int last_day_table = dataNow.CurrentDay;
            StringBuilder many_request = new StringBuilder();
            for (int i = beginDay; i <= last_day_table; i++)
            {
                many_request.Append($"UPDATE {dataNow.Mounth_year} Set Rest = '{money}'");
                many_request.Append("; ");
            }
            Execute(many_request.ToString());
        }
        private StringBuilder CreateInsert(string field, string tableName, string addValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append($"INSERT INTO { tableName} ({field}) VALUES('{addValue}')");
            return sqlCommand;
        }
        /// <summary>
        /// Общий вид: "SELECT [[MAX(***)]] FROM ***"
        /// Вернуть максимум или значение из столбца
        /// </summary>
        /// <param name="fields">Из какого столбца вернуть  значение</param>
        /// <param name="whereTable">Из какой таблицы вернуть значение</param>
        /// <returns></returns>
        public string Select(string field, string tableName, bool maxValue)
        {
            string sql = null;
            if (maxValue)
            {
                sql = $"SELECT MAX({field}) FROM {tableName}";
            }
            else
            {
                sql = $"SELECT {field} FROM {tableName}";
            }
            List<Temp> dataTemp = SelectAndReturn(sql, returnManyValue: false);
            return dataTemp[0].MaxDay;
        }
        /// <summary>
        /// Общий вид: "SELECT *** FROM ***"
        /// Вернуть много значений
        /// </summary>
        /// <param name="fields">Имена полей</param>
        /// <param name="whereTable">Откуда извлечь данные</param>
        /// <returns></returns>
        public BindingList<TodoModel> Select(List<string> fields, string tableName, ref BindingList<TodoModel> dataTableWpf)
        {
            StringBuilder allField = new StringBuilder();
            allField.Append("SELECT ");
            foreach (var item in fields)
            {
                if (fields.Last() == item)
                {
                    allField.Append($"{item} ");
                    break;
                };
                allField.Append($"{item}, ");
            }
            allField.Append($"FROM {tableName}");
            List<Temp> dataTemp = SelectAndReturn(allField.ToString(), returnManyValue: true);
            byte i = 0;
            foreach (var item in dataTemp)
            {
                TodoModel addDataCells = new TodoModel
                {
                    Day = dataTemp[i].Day,
                    Transfer = dataTemp[i].Transfer,
                    Spend = dataTemp[i].Spend
                };
                i++;
                dataTableWpf.Add(addDataCells);
            }
            return dataTableWpf;
        }
        private class Temp
        {
            public string Day { get; set; }
            public string Transfer { get; set; }
            public string Spend { get; set; }
            public string MaxDay { get; set; }
        }
        private List<Temp> SelectAndReturn(string sql, bool returnManyValue)
        {
            List<Temp> tempStorage = new List<Temp>();
            using (var connection = new SQLiteConnection("Data Source=" + _path + "; FailIfMissing=false"))
            {
                connection.Open();
                SQLiteCommand commandexp = new SQLiteCommand(sql, connection);
                if (returnManyValue)
                {
                    using (SQLiteDataReader reader = commandexp.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Temp _returnColumnValue = new Temp
                            {
                                Day = reader["Day"].ToString(),
                                Transfer = reader["Transfer"].ToString(),
                                Spend = reader["Expenses"].ToString()
                            };
                            tempStorage.Add(_returnColumnValue);
                        }
                    }
                    connection.Close();
                    return tempStorage;
                }
                Temp returnColumnValue = new Temp();
                returnColumnValue.MaxDay = commandexp.ExecuteScalar()?.ToString();
                tempStorage.Add(returnColumnValue);
                connection.Close();
                return tempStorage;
            }
        }
        private void Execute(string sql)
        {
            using (var connection = new SQLiteConnection("Data Source=" + _path + "; FailIfMissing=false"))
            {
                connection.Open();
                SQLiteCommand commandexp = new SQLiteCommand(sql, connection);
                commandexp.ExecuteNonQuery();
                connection.Close();
            }
        }
        /// <summary>
        /// Общий вид: "UPDATE *** SET *** = *** WHERE"
        /// </summary>
        /// <param name="field"></param>
        /// <param name="tableName"></param>
        /// <param name="value"></param>
        /// <param name="dataDay"></param>
        public void Update(string field, string tableName, string value, int dataDay)
        {
            DataTimeTable dataNow = new DataTimeTable();
            Execute($"UPDATE {tableName} SET {field} = '{value}' WHERE Day = '{dataDay.ToString()}'");
        }
    }
}

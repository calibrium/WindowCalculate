using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowCalculate
{
    class CalculateDay
    {
        private bool _existsCurrentDay = false;
        public CalculateDay()
        {
            GeneratedLineTable();
        }
        private void GeneratedLineTable()
        {
            DataTimeTable dataNow = new DataTimeTable();
            ManageSqlCommand sqlCommand = new ManageSqlCommand();
            Dictionary<string, string> columnName = new Dictionary<string, string>
            {
                { "Day", "INTEGER" },
                { "Transfer", "TEXT" },
                { "Expenses", "TEXT" },
                { "Rest", "TEXT" }
            };
            sqlCommand.CreateTableNE(dataNow.Mounth_year, columnName);
            Dictionary<string, string> columnNameUser = new Dictionary<string, string>
            {
                { "Name", "TEXT" },
                { "CountMoney", "TEXT" },
                { "MoneyModel", "TEXT" }
            };
            sqlCommand.CreateTableNE("UserInfo", columnNameUser);

            string lastDayInTable = sqlCommand.Select("Day", dataNow.Mounth_year, true);
            if (lastDayInTable == "")
            {
                //выводится окно, хотите ли заполнить полностью или нет?
                AddsInColumnDays();
            }
            else if (lastDayInTable != null & Int32.Parse(lastDayInTable) != dataNow.CurrentDay)
            {
                int dayCurrentOrNo = Convert.ToInt32(lastDayInTable) + 1;
                if (dayCurrentOrNo == dataNow.CurrentDay)
                {
                    _existsCurrentDay = true;
                }
                AddsInColumnDays(beginDay: Int32.Parse(lastDayInTable) + 1);
            }
            else if (Int32.Parse(lastDayInTable) == dataNow.CurrentDay)
            {
                ///Это что будет дальше
                return;
            }
            else
            {
                /// Ошибка должна быть сделана
            }
        }
        //объединить потом нужно, нет в нём какого-то смысла или функциональности
        private void AddsInColumnDays(int beginDay = 1)
        {
            //выводится окно, хотите ли заполнить полностью или нет?
            /// Ошибка должна быть сделана
            DataTimeTable dataNow = new DataTimeTable();
            ManageSqlCommand sqlCommand = new ManageSqlCommand();
            bool uWantAddAllDay = false;
            if (_existsCurrentDay == false)
            {
                uWantAddAllDay = MainKekw.InfoForAutoEnter();
            }
            switch (uWantAddAllDay)
            {
                case true:
                    sqlCommand.InsertIntoManyDay("Day", dataNow.Mounth_year, beginDay, dataNow.CurrentDay);
                    break;
                case false:
                    sqlCommand.InsertInto("Day", dataNow.Mounth_year, dataNow.CurrentDay.ToString());
                    break;
            }
        }

    }
    
}
    

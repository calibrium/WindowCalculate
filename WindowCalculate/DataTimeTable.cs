using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowCalculate
{
    class DataTimeTable
    {
        //Задание. Добавить проверку времени, чтобы добавить день в случае чего
        public string Mounth_year { private set; get; }
        public int CurrentDay { private set; get; }
        public int CountDayMounth { private set; get; }
        public DataTimeTable()
        {
            CurrentDay = DateTime.Now.Day;
            string abbrevMounth = CultureInfo.GetCultureInfo("en-en")
                .DateTimeFormat.GetAbbreviatedMonthName(DateTime.Now.Month);
            Mounth_year = string.Concat(abbrevMounth, DateTime.Now.Year.ToString());
            CountDayMounth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        }
    }
}

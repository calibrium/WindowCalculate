using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace WindowCalculate
{
    class Path
    {
        public string GetPath()
        {
            string path = Environment.CurrentDirectory + "\\SqliteData";
            if (File.Exists(path) == false)
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {

                    throw new Exception();
                }
            }
            return path;
        }
    }
}

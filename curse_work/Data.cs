using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace curse_work
{
    public class Data
    {
        public static List<Vakans> VakansList = new List<Vakans>();
        public static List<Rezume> RezumeList = new List<Rezume>();
        public DataTable table = new DataTable();
        
        public void filltable()
        {
            table.Columns.Add("Вакансия", typeof(string));
            table.Columns.Add("Сфера", typeof(string));
            table.Columns.Add("Работодатель", typeof(string));
            table.Columns.Add("Опыт", typeof(string));
            table.Columns.Add("Зарплата", typeof(string));

            table.Rows.Add("1", "2", "3", "4", "5");
            table.Rows.Add("6", "7", "8", "9", "10");
            table.DefaultView.Sort = "Вакансия";
        }
    }
}

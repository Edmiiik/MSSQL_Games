using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_MSSQL_Procvicovani
{
    internal class Games
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public double Price { get; set; }
        public int Rating { get; set; }
        public int Id_Studio { get; set; }

    }
}

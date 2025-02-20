using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_MSSQL_Procvicovani
{
    internal class Studio
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int Employees { get; set; }
        public string Reputation { get; set; }  

    }
}

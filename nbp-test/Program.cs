using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nbp_aerodrom_model;
using nbp_aerodrom_model.Data_Providers;

namespace nbp_test
{
    class Program
    {
        static void Main(string[] args)
        {
            UserDataProvider.LogInUser("nesto", "nesto");
        }
    }
}

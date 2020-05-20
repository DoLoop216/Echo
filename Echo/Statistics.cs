using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Echo
{
    public class Statistics
    {
        public static int GetNumberOfVisits(string WebRoot)
        {
            if (File.Exists(WebRoot + "/Visits"))
            {
                string _visits = System.IO.File.ReadAllText(WebRoot + "/Visits");


                if (!string.IsNullOrWhiteSpace(_visits))
                    return Convert.ToInt32(_visits);
            }
            return 0;
        }
    }
}

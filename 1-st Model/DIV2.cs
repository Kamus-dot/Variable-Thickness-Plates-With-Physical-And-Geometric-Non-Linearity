using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public static double PLAST(double x)
        {
            // Физически линейная
            if (JF == 0)
            {
                x = 3 * x;
                return x;
            }
            if (x >= ES)
            {
                return 3 * ES + 3 * G1 * (x - ES);
            }
            return 3 * x;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public double PLAST(double x)
        {
            // Физически линейная задача
            if (JF == 0)
            {
                return 3 * x;
            }
            if (x >= ES)
            {
                return 3 * ES + 3 * G1 * (x - ES);
            }
            return 3 * x;
        }
    }
}

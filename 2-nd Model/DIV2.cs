using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _2_nd_Model
{
    public partial class ModelTwo
    {
        public static double PLAST(double x)
        {
            if (JF == 0)
            {
                x = 3.0 * x;
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

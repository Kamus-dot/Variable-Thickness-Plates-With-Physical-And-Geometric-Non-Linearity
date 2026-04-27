using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        // Суть здесь в том, что рассматривается исключительно 1-ая четверть
        // пластины, поэтому получается, что отверстие находится в верхнем 
        // правом углу
        public static double QR(int I, int J, int K)
        {
            if (I >= N - K && J >= N - K) return 1;
            return 0;
        }
    }
}

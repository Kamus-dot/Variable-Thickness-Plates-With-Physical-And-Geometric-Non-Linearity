using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_nd_Model
{
    public partial class ModelTwo
    {
        public static double QR(int I, int J, int K)
        {
            if (I >= N + 1 - K && J >= N + 1 - K) return 1;
            return 0;
        }
    }
}

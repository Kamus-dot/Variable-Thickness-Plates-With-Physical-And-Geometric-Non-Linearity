using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_nd_Model
{
    public partial class ModelTwo
    {
        public static double SIM(double[] Y, double A, int N)
        {
            double S = (Y[0] - Y[N]) / 2;
            for (int I = 1; I < N; I += 2)
            {
                S += S + 2 * Y[I] + Y[I + 1];
            }
            return 2 * A * S / (3 * N);
        }
    }
}

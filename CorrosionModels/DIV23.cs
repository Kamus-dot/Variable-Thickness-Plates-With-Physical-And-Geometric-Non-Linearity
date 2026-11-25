using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        public static double PROGIB(double[,] H, int N)
        {
            int N1 = N + 1;
            double[] S1 = new double[N1];
            double[] S2 = new double[N1];
            double C = 1;
            for (int I = 0; I < N1; I++)
            {
                for (int J = 0; J < N1; J++)
                {
                    S1[J] = H[I, J];
                }
                S2[I] = Integrate(S1, C, N);
            }
            return Integrate(S2, C, N);
        }
    }
}

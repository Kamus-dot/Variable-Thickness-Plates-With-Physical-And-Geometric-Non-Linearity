using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public static double PROGIB(double[,] H, int N)
        {
            double[] S1 = new double[12];
            double[] S2 = new double[12];
            double C = 1;
            int M = N + 1;
            for (int I = 0; I < M; I++)
            {
                for (int J = 0; J < M; J++)
                {
                    S1[J] = H[I + 1, J + 1];
                }
                S2[I] = SIM(S1, C, N);
            }
            return SIM(S2, C, N);
        }
    }
}

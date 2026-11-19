using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        /// <summary>
        /// Объём пластины
        /// </summary>
        /// <param name="H">Матрица толщин</param>
        /// <param name="N">Размерность матрицы</param>
        /// <returns></returns>
        public static double Volume(double[,] H)
        {
            int M = H.GetLength(0);
            double[] S1 = new double[M];
            double[] S2 = new double[M];
            double C = 1;
            for (int I = 0; I < M; I++)
            {
                for (int J = 0; J < M; J++)
                {
                    S1[J] = H[I, J];
                }
                S2[I] = SIM(S1, C, M - 1);
            }
            return SIM(S2, C, M - 1);
        }
    }
}

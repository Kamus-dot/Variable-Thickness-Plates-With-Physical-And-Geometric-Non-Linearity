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
        /// Решение интеграла методом Симпсона
        /// </summary>
        /// <param name="Y">Матрица со значениями функций</param>
        /// <param name="A">Разность пределов интегрирования</param>
        /// <param name="N">Размерность матрицы Y (кол-во шагов на A)</param>
        /// <returns></returns>

        public static double SIM(double[] Y, double A, int N)
        {
            double S = (Y[0] - Y[N]) / 2;
            for (int I = 1; I < N; I += 2)
            {
                S += 2 * Y[I] + Y[I + 1];
            }
            return 2 * A * S / (3 * N);
        }
    }
}

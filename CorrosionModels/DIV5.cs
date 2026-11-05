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
        /// Коррекция Корнишина
        /// </summary>
        /// <param name="X">Матрица значений предыдущей итерации</param>
        /// <param name="Y">Матрица значений настоящей итерации</param>
        /// <param name="A">Параметр коррекции</param>
        /// <returns></returns>
        /// 
        public static double[,] Ytoch(double[,] X, double[,] Y, double A)
        {
            for (int i = 0; i < Y.GetLength(0); i++)
            {
                for (int j = 0; j < Y.GetLength(1); j++)
                {
                    Y[i, j] = X[i, j] + A * (Y[i, j] - X[i, j]);
                }
            }
            return Y;
        }
    }
}

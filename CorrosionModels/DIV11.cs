using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        /// <summary>
        /// Алгоритм решения СЛАУ методом исключения Гаусса
        /// </summary>
        /// <param name="A">Матрица коэффициентов, приведённая к ленточному виду</param>
        /// <param name="Q">Матрица значений</param>
        /// <param name="UNK">Результирующая матрица</param>

        public void SIMQ(double[,] A, double[] Q, ref double[,] UNK)
        {
            double[,] Out = new double[NM, N + 4];
            int NMN = NM * N;
            int NMN_1 = NMN - 1;
            int N_1 = N - 1;
            // Приводим ленточный массив к треугольному виду:
            for (int i = 0; i < NMN_1; i++)
            {
                int f = (int)i / N;
                for (int t = 0; t < NM - f; t++)
                {
                    int itn = i + t * N;
                    int itn2 = itn + 2;
                    int m = (t + f) * N;
                    int h = itn2;
                    if (t == 0) m = i + 1;
                    else if (i % N == 0) m = itn;
                    else if (i % N == 1) m = itn - 1;

                    if (f > 0) h = (f + t + 1) * N - 1;
                    else if (i % N == N - 2) h = itn + 1;
                    else if (i % N == N_1) h = itn;

                    for (int j = m; j <= h; j++)
                    {
                        if (A[j, i] == 0) continue;
                        double Ratio = A[j, i] / A[i, i];
                        Q[j] = Q[j] - Q[i] * Ratio;
                        for (int k = i; k < NMN; k++)
                        {
                            A[j, k] = A[j, k] - A[i, k] * Ratio;
                        }
                    }
                }
            }
            // Проверка выполнения:
            //for (int i = 1; i < NMN_1; i++)
            //{
            //    for (int j = 0; j < i; j++)
            //    {
            //        if (A[i, j] != 0) throw new Exception($"SIMQ exception at {i},{j}");
            //    }
            //}
            // Разрешаем полученную матрицу:
            int N1 = N + 1;
            int NM_1 = NM - 1;

            Out[NM_1, N1] = Q[NMN_1] / A[NMN_1, NMN_1];
            for (int i = NM_1; i >= 0; i--)
            {
                int IN = i * N;
                for (int j = N_1; j >= 0; j--)
                {
                    if (i == NM_1 && j == N_1) continue;
                    int IJ = IN + j;
                    double a = Q[IJ];
                    for (int k = j + 1; k <= N_1; k++)
                    {
                        a -= Out[i, k + 2] * A[IJ, IN + k];
                    }
                    Out[i, j + 2] = a / A[IJ, IJ];
                }
            }
            UNK = Out;
        }
    }
}

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
        /// Метод для определения размерности нагрузки
        /// </summary>
        /// <param name="ICON">Размерность приложенной нагрузки</param>
        public void TempQ(int ICON)
        {

            int N1 = N + 1;
            double[,] Q1 = new double[N1, N1];
            int N2 = N + 2;
            int K = ICON;

            for (int i = 0; i < N1; i++)
            {
                for (int j = 0; j < N1; j++)
                {
                    Q1[i, j] = QR(i, j, K) * Q;
                }
            }

            for (int i = 0; i < N; i++)
            {
                int I1 = i + 1;
                for (int j = 0; j < N; j++)
                {
                    int J1 = j + 1;
                    QTW[I1, J1] = -Q1[I1, J1];
                    QTF[I1, J1] = 0;
                }
            }

            for (int i = 0; i < N1; i++)
            {
                QTW[0, i] = -Q1[0, i];
                QTW[i, 0] = -Q1[i, 0];
            }
            return;
        }
        private double QR(int I, int J, int K)
        {
            if (I >= N - K && J >= N - K) return 1;
            return 0;
        }
    }
}

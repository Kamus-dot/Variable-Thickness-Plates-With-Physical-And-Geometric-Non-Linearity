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
        /// Метод для определения распределения поперечной нагрузки
        /// </summary>
        /// <param name="ICON">Размерность приложенной поперечной нагрузки</param>
        public virtual void TempQ(int ICON)
        {
            int N1 = N + 1;
            double[,] Q1 = new double[N1, N1];

            for (int i = 0; i < N1; i++)
            {
                for (int j = 0; j < N1; j++)
                {
                    Q1[i, j] = QR(i, j, ICON) * Q;
                }
            }

            for (int i = 0; i < N; i++)
            {
                int I1 = i + 1;
                for (int j = 0; j < N; j++)
                {
                    int J1 = j + 1;
                    QTW[I1, J1] = Q1[I1, J1];
                }
            }

            for (int i = 0; i < N1; i++)
            {
                QTW[0, i] = Q1[0, i];
                QTW[i, 0] = Q1[i, 0];
            }
            return;
        }
        /// <summary>
        /// Центральное распределение
        /// <para>(Используется для определения точек, к которым прилагается нагрузка)</para>
        /// </summary>
        /// <param name="I">Координата по x</param>
        /// <param name="J">Координата по y</param>
        /// <param name="K">Размерность нагрузки</param>
        /// <returns></returns>
        public virtual double QR(int I, int J, int K)
        {
            if (I >= N - K && J >= N - K) return 1;
            return 0;
        }
    }
}

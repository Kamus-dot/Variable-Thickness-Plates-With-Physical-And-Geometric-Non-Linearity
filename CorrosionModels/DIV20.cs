using System;
using static System.Math;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        /// <summary>
        /// Модель Гутмана
        /// </summary>
        /// <param name="t">Значение времени</param>
        public void GutmanThickness(double t)
        {
            // Полученные экспериментальные данные
            double Gamma = 0.00588;
            double Alpha = 0.48;
            double Theta = 0.091;

            int N1 = N + 1;

            for (int i = 0; i < N1; i++)
            {
                for (int j = 0; j < N1; j++)
                {
                    double Func = Alpha * Exp(Theta * t);
                    double Sigma = TENS_AVG[i, j];
                    double corr = Func * Exp(Gamma * Sigma) * Delta_t;
                    Corrosion[i + 1, j + 1] += corr;
                    H[i, j] -= corr;
                }
            }
            CorrosionExtrapolation();
        }
        private double GutmanThickness(double t, double Tens)
        {
            // Полученные экспериментальные данные
            double Gamma = 0.00588;
            double Alpha = 0.48;
            double Theta = 0.091;

            return Alpha * Exp(Theta * t) * Exp(Gamma * Tens) * Delta_t;
        }
    }
}

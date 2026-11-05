using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        /// <summary>
        /// Модель Долинского
        /// </summary>
        /// <param name="t">Значение времени</param>
        /// <param name="Def_Tens">Зависимость напряжения от деформации</param>
        public void DolinskyiThickness(double t, Func<double, double> Def_Tens)
        {
            // Полученные экспериментальные данные
            double A = 0.44;
            double B = 0.153;
            double Beta = 0.2;
            double K = 0.0089;

            int N1 = N + 1;

            for (int i = 0; i < N1; i++)
            {
                for (int j = 0; j < N1; j++)
                {
                    double Func = (A + B * Exp(Beta * t - 1)) / Exp(Beta * t);
                    double Sigma = Def_Tens(DEF_INT[i, j, NT]);
                    double corr = Func * (1 + K * Sigma) * Delta_t;
                    Corrosion[i + 1, j + 1] += corr;
                    H[i, j] -= corr;
                }
            }
            CorrosionExtrapolation();
        }
        private double DolinskyiThickness(double t, double Tens)
        {
            double A = 0.44;
            double B = 0.153;
            double Beta = 0.2;
            double K = 0.0089;

            double Func = (A + B * Exp(Beta * t - 1)) / Exp(Beta * t) * Delta_t;
            return Func * (1 + K * Tens);
        }
    }
}

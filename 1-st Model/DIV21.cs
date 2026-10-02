using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public void Thickness(int I, Func<double, double> Def_Tens)
        {
            // Модель Долинского

            // Полученные экспериментальные данные
            double A = 0.44;
            double B = 0.153;
            double Beta = 0.2;
            double K = 0.0089;

            int N2 = N + 2;
            double t = Delta_t * I;

            for (int i = 0; i < N2; i++)
            {
                for (int j = 0; j < N2; j++)
                {
                    double Func = (A + B * Exp(Beta * t - 1)) / Exp(Beta * t);
                    double Sigma = Def_Tens(DEF_INT[i, j]);
                    Corrosion[i, j] = Func * (1 + K * Sigma) * Delta_t;
                    H[i, j] -= Corrosion[i, j];
                }
            }
        }
    }
}

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
        // I - Номер шага
        public void Thickness(int I, Func<double, double> Def_Tens)
        {
            // Модель Долинского

            // Полученные экспериментальные данные
            double A = 0.44;
            double B = 0.153;
            double Beta = 0.2;
            double K = 0.0089;

            int N2 = N + 2;
            int N3 = N + 3;
            int N1 = N + 1;
            int N_1 = N - 1;
            double t = Delta_t * I;

            for (int i = 0; i < N1; i++)
            {
                for (int j = 0; j < N1; j++)
                {
                    double Func = (A + B * Exp(Beta * t - 1)) / Exp(Beta * t);
                    double Sigma = Def_Tens(DEF_INT[i, j, NT]);
                    Corrosion[i + 1, j + 1] = Func * (1 + K * Sigma) * Delta_t;
                    H[i, j] -= Corrosion[i + 1, j + 1];
                }
            }
            for (int i = 0; i < N3; i++)
            {
                Corrosion[i, N2] = Corrosion[i, N];
                Corrosion[N2, i] = Corrosion[N, i];
                Corrosion[N3, i] = Corrosion[N_1, i];
                Corrosion[i, N3] = Corrosion[i, N_1];
                Corrosion[0, i] = 2.0 * Corrosion[1, i] - Corrosion[2, i];
                Corrosion[i, 0] = 2.0 * Corrosion[i, 1] - Corrosion[i, 2];
            }
                Corrosion[0, 0] = 2.0 * Corrosion[1, 0] - Corrosion[2, 0];
                Corrosion[N3, 0] = Corrosion[N_1, 0];
                Corrosion[0, N3] = Corrosion[0, N_1];
        }
    }
}

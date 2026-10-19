using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Math;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public void DeformationIntesivity()
        {
            int N1 = N + 1;
            int N2 = N + 2;
            int N3 = N + 3;
            double[,] W = new double[N3, N3];
            double[,] F = new double[N3, N3];
            int NT1 = NT + 1;
            double V22 = Sqrt(V2);

            for (int i = 0; i < N1; i++)
            {
                int I1 = i + 1;
                for (int j = 0; j < N1; j++)
                {
                    W[I1, j + 1] = WXY[i, j];
                    F[I1, j + 1] = FXY[i, j];
                }
            }
            for (int i = 0; i < N3; i++)
            {
                W[0, i] = G[1] * W[2, i];
                W[N2, i] = W[N, i];
                W[i, 0] = G[0] * W[i, 2];
                W[i, N2] = W[i, N];
                F[0, i] = GF[1] * F[2, i];
                F[N2, i] = F[N, i];
                F[i, 0] = G[0] * F[i, 2];
                F[i, N2] = F[i, N];
            }
            W[0, 0] = G[0] * W[2, 0];
            W[N2, 0] = W[N, 0];
            W[0, N2] = W[0, N];
            F[0, 0] = GF[0] * F[2, 0];
            F[N2, 0] = F[N, 0];
            F[0, N2] = F[0, N];
            for (int I = 0; I < N1; I++)
            {
                int I1 = I + 1;
                int I2 = I + 2;

                for (int J = 0; J < N1; J++)
                {
                    int J1 = J + 1;
                    int J2 = J + 2;

                    double D2X = (W[I2, J1] - 2 * W[I1, J1] + W[I, J1]) / V2;
                    double D2Y = (W[I1, J2] - 2 * W[I1, J1] + W[I1, J]) / V2;
                    double D2XY = (W[I2, J2] - W[I2, J] - W[I, J2] + W[I, J]) / (4 * V2);

                    double D2PHIX = (Corrosion[I2, J1] - 2 * Corrosion[I1, J1] + Corrosion[I, J1]) / V2;
                    double D2PHIY = (Corrosion[I1, J2] - 2 * Corrosion[I1, J1] + Corrosion[I1, J]) / V2;
                    double D2PHIXY = (Corrosion[I2, J2] - Corrosion[I2, J] - Corrosion[I, J2] +
                        Corrosion[I, J]) / (4 * V2);

                    double D2FX = (F[I2, J1] - 2 * F[I1, J1] + F[I, J1]) / V2 + QL * QK;
                    double D2FY = (F[I1, J2] - 2 * F[I1, J1] + F[I1, J]) / V2 + QL;
                    double D2FXY = -(F[I2, J2] - F[I2, J] - F[I, J2] + F[I, J]) / (4 * V2);

                    double A_tilde = 1 / (Pow(A[I, J], 2) - Pow(A_nu[I, J], 2));

                    // Вычисление безразмерных деформаций

                    double EX_Memb = A_tilde * (V22 * A[I, J] * D2FX - A_nu[I, J] * D2FY);
                    double EY_Memb = A_tilde * (A[I, J] * D2FY - V22 * A_nu[I, J] * D2FX);
                    double EXY_Memb = Lambda * D2FXY / B[I, J];

                    // Толщина 1-ого слоя
                    double HZ = H[I, J] / NT;

                    // Начальная координата 
                    double Z = -H[I, J] / 2;

                    double EX = 0, EY = 0, EXY = 0;
                    for (int K = 0; K < NT1; K++)
                    {
                        // Деформация с учётом кривизны
                        EX = EX_Memb - Z * (D2X + 0.5 * D2PHIX);
                        EY = EY_Memb - Z * (D2Y + 0.5 * D2PHIY);
                        EXY = -2 * Z * (D2XY + 0.5 * D2PHIY) + EXY_Memb;

                        // Интенсивность деформаций
                        double EI = Sqrt(2 * (Pow((EX), 2) + Pow((EY), 2) + EX * EY
                            + Pow(EXY, 2) / 4)) / 3;
                        DEF_INT[I, J, K] = EI;

                        Z += HZ;
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Math;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        /// <summary>
        /// Метод для вычисления интенсивности деформации и перемещений
        /// </summary>
        /// <param name="flag">Вычислять перемещения (true - да, false - нет)</param>
        public void DeformationIntesivity(bool flag)
        {
            int N1 = N + 1;
            int N2 = N + 2;
            int N3 = N + 3;
            double[,] W = new double[N3, N3];
            double[,] F = new double[N3, N3];
            int NT1 = NT + 1;
            double Lambda2 = Pow(Lambda, 2);

            MaxEI = 0;
            HMaxEI = 0;

            for (int i = 0; i < N1; i++)
            {
                int I1 = i + 1;
                for (int j = 0; j < N1; j++)
                {
                    int J1 = j + 1;
                    W[I1, J1] = WXY[i, j];
                    F[I1, J1] = FXY[i, j];
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
                F[i, 0] = GF[0] * F[i, 2];
                F[i, N2] = F[i, N];
            }

            double[] MembX = new double[N1];
            double[] MembY = new double[N1];
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

                    double Nx = Lambda2 * ((F[I1, J2] - 2 * F[I1, J1] + F[I1, J]) / V2) - QL * QK;
                    double Ny = (F[I2, J1] - 2 * F[I1, J1] + F[I, J1]) / V2 - QL;
                    double D2FXY = -(F[I2, J2] - F[I2, J] - F[I, J2] + F[I, J]) / (4 * V2);

                    double A_tilde = 1 / (Pow(A[I1, J1], 2) - Pow(A_nu[I1, J1], 2));

                    // Вычисление безразмерных деформаций
                    double EX_Memb = A_tilde * (A[I1, J1] * Nx - A_nu[I1, J1] * Ny);
                    double EY_Memb = A_tilde * (A[I1, J1] * Ny - A_nu[I1, J1] * Nx);
                    double EXY_Memb = Lambda * D2FXY / B[I1, J1];
                    if (flag)
                    {
                        MembX[I] = EX_Memb;
                        MembY[J] = EY_Memb;
                    }
                    // Толщина 1-ого слоя
                    double HZ = H[I, J] / NT;

                    // Начальная координата 
                    double Z = -H0 / 2;

                    double EX, EY, EXY;
                    for (int K = 0; K < NT1; K++)
                    {
                        //Деформация с учётом кривизны
                        EX = EX_Memb - Z * (D2X - 0.5 * D2PHIX);
                        EY = EY_Memb - Z * (D2Y - 0.5 * D2PHIY);
                        EXY = -2 * Z * (D2XY - 0.5 * D2PHIXY) + EXY_Memb;
                        this.EX[I, J, K] = EX;
                        this.EY[I, J, K] = EY;
                        this.EXY[I, J, K] = EXY;

                        if (flag && (K == 0 || K == NT))
                        {
                            double D1X = (W[I2, J1] - W[I, J1]) * N;
                            double D1Y = (W[I1, J2] - W[I1, J]) * N;
                            //double D1PHIX = (Corrosion[I2, J1] - Corrosion[I, J1]) * N;
                            //double D1PHIY = (Corrosion[I1, J2] - Corrosion[I1, J]) * N;

                            int index = K == 0 ? 0 : 1;

                            //dU_dX[I, J, index] = Integrate(MembX, (double)I / (2 * N), I) - Z * (D1X - 0.5 * D1PHIX);
                            //dV_DY[I, J, index] = Integrate(MembY, (double)J / (2 * N), J) - Z * (D1Y - 0.5 * D1PHIY);
                            dU_dX[I, J, index] = EX - 0.5 * Pow(D1X, 2);
                            dV_DY[I, J, index] = EY - 0.5 * Pow(D1Y, 2);

                        }
                        // Интенсивность деформаций
                        double EI = Sqrt(2 * (Pow((EX), 2) + Pow((EY), 2) + EX * EY
                            + Pow(EXY, 2) / 4)) / 3;
                        DEF_INT[I, J, K] = EI;
                        if (EI > MaxEI)
                        {
                            MaxEI = EI;
                            HMaxEI = H[I, J];
                            MaxI = I;
                            MaxJ = J;
                            MaxK = K;
                        }
                        Z += HZ;
                    }
                }
            }
        }
    }
}
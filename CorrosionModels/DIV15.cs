using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        /// <summary>
        /// Решение задачи по поиску базисной функции
        /// </summary>
        /// <param name="UNK">Неизвестная функция</param>
        /// <param name="KN">Известная функция</param>
        /// <param name="Rt_unk">Функция правой стороны по направлению UNK</param>
        /// <param name="Rt_kn">Функция правой стороны по направлению KN</param>
        /// <param name="IXY">Флаг ориентации</param>
        /// <param name="IWF">Флаг типа уравнения (1 - Уравнение прогиба, 2 - Уравнение усилий)</param>
        /// <param name="IL">Флаг геометрической нелинейности</param>
        /// <returns></returns>

        public double[,] Gauss(double[,] UNK, double[,] KN, double[,] Rt_unk,
           double[,] Rt_kn, int IXY, int IWF, bool IL)
        {
            int N1 = N + 1;
            int N2 = N + 2;
            int N3 = N + 3;
            int N4 = N + 4;
            int N_2 = N - 2;
            int N_1 = N - 1;
            int IK = N * NM;

            double[] S1 = new double[N1];
            double[] S2 = new double[N1];
            double[] S3 = new double[N1];
            double[] S4 = new double[N1];
            double[] S5 = new double[N1];
            double[] S6 = new double[N1];
            double[] XK = new double[N4];
            double[] XI = new double[N4];
            double[] XS = new double[IK];

            double G1, G2, GF1, GF2;
            double[,] S = new double[IK, IK];

            if (IWF != 1)
            {
                if (IXY != 1)
                {
                    G1 = GF[1];
                    G2 = GF[0];
                    GF1 = G[1];
                    GF2 = G[0];
                }
                else
                {
                    G1 = GF[0];
                    G2 = GF[1];
                    GF1 = G[0];
                    GF2 = G[1];
                }
            }
            else
            {
                if (IXY != 1)
                {
                    G1 = G[1];
                    G2 = G[0];
                    GF1 = GF[1];
                    GF2 = GF[0];
                }
                else
                {
                    G1 = G[0];
                    G2 = G[1];
                    GF1 = GF[0];
                    GF2 = GF[1];
                }
            }

            double Lambda;
            double QL1, QL2;
            if (IXY != 1)
            {
                Lambda = 1 / this.Lambda;
                if (QK == 1) { QL1 = 1; QL2 = 1; }
                else { QL1 = 0; QL2 = 1; }
            }
            else
            {
                Lambda = this.Lambda;
                if (QK == 1) { QL1 = 1; QL2 = 1; }
                else { QL1 = 1; QL2 = 0; }
            }
            double Lambda4 = Pow(Lambda, 4);
            double Lambda2 = Pow(Lambda, 2);
            double V4 = Pow(V2, 2);

            // Экстраполяция и задание начальных значений функций прогиба
            for (int I = 0; I < NM; I++)
            {
                KN[I, 0] = G1 * KN[I, 2];
                KN[I, N2] = KN[I, N1];
                KN[I, N3] = KN[I, N];
                Rt_kn[I, 0] = GF1 * Rt_kn[I, 2];
                Rt_kn[I, N2] = Rt_kn[I, N1];
                Rt_kn[I, N3] = Rt_kn[I, N];
                Rt_unk[I, 0] = GF2 * Rt_unk[I, 2];
                Rt_unk[I, N2] = Rt_unk[I, N1];
                Rt_unk[I, N3] = Rt_unk[I, N];
            }

            for (int II = 0; II < NM; II++)
            {
                for (int I = 0; I < N4; I++)
                {
                    XI[I] = KN[II, I];
                }

                int IIN = II * N;


                for (int KK = 0; KK < NM; KK++)
                {
                    int KKN = KK * N;

                    for (int I = 0; I < N4; I++)
                    {
                        XK[I] = KN[KK, I];
                    }

                    for (int J = 2; J < N2; J++)
                    {
                        int J_1 = J - 1;
                        int J_2 = J - 2;
                        int J1 = J + 1;
                        int J2 = J + 2;

                        for (int I = 2; I < N2; I++)
                        {
                            int I_1 = I - 1;
                            int I_2 = I - 2;
                            int I1 = I + 1;
                            int I2 = I + 2;

                            double XI3 = XI[I];
                            double XK1 = XK[I_2];
                            double XK2 = XK[I_1];
                            double XK3 = XK[I];
                            double XK4 = XK[I1];
                            double XK5 = XK[I2];

                            double D1XK = (XK4 - XK2) / 2;
                            double D2XK = (XK4 - 2 * XK3 + XK2);
                            double D3XK = (XK5 - 2 * XK4 + 2 * XK2 - XK1) / 2;
                            double D4XK = (XK5 - 4 * XK4 + 6 * XK3 - 4 * XK2 + XK1);

                            int M1, M2, M3, M4, M5, L1, L2, L3, L4, L5;
                            if (IXY == 1)
                            {
                                M1 = I_2;
                                M2 = I_1;
                                M3 = I;
                                M4 = I1;
                                M5 = I2;
                                L1 = J_2;
                                L2 = J_1;
                                L3 = J;
                                L4 = J1;
                                L5 = J2;
                            }
                            else
                            {
                                M1 = J_2;
                                M2 = J_1;
                                M3 = J;
                                M4 = J1;
                                M5 = J2;
                                L1 = I_2;
                                L2 = I_1;
                                L3 = I;
                                L4 = I1;
                                L5 = I2;
                            }
                            double C4, C3, C2, C1, C;
                            double Q = 0;

                            //(Производные не делятся на шаг, т.к из-за безразмерности можно вынести
                            // и сократить на 1/V^4)
                            if (IWF == 1)
                            {
                                double D1AZY = (AZ[M3, L4] - AZ[M3, L2]) / 2;
                                double D2AZY = (AZ[M3, L4] - 2 * AZ[M3, L3] + AZ[M3, L2]);
                                double D1AZX = (AZ[M4, L3] - AZ[M2, L3]) / 2;
                                double D2AZX = (AZ[M4, L3] - 2 * AZ[M3, L3] + AZ[M2, L3]);
                                double D1AZ_nuY = (AZ_nu[M3, L4] - AZ_nu[M3, L2]) / 2;
                                double D2AZ_nuY = (AZ_nu[M3, L4] - 2 * AZ_nu[M3, L3] + AZ_nu[M3, L2]);
                                double D1AZ_nuX = (AZ_nu[M4, L3] - AZ_nu[M2, L3]) / 2;
                                double D2AZ_nuX = (AZ_nu[M4, L3] - 2 * AZ_nu[M3, L3] + AZ_nu[M2, L3]);
                                double D1BZY = (BZ[M3, L4] - BZ[M3, L2]) / 2;
                                double D1BZX = (BZ[M4, L3] - BZ[M2, L3]) / 2;
                                double D2BZXY = (BZ[M4, L4] - BZ[M4, L2] - BZ[M2, L4] + BZ[M2, L2]) / 4;
                                // Коэффициенты для уравнения прогибов
                                C4 = Lambda4 * AZ[M3, L3] * XK3 * XI3;
                                C3 = Lambda4 * 2 * D1AZY * XK3 * XI3;
                                C2 = Lambda2 * ((2 * AZ_nu[M3, L3] + BZ[M3, L3]) * D2XK * XI3 + (2 * D1AZ_nuX +
                                    D1BZX) * D1XK * XI3 + (Lambda2 * D2AZY + D2AZ_nuX + V2 * QL1 * QL) * XI3 * XK3);
                                C1 = Lambda2 * ((2 * D1AZ_nuY + D1BZY) * D2XK * XI3 + D2BZXY * D1XK * XI3);
                                C = AZ[I, J] * D4XK * XI3 + 2 * D1AZX * D3XK + (D2AZX + Lambda2 * D2AZ_nuY +
                                    V2 * QL2 * QL) * D2XK * XI3;

                                if (!IL)
                                {
                                    double XF2 = Rt_kn[KK, I_1];
                                    double XF3 = Rt_kn[KK, I];
                                    double XF4 = Rt_kn[KK, I1];

                                    double YF2 = Rt_unk[KK, J_1];
                                    double YF3 = Rt_unk[KK, J];
                                    double YF4 = Rt_unk[KK, J1];

                                    double D1XF = (XF4 - XF2) / 2;
                                    double D2XF = (XF4 - 2 * XF3 + XF2);

                                    double D1YF = (YF4 - YF2) / 2;
                                    double D2YF = (YF4 - 2 * YF3 + YF2);

                                    C -= Lambda2 * D2XK * D2YF * XF3 * XI3;
                                    C1 += Lambda2 * 2 * D1XK * D1YF * D1XF * XI3;
                                    C2 -= Lambda2 * XK3 * D2XF * YF3 * XI3;
                                }

                                if (KK == NM - 1)
                                {
                                    double D2PHIX = (Corrosion[M4, L3] - 2 * Corrosion[M3, L3] + Corrosion[M2, L3]);
                                    double D3PHIX = (Corrosion[M5, L3] - 2 * Corrosion[M4, L3] + 2 * Corrosion[M2, L3]
                                        - Corrosion[M1, L3]) / 2;
                                    double D4PHIX = (Corrosion[M5, L3] - 4 * Corrosion[M4, L3] + 6 * Corrosion[M3, L3]
                                        - 4 * Corrosion[M2, L3] + Corrosion[M1, L3]);
                                    double D2PHIY = (Corrosion[M3, L4] - 2 * Corrosion[M3, L3] + Corrosion[M3, L2]);
                                    double D3PHIY = (Corrosion[M3, L5] - 2 * Corrosion[M3, L4] + 2 * Corrosion[M3, L2]
                                        - Corrosion[M3, L1]) / 2;
                                    double D4PHIY = (Corrosion[M3, L5] - 4 * Corrosion[M3, L4] + 6 * Corrosion[M3, L3]
                                        - 4 * Corrosion[M3, L2] + Corrosion[M3, L1]);
                                    double D2PHIXY = (Corrosion[M4, L4] - Corrosion[M4, L2] - Corrosion[M2, L4] +
                                        Corrosion[M2, L2]) / 4;
                                    double D3PHIXXY = (Corrosion[M4, L4] - 2 * Corrosion[M3, L4] + Corrosion[M2, L4]
                                        - Corrosion[M4, L2] + 2 * Corrosion[M3, L2] - Corrosion[M2, L2]) / 2;
                                    double D3PHIXYY = (Corrosion[M4, L4] - 2 * Corrosion[M4, L3] + Corrosion[M4, L2] -
                                        Corrosion[M2, L4] + 2 * Corrosion[M2, L3] - Corrosion[M2, L2]) / 2;
                                    double D4PHIXXYY = (Corrosion[M4, L4] - 2 * Corrosion[M3, L4] + Corrosion[M2, L4] -
                                        2 * Corrosion[M4, L3] + 4 * Corrosion[M3, L3] - 2 * Corrosion[M2, L3] +
                                        Corrosion[M4, L2] - 2 * Corrosion[M3, L2] + Corrosion[M2, L2]);

                                    Q = -(AZ[M3, L3] * D4PHIX + 2 * D1AZX * D3PHIX + (D2AZX + Lambda2 * D2AZ_nuY) *
                                        D2PHIX + Lambda2 * ((2 * D1AZ_nuY + D1BZY) * D3PHIXXY + D2BZXY * D2PHIXY +
                                        (2 * AZ_nu[M3, L3] + BZ[M3, L3]) * D4PHIXXYY + (2 * D1AZ_nuX + D1BZX) *
                                        D3PHIXYY + (Lambda2 * D2AZ_nuY + D2AZ_nuX) * D2PHIY +
                                        Lambda2 * 2 * D1AZY * D3PHIY + Lambda2 * AZ[M3, L3] * D4PHIY)) * XI3 / 2
                                        + V4 * QTW[M2, L2] * XI3;
                                }
                            }
                            else
                            {
                                double tilde_A1X = 2 / (Pow(A[M2, L3], 2) - Pow(A_nu[M2, L3], 2));
                                double tilde_A3X = 2 / (Pow(A[M4, L3], 2) - Pow(A_nu[M4, L3], 2));
                                double tilde_A1Y = 2 / (Pow(A[M3, L2], 2) - Pow(A_nu[M3, L2], 2));
                                double tilde_A3Y = 2 / (Pow(A[M3, L4], 2) - Pow(A_nu[M3, L4], 2));
                                double tilde_A = 2 / (Pow(A[M3, L3], 2) - Pow(A_nu[M3, L3], 2));
                                double D1AY = (A[M3, L4] - A[M3, L2]) / 2;
                                double D2AY = (A[M3, L4] - 2 * A[M3, L3] + A[M3, L2]);
                                double D1AX = (A[M4, L3] - A[M2, L3]) / 2;
                                double D2AX = (A[M4, L3] - 2 * A[M3, L3] + A[M2, L3]);
                                double D1A_nuY = (A_nu[M3, L4] - A_nu[M3, L2]) / 2;
                                double D2A_nuY = (A_nu[M3, L4] - 2 * A_nu[M3, L3] + A_nu[M3, L2]);
                                double D1A_nuX = (A_nu[M4, L3] - A_nu[M2, L3]) / 2;
                                double D2A_nuX = (A_nu[M4, L3] - 2 * A_nu[M3, L3] + A_nu[M2, L3]);
                                double D1BY = (B[M3, L4] - B[M3, L2]) / 2;
                                double D1BX = (B[M4, L3] - B[M2, L3]) / 2;
                                double D2BXY = (B[M4, L4] - B[M4, L2] - B[M2, L4] + B[M2, L2]) / 4;
                                double D1tilde_AY = (tilde_A3Y - tilde_A1Y) / 2;
                                double D2tilde_AY = (tilde_A3Y - 2 * tilde_A + tilde_A1Y);
                                double D1tilde_AX = (tilde_A3X - tilde_A1X) / 2;
                                double D2tilde_AX = (tilde_A3X - 2 * tilde_A + tilde_A1X);

                                // Коэффициенты для уравненения усилий
                                C4 = -Lambda4 * tilde_A * A[M3, L3] * XK3 * XI3;
                                C3 = -Lambda4 * 2 * (D1tilde_AY * A[M3, L3] + D1AY * tilde_A) * XK3 * XI3;
                                C2 = Lambda2 * 2 * (tilde_A * A_nu[M3, L3] - (1 / B[M3, L3])) * D2XK * XI3 +
                                    2 * (D1tilde_AX * A_nu[M3, L3] + D1A_nuX * tilde_A +
                                    (1 / Pow(B[M3, L3], 2)) * D1BX) * D1XK * XI3 - (Lambda2 * (D2tilde_AY *
                                    A[M3, L3] + 2 * D1tilde_AY * D1AY + D2AY * tilde_A) - D2tilde_AX *
                                    A_nu[M3, L3] - 2 * D1tilde_AX * D1A_nuX - D2A_nuX * tilde_A) * XK3 * XI3;
                                C1 = Lambda2 * 2 * (D1tilde_AY * A_nu[M3, L3] + D1A_nuY * tilde_A +
                                    (1 / Pow(B[M3, L3], 2)) * D1BY) * D2XK * XI3 - (2 / Pow(B[M3, L3], 2)) *
                                    ((2 / B[M3, L3]) * D1BX * D1BY - D2BXY) * D1XK * XI3;
                                C = -tilde_A * A[M3, L3] * D4XK * XI3 - 2 * (D1tilde_AX * A[M3, L3] +
                                    D1AX * tilde_A) * D3XK * XI3 - (D2tilde_AX * A[M3, L3] + 2 * D1tilde_AX
                                    * D1AX + D2AX * tilde_A + Lambda2 * (D2tilde_AY * A_nu[M3, L3] - 2 * D1tilde_AY
                                    * D1A_nuY - D2A_nuY * tilde_A)) * D2XK * XI3;

                                if (KK == NM - 1)
                                {
                                    Q = (Lambda2 * (D2tilde_AY * A_nu[M3, L3] + 2 * D1tilde_AY * D1A_nuY + D2A_nuY *
                                        tilde_A) - D2tilde_AX * A[M3, L3] - 2 * D1tilde_AX * D1AX - D2AX *
                                        tilde_A) * V2 * QL1 * QL * XI3 + (D2tilde_AX * A_nu[M3, L3] +
                                        2 * D1tilde_AX * D1A_nuX + D2A_nuX * tilde_A - Lambda2 * (D2tilde_AY *
                                        A[M3, L3] + 2 * D1tilde_AY * D1AY + D2AY * tilde_A)) * V2 * QL * QL2 * XI3;

                                }
                                if (!IL)
                                {
                                    double XF2 = Rt_kn[KK, I_1];
                                    double XF3 = Rt_kn[KK, I];
                                    double XF4 = Rt_kn[KK, I1];

                                    double YF2 = Rt_unk[KK, J_1];
                                    double YF3 = Rt_unk[KK, J];
                                    double YF4 = Rt_unk[KK, J1];

                                    double D1XF = (XF4 - XF2) / 2;
                                    double D2XF = (XF4 - 2 * XF3 + XF2);

                                    double D1YF = (YF4 - YF2) / 2;
                                    double D2YF = (YF4 - 2 * YF3 + YF2);

                                    Q += Lambda2 * (D2XF * D2YF * XF3 * YF3 - Pow(D1XF * D1YF, 2)) * XI3;
                                }
                            }
                            S1[I_1] = C4 + C3 / 2; // сократилось V3, осталось "2"
                            S2[I_1] = -4 * C4 - C3 + C2 + C1 / 2;
                            S3[I_1] = 6 * C4 - 2 * C2 + C;
                            S4[I_1] = -4 * C4 + C3 + C2 - C1 / 2;
                            S5[I_1] = C4 - C3 / 2;
                            S6[I_1] = Q;
                        }
                        double T = 0.5;
                        double A1 = SIM(S1, T, N);
                        double A2 = SIM(S2, T, N);
                        double A3 = SIM(S3, T, N);
                        double A4 = SIM(S4, T, N);
                        double A5 = SIM(S5, T, N);
                        double A6 = SIM(S6, T, N);

                        int IJ = IIN + J_2;
                        int KJ = KKN + J_2;
                        if (J_2 != 0 && J_2 != 1 && J_2 != N_2 && J_2 != N_1)
                        {
                            S[IJ, KJ - 2] = A1;
                            S[IJ, KJ - 1] = A2;
                            S[IJ, KJ] = A3;
                            S[IJ, KJ + 1] = A4;
                            S[IJ, KJ + 2] = A5;
                        }
                        else if (J_2 == 0)
                        {
                            S[IJ, KJ] = A3 + G2 * A1;
                            S[IJ, KJ + 1] = A4;
                            S[IJ, KJ + 2] = A5;
                        }
                        else if (J_2 == 1)
                        {
                            S[IJ, KJ - 1] = A2;
                            S[IJ, KJ] = A3;
                            S[IJ, KJ + 1] = A4;
                            S[IJ, KJ + 2] = A5;
                        }
                        else if (J_2 == N_2)
                        {
                            S[IJ, KJ - 2] = A1;
                            S[IJ, KJ - 1] = A2;
                            S[IJ, KJ] = A3;
                            S[IJ, KJ + 1] = A4 + G2 * A5;
                        }
                        else if (J_2 == N_1)
                        {
                            S[IJ, KJ - 2] = A1;
                            S[IJ, KJ - 1] = A2 + G2 * A5;
                            S[IJ, KJ] = A3 + G2 * A4;
                        }
                        XS[IJ] += A6;
                    }
                }
            }

            SIMQ(S, XS, ref UNK);

            return UNK;
        }
    }
}

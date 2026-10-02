using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        // UNKN (unknown) - известная функция
        // KN (unknown) - известная функция
        // RtX, RtY - функции правой стороны, зависящие от X и Y
        // IXY - Флаг ориентации (расчёт по X, или по Y)
        // IWF - Флаг типа уравнения (1 - уравнение прогибов, 2 - уравнение
        // функции усилий)
        // IL - Флаг геометрической нелинейности
        public void Gauss(double[,] UNK, double[,] KN, double[,] RtY,
           double[,] RtX, int IXY, int IWF, int IL, out double[,] Y,
           out double[,] X, out double[,] YF, out double[,] XF)
        {
            double[,] A11 = new double[12, 12];
            double[,] A12 = new double[12, 12];
            double[,] B11 = new double[12, 12];
            double[,] B10 = new double[12, 12];
            double[] S61 = new double[12];
            double[] S62 = new double[12];
            double[] S1 = new double[12];
            double[] S2 = new double[12];
            double[] S3 = new double[12];
            double[] S4 = new double[12];
            double[] S5 = new double[12];
            double[] S6 = new double[12];
            double[] S7 = new double[12];
            double[] S8 = new double[12];
            double[] S9 = new double[12];
            double[] S10 = new double[12];
            double[,] S = new double[40, 44];
            double[] RS = new double[1600];
            double[] S11 = new double[12];
            double[] S12 = new double[12];
            double[] XS = new double[40];
            double[] XK = new double[14];
            double[] XI = new double[14];
            double[] S13 = new double[12];
            double[] S14 = new double[12];
            double[,] HM = new double[14, 14];
            double C21, C22, C23, A21, A22, A23, QT1 = 0, QT2 = 0, B21, B22, B23, AM1, AJ, AP1;
            double BM1, BJ, BP1, CM1, CP1, DJ, EJ;

            int N1 = N + 1;
            int N2 = N + 2;
            int N3 = N + 3;
            int N4 = N + 4;
            int IK = N * NM;
            double G1, G2, GF1, GF2;

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
            if (IXY != 1)
            {
                Lambda = 1 / this.Lambda;
            }
            else
            {
                Lambda = this.Lambda;
            }
            double Lambda4 = Math.Pow(Lambda, 4);
            double Lambda2 = Math.Pow(Lambda, 2);

            // Экстраполяция матрицы толщины на граничные узлы
            for (int I = 0; I < N2; I++)
            {
                H[N1, I] = H[N - 1, I];
                H[I, N1] = H[I, N - 1];
            }

            // Заполнение внутренней матрицы толщины для 
            for (int I = 1; I < N3; I++)
            {
                for (int J = 1; J < N3; J++)
                {
                    HM[I, J] = H[I - 1, J - 1];
                }
            }

            // Задание начальных значений
            for (int I = 0; I < N2; I++)
            {
                HM[0, I] = 2.0 * HM[1, I] - HM[2, I];
                HM[I, 0] = 2.0 * HM[I, 1] - HM[I, 2];
            }
            // Задание начальных значений на граничных узлах
            HM[0, N2] = HM[0, N];
            HM[N2, 0] = HM[N, 0];

            // Экстраполяция и задание начальных значений функций прогиба
            for (int I = 0; I < NM; I++)
            {
                KN[I, 0] = G1 * KN[I, 2];
                KN[I, N2] = KN[I, N];
                KN[I, N3] = KN[I, N - 1];
                RtX[I, 0] = GF1 * RtX[I, 2];
                RtX[I, N2] = RtX[I, N];
                RtX[I, N3] = RtX[I, N - 1];
                UNK[I, 0] = G2 * UNK[I, 2];
                UNK[I, N2] = UNK[I, N];
                UNK[I, N3] = UNK[I, N - 1];
                RtY[I, 0] = GF2 * RtY[I, 2];
                RtY[I, N2] = RtY[I, N];
                RtY[I, N3] = RtY[I, N - 1];
            }

            for (int I = 1; I < N3; I++)
            {
                for (int J = 1; J < N3; J++)
                {
                    int I1 = I - 1;
                    int J1 = J - 1;
                    double SM = 1.0 / E00[I1, J1];
                    A11[I, J] = (1.0 / E01[I1, J1] + SM) / 2.0;
                    A12[I, J] = A11[I, J] - SM;
                    SM = -E10[I1, J1] / E00[I1, J1];
                    B11[I, J] = (E11[I1, J1] / E01[I1, J1] + SM) / 2.0;
                    B10[I, J] = B11[I, J] - SM;
                }
            }
            // Экстраполяция коэффициентов
            for (int I = 0; I < N2; I++)
            {
                A11[0, I] = 2.0 * A11[1, I] - A11[2, I];
                A11[I, 0] = 2.0 * A11[I, 1] - A11[I, 2];
                A12[0, I] = 2.0 * A12[1, I] - A12[2, I];
                A12[I, 0] = 2.0 * A12[I, 1] - A12[I, 2];
                B11[0, I] = 2.0 * B11[1, I] - B11[2, I];
                B11[I, 0] = 2.0 * B11[I, 1] - B11[I, 2];
                B10[0, I] = 2.0 * B10[1, I] - B10[2, I];
                B10[I, 0] = 2.0 * B10[I, 1] - B10[I, 2];
            }

            // Задание коэффициентов на граничных узлах
            for (int I = 0; I < N3; I++)
            {
                A11[N2, I] = A11[N, I];
                A11[I, N2] = A11[I, N];
                A12[N2, I] = A12[N, I];
                A12[I, N2] = A12[I, N];
                B11[N2, I] = B11[N, I];
                B11[I, N2] = B11[I, N];
                B10[N2, I] = B10[N, I];
                B10[I, N2] = B10[I, N];
            }

            double SIG;
            switch (IPL)
            {
                case 1:
                case 2:
                SIG = QL;
                break;
                case 3:
                SIG = 1.0;
                break;
                case 4:
                SIG = QL;
                break;
                default:
                SIG = 0.0;
                break;
            }

            for (int II = 0; II < NM; II++)
            {
                for (int I = 0; I < N4; I++)
                {
                    XI[I] = KN[II, I];
                }

                int IN = II * N;

                double AF = 0.0;

                for (int KK = 0; KK < NM; KK++)
                {
                    int KN3 = KK * N + 2;

                    for (int I = 0; I < N4; I++)
                    {
                        XK[I] = KN[KK, I];
                    }

                    for (int J = 2; J < N; J++)
                    {
                        int J_1 = J - 1;
                        int J_2 = J - 2;
                        int J1 = J + 1;
                        int J2 = J + 2;

                        for (int I = 2; I < N; I++)
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

                            double XF1 = RtX[KK, I_2];
                            double XF2 = RtX[KK, I_1];
                            double XF3 = RtX[KK, I];
                            double XF4 = RtX[KK, I1];
                            double XF5 = RtX[KK, I2];

                            double D1XF = (XF4 - XF2) / 2;
                            double D2XF = (XF4 - 2 * XF3 + XF2);
                            double D3XF = (XF5 - 2 * XF4 + 2 * XF2 - XF1) / 2;
                            double D4XF = (XF5 - 4 * XF4 + 6 * XF3 - 4 * XF4 + XF1);

                            double D1XK = (XK4 - XK2) / 2;
                            double D2XK = (XK4 - 2 * XK3 + XK2);
                            double D3XK = (XK5 - 2 * XK4 + 2 * XK2 - XK1) / 2;
                            double D4XK = (XK5 - 4 * XK4 + 6 * XK3 - 4 * XK4 + XK1);

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
                            double C4 = 0, C3 = 0, C2 = 0, C1 = 0, C = 0;

                            //(Производные не делятся на шаг, т.к из-за безразмерности можно вынести 1/V2^2)
                            if (IWF == 1)
                            {
                                double D1AZY = (AZ[M3, L4] - AZ[M3, L2]) / 2;
                                double D2AZY = (AZ[M3, L4] - 2 * AZ[M3, L3] + AZ[M3, L2]);
                                double D1AZX = (AZ[M4, L3] - AZ[M2, L3]) / 2;
                                double D2AZX = (AZ[M4, L3] - AZ[M3, L3] + AZ[M2, L3]);
                                double D1AZ_nuY = (AZ_nu[M3, L4] - AZ_nu[M3, L2]) / 2;
                                double D2AZ_nuY = (AZ_nu[M3, L4] - 2 * AZ_nu[M3, L3] + AZ_nu[M3, L2]);
                                double D1AZ_nuX = (AZ_nu[M4, L3] - AZ_nu[M2, L3]) / 2;
                                double D2AZ_nuX = (AZ_nu[M4, L3] - 2 * AZ_nu[M3, L3] + AZ_nu[M2, L3]);
                                double D1BZY = (BZ[M3, L4] - BZ[M3, L2]) / 2;
                                double D1BZX = (BZ[M4, L3] - BZ[M2, L3]) / 2;
                                double D2BZXY = (BZ[M4, L4] - BZ[M4, L2] - BZ[M2, L4] + BZ[M2, L2]) / 4;
                                double D1PHIX = (Corrosion[M4, L3] - Corrosion[M2, L3]) / 2;
                                double D2PHIX = (Corrosion[M4, L3] - 2 * Corrosion[M3, L3] + Corrosion[M2, L3]);
                                double D3PHIX = (Corrosion[M5, L3] - 2 * Corrosion[M4, L3] + 2 * Corrosion[M2, L3]
                                    - Corrosion[M1, L3]) / 2;
                                double D4PHIX = (Corrosion[M5, L3] - 4 * Corrosion[M4, L3] + 6 * Corrosion[M3, L3]
                                    - 4 * Corrosion[M2, L3] + Corrosion[M1, L3]);
                                double D1PHIY = (Corrosion[M3, L4] - Corrosion[M3, L2]) / 2;
                                double D2PHIY = (Corrosion[M3, L4] - 2 * Corrosion[M3, L3] + Corrosion[M1, L2]);
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

                                // Коэффициенты для уравнения прогибов
                                C4 = Lambda4 * AZ[I, J] * XK3 * XI3;
                                C3 = Lambda4 * D1AZY * XK3 * XI3;
                                C2 = Lambda2 * ((2 * AZ_nu[I, J] + BZ[I, J]) * D2XK * XI3 + (2 * D1AZ_nuX +
                                    D1BZX) * D1XK * XI3 + (Lambda2 * D2AZY + D2AZ_nuX + V2 * QK * QL) * XI3 * XK3);
                                C1 = Pow(Lambda, 2) * ((D1AZ_nuY + D1BZY) * D2XK * XI3 + D2BZXY * D1XK * XI3);
                                C = AZ[I, J] * D4XK * XI3 + 2 * D1AZX * D3XK + (D2AZX + Lambda2 * D2AZ_nuY +
                                    V2 * QL) * D2XK * XI3;

                                QT1 = QTW[M1, L1];
                            }
                            else
                            {
                                double tilde_A1X = 2 / (Pow(A[M2, L3], 2) - Pow(A_nu[M2, L3], 2));
                                double tilde_A3X = 2 / (Pow(A[M4, L3], 2) - Pow(A_nu[M4, L3], 2));
                                double tilde_A1Y = 2 / (Pow(A[M3, L2], 2) - Pow(A_nu[M3, L2], 2));
                                double tilde_A3Y = 2 / (Pow(A[M3, L4], 2) - Pow(A_nu[M3, L4], 2));
                                double tilde_A = 2 / (Pow(A[M3, L3], 2) - Pow(A_nu[M3, L3], 2));
                                double D1AY = (A[M3, L4] - A[M3, L2]) / (2);
                                double D2AY = (A[M3, L4] - 2 * A[M3, L3] + A[M3, L2]);
                                double D1AX = (A[M4, L3] - A[M2, L3]) / (2);
                                double D2AX = (A[M4, L3] - 2 * A[M3, L3] + A[M2, L3]);
                                double D1A_nuY = (A_nu[M3, L4] - A_nu[M3, L2]) / (2);
                                double D2A_nuY = (A_nu[M3, L4] - 2 * A_nu[M3, L3] + A_nu[M3, L2]);
                                double D1A_nuX = (A_nu[M4, L3] - A_nu[M2, L3]) / (2);
                                double D2A_nuX = (A_nu[M4, L3] - 2 * A_nu[M3, L3] + A_nu[M2, L3]);
                                double D1BY = (B[M3, L4] - B[M3, L2]) / (2);
                                double D1BX = (B[M4, L3] - B[M2, L3]) / (2);
                                double D2BXY = (B[M4, L4] - B[M4, L2] - B[M2, L4] + B[M2, L2]) / 4;
                                double D1tilde_AY = (tilde_A3Y - tilde_A1Y) / 2;
                                double D2tilde_AY = (tilde_A3Y - 2 * tilde_A + tilde_A1Y);
                                double D1tilde_AX = (tilde_A3X - tilde_A1X) / 2;
                                double D2tilde_AX = (tilde_A3X - 2 * tilde_A + tilde_A1Y);

                                C4 = -Lambda4 * tilde_A * A[M3, L3] * XK3 * XI3;
                                C3 = -Lambda4 * 2 * (D1tilde_AY * A[M3, L3] + D1AY * tilde_A) * XK3 * XI3;
                                C2 = Lambda2 * (2 * tilde_A * A_nu[M3, L3] - (1 / B[M3, L3])) * D2XK * XI3 +
                                    (2 * D1tilde_AX * A_nu[M3, L3] + 2 * D1A_nuX * tilde_A +
                                    (1 / Pow(B[M3, L3], 2)) * D1BX) * D1XK * XI3 - (Lambda2 * (D2tilde_AY * 
                                    A[M3, L3] + 2 * D1tilde_AY * D1AY + D2AY * tilde_A) - D2tilde_AX * 
                                    A_nu[M3, L3] - 2 * D1tilde_AX * D1A_nuX - D2A_nuX * tilde_A) * XK3 * XI3;
                                C1 = Lambda2 * (2 * D2tilde_AY * A_nu[M3, L3] + 2 * D1A_nuY * tilde_A +
                                    (1 / Pow(B[M3, L3], 2)) * D1BY) * D2XK * XI3 - D2BXY *
                                    (1 / Pow(B[M3, L3], 2)) * D1XK * XI3;
                                C = -tilde_A * A[M3, L3] * D4XK * XI3 - 2 * (D1tilde_AX * A[M3, L3] +
                                    D1AX * tilde_A) * D3XK * XI3 - (D2tilde_AX * A[M3, L3] + 2 * D1tilde_AX
                                    * D1AX + D2AX * tilde_A - Lambda2 * (D2tilde_AY * A_nu[M3, L3] - 2 * D1tilde_AY
                                    * D1A_nuY - D2A_nuY * tilde_A)) * D2XK * XI3;

                                QT2 = QTF[M1, L1];
                            }

                            S1[I_1] = C4 + C3 / 2; // сократилось V3, осталось "2"
                            S2[I_1] = -4 * C4 - C3 + C2 + C1 / 2;
                            S3[I_1] = 6 * C4 - 2 * C2 + C;
                            S4[I_1] = -4 * C4 + C3 + C2 - C1 / 2;
                            S5[I_1] = C4 - C3 / 2;

                            if (IL != 0 && JF != 0)
                            {
                               
                            }
                            if (IXY == 1)
                            {
                                M3 = I;
                                L3 = J;
                            }
                            else
                            {
                                M3 = J;
                                L3 = I;
                            }
                            double D2HX = (HM[M3, L3] - 2 * HM[M3, L3] + HM[M1, L3]) / this.V2;
                            double D2HY = (HM[M3, L3] - 2 * HM[M3, L3] + HM[M3, L1]) / this.V2;
                            double DHXY = (HM[M3, L3] - HM[M1, L3] - HM[M3, L1]
                                + HM[M1, L1]) / (4 * this.V2);

                            // Здесь идёт расслоение задачи (решение либо функции
                            // усилий, либо функции прогибов)
                            if (IWF == 1)
                            {
                                double D2FX = (FXY[M3, L3] - 2 * FXY[M3, L3] + FXY[M1, L3]) / this.V2;
                                double D2FY = (FXY[M3, L3] - 2 * FXY[M3, L3] + FXY[M3, L1]) / this.V2;
                                double DFXY = (FXY[M3, L3] - FXY[M1, L3] - FXY[M3, L1]
                                    + FXY[M1, L1]) / (4 * this.V2);
                                double DB11X = (B11[M3, L3] - 2 * B11[M3, L3] + B11[M1, L3]) / this.V2;
                                double DB11Y = (B11[M3, L3] - 2 * B11[M3, L3] + B11[M3, L1]) / this.V2;
                                double DB10X = (B10[M3, L3] - 2 * B10[M3, L3] + B10[M1, L3]) / this.V2;
                                double DB10Y = (B10[M3, L3] - 2 * B10[M3, L3] + B10[M3, L1]) / this.V2;
                                if (KK == NM - 1)
                                {
                                    double D2X = (WXY[M3, L3] - 2 * WXY[M3, L3] + WXY[M1, L3]) / this.V2;
                                    double D2Y = (WXY[M3, L3] - 2 * WXY[M3, L3] + WXY[M3, L1]) / this.V2;
                                    double D2X0 = (W0[M3, L3] - 2 * W0[M3, L3] + W0[M1, L3]) / this.V2;
                                    double D2Y0 = (W0[M3, L3] - 2 * W0[M3, L3] + W0[M3, L1]) / this.V2;
                                    double DXY0 = (W0[M3, L3] - W0[M1, L3] - W0[M3, L1]
                                        + W0[M1, L1]) / (4 * this.V2);
                                    double PX = 0;
                                    double PY = 0;
                                    if (IPL == 3)
                                    {
                                        PX = D2X;
                                        PY = D2Y;
                                    }
                                    // Значения под воздействием поперечной нагрузки
                                    S61[I_1] = QT1 * XI[I];
                                    S6[I_1] = (-SKX * D2FY - SKY * D2FX - HH * (D2FY *
                                        D2HX + D2FX * D2HY - 2 * DFXY * DHXY) / 2) * XI[I];
                                    S62[I_1] = -SIG * (Lambda * DB11Y + DB10X + D2HX / 2 + SKX + PX +
                                        QK * (DB10Y + DB11X / Lambda + D2HY / 2 + SKY + PY)) * XI[I];
                                }
                                double SIGM = SIG;
                                // В случае, если NM = 1 - блок выше выполняется первее (значение SIGM = 1)
                                if (IPL == 3) SIGM = 0;
                                S12[I_1] = D2XK * XI[I] * (D2FY + SIGM) * this.V2;
                                S13[I_1] = XK3 * XI[I] * (D2FX + QK * SIGM) * this.V2;
                                S14[I_1] = D1XK * XI[I] * DFXY * this.V2;
                            }
                            else
                            {
                                if (KK == NM - 1)
                                {
                                    double D2XY = (WXY[M3, L3] - WXY[M1, L3] -
                                        WXY[M3, L1] + WXY[M1, L1]) / 4;
                                    double D2X = (WXY[M3, L3] - 2 * WXY[M3, L3] + WXY[M1, L3]);
                                    double D2Y = (WXY[M3, L3] - 2 * WXY[M3, L3] + WXY[M3, L1]);
                                    double D2X0 = (W0[M3, L3] - 2 * W0[M3, L3] + W0[M1, L3]);
                                    double D2Y0 = (W0[M3, L3] - 2 * W0[M3, L3] + W0[M3, L1]);
                                    double DXY0 = (W0[M3, L3] - W0[M1, L3] - W0[M3, L1]
                                    + W0[M1, L1]) / 4;
                                    double D2A1X = A11[M3, L3] - 2 * A11[M3, L3] + A11[M1, L3];
                                    double D2A1Y = A11[M3, L3] - 2 * A11[M3, L3] + A11[M3, L1];
                                    double D2A2X = A12[M3, L3] - 2 * A12[M3, L3] + A12[M1, L3];
                                    double D2A2Y = A12[M3, L3] - 2 * A12[M3, L3] + A12[M3, L1];
                                    S6[I_1] = -(QT2 * V2 + this.V2 * (SKX * (D2Y - D2Y0) + SKY *
                                    (D2X - D2X0))) * XI[I] + HH * this.V2 * ((D2XY - DXY0) * DHXY
                                    - ((D2X - D2X0) * D2HY + (D2Y - D2Y0) * D2HX) / 2) * XI[I] -
                                    SIG * (Lambda * D2A1Y + D2A2X + QK * (D2A1X / Lambda + D2A2Y)) * this.V2 * XI[I];
                                    S6[I_1] = S6[I_1] + +(Math.Pow(D2XY, 2) - D2X * D2Y
                                        - Math.Pow(DXY0, 2) + D2X0 * D2Y0) * XI[I];
                                }
                            }
                        }
                        double T = 0.5;
                        double A1 = SIM(S1, T, N);
                        double A2 = SIM(S2, T, N);
                        double A3 = SIM(S3, T, N);
                        double A4 = SIM(S4, T, N);
                        double A5 = SIM(S5, T, N);
                        double A6 = 0, A61 = 0, A62 = 0;
                        if (IL != 0 && JF != 0)
                        {
                            double AF1 = SIM(S7, T, N);
                            double AF2 = SIM(S8, T, N);
                            double AF3 = SIM(S9, T, N);
                            double AF4 = SIM(S10, T, N);
                            double AF5 = SIM(S11, T, N);
                            AF = AF1 * RtY[KK, J_2] + AF2 * RtY[KK, J_1] + AF3 * RtY[KK, J]
                                + AF4 * RtY[KK, J1] + AF5 * RtY[KK, J2];
                        }
                        if (IWF == 1)
                        {
                            if (IL != 0)
                            {
                                double PH1 = SIM(S12, T, N) * IL;
                                double PH2 = SIM(S13, T, N) * IL;
                                double PH3 = SIM(S14, T, N) * IL;
                                A2 = A2 + PH2 + PH3;
                                A3 = A3 + PH1 - 2 * PH2;
                                A4 = A4 + PH2 - PH3;
                            }
                            if (KK == NM - 1)
                            {
                                A6 = V2 * SIM(S6, T, N);
                                A61 = V2 * SIM(S61, T, N);
                                A62 = V2 * SIM(S62, T, N);
                            }
                        }
                        else if (KK == NM - 1) A6 = SIM(S6, T, N);
                        int IJ = IN + J_2;
                        int KJ = KN + J_2;
                        S[IJ, KJ - 2] = A1;
                        S[IJ, KJ - 1] = A2;
                        S[IJ, KJ] = A3;
                        S[IJ, KJ + 1] = A4;
                        S[IJ, KJ + 2] = A5;
                        XS[IJ] = XS[IJ] - IL * AF;
                        if (KK == NM - 1)
                        {
                            //if (OS > 0.01)
                            //{
                            //    double A0 = 0;
                            //    if (IXY == 1) Oridef(X0, Y0, 1, IWF, XI, J, out A0);
                            //    else Oridef(X0, Y0, 2, IWF, XI, J, out A0);
                            //    XS[IJ] = XS[IJ] + A0;
                            //}
                            if (IWF == 1)
                            {
                                switch (IPL)
                                {
                                    case 1:
                                    {
                                        XS[IJ] = XS[IJ] + A6 + A61 + A62;
                                        break;
                                    }
                                    case 2:
                                    {
                                        XS[IJ] = XS[IJ] + A6 + A61 + A62;
                                        B[IJ] = A61;
                                        break;
                                    }
                                    case 3:
                                    {
                                        XS[IJ] = XS[IJ] + A6 + A61 + A62;
                                        B[IJ] = A62;
                                        break;
                                    }
                                    case 4:
                                    {
                                        XS[IJ] = XS[IJ] + A6 + A61 + A62;
                                        break;
                                    }
                                    default:
                                    {
                                        XS[IJ] = 0;
                                        break;
                                    }
                                }
                            }
                            else { XS[IJ] = XS[IJ] + A6; }
                        }
                    }
                    //
                    S[IN, KN + 1] = S[IN, KN + 1] + G2 * S[IN, KN - 1];
                    int NN = IN + N - 1;
                    int IR = KN + N - 1;
                    S[NN - 1, IR - 1] = S[NN - 1, IR - 1] + S[NN - 1, IR + 1];
                    S[NN, IR - 2] = S[NN, IR - 2] + S[NN, IR + 2];
                    S[NN, IR - 1] = S[NN, IR - 1] + S[NN, IR + 1];
                    S[NN - 1, IR + 1] = 0;
                    S[NN, IR + 2] = 0;
                    S[IN, KN - 1] = 0;
                    S[IN, KN] = 0;
                    S[IN + 1, KN] = 0;
                }
            }
            //
            if (IWF == 1)
            {
                if (IPL != 1 && IPL != 4)
                {
                    for (int I = 0; I < NM; I++)
                    {
                        int KG = (I + 1) * N;
                        S[IK, KG + 1] = 1;
                    }
                }
            }
            for (int I = 0; I < IK; I++)
            {
                for (int J = 0; J < IK; J++)
                {
                    int KK = I * IK + J;
                    RS[KK] = S[J, I + 2];
                }
            }
            int KS = 0;
            SIMQ(RS, XS, IK, out KS);
            if (IWF == 1)
            {
                if (IPL == 2) QP = XS[IK];
                if (IPL == 3) QL = XS[IK];
            }
            for (int I = 0; I < NM; I++)
            {
                UNK[I, 2] = 0;
            }
            for (int I = 0; I < NM; I++)
            {
                int IN = I * N;
                for (int J = 0; J < N; J++)
                {
                    UNK[I, J + 2] = XS[IN + J];
                }
            }
            for (int I = 0; I < NM; I++)
            {
                UNK[I, 0] = G2 * UNK[I, 3];
                UNK[I, N3 - 1] = UNK[I, N1 - 1];
                UNK[I, N4 - 1] = UNK[I, N - 1];
            }
            X = KN;
            Y = UNK;
            YF = RtY;
            XF = RtX;
            return;
        }
    }
}

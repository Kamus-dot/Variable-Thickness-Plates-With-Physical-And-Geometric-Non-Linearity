using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        // HY1[], HX1[] - текущие базисные функции прогибов
        // HYF[], HXF[] - текущие базисные функции усилий (функции Эри)
        // IXY - Флаг ориентации (расчёт по X, или по Y)
        // IWF - Флаг типа уравнения (1 - уравнение прогибов, 2 - уравнение
        // функции усилий)
        // IL - Флаг геометрической нелинейност
        // Cистема уравнений для решения задачи слабого изгиба пластины с
        // учетом растяжения, сжатия и сдвига в срединном слое состоит
        // из уравнения Софи-Жермен-Лагранжа и уравнения L2L2F = 0
        // (в случае геометрической линейности); Данная задача 
        // фактически разбивается на две: 
        // 1. Задача определения прогиба w - задача исследования чистого
        // изгиба пластины.
        // 2. Задача исследования напряженно деформированного состояния
        // срединной поверхности пластины без изгиба – плоская задача теории
        // упругости. 
        public static void Gauss(double[,] HY1, double[,] HX1, double[,] HYF,
           double[,] HXF, int IXY, int IWF, int IL, out double[,] Y,
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
            double[] B = new double[40];
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
            double F2 = VO * VO;

            //for (int i = 0; i < 40; i++)
            //{
            //    XS[i] = 0.0;
            //    for (int j = 0; j < 44; j++)
            //    {
            //        S[i, j] = 0.0;
            //    }
            //}

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

            double C1;
            if (IXY != 1)
            {
                C1 = 1 / V2;
            }
            else
            {
                C1 = V2;
            }

            // Экстраполяция значений матриц жесткости на граничные узлы
            for (int I = 0; I < N2; I++)
            {
                H[N1, I] = H[N - 1, I];
                H[I, N1] = H[I, N - 1];
            }

            // Задание основных значений матрицы
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
                HX1[I, 0] = G1 * HX1[I, 2];
                HX1[I, N2] = HX1[I, N];
                HX1[I, N3] = HX1[I, N - 1];
                HXF[I, 0] = GF1 * HXF[I, 2];
                HXF[I, N2] = HXF[I, N];
                HXF[I, N3] = HXF[I, N - 1];
                HY1[I, 0] = G2 * HY1[I, 2];
                HY1[I, N2] = HY1[I, N];
                HY1[I, N3] = HY1[I, N - 1];
                HYF[I, 0] = GF2 * HYF[I, 2];
                HYF[I, N2] = HYF[I, N];
                HYF[I, N3] = HYF[I, N - 1];
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
                    XI[I] = HX1[II, I];
                }

                int IN = II * N;

                double AF = 0.0;

                for (int KK = 0; KK < NM; KK++)
                {
                    int KN = KK * N + 2;

                    for (int I = 0; I < N4; I++)
                    {
                        XK[I] = HX1[KK, I];
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

                            //double XI3 = XI[I];
                            double XK1 = XK[I_2];
                            double XK2 = XK[I_1];
                            double XK3 = XK[I];
                            double XK4 = XK[I1];
                            double XK5 = XK[I2];
                            double XF1 = HXF[KK, I_2];
                            double XF2 = HXF[KK, I_1];
                            double XF3 = HXF[KK, I];
                            double XF4 = HXF[KK, I1];
                            double XF5 = HXF[KK, I2];

                            // Производные не делятся на шаг 

                            double D1XI = (XI[I1] - XI[I_1]) / 2.0;
                            double D2XI = XI[I1] - 2.0 * XI[I] + XI[I_1];
                            double D1XF = (XF4 - XF2) / 2.0;
                            double D2XF = XF4 - 2.0 * XF3 + XF2;
                            double D1XK = (XK4 - XK2) / 2.0;
                            double D2XK = XK4 - 2.0 * XK3 + XK2;

                            int M, M1, M2, M3, L, L1, L2, L3;
                            if (IXY == 1)
                            {
                                M = I_1;
                                M1 = I_1;
                                M2 = I_1;
                                M3 = I1;
                                L = J_2;
                                L1 = J_1;
                                L2 = J;
                                L3 = J1;
                            }
                            else
                            {
                                M = J_2;
                                M1 = J_1;
                                M2 = J;
                                M3 = J1;
                                L = I_1;
                                L1 = I_1;
                                L2 = I_1;
                                L3 = I1;
                            }

                            if (IWF != 1)
                            {
                                C21 = 1.0 / E00[M, L];
                                C22 = 1.0 / E00[M1, L1];
                                C23 = 1.0 / E00[M2, L2];
                                A21 = (1.0 / E01[M, L] + C21) / 2.0;
                                A22 = (1.0 / E01[M1, L1] + C22) / 2.0;
                                A23 = (1.0 / E01[M2, L2] + C23) / 2.0;
                                QT2 = QTF[M1, L1];
                            }
                            else
                            {
                                C21 = Math.Pow(E10[M, L], 2) / E00[M, L] - E20[M, L];
                                C22 = Math.Pow(E10[M1, L1], 2) / E00[M1, L1] - E20[M1, L1];
                                C23 = Math.Pow(E10[M2, L2], 2) / E00[M2, L2] - E20[M2, L2];
                                A21 = (Math.Pow(E11[M, L], 2) / E01[M, L] - E21[M, L] + C21) / 2.0;
                                A22 = (Math.Pow(E11[M1, L1], 2) / E01[M1, L1] - E21[M1, L1] + C22) / 2.0;
                                A23 = (Math.Pow(E11[M2, L2], 2) / E01[M2, L2] - E21[M2, L2] + C23) / 2.0;
                                QT1 = QTW[M1, L1];
                            }

                            B21 = A21 - C21;
                            B22 = A22 - C22;
                            B23 = A23 - C23;
                            AM1 = XI[I] * XK3 * A21;
                            AJ = XI[I] * XK3 * A22;
                            AP1 = XI[I] * XK3 * A23;
                            BM1 = XI[I] * B21 * D2XK;
                            BJ = XI[I] * B22 * D2XK;
                            BP1 = XI[I] * B23 * D2XK;
                            CM1 = -C21 * D1XK * D1XI;
                            CP1 = -C23 * D1XK * D1XI;
                            DJ = A22 * D2XK * D2XI;
                            EJ = B22 * XK3 * D2XI;
                            S1[I_1] = C1 * AM1 + CM1 / 2;
                            S2[I_1] = BM1 + EJ - 2 * C1 * (AJ + AM1);
                            S3[I_1] = DJ / C1 - 2 * (BJ + EJ) + C1 * (AP1 + 4 * AJ + AM1) - (CP1 + CM1) / 2;
                            S4[I_1] = BP1 + EJ - 2 * C1 * (AP1 + AJ);
                            S5[I_1] = C1 * AP1 + CP1 / 2;
                            if (IL != 0 && JF != 0)
                            {
                                C21 = -E10[M, L] / E00[M, L];
                                C22 = -E10[M1, L1] / E00[M1, L1];
                                C23 = -E10[M2, L2] / E00[M2, L2];
                                A21 = (E11[M, L] / E01[M, L] + C21) / 2;
                                A22 = (E11[M1, L1] / E01[M1, L1] + C22) / 2;
                                A23 = (E11[M2, L2] / E01[M2, L2] + C23) / 2;
                                B21 = A21 - C21;
                                B22 = A22 - C22;
                                B23 = A23 - C23;
                                AM1 = XI[I] * XF3 * A21;
                                AJ = XI[I] * XF3 * A22;
                                AP1 = XI[I] * XF3 * A23;
                                BM1 = XI[I] * XF3 * A23;
                                BJ = XI[I] * B22 * D2XF;
                                BP1 = XI[I] * B23 * D2XF;
                                CM1 = -C21 * D1XF * D2XI;
                                CP1 = -C23 * D1XF * D1XI;
                                DJ = A22 * D2XF * D2XI;
                                EJ = B22 * XF3 * D2XI;
                                S7[I_1] = C1 * AM1 + CM1 / 2;
                                S8[I_1] = BM1 + EJ - 2 * C1 * (AJ + AM1);
                                S9[I_1] = DJ / C1 - 2 * (BJ + EJ) + C1 *
                                    (AP1 + 4 * AJ + AM1) - (CP1 + CM1) / 2;
                                S10[I_1] = BP1 + EJ - 2 * C1 * (AP1 + AJ);
                                S11[I_1] = C1 * AP1 + CP1 / 2;
                            }
                            if (IXY == 1)
                            {
                                M = I;
                                L = J;
                            }
                            else
                            {
                                M = J;
                                L = I;
                            }
                            double D2HX = (HM[M3, L] - 2 * HM[M, L] + HM[M1, L]) / VO;
                            double D2HY = (HM[M, L3] - 2 * HM[M, L] + HM[M, L1]) / VO;
                            double DHXY = (HM[M, L3] - HM[M1, L3] - HM[M3, L1]
                                + HM[M1, L1]) / (4 * VO);

                            // Здесь идёт расслоение задачи (решение либо функции
                            // усилий, либо функции прогибов)
                            if (IWF == 1)
                            {
                                double D2FX = (FXY[M3, L] - 2 * FXY[M, L] + FXY[M1, L]) / VO;
                                double D2FY = (FXY[M, L3] - 2 * FXY[M, L] + FXY[M, L1]) / VO;
                                double DFXY = (FXY[M3, L3] - FXY[M1, L3] - FXY[M3, L1]
                                    + FXY[M1, L1]) / (4 * VO);
                                double DB11X = (B11[M3, L] - 2 * B11[M, L] + B11[M1, L]) / VO;
                                double DB11Y = (B11[M, L3] - 2 * B11[M, L] + B11[M, L1]) / VO;
                                double DB10X = (B10[M3, L] - 2 * B10[M, L] + B10[M1, L]) / VO;
                                double DB10Y = (B10[M, L3] - 2 * B10[M, L] + B10[M, L1]) / VO;
                                if (KK == NM - 1)
                                {
                                    double D2X = (WXY[M3, L] - 2 * WXY[M, L] + WXY[M1, L]) / VO;
                                    double D2Y = (WXY[M, L3] - 2 * WXY[M, L] + WXY[M, L1]) / VO;
                                    double D2X0 = (W0[M3, L] - 2 * W0[M, L] + W0[M1, L]) / VO;
                                    double D2Y0 = (W0[M, L3] - 2 * W0[M, L] + W0[M, L1]) / VO;
                                    double DXY0 = (W0[M3, L3] - W0[M1, L3] - W0[M3, L1]
                                        + W0[M1, L1]) / (4 * VO);
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
                                    S62[I_1] = -SIG * (C1 * DB11Y + DB10X + D2HX / 2 + SKX + PX +
                                        QK * (DB10Y + DB11X / C1 + D2HY / 2 + SKY + PY)) * XI[I];
                                }
                                double SIGM = SIG;
                                // В случае, если NM = 1 - блок выше выполняется первее (значение SIGM = 1)
                                if (IPL == 3) SIGM = 0;
                                S12[I_1] = D2XK * XI[I] * (D2FY + SIGM) * VO;
                                S13[I_1] = XK3 * XI[I] * (D2FX + QK * SIGM) * VO;
                                S14[I_1] = D1XK * XI[I] * DFXY * VO;
                            }
                            else
                            {
                                if (KK == NM - 1)
                                {
                                    double D2XY = (WXY[M3, L3] - WXY[M1, L3] -
                                        WXY[M3, L1] + WXY[M1, L1]) / 4;
                                    double D2X = (WXY[M3, L] - 2 * WXY[M, L] + WXY[M1, L]);
                                    double D2Y = (WXY[M, L3] - 2 * WXY[M, L] + WXY[M, L1]);
                                    double D2X0 = (W0[M3, L] - 2 * W0[M, L] + W0[M1, L]);
                                    double D2Y0 = (W0[M, L3] - 2 * W0[M, L] + W0[M, L1]);
                                    double DXY0 = (W0[M3, L3] - W0[M1, L3] - W0[M3, L1]
                                    + W0[M1, L1]) / 4;
                                    double D2A1X = A11[M3, L] - 2 * A11[M, L] + A11[M1, L];
                                    double D2A1Y = A11[M, L3] - 2 * A11[M, L] + A11[M, L1];
                                    double D2A2X = A12[M3, L] - 2 * A12[M, L] + A12[M1, L];
                                    double D2A2Y = A12[M, L3] - 2 * A12[M, L] + A12[M, L1];
                                    S6[I_1] = -(QT2 * F2 + VO * (SKX * (D2Y - D2Y0) + SKY *
                                    (D2X - D2X0))) * XI[I] + HH * VO * ((D2XY - DXY0) * DHXY
                                    - ((D2X - D2X0) * D2HY + (D2Y - D2Y0) * D2HX) / 2) * XI[I] -
                                    SIG * (C1 * D2A1Y + D2A2X + QK * (D2A1X / C1 + D2A2Y)) * VO * XI[I];
                                    S6[I_1] = S6[I_1] + +(Math.Pow(D2XY, 2) - D2X * D2Y
                                        - Math.Pow(DXY0, 2) + D2X0 * D2Y0) * XI[I];
                                }
                            }
                        }
                        double C = 0.5;                        
                        double A1 = SIM(S1, C, N);
                        double A2 = SIM(S2, C, N);
                        double A3 = SIM(S3, C, N);
                        double A4 = SIM(S4, C, N);
                        double A5 = SIM(S5, C, N);
                        double A6 = 0, A61 = 0, A62 = 0;
                        if (IL != 0 && JF != 0)
                        {
                            double AF1 = SIM(S7, C, N);
                            double AF2 = SIM(S8, C, N);
                            double AF3 = SIM(S9, C, N);
                            double AF4 = SIM(S10, C, N);
                            double AF5 = SIM(S11, C, N);
                            AF = AF1 * HYF[KK, J_2] + AF2 * HYF[KK, J_1] + AF3 * HYF[KK, J]
                                + AF4 * HYF[KK, J1] + AF5 * HYF[KK, J2];
                        }
                        if (IWF == 1)
                        {
                            if (IL != 0)
                            {
                                double PH1 = SIM(S12, C, N) * IL;
                                double PH2 = SIM(S13, C, N) * IL;
                                double PH3 = SIM(S14, C, N) * IL;
                                A2 = A2 + PH2 + PH3;
                                A3 = A3 + PH1 - 2 * PH2;
                                A4 = A4 + PH2 - PH3;
                            }
                            if (KK == NM - 1)
                            {
                                A6 = F2 * SIM(S6, C, N);
                                A61 = F2 * SIM(S61, C, N);
                                A62 = F2 * SIM(S62, C, N);
                            }
                        }
                        else if (KK == NM - 1) A6 = SIM(S6, C, N);
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
                HY1[I, 2] = 0;
            }
            for (int I = 0; I < NM; I++)
            {
                int IN = I * N;
                for (int J = 0; J < N; J++)
                {
                    HY1[I, J + 2] = XS[IN + J];
                }
            }
            for (int I = 0; I < NM; I++)
            {
                HY1[I, 0] = G2 * HY1[I, 3];
                HY1[I, N3 - 1] = HY1[I, N1 - 1];
                HY1[I, N4 - 1] = HY1[I, N - 1];
            }
            X = HX1;
            Y = HY1;
            YF = HYF;
            XF = HXF;
            return;
        }
    }
}

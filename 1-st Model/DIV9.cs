using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        // PL - функция зависимости деформаций от напряжения (PLAST)
        // IP - флаг обработки результатов:
        // IP = 0 - стандартный расчет
        // IP = 1 - дополнительная обработка (определение максимальной деформации)
        // NPL - режим вывода:
        // NPL = 0 - расчет только для верхней и нижней поверхностей
        // NPL = 1 - расчет для всех слоев по толщине
        public static void Defnap(Func<double, double> PL, int IP, int NPL)
        {
            double[,] DEF1 = new double[12, 12];
            double[,] STRDEF = new double[6, 12];
            double M01;
            double HH = 0;
            int N1 = N + 1;
            int NT1 = NT + 1;
            double V22 = Math.Sqrt(V2);

            double SIGB = 0;
            double SIGH = 0;
            int NP1;
            int NP2;
            if (NPL == 0)
            {
                NP1 = 1;
                NP2 = NT1;
            }
            else
            {
                NP1 = 1;
                NP2 = 1;
            }
            for (int K = NP1; K <= NP2; K++)
            {
                for (int I = 0; I < N1; I++)
                {
                    int I1 = I + 1;
                    int I2 = I + 2;
                    for (int J = 0; J < N1; J++)
                    {
                        int J1 = J + 1;
                        int J2 = J + 2;

                        double D2X = (WXY[I2, J1] - 2 * WXY[I1, J1]
                            + WXY[I, J1]) / VO - (W0[I2, J1] - 2 * W0[I1, J1]
                            + W0[I, J1]) / VO;

                        double D2Y = (WXY[I1, J2] - 2 * WXY[I1, J1]
                            + WXY[I1, J]) / VO - (W0[I1, J2] - 2 * W0[I1, J1]
                            + W0[I1, J]) / VO;

                        double D2XY = (WXY[I2, J2] - WXY[I2, J] - WXY[I, J2]
                            + WXY[I, J]) / 4 * VO;

                        if (NPL == 0) continue;
                        // Значение действительного прогиба, прикладываемой к 
                        // пластине 
                        DX[I, J] = D2X;
                        DY[I, J] = D2Y;
                        DXY[I, J] = D2XY;

                        double T1 = (FXY[I1, J2] - 2 * FXY[I1, J1] + FXY[I1, J]) / VO
                            + QL;

                        double T2 = (FXY[I1, J2] - 2 * FXY[I1, J1] + FXY[I, J1]) / VO
                            + QK * QL;

                        S = -(FXY[I1, J2] - FXY[I2, J] - FXY[I, J2] + FXY[I, J]) / 4 * VO;

                        double H11 = H[I, J];
                        double C01 = E01[I, J];
                        double C00 = E00[I, J];
                        double C11 = E11[I, J];
                        double C10 = E10[I, J];
                        double HZ = H11 / NT;
                        double HP = 0;
                        if (JF == 1) HP = 0.5;
                        else HP = 0.2;
                        double A1 = (1 / C01 + 1 / C00) / 2;
                        double A2 = (1 / C01 - 1 / C00) / 2;
                        double B10 = (C11 / C01 + C10 / C00) / 2;
                        double B11 = (C11 / C01 - C10 / C00) / 2;
                        M01 = 0;
                        double EXX1 = V2 * A1 * T1 + A2 * T2 + B10 * D2X
                            + V2 * B11 * D2Y + M01 / C01;
                        double EYY1 = V2 * A2 * T1 + A1 * T2 + B11 * D2X
                            + V2 * B10 * D2Y + M01 / C01;
                        double EXY1 = (2 * S / C00 + 2 * (B10 - B11) * D2XY) * V22;
                        double Z = 0;
                        double EXX = 0;
                        double EYY = 0;
                        double EYY2 = 0;
                        double EXY = 0;
                        double EZZ = 0;
                        double EI = 0;
                        if (NPL == 1)
                        {
                            // Коэффициент z
                            Z = (HH - 1) * H11 / 2 + (K - 1) * HZ;
                            // Деформации 
                            EXX = EXX1 - Z * D2X;
                            EYY = EYY1 - Z * D2Y;
                            EXY = -2 * V22 * Z * D2XY + EXY1;
                            Z = -HP * (EXX + EYY) / (1 - HP);
                            // Интенсивность
                            EI = Math.Sqrt(2 * (Math.Pow((EXX - EYY), 2)
                                + Math.Pow((EYY - EZZ), 2) + Math.Pow((EXX - EZZ), 2)
                                + 3 * Math.Pow(EXY, 2) / 2)) / 3;
                            DEF[I, J] = EI;
                            if (EI > SIGB) SIGB = EI;
                        }
                        if (NPL != 1)
                        {
                            // Коэффициент z
                            Z = (1 + HH) * H11 / 2;
                            // Деформации 
                            EXX = EXX1 - Z * D2Y;
                            EYY = EYY1 - Z * D2Y;
                            EXY = -2 * V22 * Z * D2XY + EXY1;
                            EZZ = -HP * (EXX + EYY) / (1 - HP);
                            // Интенсивность
                            EI = Math.Sqrt(2 * (Math.Pow((EXX - EYY), 2)
                            + Math.Pow((EYY - EZZ), 2) + Math.Pow((EXX - EZZ), 2)
                            + 3 * Math.Pow(EXY, 2) / 2)) / 3;
                            DEF[I, J] = EI;
                            if (EI > SIGB) SIGB = EI;
                            STR[I, J] = PL(EI);
                        }
                    }
                }
                SURFS();
                if (NPL != 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int KI = (i != 0) ? 2 * i - 1 : 0;
                        for (int j = 0; j < 5; j++)
                        {
                            int KJ = j != 0 ? 2 * j - 1 : 0;
                            STRDEF[i, KJ] = DEF[KI, KJ];
                            STRDEF[i, 2 * (j + 1) - 1] = STR[KI, KJ];
                        }
                    }
                    Console.WriteLine("Tension Deformation Condition");
                }
                if (NPL != 1)
                {
                    for (int i = 0; i < N1; i++)
                    {
                        for (int j = 0; j < N1; j++)
                        {
                            DEF[i, j] = DEF1[i, j];
                            if (IP != 0)
                            {
                                SIG = SIGH;
                                if (SIGB > SIGH) SIG = SIGB;
                            }
                        }
                    }
                }
            }
            return;
        }
    }
}

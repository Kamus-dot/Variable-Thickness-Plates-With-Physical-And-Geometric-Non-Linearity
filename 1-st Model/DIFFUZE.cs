using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public static void DIFFUZE(Func<double, double> SG)
        {
            int N2, N1;
            int ICON = 1;
            double A = 0.044;
            double B = 0.0153;
            double BET = 0.2;
            double K = 0.55;
            double ALFA = 0.048;
            double NU = 0.091;
            double GAM = 3.09;
            double VEO = 0.0217;
            double KSI = 0.00022;
            double TM = 0.64;
            double DELT = 0.08;
            double V22 = Math.Sqrt(V2);
            Q = 2.56;

            using (StreamWriter file1 = new StreamWriter("T.DAT", true))
            using (StreamWriter file222 = new StreamWriter("OUTPUT.DAT", true))
            {
                Thick();
                TempQ(ICON);

                double T = 0.0;
                double FI1, T1, T2, FI2;

                do
                {
                    FI1 = B * T / Math.Exp(1) - A / (BET * Math.Exp(BET * T));
                    T += DELT;
                    T1 = T;
                    FI2 = B * T1 / Math.Exp(1) - A / (BET * Math.Exp(BET * T1));

                    if (JF == 0)
                    {
                        INTEGER(1, PLAST);
                    }

                    file1.WriteLine("------");
                    Console.WriteLine("------");

                    N2 = N + 2;
                    N1 = N + 1;
                    for (int I3 = 1; I3 < N2; I3 += 2)
                    {
                        for (int J3 = 1; J3 < N2; J3 += 2)
                        {
                            file1.Write($"{WXY[I3, J3],12:F5}");
                        }
                        file1.WriteLine();
                    }

                    if (JF == 1)
                    {
                        Par();
                    }
                    Varite();
                    if (IL == 0)
                    {
                        Varite();
                    }

                    file1.WriteLine("H=");
                    for (int I3 = 0; I3 < N1; I3 += 2)
                    {
                        for (int J3 = 0; J3 < N1; J3 += 2)
                        {
                            file1.Write($"{H[I3, J3],12:F5}");
                        }
                        file1.WriteLine();
                    }

                    double[,] SP = new double[12, 12];
                    double[,] SK = new double[12, 12];
                    double[,] SVT = new double[12, 12];
                    // 59 строка
                    for (int I = 0; I < N1; I++)
                    {
                        int I1 = I + 1;
                        int I2 = I + 2;

                        for (int J = 0; J < N1; J++)
                        {
                            int J1 = J + 1;
                            int J2 = J + 2;

                            double D2X = (WXY[I2, J1] - 2.0 * WXY[I1, J1] + WXY[I, J1]) / VO -
                                          (W0[I2, J1] - 2.0 * W0[I1, J1] + W0[I, J1]) / VO;
                            double D2Y = (WXY[I1, J2] - 2.0 * WXY[I1, J1] + WXY[I1, J]) / VO -
                                          (W0[I1, J2] - 2.0 * W0[I1, J1] + W0[I1, J]) / VO;
                            double D2XY = (WXY[I2, J2] - WXY[I2, J] - WXY[I, J2] + WXY[I, J]) / (4.0 * VO) -
                                          (W0[I2, J2] - W0[I2, J] - W0[I, J2] + W0[I, J]) / (4.0 * VO);
                            T1 = (FXY[I1, J2] - 2.0 * FXY[I1, J1] + FXY[I1, J]) / VO + QL;
                            T2 = (FXY[I2, J1] - 2.0 * FXY[I1, J1] + FXY[I, J1]) / VO + QK * QL;
                            double S = -(FXY[I2, J2] - FXY[I2, J] - FXY[I, J2] + FXY[I, J]) / (4.0 * VO);

                            double H11 = H[I, J];
                            double C01 = E01[I, J];
                            double C00 = E00[I, J];
                            double C11 = E11[I, J];
                            double C10 = E10[I, J];
                            double Z = H11 / 2.0;
                            double HP = HUO;
                            double A1 = (1.0 / C01 + 1.0 / C00) / 2.0;
                            double A2 = (1.0 / C01 - 1.0 / C00) / 2.0;
                            double B10 = (C11 / C01 + C10 / C00) / 2.0;
                            double B11 = (C11 / C01 - C10 / C00) / 2.0;
                            double M01 = 0.0;

                            double EXX1 = V2 * A1 * T1 + A2 * T2 + B10 * D2X + V2 * B11 * D2Y + M01 / C01;
                            double EYY1 = V2 * A2 * T1 + A1 * T2 + B11 * D2X + V2 * B10 * D2Y + M01 / C01;
                            double EXY1 = (2.0 * S / C00 + 2.0 * (B10 - B11) * D2XY) * V22;
                            double EXX = EXX1 - Z * D2X;
                            double EYY = EYY1 - Z * D2Y;
                            double EXY = -2.0 * V22 * Z * D2XY + EXY1;
                            double EZZ = -HP * (EXX + EYY) / (1.0 - HP);
                            double EI = Math.Sqrt(2.0 * ((EXX - EYY) * (EXX - EYY) + (EYY - EZZ) * (EYY - EZZ) +
                                                (EXX - EZZ) * (EXX - EZZ) + 3 * EXY * EXY / 2.0)) / 3.0;

                            double ENN = EN[I, J];
                            double NAPXX = ENN * (EXX + HUO * EYY) / (1.0 - HUO * HUO);
                            double NAPYY = ENN * (EYY + HUO * EXX) / (1.0 - HUO * HUO);
                            double NAPXY = ENN * EXY / (2.0 * (1.0 + HUO));

                            double WEN = (NAPXX + NAPYY) / 2.0;
                            double MVO = (NAPXX * NAPXX + NAPYY * NAPYY - 2.0 * HUO * NAPXX * NAPYY +
                                  2.0 * (1.0 + HUO) * (1.0 + HUO)) / (2.0 * EO);
                            SVO[I, J] = MVO;

                            SEN[I, J] = WEN;
                            DEF[I, J] = EI;

                            double SI;
                            if (JF == 0) SI = 3 * EI;
                            else SI = SG(EI);

                            SQ[I, J] = SI;
                            double DH = (1 + K * SI) * (FI2 - FI1);
                            SP[I, J] = DH;
                            H[I, J] -= DH;
                        }
                    }

                    file1.WriteLine("H1=");

                    for (int I3 = 0; I3 < N1; I3 += 2)
                    {
                        for (int J3 = 0; J3 < N1; J3 += 2)
                        {
                            file1.Write($"{H[I3, J3],12:F5}");
                        }
                        file1.WriteLine();
                    }

                    double HSP = H[N1, N1];
                    file1.WriteLine($" ТОЛЩИНА = {HSP,12:F5}");
                    file222.WriteLine($" ТОЛЩИНА = {HSP,12:F5}");

                    SURFH();

                    double VOL = Volume(H, N);

                    file1.WriteLine($" ОБЪЕМ ПЛАСТИНКИ = {VOL,6:F3}");
                    file222.WriteLine($" ОБЪЕМ ПЛАСТИНКИ = {VOL,6:F3}");
                    Console.WriteLine($" ОБЪЕМ ПЛАСТИНКИ = {VOL,6:F3}");

                    double PRO = PROGIB(WXY, N);
                    file1.WriteLine($" ПРОГИБ ПО ПЛАНУ = {PRO,6:F3}");
                    file222.WriteLine($" ПРОГИБ ПО ПЛАНУ = {PRO,6:F3}");
                    Console.WriteLine($" ПРОГИБ ПО ПЛАНУ = {PRO,6:F3}");

                    file1.WriteLine("ИНТЕНСИВНОСТЬ=");
                    Console.WriteLine("ИНТЕНСИВНОСТЬ=");

                    for (int I3 = 0; I3 < N1; I3 += 2)
                    {
                        for (int J3 = 0; J3 < N1; J3 += 2)
                        {
                            file1.Write($"{SQ[I3, J3],12:F5}");
                        }
                        file1.WriteLine();
                    }

                    SURFSI();

                    file1.WriteLine("ЭНЕРГИЯ=");
                    Console.WriteLine("ЭНЕРГИЯ=");

                    for (int I3 = 0; I3 < N1; I3 += 2)
                    {
                        for (int J3 = 0; J3 < N1; J3 += 2)
                        {
                            file1.Write($"{SVO[I3, J3],12:F5}");
                        }
                        file1.WriteLine();
                    }

                    file1.WriteLine("СРЕДНЕЕ=");
                    Console.WriteLine("СРЕДНЕЕ=");

                    for (int I3 = 0; I3 < N1; I3 += 2)
                    {
                        for (int J3 = 0; J3 < N1; J3 += 2)
                        {
                            file1.Write($"{SEN[I3, J3],12:F5}");
                        }
                        file1.WriteLine();
                    }

                    double WSP = WXY[N2 - 1, N2 - 1];
                    file1.WriteLine($"ПРОГИБ В ЦЕНТРЕ={WSP}");
                    file222.WriteLine($"ПРОГИБ В ЦЕНТРЕ={WSP}");

                    file1.WriteLine("WXY=");
                    Console.WriteLine("WXY=");

                    SURFW();

                    for (int I3 = 1; I3 < N2; I3 += 2)
                    {
                        for (int J3 = 1; J3 < N2; J3 += 2)
                        {
                            file1.Write($"{WXY[I3, J3],12:F5}");
                        }
                        file1.WriteLine();
                    }
                } 
                while (T - TM < 0);

                return;
            }
        }
    }
}

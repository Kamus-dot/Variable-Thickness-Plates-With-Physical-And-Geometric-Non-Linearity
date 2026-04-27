using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_nd_Model
{
    public partial class ModelTwo
    {
        public static void DIFFUZE(Func<double, double> SG)
        {
            double[,] SQ = new double[12, 12];
            double[,] SVO = new double[12, 12];
            int N2, N1;
            int ICON = 8;
            double A = 0.044;
            double B = 0.0153;
            double BET = 0.2;
            double K = 1.65;
            double ALFA = 0.048;
            double NU = 0.091;
            double GAM = 0.362;
            double VEO = 0.0217;
            double KSI = 0.00022;
            double TM = 0.64;
            double DELT = 0.08;
            double V22 = Math.Sqrt(V2);
            Q = 2.56;

            using (StreamWriter file1 = new StreamWriter("T.DAT"))
            using (StreamWriter file2 = new StreamWriter("W.DAT"))
            using (StreamWriter file3 = new StreamWriter("H.DAT"))
            using (StreamWriter file6 = new StreamWriter("SI.DAT"))
            using (StreamWriter file222 = new StreamWriter("OUTPUT.DAT"))
            {
                Thick();
                TempQ(ICON);

                double T = 0.0;
                double FI1, T1, T2, FI2;

                do
                {
                    FI1 = (Math.Exp(NU * T)) / NU;
                    T += DELT;
                    T1 = T;
                    FI2 = (Math.Exp(NU * T1)) / NU;

                    if (JF == 0)
                    {
                        INTEGR(1, PLAST);
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

                    if (JF == 0 && IL == 1)
                    {
                        Var();
                    }

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

                            DEF[I, J] = EI;
                            double SI = (JF == 0) ? 3 * EI : SG(EI);
                            SQ[I, J] = SI;

                            double ENN = EN[I, J];
                            double NAPXX = ENN * (EXX + HUO * EYY) / (1.0 - HUO * HUO);
                            double NAPYY = ENN * (EYY + HUO * EXX) / (1.0 - HUO * HUO);
                            double NAPXY = ENN * EXY / (2.0 * (1.0 + HUO));

                            double WEN = (NAPXX + NAPYY) / 2.0;
                            double MVO = (NAPXX * NAPXX + NAPYY * NAPYY + 2.0 * HUO * NAPXX * NAPYY +
                                  2.0 * (1.0 + HUO) * (1.0 + HUO)) / (2.0 * EO);
                            SVO[I, J] = MVO;

                            double KEN = GAM * WEN;
                            double MVT = Math.Exp(KEN);
                            SVT[I, J] = MVT;
                            SEN[I, J] = WEN;

                            double DH = ALFA * MVT * (FI2 - FI1);
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

                    SURFSI();

                    for (int I3 = 0; I3 < N1; I3 += 2)
                    {
                        for (int J3 = 0; J3 < N1; J3 += 2)
                        {
                            file1.Write($"{SEN[I3, J3],12:F5}");
                        }
                        file1.WriteLine();
                    }

                    file1.WriteLine("WXY=");
                    Console.WriteLine("WXY=");

                    for (int I3 = 1; I3 < N2; I3 += 2)
                    {
                        for (int J3 = 1; J3 < N2; J3 += 2)
                        {
                            file1.Write($"{WXY[I3, J3],12:F5}");
                        }
                        file1.WriteLine();
                    }

                    SURFW();
                    //152 строка
                } while (T - TM < 0);

                using (StreamWriter file12 = new StreamWriter("H1.DAT"))
                using (StreamWriter file13 = new StreamWriter("H2.DAT"))
                using (StreamWriter file14 = new StreamWriter("H3.DAT"))
                {
                    double hx = 0.0;
                    for (int i = 0; i < 9; i++)
                    {
                        double zzz = hx;
                        file12.WriteLine($"{zzz,4:F1}{H[9, i],14:F6}");
                        file13.WriteLine($"{zzz,4:F1}{H[i, 9],14:F6}");
                        file14.WriteLine($"{zzz,4:F1}{H[i, i],14:F6}");
                        hx += 1.0;
                    }
                }

                using (StreamWriter file22 = new StreamWriter("S1.DAT"))
                using (StreamWriter file23 = new StreamWriter("S2.DAT"))
                using (StreamWriter file24 = new StreamWriter("S3.DAT"))
                {
                    double hx = 0;
                    for (int i = 0; i < 9; i++)
                    {
                        double zzz = hx;
                        file22.WriteLine($"{zzz,4:F1}{H[9, i],14:F6}");
                        file23.WriteLine($"{zzz,4:F1}{H[i, 9],14:F6}");
                        file24.WriteLine($"{zzz,4:F1}{H[i, i],14:F6}");
                        hx += 1.0;
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public static void INTEGER(int IP, Func<double, double> SIG)
        {
            double[] S1 = new double[17];
            double[] S2 = new double[17];
            double[] S3 = new double[17];
            double[] S4 = new double[17];
            double[] S5 = new double[17];
            double[] S6 = new double[17];
            
            double HH = 0;
            int N1 = N + 1;
            int NT1 = NT + 1;
            int N2 = N + 2;

            if (IP != 1)
            {
                double V22 = Math.Sqrt(V2);
                double GOM = 3.0 * OMY;
                EMIN = EO;

                for (int I = 0; I < N1; I++)
                {
                    int I1 = I + 1;
                    int I2 = I + 2;

                    for (int J = 0; J < N1; J++)
                    {
                        int J1 = J + 1;
                        int J2 = J + 2;

                        // Расчёт деформаций срединной поверхности

                        // Направление в сторону оси X
                        double D2X = (WXY[I2, J1] - 2.0 * WXY[I1, J1] + WXY[I, J1]) / VO -
                                    (W0[I2, J1] - 2.0 * W0[I1, J1] + W0[I, J1]) / VO;

                        // Направление в сторону оси Y
                        double D2Y = (WXY[I1, J2] - 2.0 * WXY[I1, J1] + WXY[I1, J]) / VO -
                                    (W0[I1, J2] - 2.0 * W0[I1, J1] + W0[I1, J]) / VO;

                        double D2XY = (WXY[I2, J2] - WXY[I2, J] - WXY[I, J2] + WXY[I, J]) / (4.0 * VO) -
                                     (W0[I2, J2] - W0[I2, J] - W0[I, J2] + W0[I, J]) / (4.0 * VO);


                        double T2 = (FXY[I2, J1] - 2.0 * FXY[I1, J1] + FXY[I, J1]) / VO + QL * QK;
                        double T1 = (FXY[I1, J2] - 2.0 * FXY[I1, J1] + FXY[I1, J]) / VO + QL;
                        double S = -(FXY[I2, J2] - FXY[I2, J] - FXY[I, J2] + FXY[I, J]) / (4.0 * VO);

                        double H11 = H[I, J];
                        double C01 = E01[I, J];
                        double C00 = E00[I, J];
                        double C11 = E11[I, J];
                        double C10 = E10[I, J];

                        double A1 = (1.0 / C01 + 1.0 / C00) / 2.0;
                        double A2 = (1.0 / C01 - 1.0 / C00) / 2.0;
                        double B10 = (C11 / C01 + C10 / C00) / 2.0;
                        double B11 = (C11 / C01 - C10 / C00) / 2.0;

                        double M01 = 0.0;

                        // Вычисление деформаций
                        // V2 - коэффициент, необходимый для сохранения одинаковой раз-
                        // мерности сетки
                        double EXX1 = V2 * A1 * T1 + A2 * T2 + B10 * D2X + V2 * B11 * D2Y + M01 / C01;
                        double EYY1 = V2 * A2 * T1 + A1 * T2 + B11 * D2X + V2 * B10 * D2Y + M01 / C01;
                        double EXY1 = (2.0 * S / C00 + 2.0 * (B10 - B11) * D2XY) * V22;

                        // Толщина 1-ого слоя
                        double HZ = H11 / NT;

                        // Начальная координата 
                        double Z = (HH - 1.0) * H11 / 2.0;

                        int IS = 0;
                        double E = 0;

                        for (int K = 0; K < NT1; K++)
                        {
                            double HP = 0.5 ; // - Коэффициент Пуассона 

                            // Деформация с учётом кривизны
                            double EXX = EXX1 - Z * D2X;
                            double EYY = EYY1 - Z * D2Y;
                            double EXY = -2.0 * V22 * Z * D2XY + EXY1;

                            // Поперечная деформация
                            double EZZ = -HP * (EXX + EYY) / (1.0 - HP);

                            // Интенсивность деформаций
                            double EI = Math.Sqrt(2 * (Math.Pow((EXX - EYY), 2) + Math.Pow((EYY - EZZ), 2) +
                                        Math.Pow((EXX - EZZ), 2)) + 3 * Math.Pow(EXY, 2) / 2.0) / 3.0;

                            // Секущий модуль
                            double G = 0;

                            if (EI > ES)
                            {
                                IS++;
                                G = SIG(EI) / (3 * EI);
                            }
                            else
                            {
                                // Упругая область 
                                if (EI <= 1e-10)
                                {
                                    G = SS / (3.0 * ES);
                                }
                                else
                                {
                                    G = SIG(EI) / (3 * EI);
                                }
                            }

                            double GOK = GOM + G;
                            // Эффективный модуль
                            E = 3 * GOM * G / GOK;

                            double R1 = E / (1.0 + HP);
                            double R2 = E / (1.0 - HP);

                            S1[K] = R1;
                            S2[K] = R2;
                            S3[K] = R1 * Z;
                            S4[K] = R2 * Z;
                            S5[K] = R2 * Z * Z;
                            S6[K] = R1 * Z * Z;
                            Z += HZ;
                        }

                        EN[I, J] = E;
                        if (E < EMIN) EMIN = E;

                        E00[I, J] = SIM(S1, H11, NT);
                        E01[I, J] = SIM(S2, H11, NT);
                        E10[I, J] = SIM(S3, H11, NT);
                        E11[I, J] = SIM(S4, H11, NT);
                        E21[I, J] = SIM(S5, H11, NT);
                        E20[I, J] = SIM(S6, H11, NT);
                    }
                }
            }
            else
            {
                double G11 = EO / (1 + HUO);
                double G21 = EO / (1 - HUO);

                for (int I = 0; I < N1; I++)
                {
                    for (int J = 0; J < N1; J++)
                    {
                        // Коэффициент ослабления
                        double F = 1.0 - (1.0 - EV) * QR(I, J, IV);
                        double G1 = G11 * F;
                        double G2 = G21 * F;
                        double H11 = H[I, J];
                        double PHH = Math.Pow(H11, 3) / (3.0 * Math.Pow(2.0 - HH, 2));

                        E00[I, J] = G1 * H11;
                        E01[I, J] = G2 * H11;
                        E10[I, J] = G1 * H11 * H11 * HH / 2.0;
                        E11[I, J] = G2 * H11 * H11 * HH / 2.0;
                        E21[I, J] = G2 * PHH;
                        E20[I, J] = G1 * PHH;
                        EN[I, J] = EO;
                    }
                }

                EMIN = EO;
            }

            // Экстраполяция значений матриц жесткости на граничные узлы
            for (int I = 0; I < N2; I++)
            {
                E00[I, N2 - 1] = E00[I, N - 1];
                E00[N2 - 1, I] = E00[N - 1, I];
                E01[I, N2 - 1] = E01[I, N - 1];
                E01[N2 - 1, I] = E01[N - 1, I];
                E10[I, N2 - 1] = E10[I, N - 1];
                E10[N2 - 1, I] = E10[N - 1, I];
                E11[I, N2 - 1] = E11[I, N - 1];
                E11[N2 - 1, I] = E11[N - 1, I];
                E21[I, N2 - 1] = E21[I, N - 1];
                E21[N2 - 1, I] = E21[N - 1, I];
                E20[I, N2 - 1] = E20[I, N - 1];
                E20[N2 - 1, I] = E20[N - 1, I];
                EN[I, N2 - 1] = EN[I, N - 1];
                EN[N2 - 1, I] = EN[N - 1, I];
            }
        }
    }
}

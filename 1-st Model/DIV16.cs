using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public void INTEGER(int IP, Func<double, double> SIG)
        {
            double[] S1 = new double[17];
            double[] S2 = new double[17];
            double[] S3 = new double[17];
            double[] S4 = new double[17];
            double[] S5 = new double[17];
            double[] S6 = new double[17];

            int N1 = N + 1;
            int NT1 = NT + 1;
            int N2 = N + 2;

            if (IP != 1)
            {
                double V22 = Math.Sqrt(Lambda);
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

                        double D2X = (WXY[I2, J1] - 2 * WXY[I1, J1] + WXY[I, J1]) / V2;
                        double D2Y = (WXY[I1, J2] - 2 * WXY[I1, J1] + WXY[I1, J]) / V2;
                        double D2XY = (WXY[I2, J2] - WXY[I2, J] - WXY[I, J2] + WXY[I, J]) / (4 * V2);

                        double D2PHIX = (Corrosion[I2, J1] - 2 * Corrosion[I1, J1] + Corrosion[I, J1]) / V2;
                        double D2PHIY = (Corrosion[I1, J2] - 2 * Corrosion[I1, J1] + Corrosion[I1, J]) / V2;
                        double D2PHIXY = (Corrosion[I2, J2] - Corrosion[I2, J] - Corrosion[I, J2] +
                            Corrosion[I, J]) / (4 * V2);

                        double D2FX = (FXY[I2, J1] - 2 * FXY[I1, J1] + FXY[I, J1]) / V2 + QL * QK;
                        double D2FY = (FXY[I1, J2] - 2 * FXY[I1, J1] + FXY[I1, J]) / V2 + QL;
                        double D2FXY = -(FXY[I2, J2] - FXY[I2, J] - FXY[I, J2] + FXY[I, J]) / (4 * V2);

                        double A_tilde = 1 / (Pow(A[I, J], 2) - Pow(A_nu[I, J], 2));

                        // Вычисление безразмерных деформаций

                        double EX_Memb = A_tilde * (V22 * A[I, J] * D2FX - A_nu[I, J] * D2FY);
                        double EY_Memb = A_tilde * (A[I, J] * D2FY - V22 * A_nu[I, J] * D2FX);
                        double EXY_Memb = Lambda * D2FXY / B[I, J];

                        // Толщина 1-ого слоя
                        double HZ = H[I, J] / NT;

                        // Начальная координата 
                        double Z = -H[I, J] / 2;

                        for (int K = 0; K < NT1; K++)
                        {
                            // Деформация с учётом кривизны
                            double EX = EX_Memb - Z * (D2X + 0.5 * D2PHIX);
                            double EY = EY_Memb - Z * (D2Y + 0.5 * D2PHIY);
                            double EXY = -2 * Z * (D2XY + 0.5 * D2PHIY) + EXY_Memb;

                            // Интенсивность деформаций
                            double EI = Sqrt(2 * (Pow((EX), 2) + Pow((EY), 2) + EX * EY
                                + Pow(EXY, 2) / 4)) / 3;
                            DEF_INT[I, J] = EI;

                            double HP = 0; // - Коэффициент Пуассона 
                            double G = 0; // - Модуль сдвига
                            double E = 0; // - Модуль упругости  

                            // Упругая область 
                            if (EI <= 1e-10)
                            {
                                G = SS / (3 * ES);
                            }
                            else
                            {
                                G = SIG(EI) / (3 * EI);
                            }

                            double GOK = GOM + G;

                            E = 3 * GOM * G / GOK;
                            HP = (GOM - 2 * G) / 2 * (2 * OMY + G);

                            double R1 = E / (1 - Pow(HP, 2));
                            double R2 = E / (1 + HP);

                            S1[K] = R1;
                            S2[K] = R1 * HP;
                            S3[K] = R1 * Pow(Z, 2);
                            S4[K] = R1 * Pow(Z, 2) * HP;
                            S5[K] = R2 / 2;
                            S6[K] = R2 * Pow(Z, 2);
                            Z += HZ;
                        }

                        A[I, J] = SIM(S1, H[I, J], NT);
                        A_nu[I, J] = SIM(S2, H[I, J], NT);
                        AZ[I, J] = SIM(S3, H[I, J], NT);
                        AZ_nu[I, J] = SIM(S4, H[I, J], NT);
                        B[I, J] = SIM(S5, H[I, J], NT);
                        BZ[I, J] = SIM(S6, H[I, J], NT);
                    }
                }
            }
            else
            {
                for (int I = 0; I < N1; I++)
                {
                    for (int J = 0; J < N1; J++)
                    {
                        A[I, J] = H[I, J] * EO / (1 - Pow(HUO, 2));
                        A_nu[I, J] = H[I, J] * EO * HUO / (1 - Pow(HUO, 2));
                        AZ[I, J] = Pow(H[I, J], 3) * EO / (3 * (1 - Pow(HUO, 2)));
                        AZ_nu[I, J] = Pow(H[I, J], 3) * EO * HUO / (1 - Pow(HUO, 2));
                        B[I, J] = H[I, J] * EO / (2 * (1 + HUO));
                        BZ[I, J] = Pow(H[I, J], 3) * EO / (3 * (1 + HUO));
                        EN[I, J] = EO;
                    }
                }
                EMIN = EO;
            }

            // Экстраполяция значений матриц жесткости на граничные узлы
            for (int I = 0; I < N2; I++)
            {
                A[I, N1] = A[I, N - 1];
                A[N1, I] = A[N - 1, I];
                A_nu[I, N1] = A_nu[I, N - 1];
                A_nu[N1, I] = A_nu[N - 1, I];
                AZ[I, N1] = AZ[I, N - 1];
                AZ[N1, I] = AZ[N - 1, I];
                AZ_nu[I, N1] = AZ_nu[I, N - 1];
                AZ_nu[N1, I] = AZ_nu[N - 1, I];
                B[I, N1] = B[I, N - 1];
                B[N1, I] = B[N - 1, I];
                BZ[I, N1] = BZ[I, N - 1];
                BZ[N1, I] = BZ[N - 1, I];
                EN[I, N1] = EN[I, N - 1];
                EN[N1, I] = EN[N - 1, I];
            }
        }
    }
}

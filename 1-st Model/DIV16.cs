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
            int N3 = N + 3;
            int N4 = N + 4;
            int N_1 = N - 1;

            if (IP != 1)
            {
                double V22 = Sqrt(Lambda);
                double GOM = 3.0 * OMY;
                EMIN = EO;
                DeformationIntesivity();

                for (int I = 0; I < N1; I++)
                {
                    int I1 = I + 1;

                    for (int J = 0; J < N1; J++)
                    {
                        int J1 = J + 1;

                        // Толщина 1-ого слоя
                        double HZ = H[I, J] / NT;

                        // Начальная координата 
                        double Z = -H[I, J] / 2;

                        double E = 0; // - Модуль упругости  

                        for (int K = 0; K < NT1; K++)
                        {
                            // Деформация с учётом кривизны
                            double EI = DEF_INT[I, J, K];
                            double HP = 0; // - Коэффициент Пуассона 
                            double G = 0; // - Модуль сдвига

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
                        if (E < EMIN) EMIN = E;

                        A[I1, J1] = SIM(S1, H[I, J], NT);
                        A_nu[I1, J1] = SIM(S2, H[I, J], NT);
                        AZ[I1, J1] = SIM(S3, H[I, J], NT);
                        AZ_nu[I1, J1] = SIM(S4, H[I, J], NT);
                        B[I1, J1] = SIM(S5, H[I, J], NT);
                        BZ[I1, J1] = SIM(S6, H[I, J], NT);
                    }
                }
            }
            else
            {
                for (int I = 0; I < N1; I++)
                {
                    int I1 = I + 1;
                    for (int J = 0; J < N1; J++)
                    {
                        int J1 = J + 1;

                        A[I1, J1] = H[I, J] * EO / (1 - Pow(HUO, 2));
                        A_nu[I1, J1] = H[I, J] * EO * HUO / (1 - Pow(HUO, 2));
                        AZ[I1, J1] = Pow(H[I, J], 3) * EO / (3 * (1 - Pow(HUO, 2)));
                        AZ_nu[I1, J1] = Pow(H[I, J], 3) * EO * HUO / (1 - Pow(HUO, 2));
                        B[I1, J1] = H[I, J] * EO / (2 * (1 + HUO));
                        BZ[I1, J1] = Pow(H[I, J], 3) * EO / (3 * (1 + HUO));
                    }
                }
                EMIN = EO;
            }

            // Экстраполяция значений матриц жесткости на граничные узлы
            for (int I = 0; I < N4; I++)
            {
                A[I, N2] = A[I, N];
                A[N2, I] = A[N, I];
                AZ[I, N2] = AZ[I, N];
                AZ[N2, I] = AZ[N, I];
                A_nu[I, N2] = A_nu[I, N];
                A_nu[N2, I] = A_nu[N, I];
                AZ_nu[I, N2] = AZ_nu[I, N];
                AZ_nu[N2, I] = AZ_nu[N, I];
                B[I, N2] = B[I, N];
                B[N2, I] = B[N, I];
                BZ[I, N2] = BZ[I, N];
                BZ[N2, I] = BZ[N, I];
                A[N3, I] = A[N_1, I];
                A[I, N3] = A[I, N_1];
                AZ[N3, I] = AZ[N_1, I];
                AZ[I, N3] = AZ[I, N_1];
                A_nu[N3, I] = A_nu[N_1, I];
                A_nu[I, N3] = A_nu[I, N_1];
                AZ_nu[N3, I] = AZ_nu[N_1, I];
                AZ_nu[I, N3] = AZ_nu[I, N_1];
                B[N3, I] = B[N_1, I];
                B[I, N3] = B[I, N_1];
                BZ[N3, I] = BZ[N_1, I];
                BZ[I, N3] = BZ[I, N_1];
                A[I, 0] = 2.0 * A[I, 1] - A[I, 2];
                AZ[I, 0] = 2.0 * AZ[I, 1] - AZ[I, 2];
                A_nu[I, 0] = 2.0 * A_nu[I, 1] - A_nu[I, 2];
                AZ_nu[I, 0] = 2.0 * AZ_nu[I, 1] - AZ_nu[I, 2];
                B[I, 0] = 2.0 * B[I, 1] - B[I, 2];
                BZ[I, 0] = 2.0 * BZ[I, 1] - BZ[I, 2];
                A[0, I] = 2.0 * A[1, I] - A[2, I];
                AZ[0, I] = 2.0 * AZ[1, I] - AZ[2, I];
                A_nu[0, I] = 2.0 * A_nu[1, I] - A_nu[2, I];
                AZ_nu[0, I] = 2.0 * AZ_nu[1, I] - AZ_nu[2, I];
                BZ[0, I] = 2.0 * BZ[1, I] - BZ[2, I];
                B[0, I] = 2.0 * B[1, I] - B[2, I];
            }
            A[0, 0] = 2.0 * A[0, 1] - A[0, 2];
            AZ[0, 0] = 2.0 * AZ[0, 1] - AZ[0, 2];
            A_nu[0, 0] = 2.0 * A_nu[0, 1] - A_nu[0, 2];
            AZ_nu[0, 0] = 2.0 * AZ_nu[0, 1] - AZ_nu[0, 2];
            B[0, 0] = 2.0 * B[0, 1] - B[0, 2];
            BZ[0, 0] = 2.0 * BZ[0, 1] - BZ[0, 2];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Рассчитывает физические свойства пластины
        /// </summary>
        /// <param name="JF">Флаг физической линейности</param>
        /// <param name="SIG">Зависимость напряжения от деформации</param>
        public void INTEGER(double omega, Func<double, double> SIG)
        {

            int N1 = N + 1;
            int NT1 = NT + 1;

            double[] S1 = new double[NT1];
            double[] S2 = new double[NT1];
            double[] S3 = new double[NT1];
            double[] S4 = new double[NT1];
            double[] S5 = new double[NT1];
            double[] S6 = new double[NT1];
            double GOM = 3 * OMY;
            EMIN = EO;
            DeformationIntesivity();

            if (CorrosionModel == CorrosionModels.Gutman)
            {
                MaxAVG = 0;
                HMaxAVG = 0;
            }

            for (int I = 0; I < N1; I++)
            {
                int I1 = I + 1;

                for (int J = 0; J < N1; J++)
                {
                    int J1 = J + 1;

                    // Толщина 1-ого слоя
                    double HZ = H[I, J] / NT;

                    // Начальная координата 
                    double Z = -H0 / 2;

                    double E; // - Модуль упругости  

                    for (int K = 0; K < NT1; K++)
                    {
                        // Деформация с учётом кривизны
                        double EI = DEF_INT[I, J, K];
                        double HP; // - Коэффициент Пуассона 
                        double G; // - Модуль сдвига

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
                        E = (1 - omega) * Resilience[I, J, K] + omega * E;
                        Resilience[I, J, K] = E;
                        //HP = (GOM - 2 * G) / (2 * (2 * OMY + G));
                        double G_relax = E * GOM / (3.0 * GOM - E);
                        HP = (GOM - 2.0 * G_relax) / (2.0 * (2.0 * OMY + G_relax));

                        double R1 = E / (1 - Pow(HP, 2));
                        double R2 = E / (1 + HP);
                        if (CorrosionModel == CorrosionModels.Gutman && K == NT)
                        {
                            double SigmaX, SigmaY, AvgTens;
                            SigmaX = R1 * (EX[I, J, K] + HP * EY[I, J, K]);
                            SigmaY = R1 * (EY[I, J, K] + HP * EX[I, J, K]);
                            AvgTens = (SigmaX + SigmaY) / 3;
                            TENS_AVG[I, J] = AvgTens;
                            if (AvgTens > MaxAVG)
                            {
                                MaxAVG = AvgTens;
                                HMaxAVG = H[I, J];
                            }
                        }

                        S1[K] = R1;
                        S2[K] = R1 * HP;
                        S3[K] = R1 * Pow(Z, 2);
                        S4[K] = R1 * Pow(Z, 2) * HP;
                        S5[K] = R2 / 2;
                        S6[K] = R2 * Pow(Z, 2);
                        Z += HZ;
                        if (E < EMIN) EMIN = E;
                    }

                    A[I1, J1] = SIM(S1, H[I, J], NT);
                    A_nu[I1, J1] = SIM(S2, H[I, J], NT);
                    AZ[I1, J1] = SIM(S3, H[I, J], NT);
                    AZ_nu[I1, J1] = SIM(S4, H[I, J], NT);
                    B[I1, J1] = SIM(S5, H[I, J], NT);
                    BZ[I1, J1] = SIM(S6, H[I, J], NT);
                }
            }
            Extrapolation();
        }

        public void INTEGER()
        {
            int N1 = N + 1;
            for (int I = 0; I < N1; I++)
            {
                int I1 = I + 1;
                for (int J = 0; J < N1; J++)
                {
                    int J1 = J + 1;
                    for (int K = 0; K < NT + 1; K++) { Resilience[I, J, K] = EO; }

                    A[I1, J1] = H[I, J] * EO / (1 - Pow(HUO, 2));
                    A_nu[I1, J1] = H[I, J] * EO * HUO / (1 - Pow(HUO, 2));
                    AZ[I1, J1] = Pow(H[I, J], 3) * EO / (3 * (1 - Pow(HUO, 2)));
                    AZ_nu[I1, J1] = Pow(H[I, J], 3) * EO * HUO / (1 - Pow(HUO, 2));
                    B[I1, J1] = H[I, J] * EO / (2 * (1 + HUO));
                    BZ[I1, J1] = Pow(H[I, J], 3) * EO / (3 * (1 + HUO));
                }
            }
            EMIN = EO;
            Extrapolation();
        }
        private void Extrapolation()
        {
            int N1 = N + 1;
            int N2 = N + 2;
            int N3 = N + 3;
            int N4 = N + 4;

            for (int I = 0; I < N4; I++)
            {
                A[I, N2] = A[I, N1];
                A[N2, I] = A[N1, I];
                AZ[I, N2] = AZ[I, N1];
                AZ[N2, I] = AZ[N1, I];
                A_nu[I, N2] = A_nu[I, N1];
                A_nu[N2, I] = A_nu[N1, I];
                AZ_nu[I, N2] = AZ_nu[I, N1];
                AZ_nu[N2, I] = AZ_nu[N1, I];
                B[I, N2] = B[I, N1];
                B[N2, I] = B[N1, I];
                BZ[I, N2] = BZ[I, N1];
                BZ[N2, I] = BZ[N1, I];
                A[N3, I] = A[N, I];
                A[I, N3] = A[I, N];
                AZ[N3, I] = AZ[N, I];
                AZ[I, N3] = AZ[I, N];
                A_nu[N3, I] = A_nu[N, I];
                A_nu[I, N3] = A_nu[I, N];
                AZ_nu[N3, I] = AZ_nu[N, I];
                AZ_nu[I, N3] = AZ_nu[I, N];
                B[N3, I] = B[N, I];
                B[I, N3] = B[I, N];
                BZ[N3, I] = BZ[N, I];
                BZ[I, N3] = BZ[I, N];
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


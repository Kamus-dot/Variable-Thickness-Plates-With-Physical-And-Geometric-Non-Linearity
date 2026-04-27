using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public static void NELMID()
        {
            double ALFA, BETA, GAMA, DIFER, SUMH, SUML, SUMS,
                SUM2, XNX;
            int INDEX, COUNT, NP, NITER;

            NP = 0;
            NITER = 0;
            ALFA = 1.0;
            BETA = 0.5;
            GAMA = 2.0;
            DIFER = 0.0;
            Console.WriteLine($"NX = {NX}");
            XNX = NX;
            IN = 1;
            SUMR();
            KLN = 1;

            Console.WriteLine($"FUNCTION STARTING VALUE {SUM[0]:F5}");
            Console.WriteLine("THE X ARRAY IS:");
            for (int i = 0; i < NX; i++)
            {
                Console.Write($"{X_[i],11:E4} ");
            }
            Console.WriteLine();
            Console.WriteLine($"STEP={STEP:F2}");

            K1 = NX + 1;
            int K2 = NX + 2;
            int K3 = NX + 3;
            int K4 = NX + 4;

            Start();

            for (int I = 0; I < K1; I++)
            {
                for (int J = 0; J < NX; J++)
                {
                    X_[J] = X1[I, J];
                }
                IN = I;
                SUMR();
            }
            do
            {
                SUMH = SUM[0];
                INDEX = 0;
                for (int I = 1; I < K1; I++)
                {
                    if (SUM[I] <= SUMH) continue;
                    SUMH = SUM[I];
                    INDEX = I;
                }

                SUML = SUM[0];
                COUNT = 0;
                for (int I = 1; I < K1; I++)
                {
                    if (SUML <= SUM[I]) continue;
                    SUML = SUM[I];
                    COUNT = I;
                }


                for (int J = 0; J < NX; J++)
                {
                    SUM2 = 0.0;
                    for (int I = 0; I < K1; I++)
                    {
                        SUM2 += X1[I, J];
                    }
                    X1[K2 - 1, J] = (1.0 / XNX) * (SUM2 - X1[INDEX - 1, J]);
                    X1[K3 - 1, J] = (1.0 + ALFA) * X1[K2 - 1, J] - ALFA * X1[INDEX, J];
                    X_[J] = X1[K3 - 1, J];
                }
                IN = K3 - 1;
                SUMR();

                ////////////////////////////////

                if (SUM[K3 - 1] < SUML)
                {
                    for (int J = 0; J < NX; J++)
                    {
                        X1[K4 - 1, J] = (1 - GAMA) * X1[K2 - 1, J] + GAMA * X1[K3 - 1, J];
                        X_[J] = X1[K4 - 1, J];
                    }
                    IN = K4 - 1;
                    SUMR();
                    ////// Здесь 82-83
                    if (SUM[K4 - 1] >= SUML)
                    {
                        goto L14;
                    }
                }
                else
                {

                    if (INDEX == 1)
                    {
                        SUMS = SUM[1];
                    }
                    else
                    {
                        SUMS = SUM[0];
                    }

                    for (int I = 0; I < K1; I++)
                    {
                        if ((INDEX - I) == 0) continue;
                        if (SUM[I] <= SUMS) continue;
                        SUMS = SUM[I];
                    }

                    // 75
                    if (SUM[K3 - 1] <= SUMS)
                    {
                        goto L14;
                    }
                    
                    else
                    {
                        if (SUM[K3 - 1] <= SUMH)
                        {
                            for (int J = 0; J < NX; J++)
                            {
                                X1[INDEX, J] = X1[K3 - 1, J];
                            }
                        }
                        for (int J = 0; J < NX; J++)
                        {
                            X1[K4 - 1, J] = BETA * X1[INDEX, J] + (1 - BETA) * X1[K2 - 1, J];
                            X_[J] = X1[K4 - 1, J];
                        }
                        IN = K4 - 1;
                        SUMR();
                      

                        if (SUMH <= SUM[K4 - 1])
                        {
                            
                            for (int J = 0; J < NX; J++)
                            {
                                for (int I = 0; I < K1; I++)
                                {
                                    X1[I, J] = 0.5 * (X1[I, J] + X1[COUNT, J]);
                                }
                            }

                            for (int I = 0; I < K1; I++)
                            {
                                for (int J = 0; J < NX; J++)
                                {
                                    X_[J] = X1[I, J];
                                }
                                IN = I;
                                SUMR();
                            }
                            goto L26;
                        }
                    }

                    // 16 label - 104 строка
                    for (int J = 0; J < NX; J++)
                    {
                        X1[INDEX, J] = X1[K4 - 1, J];
                        X_[J] = X1[INDEX, J];
                    }
                    IN = INDEX;
                    SUMR();
                    goto L26;
                }

            L14:
               
                for (int J = 0; J < NX; J++)
                {
                    X1[INDEX, J] = X1[K3 - 1, J];
                    X_[J] = X1[INDEX, J];
                }
                IN = INDEX;
                SUMR();
                goto L26;

            L26:
                // Prepare for next iteration
                for (int J = 0; J < NX; J++)
                {
                    X_[J] = X1[K2 - 1, J];
                }
                IN = K2 - 1;
                SUMR();
                DIFER = 0;
                for (int I = 0; I < K1; I++)
                {
                    DIFER += Math.Pow(SUM[I] - SUM[K2 - 1], 2);
                }
                DIFER = (1.0 / XNX) * Math.Sqrt(DIFER);

                NITER++;
                if (NITER > NIT)
                {
                    break;
                }

                NP++;
                if (NP >= NI)
                {
                    NP = 0;
                    double VOL = Volume(H, N);
                    Console.WriteLine($"ОБЪЁМ ПЛАСТИНКИ={VOL:F3}");

                    R = R * SR;
                    if (R > RM1)
                    {
                        R = RM1;
                    }
                }
            }
            while (DIFER > EPS);

            Console.WriteLine($"Final SUML={SUML:E3}");
            for (int J = 0; J < NX; J++)
            {
                Console.Write($"{X1[COUNT, J],10:E3} ");
            }
            Console.WriteLine($"{DIFER,16:E6}");
        }
    }
}

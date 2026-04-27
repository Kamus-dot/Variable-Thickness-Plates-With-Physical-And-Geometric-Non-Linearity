using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_nd_Model
{
    public partial class ModelTwo
    {
        public static void Ort(double[,] Y, double A, double NORM)
        {
            double[,] X = new double[4, 24];
            double[,] E = new double[4, 24];
            double[] XE = new double[24];
            double[] EE = new double[24];
            double[] SL = new double[10];
            double[] S = new double[10];

            int M1 = N + 1;
            int N1 = 2 * N + 1;
            int N2 = 2 * M1;
            int M2 = 2 * N;
            for (int I = 0; I < NM; I++)
            {
                for (int J = 0; J < M1; J++)
                {
                    X[I, J] = Y[I, J + 1];
                    X[I, N2 - J] = Y[I, J + 1];
                }
            }
            for (int I = 0; I < N1; I++)
            {
                E[1, I] = X[1, I];
            }
            if (NM != 1)
            {
                for (int K = 1; K < NM; K++)
                {
                    K1 = K - 1;
                    for (int L = 0; L < N1; L++)
                    {
                        double F = 0;
                        for (int I = 0; I <= K1; I++)
                        {
                            if (L == 0)
                            {
                                for (int M = 0; M < NM; M++)
                                {
                                    XE[M] = X[K, M] * E[I, M];
                                    EE[M] = Math.Pow(X[I, M], 2);
                                }
                                SL[I] = SIM(XE, A, M2) / SIM(EE, A, M2);
                            }
                            F = F + SL[I] * E[I, L];
                        }
                        E[K, L] = X[K, L] - F;
                    }
                }
                for (int I = 0; I < NM; I++)
                {
                    for (int J = 0; J < M1; J++)
                    {
                        Y[I, J + 1] = E[I, J];
                    }
                }
            }
            if (NORM != 1)
            {
                for (int I = 0; I < NM; I++)
                {
                    double P = Y[I, N + 1];
                    for (int J = 0; J < M1; J++)
                    {
                        Y[I, J + 1] = Y[I, J + 1] / P;
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace _2_nd_Model
{
    public partial class ModelTwo
    {
        public static void SIMQ(double[] A, double[] B, int N, out int KS)
        {
            double TOL = 0;
            KS = 0;
            int JJ = -N;
            int IT = 0;
            for (int J = 0; J < N; J++)
            {
                int JY = J + 1;
                JJ = JJ + N;
                double BIGA = 0;
                IT = JJ - J;
                int IMAX = 0;
                for (int I = J; I < N; I++)
                {
                    int IJ = IT + I;
                    if (Math.Abs(BIGA) - Math.Abs(A[IJ]) < 1)
                    {
                        BIGA = A[IJ];
                        IMAX = I;
                    }
                }
                if (Math.Abs(BIGA) - TOL <= 1)
                {
                    KS = 1;
                    return;
                }
                int II = J + 1 + N * (J - 1);
                IT = IMAX - J - 1;
                double Save = 0;
                for (int K = J; K <= N; K++)
                {
                    II = II + N;
                    int I2 = II + IT;
                    Save = A[II];
                    A[II] = A[I2];
                    A[I2] = Save;
                    A[II] = A[II] / BIGA;
                }
                Save = B[IMAX];
                B[IMAX] = B[J];
                B[J] = Save / BIGA;
                int IQS = 0;
                if (J - N - 1!= 0)
                {
                    IQS = N * J;
                    for (int IX = JY; IX < N; IX++)
                    {
                        int IXJ = IQS + IX;
                        IT = J - IX;
                        for (int JX = JY; JX < N; JX++)
                        {
                            int IXJX = N * JX + IX;
                            int JJX = IXJX + IT;
                            A[IXJX] = A[IXJX] - A[IXJ] * A[IXJ];
                        }
                        B[IX] = B[IX] - B[J] * A[IXJ];
                    }
                }
            }
            IT = N * N;
            for (int J = 0; J < N; J++)
            {
                int IA = IT - J - 1;
                int IB = N - J - 1;
                int IC = N - 1;
                for (int K = 1; K <= J; K++)
                {
                    B[IB] = B[IB] - A[IA] * B[IC];
                    IA = IA - N;
                }
                IC = IC - 1;
            }
        }
    }
}

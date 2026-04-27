using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public static void SUMR()
        {
            double[,] CMN = new double[10, 10];
            double[,] SIGMA = new double[14, 14];
            int N1, NR1, NS1, K, M, I, J;
            double HEV, VOL, SS;

            N1 = N + 1;
            NR1 = NR + 1;
            NS1 = NS + 1;
            K = 0;

            for (I = 0; I < N1; I++)
            {
                for (J = 0; J < N1; J++)
                {
                    H[I, J] = H1;
                }
            }

            M = N - 2;
            for (I = M; I < N1; I++)
            {
                for (J = 0; J <= M; J++)
                {
                    H[I, J] = H1 + Math.Abs(X_[J]);
                    H[J, I] = H[I, J];
                }
            }

            H[N1 - 1, N1 - 1] = H[M, M];
            H[N1 - 1, N - 1] = H[M, M];
            H[N - 1, N1 - 1] = H[M, M];
            H[N - 1, N - 1] = H[M, M];

            if (KLN == 1 || KLN == 3)
            {
                Param();
            }
            else
            {
                Par();
            }

            Defnap(PLAST, 0, 0);

            for (I = 0; I < N1; I++)
            {
                for (J = 0; J < N1; J++)
                {
                    SS = STR[I, J];
                    SIGMA[I, J] = 0.0;
                    if (SS > SIG)
                    {
                        SIGMA[I, J] = SS - SIG;
                    }
                }
            }

            HEV = Volume(SIGMA, N);
            VOL = Volume(H, N);

            SUM[IN] = VOL + R * HEV;

        }   
    }
}

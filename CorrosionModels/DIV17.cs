using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        public virtual void Start()
        {

            int N1 = N + 1;
            int M1 = N + 3;

            for (int i = 0; i < N1; i++)
            {
                for (int j = 0; j < N1; j++)
                {
                    H[i, j] = H0;
                }
            }

            double SL = 0.5 / N;
            double SP = Math.PI * SL;

            for (int K = 0; K < NM; K++)
            {
                int KL = 2 * K + 1;
                for (int I = 0; I < M1; I++)
                {
                    double P = Math.Sin(KL * I * SP) / KL;
                    X[K, I + 1] = P;
                    XF[K, I + 1] = P;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_nd_Model
{
    public partial class ModelTwo
    {
        public static void W00(double S)
        {
            static double X(double p) => Math.Sin(Math.PI * p);
            static double Y(double p) => Math.Sin(Math.PI * p);

            int N2 = N + 2;
            int N4 = N + 4;
            double SL = 0.5 / N;

            // Convert to 1-based indexing to match Fortran
            for (int i = 0; i < N4; i++)
            {
                double P = SL * (i - 2);
                X0[i] = X(P);
                Y0[i] = Y(P) * S;
            }

            for (int i = 0; i < N4; i++)
            {
                for (int j = 0; j < N4; j++)
                {
                    W0[i, j] = X0[i] * Y0[j];
                }
            }
        }
    }
}

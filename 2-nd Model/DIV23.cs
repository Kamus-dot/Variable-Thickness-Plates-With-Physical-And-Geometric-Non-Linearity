using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_nd_Model
{
    public partial class ModelTwo
    {
        public static void Oridef(double[] X0, double[] Y0, int IXY, int IWF, double[] XI, int J, out double A0)
        {
            double[] S1 = new double[12];
            double[] S2 = new double[12];
            double[] S3 = new double[12];
            double[] S4 = new double[12];
            double[] S5 = new double[12];

            double C1 = V2;
            if (IXY != 1) C1 = 1.0 / V2;

            int N1 = N + 1;
            int N2 = N + 2;
            int N4 = N + 4;
            int J1 = J - 1;
            int J2 = J - 2;
            int J3 = J + 1;
            int J4 = J + 2;

            for (int I = 2; I <= N2; I++)
            {
                int I1 = I - 1;
                int I2 = I - 2;
                int I3 = I + 1;
                int I4 = I + 2;

                double XI3 = XI[I];
                double XK1 = X0[I2];
                double XK2 = X0[I1];
                double XK3 = X0[I];
                double XK4 = X0[I3];
                double XK5 = X0[I4];

                double D1XI = (XI[I3] - XI[I1]) / 2.0;
                double D2XI = XI[I3] - 2.0 * XI3 + XI[I1];
                double D1XK = (XK4 - XK2) / 2.0;
                double D2XK = (XK4 - 2.0 * XK3 + XK2);

                int M, M1, M2, M3, L, L1, L2, L3;

                if (IXY != 1)
                {
                    M = J2;
                    M1 = J1;
                    M2 = J;
                    M3 = J3;
                    L = I1;
                    L1 = I1;
                    L2 = I1;
                    L3 = I3;
                }
                else
                {
                    M = I1;
                    M1 = I1;
                    M2 = I1;
                    M3 = I3;
                    L = J2;
                    L1 = J1;
                    L2 = J;
                    L3 = J3;
                }

                double C21, C22, C23, A21, A22, A23;

                if (IWF != 1)
                {
                    C21 = -E10[M, L] / E00[M, L];
                    C22 = -E10[M1, L1] / E00[M1, L1];
                    C23 = -E10[M2, L2] / E00[M2, L2];
                    A21 = (E11[M, L] / E01[M, L] + C21) / 2.0;
                    A22 = (E11[M1, L1] / E01[M1, L1] + C22) / 2.0;
                    A23 = (E11[M2, L2] / E01[M2, L2] + C23) / 2.0;
                }
                else
                {
                    C21 = Math.Pow(E10[M, L], 2) / E00[M, L] - E20[M, L];
                    C22 = Math.Pow(E10[M1, L1], 2) / E00[M1, L1] - E20[M1, L1];
                    C23 = Math.Pow(E10[M2, L2], 2) / E00[M2, L2] - E20[M2, L2];
                    A21 = (Math.Pow(E11[M, L], 2) / E01[M, L] - E21[M, L] + C21) / 2.0;
                    A22 = (Math.Pow(E11[M1, L1], 2) / E01[M1, L1] - E21[M1, L1] + C22) / 2.0;
                    A23 = (Math.Pow(E11[M2, L2], 2) / E01[M2, L2] - E21[M2, L2] + C23) / 2.0;
                }

                double B21 = A21 - C21;
                double B22 = A22 - C22;
                double B23 = A23 - C23;

                double AM1 = XI3 * XK3 * A21;
                double AJ = XI3 * XK3 * A22;
                double AP1 = XI3 * XK3 * A23;
                double BM1 = XI3 * B21 * D2XK;
                double BJ = XI3 * B22 * D2XK;
                double BP1 = XI3 * B23 * D2XK;
                double CM1 = -C21 * D1XI * D1XK;
                double CP1 = -C23 * D1XK * D1XI;
                double DJ = A22 * D2XK * D2XI;
                double EJ = B22 * XK3 * D2XI;

                S1[I1] = C1 * AM1 + CM1 / 2.0;
                S2[I1] = BM1 + EJ - 2.0 * C1 * (AJ + AM1);
                S3[I1] = DJ / C1 - 2.0 * (BJ + EJ) + C1 * (AP1 + 4.0 * AJ + AM1) - (CP1 + CM1) / 2.0;
                S4[I1] = BP1 + EJ - 2.0 * C1 * (AP1 + AJ);
                S5[I1] = C1 * AP1 + CP1 / 2.0;
            }

            double C = 0.5;
            double A1 = SIM(S1, C, N);
            double A2 = SIM(S2, C, N);
            double A3 = SIM(S3, C, N);
            double A4 = SIM(S4, C, N);
            double A5 = SIM(S5, C, N);

            A0 = A1 * Y0[J2] + A2 * Y0[J1] + A3 * Y0[J] + A4 * Y0[J3] + A5 * Y0[J4];
        }
    }
}

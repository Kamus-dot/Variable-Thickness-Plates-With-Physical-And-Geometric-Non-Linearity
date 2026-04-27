using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        // Вызывается в случае физической и (или) геометрической линейности
        public static void Varite()
        {
            double[,] WT = new double[14, 14];
            double[,] FT = new double[14, 14];
            int M = N;
            double A = AK;
            double B = BK;
            double P2 = 0.0;
            double P1 = P2;
            // Аппроксимация прогиба
            for (int i = 0; i < NM; i++)
            {
                P2 += X[i, M] * Y[i, M];
            }
            do
            {
                P1 = P2;
                X = Ort(X, 1, 0);
                Gauss(Y, X, YF, XF, 1, 1, 0, out Y, out X, out YF, out XF);
                Y = Ort(Y, 1, 0);
                Gauss(X, Y, XF, YF, 2, 1, 0, out X, out Y, out XF, out YF);
                P2 = 0;
                for (int i = 0; i < NM; i++)
                {
                    P2 += X[i, M] * Y[i, M];
                }
            }
            while (Math.Abs((P1 - P2) / P2) > EPSV);
            Deflec(1);
            if (IL == 0) return;
            int NI = 0;
            Deflec(2);
            double S2 = 0;
            double S1 = 0;
            do
            {
                S1 = S2;
                Prisv(WT, WXY);
                P2 = 0;
                int NI1 = 0;
                do
                {
                    P1 = P2;
                    X = Ort(X, 1, 0);
                    Gauss(Y, X, YF, XF, 1, 1, 1, out Y, out X, out YF, out XF);
                    Y = Ort(Y, 1, 0);
                    Gauss(X, Y, XF, YF, 2, 1, 1, out X, out Y, out XF, out YF);
                    Deflec(1);
                    P2 = WXY[M, M];
                    NI1 = NI1 + 1;
                    if (NI1 < 50) continue;
                    Console.WriteLine("Exit by W");
                    break;
                }
                while (Math.Abs((P1 - P2) / P2) > EPSV);
                Ytoch(WT, WXY, A, 1);
                S2 = WXY[M, M];
                Prisv(FT, FXY);
                P2 = 0;
                NI1 = 0;
                do
                {
                    P1 = P2;
                    Ort(XF, 1, 0);
                    Gauss(YF, XF, Y, X, 1, 2, 1, out YF, out XF, out Y, out X);
                    Ort(YF, 1, 0);
                    Gauss(XF, YF, X, Y, 2, 2, 1, out XF, out YF, out X, out Y);
                    Deflec(2);
                    P2 = FXY[M, M];
                    NI1 = NI1 + 1;
                    if (NI1 < 50) continue;
                    Console.WriteLine("Exit by F");
                    break;
                }
                while (Math.Abs((P1 - P2) / P2) > EPSV);
                Ytoch(FT, FXY, B, 2);
                NI = NI + 1;
                if (NI > 40)
                {
                    Console.WriteLine("Approximate is not reached");
                    return;
                }
            }
            while (Math.Abs((S1 - S2) / S1) < EPSV);
            return;
        }
        public static void Var()
        {
            double[,] WT = new double[14, 14];
            double[,] FT = new double[14, 14];
            int M = N;
            int M1 = N + 1;
            int M2 = N + 2;
            double A = AK;
            double B = BK;
            double P2 = 0.0;
            double P1 = P2;
            int NI = 0;
            Deflec(2);
            double S2 = 0;
            double S1 = 0;
            do
            {
                S1 = S2;
                Prisv(WT, WXY);
                P2 = 0;
                int NI1 = 0;
                do
                {
                    P1 = P2;
                    Ort(X, 1, 0);
                    Gauss(Y, X, YF, XF, 1, 1, 1, out Y, out X, out YF, out XF);
                    Ort(Y, 1, 0);
                    Gauss(X, Y, XF, YF, 2, 1, 1, out X, out Y, out XF, out YF);
                    Deflec(1);
                    P2 = WXY[M, M];
                    NI1 = NI1 + 1;
                    if (NI1 < 50) continue;
                    Console.WriteLine("Exit by W");
                }
                while (Math.Abs((P1 - P2) / P2) > EPSV);
                Ytoch(WT, WXY, A, 1);
                S2 = WXY[M, M];
                Prisv(FT, FXY);
                P2 = 0;
                NI1 = 0;
                do
                {
                    P1 = P2;
                    Ort(YF, 1, 0);
                    Gauss(XF, YF, X, Y, 2, 2, 1, out XF, out YF, out X, out Y);
                    Deflec(2);
                    P2 = FXY[M, M];
                    NI1 = NI1 + 1;
                    if (NI1 < 50) continue;
                    Console.WriteLine("Exit by F");
                }
                while (Math.Abs((P1 - P2) / P2) > EPSV);
                Ytoch(FT, FXY, B, 2);
                NI = NI + 1;
                if (NI > 40)
                {
                    Console.WriteLine("Approximate is not reached");
                    return;
                }
            }
            while (Math.Abs((S1 - S2) / S1) < EPSV);
            return;
        }
    }
}

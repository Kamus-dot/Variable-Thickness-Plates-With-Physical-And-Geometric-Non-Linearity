using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        /// <summary>
        /// Инициализация метода вариационной итерации,
        /// а также вызывается в случае и физической, и геометрической линейностей. 
        /// </summary>
        public void Varite()
        {
            int Count = 0;
            double P1, P2 = 0;
            double omega = 0.5;
            double prevDiff = 0;
            //Аппроксимация прогиба
            do
            {
                double[,] X_old = (double[,])X.Clone();
                P1 = P2;
                X = Ort(X, 1, true);
                Y = Gauss(Y, X, YF, XF, 1, 1, true);
                Y = Ort(Y, 1, true);
                X = Gauss(X, Y, XF, YF, 2, 1, true);
                X = Ytoch(X_old, X, omega);
                Deflec(1);
                P2 = WXY[N, N];
                double currentDiff = P2 - P1;
                if (Count > 1)
                {
                    if (currentDiff * prevDiff < 0)
                    {
                        omega = Math.Max(0.05, omega * 0.5);
                    }
                    else if (Count % 20 == 0)
                    {
                        omega = Math.Min(1, omega + 0.05);
                    }
                }
                prevDiff = currentDiff;
                Count++;
                if (Count > 500)
                {
                    throw new Exception("Потеря сходимости метода МВИ (пластина разрушена)");
                }
                //Console.WriteLine($"VARITE XY: Разница: {Math.Abs((P1 - P2) / P2) / omega}, P1 = {P1}, P2 = {P2}");
            }
            while (Math.Abs((P1 - P2) / P2) / omega > EPSV);
        }

        /// <summary>
        /// Метод вариационной итерации
        /// </summary>
        public void Var()
        {
            int N1 = N + 1;
            double[,] WF1 = new double[N1, N1];
            int Count = 0;
            double P1, P2;
            double S1, S2 = WXY[N, N];
            do
            {
                double omega = 0.1;
                S1 = S2;
                Prisv(ref WF1, WXY);
                P2 = 0;
                do
                {
                    P1 = P2;
                    P2 = 0;
                    double[,] X_old = X;
                    double[,] Y_old = Y;
                    Y = Gauss(Y, X, YF, XF, 1, 1, false);
                    //Y = Ort(Y, 1, true);
                    X = Gauss(X, Y, XF, YF, 2, 1, false);
                    //X = Ort(X, 1, true);
                    if (Count > 20)
                    {
                        omega += 0.05;
                        Count = 0;
                    }
                    for (int i = 0; i < NM; i++)
                    {
                        for (int j = 0; j < Y.GetLength(1); j++)
                        {
                            Y[i, j] = omega * Y[i, j] + (1 - omega) * Y_old[i, j];
                            X[i, j] = omega * X[i, j] + (1 - omega) * X_old[i, j];
                        }
                    }
                    for (int i = 0; i < NM; i++)
                    {
                        P2 += X[i, N1] * Y[i, N1];
                    }
                    Count++;
                    Console.WriteLine($"VAR XY: Разница: {Math.Abs((P1 - P2) / P2)}, P1 = {P1}, P2 = {P2}");
                }
                while (Math.Abs((P1 - P2) / P2) > EPSV);
                Count = 0;
                omega = 0.1;
                Deflec(1);
                //WXY = Ytoch(WF1, WXY, 0.3);
                S2 = WXY[N, N];
                Prisv(ref WF1, FXY);
                P2 = 0;
                do
                {
                    P1 = P2;
                    P2 = 0;
                    double[,] YF_old = YF;
                    double[,] XF_old = XF;
                    YF = Gauss(YF, XF, Y, X, 1, 2, IL);
                    YF = Ort(YF, 1, true);
                    XF = Gauss(XF, YF, X, Y, 2, 2, IL);
                    XF = Ort(XF, 1, true);
                    //if (Count > 20)
                    //{
                    //    if (omega == 0.5) omega = 0;
                    //    omega += 0.05;
                    //    Count = 0;
                    //}
                    for (int i = 0; i < NM; i++)
                    {
                        for (int j = 0; j < YF.GetLength(1); j++)
                        {
                            YF[i, j] = omega * YF[i, j] + (1 - omega) * YF_old[i, j];
                            XF[i, j] = omega * XF[i, j] + (1 - omega) * XF_old[i, j];
                        }
                    }
                    for (int i = 0; i < NM; i++)
                    {
                        P2 += YF[i, N1] * XF[i, N1];
                    }
                    Console.WriteLine($"VAR XFYF: Разница: {Math.Abs((P1 - P2) / P2)}, P1 = {P1}, P2 = {P2}");
                }
                while (Math.Abs((P1 - P2) / P2) > EPSV);
                Console.WriteLine($"VAR: Разница: {Math.Abs((S1 - S2) / S2)}, S1 = {S1}, S2 = {S2}");
            }
            while (Math.Abs((S1 - S2) / S2) > EPSV);
            Deflec(2);
            return;
        }
    }
}

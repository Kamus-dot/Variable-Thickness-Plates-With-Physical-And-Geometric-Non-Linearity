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
        /// а также вызывается в случае физической и геометрической линейности. 
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
            double S1, S2 = WXY[N, N];
            double outerOmega = 0.25;
            double prevOuterDiff = 0;
            int outerCount = 0;
            do
            {
                double[,] X_outer_old = (double[,])X.Clone();
                double[,] XF_outer_old = (double[,])XF.Clone();
                double omega = 0.5;
                int Count = 0;
                double prevDiff = 0;
                double P1, P2 = 0;
                S1 = S2;
                //Аппроксимация прогиба
                do
                {
                    double[,] X_old = (double[,])X.Clone();
                    P1 = P2;
                    X = Ort(X, 1, true);
                    Y = Gauss(Y, X, YF, XF, 1, 1, false);
                    Y = Ort(Y, 1, true);
                    X = Gauss(X, Y, XF, YF, 2, 1, false);
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
                    //Console.WriteLine($"VAR XY: Разница: {Math.Abs((P1 - P2) / P2) / omega}, P1 = {P1}, P2 = {P2}");
                }
                while (Math.Abs((P1 - P2) / P2) / omega > EPSV);
                Count = 0;
                omega = 0.5;
                prevDiff = 0;
                if (outerCount > 0)
                {
                    X = Ytoch(X_outer_old, X, outerOmega);
                    Deflec(1);
                }
                S2 = WXY[N, N];
                P2 = 0;
                do
                {
                    double[,] XF_old = (double[,])XF.Clone();
                    P1 = P2;
                    XF = Ort(XF, 1, true);
                    YF = Gauss(YF, XF, Y, X, 1, 2, false);
                    YF = Ort(YF, 1, true);
                    XF = Gauss(XF, YF, X, Y, 2, 2, false);
                    XF = Ytoch(XF_old, XF, omega);
                    Deflec(2);
                    P2 = FXY[N, N];
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
                    //Console.WriteLine($"VAR XFYF: Разница: {Math.Abs((P1 - P2) / P2) / omega}, P1 = {P1}, P2 = {P2}");
                }
                while (Math.Abs((P1 - P2) / P2) / omega > EPSV);
                if (outerCount > 0)
                {
                    XF = Ytoch(XF_outer_old, XF, outerOmega);
                    Deflec(2);
                }
                double outerDiff = S2 - S1;
                if (outerCount > 1 && outerDiff * prevOuterDiff < 0)
                {
                    outerOmega = Math.Max(0.05, outerOmega * 0.5);
                }
                else if (outerCount % 20 == 0)
                {
                    outerOmega = Math.Min(1, outerOmega + 0.05);
                }
                prevOuterDiff = outerDiff;
                outerCount++;
                Console.WriteLine($"VAR: Разница: {Math.Abs((S1 - S2) / S2) / outerOmega}, S1 = {S1}, S2 = {S2}");
                if (outerCount > 100) throw new Exception ("Пластина не сходится");
            }
            while (Math.Abs((S1 - S2) / S2) / outerOmega > EPSV);
            return;
        }
    }
}

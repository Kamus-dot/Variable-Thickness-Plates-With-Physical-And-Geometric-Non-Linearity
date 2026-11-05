using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        /// <summary>
        /// Метод переменных параметров упругости по модели Биргера
        /// </summary>
        public void Param()
        {
            double P2 = EMIN;
            double P = 0;
            double omega = 0.5;
            double prevDiff = 0;
            int Count = 0;
            do
            {
                double P1 = P2;
                if (JF) return;
                if (IL) Varite();
                else Var();
                INTEGER(omega, PLAST);
                P2 = EMIN;
                double diff = P2 - P1;
                if (Count > 0 && diff * prevDiff < 0)
                {
                    omega = Math.Max(0.1, omega * 0.5);
                }
                prevDiff = diff;
                P = Math.Abs((P1 - P2) / P2 / omega);
                Count++;
                if (Count > 100)
                {
                    throw new Exception("Превышено число итераций по пластичности");
                }
            }
            while (P > EPSP);
            return;
        }
    }
}


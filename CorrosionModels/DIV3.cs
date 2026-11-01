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
        /// Метод переменных параметров упругости по модели Биргера
        /// </summary>
        public void Param()
        {
            INTEGER(false, PLAST);
            double P2 = EMIN;
            double P = 0;
            do
            {
                double P1 = P2;
                if (JF) return;
                if (IL) Varite();
                else Var();
                INTEGER(false, PLAST);
                P2 = EMIN;
                P = Math.Abs((P1 - P2) / P2);
                Console.WriteLine($"Interger Notification : ({P})");
            }
            while (P > EPSP);
            return;
        }
    }
}


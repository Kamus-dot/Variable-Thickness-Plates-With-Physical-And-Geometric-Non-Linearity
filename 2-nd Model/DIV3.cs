using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_nd_Model
{
    public partial class ModelTwo
    {
        public static void Param()
        {
            INTEGR(1, PLAST);
            Varite();
            int MM = JF + 1;

            INTEGR(MM, PLAST);
            double P2 = EMIN;
            double P = 0;
            do
            {
                double P1 = P2;
                if (IL == 0) Varite();
                if (IL != 0) Var();
                if (JF == 0) return;
                INTEGR(2, PLAST);
                P2 = EMIN;
                P = Math.Abs((P1 - P2) / P2);
            }
            while (P > EPSP);
            return;
        }
        public static void Par()
        {
            INTEGR(0, PLAST);
            double P2 = EMIN;
            double P = 0;
            do
            {
                double P1 = P2;
                if (IL == 0) Varite();
                if (IL != 0) Var();
                if (JF == 0) return;
                INTEGR(2, PLAST);
                P2 = EMIN;
                P = Math.Abs((P1 - P2) / P2);
            }
            while (P > EPSP);
            return;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        // Вызывается в случае физически нелинейной задачи
        public static void Param()
        {
            INTEGER(1, PLAST);
            Varite();
            int MM = JF + 1;

            INTEGER(MM, PLAST);
            double P2 = EMIN;
            double P = 0;
            do
            {
                double P1 = P2;
                if (IL == 0) Varite();
                if (IL != 0) Var();
                if (JF == 0) return;
                INTEGER(2, PLAST);
                P2 = EMIN;
                P = Math.Abs((P1 - P2) / P2);
            }
            while (P > EPSP);
            return;
        }
        public static void Par()
        {
            INTEGER(0, PLAST);
            double P2 = EMIN;
            double P = 0;
            do
            {
                double P1 = P2;
                if (IL == 0) Varite();
                if (IL != 0) Var();
                if (JF == 0) return;
                INTEGER(2, PLAST);
                P2 = EMIN;
                P = Math.Abs((P1 - P2) / P2);
            }
            while (P > EPSP);
            return;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        public void StartLoad()
        {
            INTEGER();
            QL = QL0;
            Q = QP0;
            TempQ(ICON);

            Varite();
            if (!JF) Param();
            else if (!IL) Var();
            DeformationIntesivity();
            Console.WriteLine($"{MaxEI} is Max Deformation Intensivity");
            Console.WriteLine($"ПОПЕР.НАГР.={Q} ПРОД.НАГР.={QL} ПРОГИБ В ЦЕНТРЕ={WXY[N, N]}");

            double PRO = PROGIB(WXY, N);
            Console.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO}");

            SURFW();
            double VOL = Volume(H, N);

            Console.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL}");

            switch (LoadType)
            {
                case IPL.Transverse:
                do
                {
                    if (Math.Round(MaxEI, 3) >= (double)ES / 2) break;
                    Q += DQP;
                    TempQ(ICON);
                    CycleBuild();
                } while (Math.Abs(Q) <= Math.Abs(QPE));
                break;

                case IPL.Longitudinal:
                do
                {
                    if (Math.Round(MaxEI, 3) >= (double)ES / 2) break;
                    QL += DQL;
                    TempQ(ICON);
                    CycleBuild();
                } while (Math.Abs(QL) <= Math.Abs(QLE));
                break;

                case IPL.Both:
                do
                {
                    if (Math.Round(MaxEI, 3) >= (double)ES / 2) break;
                    Q += DQP;
                    QL += DQL;
                    TempQ(ICON);
                    CycleBuild();
                }
                while (Math.Abs(QL) <= Math.Abs(QLE) || Math.Abs(Q) <= Math.Abs(QPE));
                break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrosionModel
{
    public partial class MathCorrosionModel
    {
        public void Cycle(int ICON)
        {
            int N1 = N + 1;
            int N2 = N + 2;

            INTEGER(true, PLAST);

            switch (IPL)
            {
                case 1:
                QP = QP0;
                QL = QL0;
                do
                {
                    Q = QP;
                    TempQ(ICON);
                    if (Flag == 0)
                    {
                        Flag = 1;
                        Varite();
                        if (!JF) Param();
                        else if (IL) continue;
                        else Var();
                    }
                    else
                    {
                        if (!JF) Param();
                        else if (IL) Varite();
                        else Var();
                    }
                    double WCP = WXY[N, N];
                    DeformationIntesivity();
                    Console.WriteLine($"{Max} is Max Deformation Intensivity");
                    Console.WriteLine($"ПОПЕР.НАГР.={QP} ПРОД.НАГР.={QL} ПРОГИБ В ЦЕНТРЕ={WCP}");

                    double PRO = PROGIB(WXY, N);
                    Console.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO}");

                    SURFW();
                    //Defnap(PLAST, 0, 0);
                    double VOL = Volume(H, N);

                    Console.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL}");

                    QP += DQP;
                } while (QP <= QPE);
                break;

                case 4:
                QL = QL0;
                Q = QP0;
                QP = QP0;
                do
                {
                    TempQ(ICON);

                    if (Flag == 0)
                    {
                        Flag = 1;
                        Varite();
                        if (!JF) Param();
                        else if (IL) continue;
                        else Var();
                    }
                    else
                    {
                        if (!JF) Param();
                        else if (IL) Varite();
                        else Var();
                    }
                    double WCP = WXY[N, N];

                    using (StreamWriter file1 = new StreamWriter("T.DAT", true))
                    using (StreamWriter file2 = new StreamWriter("REZ.DAT", true))
                    {
                        Console.WriteLine($"ПОПЕР.НАГР.={QP:F2} ПРОД.НАГР.={QL:F2} ПРОГИБ В ЦЕНТРЕ={WCP:F4}");
                        file1.WriteLine($"ПОПЕР.НАГР.={QP:F2} ПРОД.НАГР.={QL:F2} ПРОГИБ В ЦЕНТРЕ={WCP:F4}");
                        file2.WriteLine($"ПОПЕР.НАГР.={QP:F2} ПРОД.НАГР.={QL:F2} ПРОГИБ В ЦЕНТРЕ={WCP:F4}");

                        double PRO = PROGIB(WXY, N);
                        Console.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO:F3}");
                        file1.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO:F3}");
                        file2.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO:F3}");

                        SURFW();
                        double VOL = Volume(H, N);

                        Console.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        file1.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        file2.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                    }
                    QL += DQL;
                } while (QL >= QLE);
                break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public void Cycle(int ICON)
        {
            int N1 = N + 1;
            int N2 = N + 2;
            StreamWriter file = new StreamWriter("REZ.DAT", false);
            file.Close();

            INTEGER(1, PLAST);

            int NP = 0;

            switch (IPL)
            {
                case 1:
                QP = QP0;
                QL = QL0;
                do
                {
                    Q = QP;
                    TempQ(ICON);
                    if (NP != 1)
                    {
                        if (JF == 1) Param();
                        if (JF == 0) Varite();
                    }
                    else
                    {
                        if (JF == 1) Par();
                        else Var();
                    }
                    //
                    double WCP = WXY[N, N];
                    NP = 1;
                    using (StreamWriter file1 = new StreamWriter("T.DAT", true))
                    using (StreamWriter file2 = new("REZ.DAT", true))
                    {
                        Console.WriteLine($"ПОПЕР.НАГР.={QP} ПРОД.НАГР.={QL} ПРОГИБ В ЦЕНТРЕ={WCP}");
                        file1.WriteLine($"ПОПЕР.НАГР.={QP} ПРОД.НАГР.={QL} ПРОГИБ В ЦЕНТРЕ={WCP}");
                        file2.WriteLine($"ПОПЕР.НАГР.={QP} ПРОД.НАГР.={QL} ПРОГИБ В ЦЕНТРЕ={WCP}");

                        double PRO = PROGIB(WXY, N);
                        Console.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO}");
                        file1.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO}");
                        file2.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO}");

                        SURFW();
                        //Defnap(PLAST, 0, 0);
                        double VOL = Volume(H, N);

                        Console.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL}");
                        file1.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL}");
                        file2.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL}");
                    }

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
                    if (NP != 1)
                    {
                        if (JF == 1) Param();
                        if (JF == 0) Varite();
                    }
                    else
                    {
                        if (JF == 1) Par();
                        else Var();
                    }

                    double WCP = WXY[N, N];
                    NP = 1;
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

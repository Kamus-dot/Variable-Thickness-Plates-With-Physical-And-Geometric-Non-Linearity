using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        public static void Cycle(int ICON)
        {
            int N1 = N + 1;
            int N2 = N + 2;
            int M10 = N2;
            StreamWriter file = new StreamWriter("REZ.DAT", false);
            file.Close();
            Console.WriteLine("H matrix:");
            for (int i = 0; i < N1; i++)
            {
                for (int j = 0; j < N1; j++)
                {
                    Console.Write($"{H[i, j]:F3}\t");
                }
                Console.WriteLine();
            }

            INTEGER(1, PLAST);

            Console.WriteLine();
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
                        if (JF == 0 && IL == 1) Var();
                        if (IL == 0) Varite();
                    }
                    //
                    N2 = M10;
                    double WCP = WXY[N2 - 1, N2 - 1];
                    NP = 1;
                    using (StreamWriter file1 = new StreamWriter("T.DAT", true))
                    using (StreamWriter file2 = new("REZ.DAT", true))
                    {
                        Console.WriteLine($"ПОПЕР.НАГР.={QP:F2} ПРОД.НАГР.={QL:F2} ПРОГИБ В ЦЕНТРЕ={WCP:F4}");
                        file1.WriteLine($"ПОПЕР.НАГР.={QP:F2} ПРОД.НАГР.={QL:F2} ПРОГИБ В ЦЕНТРЕ={WCP:F4}");
                        file2.WriteLine($"ПОПЕР.НАГР.={QP:F2} ПРОД.НАГР.={QL:F2} ПРОГИБ В ЦЕНТРЕ={WCP:F4}");

                        double PRO = PROGIB(WXY, N);
                        Console.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO:F3}");
                        file1.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO:F3}");
                        file2.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO:F3}");

                        SURFW();
                        Defnap(PLAST, 0, 0);
                        double VOL = Volume(H, N);

                        Console.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        file1.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        file2.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                    }

                    QP += DQP;
                } while (QP <= QPE);
                break;

                case 2:
                WC = WC0;
                Q = 1;
                QL = QL0;
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
                        if (JF == 0 && IL == 1) Var();
                        if (IL == 0) Varite();
                    }

                    N2 = M10;
                    double WCP = WXY[N2 - 1, N2 - 1];
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
                        Defnap(PLAST, 0, 0);
                        double VOL = Volume(H, N);

                        Console.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        file1.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        file2.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                    }

                    WC += DWC;
                } while (WC <= WCE);
                break;

                case 3:
                WC = WC0;
                Q = QP0;
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
                        if (JF == 0 && IL == 1) Var();
                        if (IL == 0) Varite();
                    }

                    N2 = M10;
                    double WCP = WXY[N2 - 1, N2 - 1];
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
                        Defnap(PLAST, 0, 0);
                        double VOL = Volume(H, N);

                        Console.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        file1.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        file2.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                    }
                    WC += DWC;
                } while (WC >= WCE);
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
                        if (JF == 0 && IL == 1) Var();
                        if (IL == 0) Varite();
                    }

                    N2 = M10;
                    double WCP = WXY[N2 - 1, N2 - 1];
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
                        Defnap(PLAST, 0, 0);
                        double VOL = Volume(H, N);

                        Console.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        file1.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        file2.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}"); 
                    }
                    QL += DQL;
                    //if (Math.Abs(QL + 3) < 0.001) DIFFUZE(PLAST);
                } while (QL >= QLE);
                break;
            }
        }
    }
}

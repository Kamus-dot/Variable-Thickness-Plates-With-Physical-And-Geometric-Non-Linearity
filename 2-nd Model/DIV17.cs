using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_nd_Model
{
    public partial class ModelTwo
    {
        public static void Cycle(int ICON)
        {
            int N1 = N + 1;
            int N2 = N + 2;
            int M10 = N2;

            // Print H matrix
            Console.WriteLine("H matrix:");
            for (int i = 0; i < N1; i++)
            {
                for (int j = 0; j < N1; j++)
                {
                    Console.Write($"{H[i, j]:F3}\t");
                }
                Console.WriteLine();
            }

            // Open files
            using (StreamWriter file1 = new StreamWriter("T.DAT"))
            using (StreamWriter file2 = new StreamWriter("W.DAT"))
            {
                INTEGR(1, PLAST);

                int NP = 0;

                // Switch based on IPL value
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
                        Console.WriteLine($"N2={N2}");
                        double WCP = WXY[N2 - 1, N2 - 1];
                        NP = 1;
                        Console.WriteLine($"ПОПЕР.НАГР.={QP:F2} ПРОД.НАГР.={QL:F2} ПРОГИБ В ЦЕНТРЕ={WCP:F4}");
                        file1.WriteLine($"ПОПЕР.НАГР.={QP:F2} ПРОД.НАГР.={QL:F2} ПРОГИБ В ЦЕНТРЕ={WCP:F4}");

                        Console.WriteLine($"N2={N2}");
                        SURFW();
                        Defnap(PLAST, 0, 0);
                        double VOL = Volume(H, N);
                        Console.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        file1.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        double PRO = PROGIB(H, N);
                        Console.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO:F3}");
                        file2.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO:F3}");

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
                        Console.WriteLine($"N2={N2}");
                        double WCP = WXY[N2 - 1, N2 - 1];
                        NP = 1;
                        Console.WriteLine($"ПОПЕР.НАГР.={QP:F2} ПРОД.НАГР.={QL:F2} ПРОГИБ В ЦЕНТРЕ={WCP:F4}");
                        file1.WriteLine($"ПОПЕР.НАГР.={QP:F2} ПРОД.НАГР.={QL:F2} ПРОГИБ В ЦЕНТРЕ={WCP:F4}");

                        Console.WriteLine($"N2={N2}");
                        SURFW();
                        Defnap(PLAST, 0, 0);
                        double VOL = Volume(H, N);
                        Console.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        file1.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        double PRO = PROGIB(WXY, N);
                        Console.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO:F3}");
                        file2.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO:F3}");

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
                        Console.WriteLine($"N2={N2}");
                        double WCP = WXY[N2 - 1, N2 - 1];
                        NP = 1;
                        Console.WriteLine($"ПОПЕР.НАГР.={QP:F2} ПРОД.НАГР.={QL:F2} ПРОГИБ В ЦЕНТРЕ={WCP:F4}");
                        file1.WriteLine($"ПОПЕР.НАГР.={QP:F2} ПРОД.НАГР.={QL:F2} ПРОГИБ В ЦЕНТРЕ={WCP:F4}");

                        Console.WriteLine($"N2={N2}");
                        SURFW();
                        Defnap(PLAST, 0, 0);
                        double VOL = Volume(H, N);
                        Console.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        file1.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        double PRO = PROGIB(WXY, N);
                        Console.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO:F3}");
                        file2.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO:F3}");

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
                        Console.WriteLine($"ПОПЕР.НАГР.={QP:F2} ПРОД.НАГР.={QL:F2} ПРОГИБ В ЦЕНТРЕ={WCP:F4}");
                        file1.WriteLine($"ПОПЕР.НАГР.={QP:F2} ПРОД.НАГР.={QL:F2} ПРОГИБ В ЦЕНТРЕ={WCP:F4}");

                        Console.WriteLine($"N2={N2}");
                        SURFW();
                        Defnap(PLAST, 0, 0);
                        double VOL = Volume(H, N);
                        Console.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        file1.WriteLine($"ОБЬЕМ ПЛАСТИНКИ = {VOL:F3}");
                        double PRO = PROGIB(WXY, N);
                        Console.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO:F3}");
                        file2.WriteLine($"ПРОГИБ ПО ПЛАНУ = {PRO:F3}");

                        QL += DQL;
                    } while (QL >= QLE);
                    break;
                }
            }
        }
    }
}

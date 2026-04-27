using _1_st_Model;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.Intrinsics.Arm;
public class Program : ModelOne
{
    // Точка входа программы.
    static void Main(string[] args)
    {
        // Initialize H array
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                H[i, j] = 0.749;
            }
        }

        // Read input data
        using (StreamReader reader = new StreamReader("DIVD.DAT"))
        {
            var customNumberFormat = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
            };
            string line;
            string[] parts;
            // Read first line
            line = reader.ReadLine();
            parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            NM = int.Parse(parts[0]);
            N = int.Parse(parts[1]);
            NT = int.Parse(parts[2]);
            EPSP = double.Parse(parts[3], customNumberFormat);
            EPSV = double.Parse(parts[4], customNumberFormat);

            // Read second line
            line = reader.ReadLine();
            parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            EO = double.Parse(parts[0], customNumberFormat);
            HUO = double.Parse(parts[1], customNumberFormat);
            OMY = double.Parse(parts[2], customNumberFormat);
            ES = double.Parse(parts[3], customNumberFormat);
            SS = double.Parse(parts[4], customNumberFormat);

            // Read third line
            line = reader.ReadLine();
            parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            G[0] = double.Parse(parts[0], customNumberFormat);
            G[1] = double.Parse(parts[1], customNumberFormat);
            GF[0] = double.Parse(parts[2], customNumberFormat);
            GF[1] = double.Parse(parts[3], customNumberFormat);
            JF = int.Parse(parts[4]);
            IL = int.Parse(parts[5]);

            // Read fourth line
            line = reader.ReadLine();
            parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            V2 = double.Parse(parts[0], customNumberFormat);
            S = double.Parse(parts[1], customNumberFormat);


            // Read remaining lines
            line = reader.ReadLine();
            parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            WC0 = double.Parse(parts[0], customNumberFormat);
            WCE = double.Parse(parts[1], customNumberFormat);
            DWC = double.Parse(parts[2], customNumberFormat);

            line = reader.ReadLine();
            parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            QP0 = double.Parse(parts[0], customNumberFormat);
            QPE = double.Parse(parts[1], customNumberFormat);
            DQP = double.Parse(parts[2], customNumberFormat);

            line = reader.ReadLine();
            parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            QL0 = double.Parse(parts[0], customNumberFormat);
            QLE = double.Parse(parts[1], customNumberFormat);
            DQL = double.Parse(parts[2], customNumberFormat);

            line = reader.ReadLine();
            parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            IPL = int.Parse(parts[0]);
            ICON = int.Parse(parts[1]);
            QK = double.Parse(parts[2], customNumberFormat);
            HH = double.Parse(parts[3], customNumberFormat);
            SKX = double.Parse(parts[4], customNumberFormat);
            SKY = double.Parse(parts[5], customNumberFormat);
            G1 = double.Parse(parts[6], customNumberFormat);

            line = reader.ReadLine();
            parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            H1 = double.Parse(parts[0], customNumberFormat);
            H2 = double.Parse(parts[1], customNumberFormat);
            H3 = double.Parse(parts[2], customNumberFormat);

            line = reader.ReadLine();
            parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            K1 = int.Parse(parts[0]);
            K2 = int.Parse(parts[1]);
            K3 = int.Parse(parts[2]);

            line = reader.ReadLine();
            parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            K1T = int.Parse(parts[0]);
            K2T = int.Parse(parts[1]);
            K3T = int.Parse(parts[2]);

            line = reader.ReadLine();
            parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            EV = double.Parse(parts[0], customNumberFormat);
            IV = int.Parse(parts[1]);
        }

        AK = 0.3;
        BK = 0.3;
        double EPS = 0.001;

        if (JF == 0) NT = 2;

        // Open output files
        StreamWriter writer1 = new StreamWriter("T.DAT");

        // Write output headers
        Console.WriteLine($"NM={NM}, N={N}, NT={NT}, EPSP={EPSP:F5}, EPSV={EPSV:F5}");
        writer1.WriteLine($"NM={NM}, N={N}, NT={NT}, EPSP={EPSP:F5}, EPSV={EPSV:F5}");

        Console.WriteLine($"\nEO={EO:F3}, HUO={HUO:F3}, OMY={OMY:F3}, ES={ES:F3}, SS={SS:F3}");
        writer1.WriteLine($"\nEO={EO:F3}, HUO={HUO:F3}, OMY={OMY:F3}, ES={ES:F3}, SS={SS:F3}");

        Console.WriteLine($"\nG(1)={G[0]:F2}, G(2)={G[1]:F2}, GF(1)={GF[0]:F2}, GF(2)={GF[1]:F2}, JF={JF}, IL={IL}");
        writer1.WriteLine($"\nG(1)={G[0]:F2}, G(2)={G[1]:F2}, GF(1)={GF[0]:F2}, GF(2)={GF[1]:F2}, JF={JF}, IL={IL}");

        Console.WriteLine($"\nV2={V2:F3}, S={S:F3}");
        writer1.WriteLine($"\nV2={V2:F3}, S={S:F3}");

        Console.WriteLine($"\nWC0={WC0:F3}, WCE={WCE:F3}, DWC={DWC:F3}");
        writer1.WriteLine($"\nWC0={WC0:F3}, WCE={WCE:F3}, DWC={DWC:F3}");

        Console.WriteLine($"\nQP0={QP0:F3}, QPE={QPE:F3}, DQP={DQP:F3}");
        writer1.WriteLine($"\nQP0={QP0:F3}, QPE={QPE:F3}, DQP={DQP:F3}");

        Console.WriteLine($"\nQL0={QL0:F3}, QLE={QLE:F3}, DQL={DQL:F3}");
        writer1.WriteLine($"\nQL0={QL0:F3}, QLE={QLE:F3}, DQL={DQL:F3}");

        Console.WriteLine($"\nIPL={IPL}, ICON={ICON}, QK={QK:F2}, HH={HH:F2}, SKX={SKX:F2}, SKY={SKY:F2}, G1={G1:F2}");
        writer1.WriteLine($"\nIPL={IPL}, ICON={ICON}, QK={QK:F2}, HH={HH:F2}, SKX={SKX:F2}, SKY={SKY:F2}, G1={G1:F2}");

        Console.WriteLine($"\nH1={H1:F3}, H2={H2:F3}, H3={H3:F3}");
        writer1.WriteLine($"\nH1={H1:F3}, H2={H2:F3}, H3={H3:F3}");

        Console.WriteLine($"\nK1={K1}, K2={K2}, K3={K3}");
        writer1.WriteLine($"\nK1={K1}, K2={K2}, K3={K3}");

        Console.WriteLine($"\nK1T={K1T}, K2T={K2T}, K3T={K3T}");
        writer1.WriteLine($"\nK1T={K1T}, K2T={K2T}, K3T={K3T}");

        Console.WriteLine($"\nEV={EV:F6}, IV={IV}");
        writer1.WriteLine($"\nEV={EV:F6}, IV={IV}");

        // Close all writers
        writer1.Close();

        double SL = 0.5 / N;
        VO = SL * SL;
        double SP = Math.PI * SL;
        int N1 = N + 1;
        int M1 = N + 4;

        for (int K = 0; K < NM; K++)
        {
            int KL = 2 * K + 1;
            for (int I = 0; I < M1; I++)
            {
                double P = Math.Sin(KL * I * SP) / KL;
                XF[K, I + 1] = P;
                YF[K, I + 1] = 0.0;
                Y[K, I + 1] = 0.0;
                X[K, I + 1] = P;    
            }
         }

        for (int I = 0; I < NM; I++)
        {
            for (int J = 0; J < N1; J++)
            {
                Console.Write($"{X[I, J]:F10} ");
            }
            Console.WriteLine();
        }

        int NT1 = NT + 1;

        W00(S);
        Cycle(ICON);
        Deflec(1);
        Deflec(2);
        Thick();

        Console.WriteLine("Program completed.");
    }
}

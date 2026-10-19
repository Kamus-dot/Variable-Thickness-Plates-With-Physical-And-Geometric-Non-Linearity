using _1_st_Model;
using System;
using System.Globalization;

public class Program : ModelOne
{
    // Точка входа программы.
    public static void Main(string[] args)
    {
        ModelOne model = new();

        int N1 = model.N + 1;
        int M1 = model.N + 3;

        for (int i = 0; i < N1; i++)
        {
            for (int j = 0; j < N1; j++)
            {
                model.H[i, j] = 0.749;
            }
        }

        if (model.JF == 0) model.NT = 2;

        double SL = 0.5 / model.N;
        model.V2 = SL * SL;
        double SP = Math.PI * SL;

        for (int K = 0; K < model.NM; K++)
        {
            int KL = 2 * K + 1;
            for (int I = 0; I < M1; I++)
            {
                double P = Math.Sin(KL * I * SP) / KL;
                model.X[K, I + 1] = P;
                model.XF[K, I + 1] = P;
            }
        }
        model.X = model.Ort(model.X, 1, 0);
        model.XF = model.Ort(model.XF, 1, 0);
        model.Delta_t = 0.2;

        int NT1 = model.NT + 1;
        for (int i = 1; i <= 5; i++)
        {
            model.TimeStep = i;
            model.Cycle(model.ICON);
            model.Deflec(1);
            model.Deflec(2);
            model.DeformationIntesivity();
            model.Thickness(model.TimeStep, model.PLAST);
        }
        Console.WriteLine("Program completed.");
    }
}

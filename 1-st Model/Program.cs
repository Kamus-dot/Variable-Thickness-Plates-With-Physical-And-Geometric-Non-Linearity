using CorrosionModel;
using System;
using System.Globalization;

public class Program
{
    // Точка входа программы.
    public static void Main()
    {
        MathCorrosionModel m = new();
        MathCorrosionModel model = new()
        {
            N = 30,
            ICON = 10,
            LoadType = MathCorrosionModel.IPL.Transverse,

        };
        model.DQL /= model.ICON;
        model.QL0 /= model.ICON;
        model.DQP /= model.ICON;
        model.QP0 /= model.ICON;
        model.Start();
        model.StartLoad();
        try
        {
            m = model.Cycle(model.PLAST);
        }
        catch (Exception ex)
        {
            if (model.EmergencyStop)
            {
                Console.WriteLine(ex.Message + ", выведена последняя устойчивая версия");
                throw new Exception();
            }
            else m = model;
        }

        Console.WriteLine("Program completed.");
    }
}

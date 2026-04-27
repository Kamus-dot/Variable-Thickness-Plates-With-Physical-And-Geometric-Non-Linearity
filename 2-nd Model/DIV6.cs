namespace _2_nd_Model
{
    public partial class ModelTwo
    {
        public static void Deflec(int IWF)
        {
            int M1 = N + 4;
            int N3 = N + 3;

            for (int i = 0; i < M1; i++)
            {
                for (int j = 0; j < M1; j++)
                {
                    double SP1 = 0.0;
                    double SP2 = 0.0;
                    for (int k = 0; k < NM; k++)
                    {
                        SP1 += X[k, i] * Y[k, j];
                        SP2 += X[k, i] * YF[k, j];
                    }
                    if (IWF == 2)
                    {
                        FXY[i, j] = SP2;
                        WXY[i, j] = SP1;
                    }
                }
            }
        }
    }
}

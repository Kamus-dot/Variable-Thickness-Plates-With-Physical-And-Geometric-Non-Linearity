namespace _2_nd_Model
{
    public partial class ModelTwo
    {
        public static void TempQ(int ICON)
        {

            double[,] Q1 = new double[12, 12];
            int N1 = N + 1;
            int N2 = N + 2;
            int K = ICON;

            for (int i = 0; i < N2; i++)
            {
                for (int j = 0; j < N2; j++)
                {
                    Q1[i, j] = QR(i, j, K) * Q;
                    if (EV >= 1) break;
                    Q1[i, j] = (1 - QR(i, j, IV) * Q);
                }
                if (EV >= 1) break;
            }

            for (int i = 0; i < N; i++)
            {
                int I1 = i + 1;
                for (int j = 0; j < N; j++)
                {
                    int J1 = j + 1;
                    QTW[I1, J1] = -Q1[I1, J1];
                    QTF[I1, J1] = 0;
                }
            }

            for (int i = 0; i < N1; i++)
            {
                QTW[1, i] = -Q1[1, i];
                QTW[i, 1] = -Q1[i, 1];
            }
            return;
        }
    }
}

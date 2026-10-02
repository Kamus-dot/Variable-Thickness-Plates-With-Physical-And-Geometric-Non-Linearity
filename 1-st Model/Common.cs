using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        // Матрицы коэффициентов физических параметров пластины
        public double[,] A = new double[14, 14];
        public double[,] A_nu = new double[14, 14];
        public double[,] AZ = new double[14, 14];
        public double[,] AZ_nu = new double[14, 14];
        public double[,] B = new double[14, 14];
        public double[,] BZ = new double[14, 14];

        // Базисные функции прогибов по осям
        public double[,] X = new double[4, 14];
        public double[,] Y = new double[4, 14];

        // Базисные функции усилий
        public double[,] XF = new double[4, 14];
        public double[,] YF = new double[4, 14];

        // Lambda = a/b (или наоборот) - Коэффициент прямоугольности пластинки
        public double Lambda;

        // V1 - шаг сетки
        public double V2;

        // Число приближений в методе вариационных итераций (<= 4)
        public int NM;

        // Число шагов конечно-разностной сетки по одной из координат
        public int N = 12; 

        // Число слоёв по толщине
        public int NT;

        public double EMIN;

        // Погрешность вычислений в методе переменных параметров упругости
        public double EPSP;

        // Погрешность вычислений в методе вариационных итераций
        public double EPSV;

        // Модуль упругости в физически линейной задаче
        public double EO;

        // Коэффициент Пуассона в физически линейной задаче
        public double HUO;

        // Объемный модуль упругости
        public double OMY;

        // Деформация текучести
        public double ES;

        // Напряжение текучести
        public double SS;

        // Поперечная нагрузка 
        public double Q;

        // Модуль cдвига
        public double[] G = new double[2];
        public double[] GF = new double[2];

        // Поперечная нагрузка для уравнения прогибов
        public double[,] QTW = new double[12, 12];

        // Нагрузка для уравнения функции усилий (равна 0)
        public double[,] QTF = new double[12, 12];

        // Толщина пластинки
        public double[,] H = new double[12, 12];

        // Поля прогибов
        public double[,] WXY = new double[14, 14];

        // Функция усилий (Функция Эри)
        public double[,] FXY = new double[14, 14];

        // JF = 0 - физически линейная задача, JF = 1 - нелинейная)
        public int JF;

        // IL = 0 - геометрически линейная задача, IL = 1 - нелинейная)
        public int IL;

        // Интенсивность
        public double[,] DEF_INT = new double[12, 12];

        // 
        public double[,] STR = new double[12, 12];
        public double SIG;
        public double[,] SQ = new double[12, 12];
        public double[,] DX = new double[12, 12];
        public double[,] DY = new double[12, 12];
        public double[,] DXY = new double[12, 12];

        // Размер площадки в центре пластинки,к которой прикладывается нагрузка
        public int ICON = 0;

        // Модуль упругости в зоне квадратного отверстия в центре пластинки
        public double EV;

        // Размер отверстия в центре пластинки
        public int IV;

        // Начальное значение прогиба в центре пластинки
        public double WC0;

        // Конечное значение прогиба в центре пластинки
        public double WCE;

        // Шаг изменения прогиба в центре
        public double DWC;

        // Начальное значение поперечной нагрузки
        public double QP0;

        // Конечное значение поперечной нагрузки
        public double QPE;

        // Шаг изменения поперечной нагрузки
        public double DQP;

        // Начальное значение продольной нагрузки
        public double QL0;

        // Конечное значение продольной нагрузки
        public double QLE;

        // Шаг изменения продольной нагрузки
        public double DQL;

        // Если IPL = 1 - нагрузка поперечная
        // Если IPL = 4 - нагрузка продольная
        public int IPL;

        // НН = 0 - Верхняя и нижняя поверхности пластинки плоские,
        // НН = 1 - Верхняя плоская, нижняя переменной толщины
        public double HH;

        // Поперечная нагрузка
        public double QP;

        // Продольная нагрузка
        public double QL;

        // Прогиб в центре
        public double WC;

        // QK = 0 - Одноостное сжатие
        // QK = 1 - Двуостное сжатие
        public double QK;

        // Модуль сдвига
        public double G1;

        // Значение начального прогиба в центре пластинки
        public double S;

        // Толщины пересекающихся ребер
        public double H1, H2, H3;

        // Начало первого, второго, третьего ребер в узлах сетки
        public int K1, K2, K3;

        // Концы ребер в узлах сетки по ширине
        public int K1T, K2T, K3T;

        internal static double[,] EN = new double[12, 12];
        internal static double[,] SEN = new double[12, 12];
        public double WEN;

        // Энергия
        internal static double[,] SVO = new double[12, 12];
        public double MVO;

        // Значение начального прогиба, выражающееся произведением функций
        // начального прогиба по осям
        internal static double[,] W0 = new double[14, 14];

        // Функции начального прогиба по осям
        internal static double[] X0 = new double[14];
        internal static double[] Y0 = new double[14];

        // Безразмерный шаг по времени 
        public double Delta_t;

        // Матрица изменений толщины в результате коррозии
        public double[,] Corrosion = new double[14, 14];

        public double OS;
        public int KLN;
        public int IN;
        public int NX;
        internal static double[] SUM = new double[50];
        public double STEP;
        internal static double[] X_ = new double[50];
        internal static double[,] X1 = new double[50, 50];
        public   double R;
        public int NIT;
        public int NI;
        public double SR;
        public double RM1;
        public double EPS;
        public int NS;
        public int NR;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_st_Model
{
    public partial class ModelOne
    {
        // Мембранная жёсткость
        internal static double[,] E00 = new double[12, 12];
        internal static double[,] E01 = new double[12, 12];

        // Связная жёсткость
        internal static double[,] E10 = new double[12, 12];
        internal static double[,] E11 = new double[12, 12];

        // Изгибная жёсткость
        internal static double[,] E21 = new double[12, 12];
        internal static double[,] E20 = new double[12, 12];

        // Базисы для прогибов по осям (для каждого слоя)
        internal static double[,] X = new double[4, 14];
        internal static double[,] Y = new double[4, 14];

        // Базисные функции для функции усилий
        internal static double[,] XF = new double[4, 14];
        internal static double[,] YF = new double[4, 14];

        // V2 = a/b - Коэффициент прямоугольности пластинки (лямбда)
        internal static double V2;

        // VO - шаг сетки в квадрате
        internal static double VO;

        // Число приближений в методе вариационных итераций (<= 4)
        internal static int NM;

        // Число шагов конечно-разностной сетки по одной из координат
        internal static int N;

        // Число слоёв по толщине
        internal static int NT;

        internal static double EMIN;

        // Погрешность вычислений в методе переменных параметров упругости
        internal static double EPSP;

        // Погрешность вычислений в методе вариационных итераций
        internal static double EPSV;

        // Модуль упругости в физически линейной задаче
        internal static double EO;

        // Коэффициент Пуассона в физически линейной задаче
        internal static double HUO;

        // Объемный модуль упругости
        internal static double OMY;

        // Деформация текучести
        internal static double ES;

        // Напряжение текучести
        internal static double SS;

        // Поперечная нагрузка 
        internal static double Q;

        internal static double Alfa; 
        internal static double Bet;

        // Модуль cдвига
        internal static double[] G = new double[2];
        internal static double[] GF = new double[2];

        // Поперечная нагрузка для уравнения прогибов
        internal static double[,] QTW = new double[12, 12];

        // Нагрузка для уравнения функции усилий (равна 0)
        internal static double[,] QTF = new double[12, 12];

        // Толщина пластинки
        internal static double[,] H = new double[12, 12];

        // Поля прогибов
        internal static double[,] WXY = new double[14, 14];

        // Функция усилий (Функция Эри)
        internal static double[,] FXY = new double[14, 14];

        // JF = 0 - физически линейная задача, JF = 1 - нелинейная)
        internal static int JF;

        // IL = 0 - геометрически линейная задача, IL = 1 - нелинейная)
        internal static int IL;

        // Интенсивность
        internal static double[,] DEF = new double[12, 12];

        // 
        internal static double[,] STR = new double[12, 12];
        internal static double SIG;
        internal static double[,] SQ = new double[12, 12];
        internal static double[,] DX = new double[12, 12];
        internal static double[,] DY = new double[12, 12];
        internal static double[,] DXY = new double[12, 12];

        // Размер площадки в центре пластинки,к которой прикладывается нагрузка
        internal static int ICON = 0;

        // Модуль упругости в зоне квадратного отверстия в центре пластинки
        internal static double EV;

        // Размер отверстия в центре пластинки
        internal static int IV;

        // Начальное значение прогиба в центре пластинки
        internal static double WC0;

        // Конечное значение прогиба в центре пластинки
        internal static double WCE;

        // Шаг изменения прогиба в центре
        internal static double DWC;

        // Начальное значение поперечной нагрузки
        internal static double QP0;

        // Конечное значение поперечной нагрузки
        internal static double QPE;

        // Шаг изменения поперечной нагрузки
        internal static double DQP; 

        // Начальное значение продольной нагрузки
        internal static double QL0;

        // Конечное значение продольной нагрузки
        internal static double QLE; 

        // Шаг изменения продольной нагрузки
        internal static double DQL;

        // Если IPL = 1 - нагрузка поперечная
        // Если IPL = 4 - нагрузка продольная
        internal static int IPL; 

        // НН = 0 - Верхняя и нижняя поверхности пластинки плоские,
        // НН = 1 - Верхняя плоская, нижняя переменной толщины
        internal static double HH;

        // Поперечная нагрузка
        internal static double QP;

        // Продольная нагрузка
        internal static double QL;

        // Прогиб в центре
        internal static double WC;
        
        // QK = 0 - Одноостное сжатие
        // QK = 1 - Двуостное сжатие
        internal static double QK;
        
        // Модуль сдвига
        internal static double G1;

        // Кривизны оболочки
        internal static double SKX, SKY;

        internal static double AK, BK;

        // Значение начального прогиба в центре пластинки
        internal static double S;

        // Толщины пересекающихся ребер
        internal static double H1, H2, H3;

        // Начало первого, второго, третьего ребер в узлах сетки
        internal static int K1, K2, K3;

        // Концы ребер в узлах сетки по ширине
        internal static int K1T, K2T, K3T;

        internal static double[,] EN = new double[12, 12];
        internal static double[,] SEN = new double[12, 12];
        internal static double WEN;

        // Энергия
        internal static double[,] SVO = new double[12, 12];
        internal static double MVO;
        
        // Значение начального прогиба, выражающееся произведением функций
        // начального прогиба по осям
        internal static double[,] W0 = new double[14, 14];
        
        // Функции начального прогиба по осям
        internal static double[] X0 = new double[14];
        internal static double[] Y0 = new double[14];

        internal static double OS;
        internal static int KLN;
        internal static int IN;
        internal static int NX;
        internal static double[] SUM = new double[50];
        internal static double STEP;
        internal static double[] X_ = new double[50];
        internal static double[,] X1 = new double[50, 50];
        internal static double R;
        internal static int NIT;
        internal static int NI;
        internal static double SR;
        internal static double RM1;
        internal static double EPS;
        internal static int NS;
        internal static int NR;
    }
}

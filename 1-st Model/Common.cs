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
        public double[,] A { get; set; }
        public double[,] A_nu { get; set; }
        public double[,] AZ { get; set; }
        public double[,] AZ_nu { get; set; }
        public double[,] B { get; set; }
        public double[,] BZ { get; set; }
        // Базисные функции прогибов по осям
        public double[,] X { get; set; }
        public double[,] Y { get; set; }
        // Базисные функции усилий
        public double[,] XF { get; set; }
        public double[,] YF { get; set; }
        // Lambda = a/b (или наоборот) - Коэффициент прямоугольности пластинки
        public double Lambda { get; set; }
        // V2 - шаг сетки в квадрате
        public double V2 { get; set; }
        // Число приближений в методе вариационных итераций (<= 4)
        public int NM { get; set; }
        // Число шагов конечно-разностной сетки по одной из координат
        public int N { get; set; } = 12;
        // Число слоёв по толщине
        public int NT { get; set; }
        public double EMIN { get; set; }
        // Погрешность вычислений в методе переменных параметров упругости
        public double EPSP { get; set; }
        // Погрешность вычислений в методе вариационных итераций
        public double EPSV { get; set; }
        // Модуль упругости в физически линейной задаче
        public double EO { get; set; }
        // Коэффициент Пуассона в физически линейной задаче
        public double HUO { get; set; }
        // Объемный модуль упругости
        public double OMY { get; set; }
        // Деформация текучести
        public double ES { get; set; }
        // Напряжение текучести
        public double SS { get; set; }
        // Поперечная нагрузка 
        public double Q { get; set; }
        // Размерность приложенной нагрузки на пластину
        public int ICON { get; set; }
        // Коэффиценты граничных условий
        public double[] G { get; set; } = new double[2];
        public double[] GF { get; set; } = new double[2];
        // Поперечная нагрузка для уравнения прогибов
        public double[,] QTW { get; set; }
        // Нагрузка для уравнения функции усилий (равна 0)
        public double[,] QTF { get; set; }
        // Толщина пластинки
        public double[,] H { get; set; }
        // Поля прогибов
        public double[,] WXY { get; set; }
        // Функция усилий (Функция Эри)
        public double[,] FXY { get; set; }
        // JF = 0 - физически линейная задача, JF = 1 - нелинейная)
        public int JF { get; set; }
        // IL = 0 - геометрически линейная задача, IL = 1 - нелинейная)
        public int IL { get; set; }
        // Интенсивность деформации
        public double[,,] DEF_INT { get; set; }
        // Интенсивность напряжения
        public double[,] TENS_INT { get; set; }
        // Среднее напряжение
        public double[,] TENS_AVG { get; set; }
        // Начальное значение поперечной нагрузки
        public double QP0 { get; set; }
        // Конечное значение поперечной нагрузки
        public double QPE { get; set; }
        // Шаг изменения поперечной нагрузки
        public double DQP { get; set; }
        // Начальное значение продольной нагрузки
        public double QL0 { get; set; }
        // Конечное значение продольной нагрузки
        public double QLE { get; set; }
        // Шаг изменения продольной нагрузки
        public double DQL { get; set; }
        // Если IPL = 1 - нагрузка поперечная
        // Если IPL = 4 - нагрузка продольная
        public int IPL { get; set; }
        // НН = 0 - Верхняя и нижняя поверхности пластинки плоские,
        // НН = 1 - Верхняя плоская, нижняя переменной толщины
        public double HH { get; set; }
        // Поперечная нагрузка
        public double QP { get; set; }
        // Продольная нагрузка
        public double QL { get; set; }
        // Прогиб в центре
        public double WC { get; set; }
        // QK = 0 - Одноостное сжатие
        // QK = 1 - Двуостное сжатие
        public double QK { get; set; }
        // Модуль сдвига
        public double G1 { get; set; }
        public double[,] SEN { get; set; }
        // Энергия
        public double[,] SVO { get; set; }
        public double MVO;
        // Безразмерный шаг по времени 
        public double Delta_t { get; set; }
        // Шаг по времени
        public int TimeStep { get; set; }
        // Матрица изменений толщины в результате коррозии
        public double[,] Corrosion { get; set; }

        public ModelOne()
        {
            Initialize();

            int N4 = N + 4;
            int N1 = N + 1;

            X = new double[NT, N4];
            Y = new double[NT, N4];
            XF = new double[NT, N4];
            YF = new double[NT, N4];

            A = new double[N4, N4];
            A_nu = new double[N4, N4];
            AZ = new double[N4, N4];
            AZ_nu = new double[N4, N4];
            B = new double[N4, N4];
            BZ = new double[N4, N4];
            Corrosion = new double[N4, N4];

            H = new double[N1, N1];
            QTW = new double[N1, N1];
            QTF = new double[N1, N1];
            WXY = new double[N1, N1];
            FXY = new double[N1, N1];
            SVO = new double[N1, N1];
            SEN = new double[N1, N1];

            TENS_INT = new double[N1, N1];
            TENS_AVG = new double[N1, N1];

            DEF_INT = new double[N1, N1, NT + 1];
        }
    }
}

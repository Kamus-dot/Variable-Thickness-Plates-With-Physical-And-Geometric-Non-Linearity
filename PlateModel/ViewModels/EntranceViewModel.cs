using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CorrosionModel;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Effects;
using WPFPlateModel.Models;


namespace WPFPlateModel.ViewModels
{
    public partial class EntranceViewModel : ObservableObject, INotifyDataErrorInfo
    {
        private readonly PlateModel _model;

        public PlateModel Model => _model;

        public EntranceViewModel() : this(new PlateModel())
        {
        }
        public EntranceViewModel(PlateModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        #region Exception Handeling

        private readonly Dictionary<string, List<string>> ErrorsDict = [];
        public IEnumerable GetErrors(string? propertyName)
        {
            return ErrorsDict.ContainsKey(propertyName) ? ErrorsDict[propertyName] : null;
        }
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => ErrorsDict.Count != 0;
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        private void ErrorsAdd(string propertyName, string error)
        {
            if (ErrorsDict.TryGetValue(propertyName, out List<string>? list))
            {
                if (!list.Contains(error)) list.Add(error);
            }
            else ErrorsDict.Add(propertyName, [error]);
            OnErrorsChanged(propertyName);
        }
        private void ErrorsClear(string propertyName)
        {
            ErrorsDict.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }

        public Dictionary<string, string>? ConverterException
        {
            get => null;
            set
            {
                if (value is not null)
                {
                    foreach (string key in value.Keys)
                    {
                        ErrorsAdd(key, value[key]);
                    }
                }
            }
        }
        #endregion

        #region Bound Properties from EntranceWindow.xaml

        /// <summary>
        /// Начальная толщина пластинки
        /// </summary>
        public double H0
        {
            get => _model.H0;
            set
            {
                try
                {
                    _model.H0 = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(H0));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(H0), e.Message);
                }
            }
        }

        /// <summary>
        /// Число шагов конечно-разностной сетки по одной из координат (Разбиение по плану)
        /// </summary>
        public int N
        {
            get => _model.N;
            set
            {
                try
                {
                    if (_model.N != value)
                    {
                        _model.N = value;
                        _model.ReallocateGrid();
                        OnPropertyChanged();
                    }
                    ErrorsClear(nameof(N));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(N), e.Message);
                }
            }
        }


        /// <summary>
        /// Число слоёв по толщине (Разбиение пластинки по толщине)
        /// </summary>
        public int NT
        {
            get => _model.NT;
            set
            {
                try
                {
                    if (_model.NT != value)
                    {
                        _model.NT = value;
                        _model.ReallocateGrid();
                        OnPropertyChanged();
                    }
                    ErrorsClear(nameof(NT));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(NT), e.Message);
                }
            }
        }

        /// <summary>
        /// Отношение длины к ширине пластины (Lambda = a/b)
        /// </summary>
        public double Lambda
        {
            get => _model.Lambda;
            set
            {
                try
                {
                    _model.Lambda = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(Lambda));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(Lambda), e.Message);
                }
            }
        }

        /// <summary>
        /// Коэффициент Пуассона в физически линейной задаче
        /// </summary>
        public double HUO
        {
            get => _model.HUO;
            set
            {
                try
                {
                    _model.HUO = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(HUO));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(HUO), e.Message);
                }
            }
        }

        /// <summary>
        /// Модуль упругости в физически линейной задаче
        /// </summary>
        public double EO
        {
            get => _model.EO;
            set
            {
                try
                {
                    _model.EO = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(EO));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(EO), e.Message);
                }
            }
        }

        /// <summary>
        /// Объемный модуль упругости
        /// </summary>
        public double OMY
        {
            get => _model.OMY;
            set
            {
                try
                {
                    _model.OMY = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(OMY));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(OMY), e.Message);
                }
            }
        }

        /// <summary>
        /// Модуль сдвига
        /// </summary>
        public double G1
        {
            get => _model.G1;
            set
            {
                try
                {
                    _model.G1 = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(G1));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(G1), e.Message);
                }
            }
        }

        /// <summary>
        /// Деформация текучести
        /// </summary>
        public double ES
        {
            get => _model.ES;
            set
            {
                try
                {
                    _model.ES = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(ES));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(ES), e.Message);
                }
            }
        }

        /// <summary>
        /// Напряжение текучести
        /// </summary>
        public double SS
        {
            get => _model.SS;
            set
            {
                try
                {
                    _model.SS = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(SS));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(SS), e.Message);
                }
            }
        }

        /// <summary>
        /// Предел прочности пластины
        /// </summary>
        public double TensileStrength
        {
            get => _model.TensileStrength;
            set
            {
                try
                {
                    _model.TensileStrength = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(TensileStrength));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(TensileStrength), e.Message);
                }
            }
        }

        /// <summary>
        /// Погрешность вычислений в методе переменных параметров упругости
        /// </summary>
        public double EPSP
        {
            get => _model.EPSP;
            set
            {
                try
                {
                    _model.EPSP = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(EPSP));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(EPSP), e.Message);
                }
            }
        }

        /// <summary>
        /// Погрешность вычислений в методе вариационных итераций
        /// </summary>
        public double EPSV
        {
            get => _model.EPSV;
            set
            {
                try
                {
                    _model.EPSV = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(EPSV));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(EPSV), e.Message);
                }
            }
        }

        /// <summary>
        /// Количество шагов по времени / Лимит по времени
        /// </summary>
        public double TimeLimit
        {
            get => _model.TimeLimit;
            set
            {
                try
                {
                    _model.TimeLimit = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(TimeLimit));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(TimeLimit), e.Message);
                }
            }
        }

        /// <summary>
        /// Размерность приложенной нагрузки на пластину
        /// </summary>
        public int ICON
        {
            get => _model.ICON;
            set
            {
                try
                {
                    _model.ICON = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(ICON));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(ICON), e.Message);
                }
            }
        }

        /// <summary>
        /// Начальное значение поперечной нагрузки
        /// </summary>
        public double QP0
        {
            get => _model.QP0;
            set
            {
                try
                {
                    _model.QP0 = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(QP0));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(QP0), e.Message);
                }
            }
        }

        /// <summary>
        /// Шаг изменения поперечной нагрузки
        /// </summary>
        public double DQP
        {
            get => _model.DQP;
            set
            {
                try
                {
                    _model.DQP = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(DQP));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(DQP), e.Message);
                }
            }
        }

        /// <summary>
        /// Конечное значение поперечной нагрузки
        /// </summary>
        public double QPE
        {
            get => _model.QPE;
            set
            {
                try
                {
                    _model.QPE = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(QPE));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(QPE), e.Message);
                }
            }
        }

        /// <summary>
        /// Начальное значение продольной нагрузки
        /// </summary>
        public double QL0
        {
            get => _model.QL0;
            set
            {
                try
                {
                    _model.QL0 = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(QL0));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(QL0), e.Message);
                }
            }
        }

        /// <summary>
        /// Шаг изменения продольной нагрузки
        /// </summary>
        public double DQL
        {
            get => _model.DQL;
            set
            {
                try
                {
                    _model.DQL = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(DQL));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(DQL), e.Message);
                }
            }
        }

        /// <summary>
        /// Конечное значение продольной нагрузки
        /// </summary>
        public double QLE
        {
            get => _model.QLE;
            set
            {
                try
                {
                    _model.QLE = value;
                    OnPropertyChanged();
                    ErrorsClear(nameof(QLE));
                }
                catch (Exception e)
                {
                    ErrorsAdd(nameof(QLE), e.Message);
                }
            }
        }

        #endregion

        #region ComboBox Bindings

        /// <summary>
        /// Модель коррозийного износа: 0 - Долинского, 1 - Гутмана
        /// </summary>
        public int SelectedCorrosionModelIndex
        {
            get => (int)_model.CorrosionModel;
            set
            {
                if ((int)_model.CorrosionModel != value && Enum.IsDefined(typeof(MathCorrosionModel.CorrosionModels), value))
                {
                    _model.CorrosionModel = (MathCorrosionModel.CorrosionModels)value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Физическая линейность: 0 - Да (JF = true), 1 - Нет (JF = false)
        /// </summary>
        public int SelectedPhysicalLinearityIndex
        {
            get => _model.JF ? 0 : 1;
            set
            {
                if (value == 0)
                {
                    _model.JF = true;
                    NT = default;
                }
                else if (value == 1) _model.JF = false;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Геометрическая линейность: 0 - Да (IL = true), 1 - Нет (IL = false)
        /// </summary>
        public int SelectedGeometricalLinearityIndex
        {
            get => _model.IL ? 0 : 1;
            set
            {
                bool il = (value == 0);
                if (_model.IL != il)
                {
                    _model.IL = il;
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedBoundaryConditionIndex = 0;

        /// <summary>
        /// Вид закрепления на границе: 0 - Шарнирное опирание, 1 - Жёсткое защемление
        /// </summary>
        public int SelectedBoundaryConditionIndex
        {
            get => _selectedBoundaryConditionIndex;
            set
            {
                if (_selectedBoundaryConditionIndex != value)
                {
                    _selectedBoundaryConditionIndex = value;
                    if (value == 0)
                    {
                        _model.G[0] = -1;
                        _model.G[1] = -1;
                        _model.GF[0] = 1;
                        _model.GF[1] = 1;
                    }
                    else
                    {
                        _model.G[0] = 1;
                        _model.G[1] = 1;
                        _model.GF[0] = -1;
                        _model.GF[1] = -1;
                    }
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Вид прикладываемой внешней нагрузки: 0 - Продольная, 1 - Поперечная, 2 - Продольная и поперечная
        /// </summary>
        public int SelectedExternalLoadingIndex
        {
            get => _model.LoadType switch
            {
                MathCorrosionModel.IPL.Longitudinal => 0,
                MathCorrosionModel.IPL.Transverse => 1,
                MathCorrosionModel.IPL.Both => 2,
                _ => 1
            };
            set
            {
                MathCorrosionModel.IPL loadType = value switch
                {
                    0 => MathCorrosionModel.IPL.Longitudinal,
                    1 => MathCorrosionModel.IPL.Transverse,
                    2 => MathCorrosionModel.IPL.Both,
                    _ => MathCorrosionModel.IPL.Transverse
                };
                if (_model.LoadType != loadType)
                {
                    _model.LoadType = loadType;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Вид сжатия: 0 - Одноостное, 1 - Двухостное
        /// </summary>
        public int SelectedCompressionIndex
        {
            get => (int)_model.QK;
            set
            {
                if ((int)_model.QK != value && (value == 0 || value == 1))
                {
                    _model.QK = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands
        public RelayCommand<string> RemoveCommand => new(execute: ErrorsClear, 
            canExecute: ErrorsDict.ContainsKey);
        public RelayCommand ApplyCommand => new(execute => Apply(),
            canExcecute => !HasErrors);
        public RelayCommand<string> UpdateCommand => new(execute: (string propertyName) => OnPropertyChanged(propertyName));
        private static void Apply()
        {
            var MainWindow = new MainWindow();
            MainWindow.Show();
        }
        #endregion
    }
}

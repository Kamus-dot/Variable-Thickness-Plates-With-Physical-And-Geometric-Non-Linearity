using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WPFPlateModel.Models;

namespace WPFPlateModel
{
    /// <summary>
    /// Хранилище состояния пластины между итерациями
    /// </summary>
    public partial class PlateStateStorage : ObservableObject
    {
        public PlateStateStorage() { }
        public PlateStateStorage(double t, PlateModel plateModel)
        {
            _plateShifts.Add(t, plateModel);
        }

        [ObservableProperty]
        private Dictionary<double, PlateModel> _plateShifts = [];
    }
}

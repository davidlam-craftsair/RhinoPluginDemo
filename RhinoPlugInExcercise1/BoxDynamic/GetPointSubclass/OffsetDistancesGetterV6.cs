using Rhino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPlugInExcercise1
{
    public class OffsetDistancesGetterV6: IOffsetDistancesGetter
    {
        public bool Get(out double dx, out double dy)
        {
            var window = new OffsetDistanceWindow();
            OffDistanceViewModel offDistanceViewModel = new OffDistanceViewModel();
            window.DataContext = offDistanceViewModel;
            window.ShowDialog();
            if (window.DialogResult == true)
            {
                dx = offDistanceViewModel.Input1;
                dy = offDistanceViewModel.Input2;
                return true;
            }
            dx = 0;
            dy = 0;
            return false;
        }
    }
}

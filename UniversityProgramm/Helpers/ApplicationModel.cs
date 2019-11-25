using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityProgramm.Helpers
{
    public class ApplicationModel : ViewModelBase
    {
        private double _mapHeight = 800;
        public double MapHeight
        {
            get => _mapHeight;
            set => SetProperty(ref _mapHeight, value);
        }

        public ApplicationModel()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityProgramm.Helpers
{
    public class ApplicationModel : ViewModelBase
    {
        private double _mapHeight = 960;
        public double MapHeight
        {
            get => _mapHeight;
            set => SetProperty(ref _mapHeight, value);
        }

        private double _mapWidth = 1280;
        public double MapWidth
        {
            get => _mapWidth;
            set => SetProperty(ref _mapWidth, value);
        }

        public ApplicationModel()
        {

        }
    }
}

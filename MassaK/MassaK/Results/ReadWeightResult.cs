using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassaK.Rnums;

namespace MassaK.Results
{
    public class ReadWeightResult : SmartClasses.Results.BaseResult
    {
        public UnitOfMeasurement UnitOfMeasurement { get; set; }
        public int               Reading { get; set; }
    }
}

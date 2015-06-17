using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassaK.Results;
using MassaK.Rnums;
using ScalesMassaK;
using SmartClasses.Extensions;

namespace MassaK.Providers
{
    public static class ScalesProvider
    {
        public static ReadWeightResult ReadWeight(string port)
        {
            var result = new ReadWeightResult();

            try
            {
                var scale = new Scale {Connection = port};
                var connectionResult = scale.OpenConnection();

                if (connectionResult != 0)
                {
                    result.IsOK    = false;
                    result.Message = ((Error) connectionResult).GetDescription();
                    return result;
                }
                
                while (true)
                {
                    var readResult = scale.ReadWeight();
                    if (readResult != 0)
                    {
                        result.IsOK    = false;
                        result.Message = ((Error) readResult).GetDescription();
                        return result;
                    }
                    
                    var repeatTimes = 0;
                    
                    if (scale.Stable != 1)
                    {
                        repeatTimes++;
                        if (repeatTimes > 10)
                        {
                            result.IsOK    = false;
                            result.Message = "Вес не стабилен";
                            break;
                        }

                        Thread.Sleep(100);
                    }
                    else
                    {
                        result.IsOK              = true;
                        result.Reading           = scale.Weight;
                        result.UnitOfMeasurement = (UnitOfMeasurement)scale.Division;
                        break;
                    }
                }

                scale.CloseConnection();
            }
            catch (Exception ex)
            {
                result.IsOK    = false;
                result.Message = ex.GetMessage();
            }

            return result;
        }
    }
}


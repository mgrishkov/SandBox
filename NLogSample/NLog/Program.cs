using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace NLogSample
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            logger.Info("Start");
            try
            {
                throw new InvalidOperationException("Test exception");
            }
            catch (Exception ex)
            {
                //отправится сообщение по почте на Maxim.Grishkov@jti.com
                //сохранится инф-ия в eventLog
                logger.ErrorException("Test exception has been handled:", ex);
            };
            logger.Info("Stop");

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using SmartClasses.RFID;
using System.IO.Ports;
using System.Timers;

namespace RFIDRead
{
    class Program
    {
        
        static void Main(string[] args)
        {
            RFIDReader reader = new RFIDReader("COM3");
            reader.ValueChanged += reader_ValueChanged; 
            reader.Start();
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
            };
            reader.Stop();
            Console.ReadKey();
        }

        protected static void reader_ValueChanged(object sender, RFIDReader.ValueChangedEventargs e)
        {
            Console.WriteLine(e.ID);
        }
    }
}

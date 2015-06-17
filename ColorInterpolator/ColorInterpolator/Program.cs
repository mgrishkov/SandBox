using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorInterpolator
{
    class Program
    {
        static void Main(string[] args)
        {
            var colorA = (Color)ColorConverter.ConvertFromString("#55BF3B");
            var colorB = (Color)ColorConverter.ConvertFromString("#DDDF0D");
            var colorC = (Color)ColorConverter.ConvertFromString("#DF5353");
            var colorD = ColorInterpolator.InterpolateBetween(colorA, colorB, colorC, 0.7);

            Console.WriteLine(colorD.ToString());

            Console.ReadKey();
        }
    }
}

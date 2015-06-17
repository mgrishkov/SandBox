using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;

namespace ColorInterpolator
{
    class ColorInterpolator
    {



        delegate byte ComponentSelector(Color color);
        static ComponentSelector _redSelector = color => color.R;
        static ComponentSelector _greenSelector = color => color.G;
        static ComponentSelector _blueSelector = color => color.B;

        public static Color InterpolateBetween(
            Color endPoint1,
            Color endPoint2,
            double lambda)
        {
            if (lambda < 0 || lambda > 1)
            {
                throw new ArgumentOutOfRangeException("lambda");
            }
            Color color = Color.FromRgb(
                InterpolateComponent(endPoint1, endPoint2, lambda, _redSelector),
                InterpolateComponent(endPoint1, endPoint2, lambda, _greenSelector),
                InterpolateComponent(endPoint1, endPoint2, lambda, _blueSelector)
            );

            return color;
        }
        public static Color InterpolateBetween(
            Color endPoint1,
            Color endPoint2,
            Color endPoint3,
            double lambda)
        {
            if (lambda < 0 || lambda > 1)
            {
                throw new ArgumentOutOfRangeException("lambda");
            }

            var color1 = lambda < 0.5 ? endPoint1 : endPoint2;
            var color2 = lambda < 0.5 ? endPoint2 : endPoint3;
            var l = lambda < 0.5 ? lambda + 0.5 : lambda - 0.5;

            Color color = Color.FromRgb(
                InterpolateComponent(color1, color2, l, _redSelector),
                InterpolateComponent(color1, color2, l, _greenSelector),
                InterpolateComponent(color1, color2, l, _blueSelector)
            );

            return color;
        }

        static byte InterpolateComponent(
            Color endPoint1,
            Color endPoint2,
            double lambda,
            ComponentSelector selector)
        {
            return (byte)(selector(endPoint1) * (1 - lambda) + selector(endPoint2)* lambda);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TappersWPF
{
    public class Utils
    {

        public static Color getBackgroundColour(string hex)
        {
            Color col = (Color)ColorConverter.ConvertFromString(hex);
            return col;
        }

    }
}

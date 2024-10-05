using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSBuildSupportWPF.resources.codeNodeColor
{
    internal class ColorToSerialize
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public Color ToColor()
        {
            return Color.FromArgb(R,G,B);
        }
    }
}

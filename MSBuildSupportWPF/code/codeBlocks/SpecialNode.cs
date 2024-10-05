using MSBuildSupport.code.codeBlocks;
using MSBuildSupportWPF.resources.codeNodeColor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSBuildSupportWPF.code.codeBlocks
{
    //Special node is for special symbols
    //Example:
    // In <Project Sdk="Microsoft.NET.Sdk">
    // it will be "="
    class SpecialNode : CodeNode
    {
        public SpecialNode(int lenght, int position, string codePart) : base(lenght, position, CodeNodeColorResource.GetColor("SpecialNode"), codePart)
        {
        }
        public override string ToString()
        {
            return base.ToString() + "SpecialNode";
        }
    }
}

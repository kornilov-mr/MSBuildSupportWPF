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
    //Node for "\n" symbol
    public class WhiteSpaceNode : CodeNode
    {
        public WhiteSpaceNode(int position) : base(1, position, CodeNodeColorResource.GetColor("WhiteSpaceNode"), "\n")
        {
        }
        public override string ToString()
        {
            return base.ToString() + "WhiteSpaceNode";
        }
    }
}

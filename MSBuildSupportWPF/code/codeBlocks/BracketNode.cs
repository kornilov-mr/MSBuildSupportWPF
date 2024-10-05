using MSBuildSupport.code.codeBlocks;
using MSBuildSupportWPF.resources.codeNodeColor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MSBuildSupportWPF.code.codeBlocks
{
    //Bracket node is for brackets around tag: "<",">","</"
    //Example:
    // in <Project Sdk="Microsoft.NET.Sdk">
    // it will be "<" and ">"
    class BracketNode : CodeNode
    {
        public BracketNode(int lenght, int position, string codePart) : base(lenght, position, CodeNodeColorResource.GetColor("BracketNode"), codePart)
        {
        }
        public override string ToString()
        {
            return base.ToString() + "BracketNode";
        }
    }
}

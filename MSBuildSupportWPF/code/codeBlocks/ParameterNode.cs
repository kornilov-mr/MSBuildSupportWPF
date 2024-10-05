using MSBuildSupportWPF.resources.codeNodeColor;
using System.Drawing;

namespace MSBuildSupport.code.codeBlocks;

public sealed class ParameterNode : CodeNode
{
    //Parameter node is for attribute values in tag
    //Example:
    // In <Project Sdk="Microsoft.NET.Sdk">
    // it will be "Microsoft.NET.Sdk"
    public ParameterNode(int lenght, int position, string codePart) : base(lenght, position, CodeNodeColorResource.GetColor("ParameterNode"), codePart)
    {
    }

    public override bool Equals(object? obj)
    {
        if (!(obj is ParameterNode)) return false;
        return base.Equals(obj);
    }
    public override string ToString()
    {
        return base.ToString() + "ParameterNode";
    }
}
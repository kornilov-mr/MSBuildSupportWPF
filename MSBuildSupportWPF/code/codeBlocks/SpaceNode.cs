using MSBuildSupportWPF.resources.codeNodeColor;
using System.Reflection.Metadata;
using System;

namespace MSBuildSupport.code.codeBlocks;

public sealed class SpaceNode : CodeNode
{
    public SpaceNode(int lenght, int position) : base(lenght, position, CodeNodeColorResource.GetColor("SpaceNode"))
    {
    }

    public override bool Equals(object? obj)
    {
        if (!(obj is SpaceNode)) return false;
        return base.Equals(obj);
    }
    public override string ToString()
    {
        return base.ToString() + "SpaceNode";
    }
}
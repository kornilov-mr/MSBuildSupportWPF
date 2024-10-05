using MSBuildSupportWPF.resources.codeNodeColor;
using System.Drawing;

namespace MSBuildSupport.code.codeBlocks;

public sealed class TagBodyNode : CodeNode
{
    //TagBodynode is for content in tag
    //Example:
    // In <Nullable>enable</Nullable>
    // it will be "enable"
    public TagBodyNode(int lenght, int position, string codePart) : base(lenght, position, CodeNodeColorResource.GetColor("TagBodyNode"), codePart)
    {
    }

    public override bool Equals(object? obj)
    {
        if (!(obj is TagBodyNode)) return false;
        return base.Equals(obj);
    }
    public override string ToString()
    {
        return base.ToString() + "TagBodyNode";
    }
}
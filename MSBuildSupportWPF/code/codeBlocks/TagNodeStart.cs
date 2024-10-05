using MSBuildSupportWPF.code.codeBlocks;
using MSBuildSupportWPF.resources.codeNodeColor;
using System.CodeDom;
using System.Drawing;

namespace MSBuildSupport.code.codeBlocks;

public sealed class TagNodeStart : LinkedNode
{
    //TagNodeEnd node is for tag opening
    //Example:
    // In <Nullable>enable</Nullable>
    // it will be "Nullable" at the start
    public readonly int deapth;
    public readonly string tagName;
    public TagNodeStart(int lenght, int position, string codePart, int deapth, string tagName) :base(lenght, position, CodeNodeColorResource.GetColor("TagNode"), codePart)
    {
        this.deapth = deapth;
        this.tagName = tagName;
    }

    public override bool CanLinkTo(LinkedNode node)
    {
        if (!(node is TagNodeEnd)) return false;
        TagNodeEnd tagNodeEnd = (TagNodeEnd)node;
        if (deapth != tagNodeEnd.deapth) return false;
        if (!String.Equals(tagName, tagNodeEnd.tagName)) return false;
        if (tagNodeEnd.LinkedNodes.Count>0) return false;
        return true;
    }
    public override bool Equals(object? obj)
    {
        if (!(obj is TagNodeStart)) return false;
        return base.Equals(obj);
    }
    public override string ToString()
    {
        return base.ToString() + "TagNodeStart";
    }
}
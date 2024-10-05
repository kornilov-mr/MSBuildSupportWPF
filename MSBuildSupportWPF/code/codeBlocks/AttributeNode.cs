using MSBuildSupportWPF.resources.codeNodeColor;
using Color = System.Drawing.Color;

namespace MSBuildSupport.code.codeBlocks;

public sealed class AttributeNode : CodeNode
{
    //Attribute node is for attribute name in tag
    //Example:
    // in <Project Sdk="Microsoft.NET.Sdk">
    // it will be "SdK"
    public AttributeNode(int lenght, int position, string codePart) : base(lenght, position, CodeNodeColorResource.GetColor("AttributeNode"), codePart)
    {
    }

    public override bool Equals(object? obj)
    {
        if (!(obj is AttributeNode)) return false;
        return base.Equals(obj);
    }
    public override string ToString()
    {
        return base.ToString()+"AttributeNode";
    }
}
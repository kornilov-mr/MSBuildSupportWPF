using MSBuildSupportWPF.resources.codeNodeColor;
using System.Drawing;

namespace MSBuildSupport.code.codeBlocks;

public class CommentNode : CodeNode
{
    //Comment node is for comment lines
    //Example:
    // In <!-- Target framework (e.g., net6.0, net7.0, netstandard2.1, etc.) -->
    // it will be "<!-- Target framework (e.g., net6.0, net7.0, netstandard2.1, etc.) -->"
    public CommentNode(int lenght, int position, string codePart) : base(lenght, position, CodeNodeColorResource.GetColor("CommentNode"), codePart)
    {
    }
    public override string ToString()
    {
        return base.ToString() + "CommentNode";
    }
}
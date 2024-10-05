using System.Collections.Generic;

namespace MSBuildSupport.code.codeBlocks;

public sealed class NodeComparator : IComparer<CodeNode>
{
    //Comparator for sortedList to contain nodes in position order
    public int Compare(CodeNode x, CodeNode y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if(x.Position > y.Position) return 1;
        if(x.Position < y.Position) return -1;
        return 0;
    }
}
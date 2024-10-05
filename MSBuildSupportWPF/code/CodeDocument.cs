using MSBuildSupport.code.codeBlocks;
using MSBuildSupportWPF.code.codeBlocks;
using MSBuildSupportWPF.documents;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSBuildSupportWPF.code
{
    //Class for navigation and orientation in text, which is represented by block tree
    public class CodeDocument : Document
    {

        public void RebuildCodeDocument(SortedSet<WhiteSpaceNode> whiteSpaceNodes)
        {
            CountOffsetOnStartLine(whiteSpaceNodes);
            calculateCharOnLineFromOffsetOnStartLine();
        }
        private void CountOffsetOnStartLine(SortedSet<WhiteSpaceNode> whiteSpaceNodes)
        {
            OffsetOnStartLine.Add(0);
            foreach (CodeNode node in whiteSpaceNodes)
            {
                OffsetOnStartLine.Add(node.Position + 1);

            }
        }
    }
}

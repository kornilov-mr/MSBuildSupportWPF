using MSBuildSupport.code.codeBlocks;
using MSBuildSupportWPF.resources.codeNodeColor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSBuildSupportWPF.code.codeBlocks
{
    //TagNodeEnd node is for tag opening
    //Example:
    // In <Nullable>enable</Nullable>
    // it will be "Nullable" at the end
    internal class TagNodeEnd : LinkedNode
    {
        public readonly int deapth;
        public readonly string tagName;
        public TagNodeEnd(int lenght, int position, string codePart, int deapth, string tagName) : base(lenght, position, CodeNodeColorResource.GetColor("TagNode"), codePart)
        {
            this.deapth = deapth;
            this.tagName = tagName;
        }
        public override bool CanLinkTo(LinkedNode node)
        {
            if (!(node is TagNodeStart)) return false;
            TagNodeStart tagNodeStart = (TagNodeStart)node;
            if (deapth != tagNodeStart.deapth) return false;
            if (!String.Equals(tagName, tagNodeStart.tagName)) return false;
            if (tagNodeStart.LinkedNodes.Count > 0) return false;
            return true;
        }
        public override bool Equals(object? obj)
        {
            if (!(obj is TagNodeEnd)) return false;
            return base.Equals(obj);
        }
        public override string ToString()
        {
            return base.ToString() + "TagNodeEnd";
        }
    }
}

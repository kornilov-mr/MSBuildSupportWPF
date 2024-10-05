using MSBuildSupport.code.codeBlocks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MSBuildSupportWPF.code.codeBlocks
{
    //Linked node is for code nodes, which if you edit one tag all of the linked node to it will be edited to
    //Example:
    // In <Nullable>enable</Nullable>
    // it will be "Nullable" at the start and "Nullable" at the end
    public abstract class LinkedNode : CodeNode
    {
        public HashSet<LinkedNode> LinkedNodes { get; } = new HashSet<LinkedNode>(); 
        public LinkedNode(int lenght, int position, Color hightLightColor, string codePart) : base(lenght, position, hightLightColor, codePart)
        {
        }
        public void BaseInsertChar(char c, int position)
        {
            base.InsertChar(c, position);
        }
        public override void InsertChar(char c, int position)
        {
            base.InsertChar(c, position);
            foreach (LinkedNode node in LinkedNodes)
            {
                node.BaseInsertChar(c, position);
            }
        }
        public void BaseInsertString(string s, int position)
        {
            base.InsertString(s, position);
        }
        public override void InsertString(string s, int position)
        {
            base.InsertString(s, position);
            foreach (LinkedNode node in LinkedNodes)
            {
                node.BaseInsertString(s, position);
            }
        }
        public void BaseDeleteChar(int position)
        {
            base.DeleteChar(position);
        }
        public override void DeleteChar(int position)
        {
            foreach (LinkedNode node in LinkedNodes)
            {
                node.BaseDeleteChar(position);
            }
            base.DeleteChar(position);

        }
        public abstract bool CanLinkTo(LinkedNode node);
        public void LinkNode(LinkedNode node)
        {
            if (!CanLinkTo(node)) return;
            if (!node.CanLinkTo(this)) return;
            LinkedNodes.Add(node);
            node.LinkNode(this);
        }
        public void UnLinkAll()
        {
            foreach (LinkedNode node in LinkedNodes)
            {
                node.LinkedNodes.Remove(node);
            }
            LinkedNodes.Clear();
        }
        public string BaseToString()
        {
            return base.ToString();
        }
        public override string ToString()
        {
            string linkedString = "";
            foreach(LinkedNode node in LinkedNodes)
            {
                linkedString += node.BaseToString();
            }
            return base.ToString()+ "linked to "+ linkedString+" ";
        }
    }
}

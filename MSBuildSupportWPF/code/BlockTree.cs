using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MSBuildSupport.code.codeBlocks;
using MSBuildSupport.XML;
using MSBuildSupportWPF.code;
using MSBuildSupportWPF.code.codeBlocks;
using MSBuildSupportWPF.UI.UIComponents;

namespace MSBuildSupport.code;

public sealed class BlockTree : IEnumerable<CodeNode>
{
    public SortedSet<CodeNode> nodes { get; private set; } = new SortedSet<CodeNode>(new NodeComparator());
    public SortedSet<WhiteSpaceNode> WhiteSpaceNodes { get; private set; } = new SortedSet<WhiteSpaceNode>(new NodeComparator());
    public List<LinkedNode> linkedNodes { get; private set; } = new List<LinkedNode>(); 
    private CodeNode startNode;
    private CodeNode endNode;
    //Object for orientation in Text which block tree represents
    public CodeDocument Document { get; }
    //Object, which parses XMl File
    private XMLValidor XMLValidor { get; }
    public MainCodeDisplay MainCodeDisplayAttached { private get; set; }
    public BlockTree()
    {
        Document= new CodeDocument();
        XMLValidor = new XMLValidor();
        startNode = new StartBlock();
        endNode = startNode;
        nodes.Add(startNode);
    }
    //Recalculates all offsets in block tree
    public void RevalidateTree()
    {
        endNode.RevalidateAllPositionBefore();
    }
    //Adds node on top and links to the top node
    public void AddNodeOnTop(CodeNode node)
    {
        if(node is WhiteSpaceNode)
        {
            WhiteSpaceNodes.Add((WhiteSpaceNode)node);
        }
        if(node is LinkedNode)
        {
            linkedNodes.Add((LinkedNode)node);
        }
        endNode.Child = node;
        node.Parent = endNode;
        endNode = node;
        nodes.Add(node);

        node.BlockTreeAttached = this;
    }
    //Deletes all Code nodes whose text are selected
    //Deletion goes from end to start
    public void DeleteBlockOfText(int startOffset, int length)
    {
        CodeNode codeNode = GetNodeOnPosition(startOffset);
        int positionInFirstCode = startOffset - codeNode.Position;

        if(positionInFirstCode >= length)
        {
            //If string to delete is only in one code Node
            codeNode.DeleteString(positionInFirstCode - length, positionInFirstCode);
            return;
        }
        int lengthRemaining = length;

        CodeNode currNode = codeNode;
        //Deletes all nodes which have selected string
        while (lengthRemaining > 0)
        {
            CodeNode prevNode = currNode.Parent;
            lengthRemaining -= currNode.Lenght;
            currNode.DeleteString(currNode.Lenght-lengthRemaining- currNode.Lenght, currNode.Lenght);
            currNode = prevNode;
        }
        RevalidateTree();
    }
    //Deletes node from all 3 sets
    private void DeleteNodeFromSets(CodeNode node)
    {
        nodes.Remove(node);
        if (node is WhiteSpaceNode)
        {
            WhiteSpaceNodes.Remove((WhiteSpaceNode)node);
        }
        if (node is LinkedNode)
        {
            LinkedNode linkedNode = (LinkedNode)node;
            linkedNode.UnLinkAll();
            linkedNodes.Remove(linkedNode);
        }
    }
    //Excludes node from tree, properly changing parents and childs of closest nodes
    public void ExcludeNodeFromTree(CodeNode node)
    {
       
        CodeNode prevNode = node.Parent;
        CodeNode nextNode = node.Child;

        if (prevNode is null)
        {
            nextNode.Parent = new StartBlock();
            DeleteNodeFromSets(node);
            return;
        }
        if (nextNode is null)
        {
            prevNode.Child = null;
            DeleteNodeFromSets(node);
            return;
        }

        prevNode.Child = nextNode;
        nextNode.Parent = prevNode;
        DeleteNodeFromSets(node);

    }

    //Fills all blanks in Tree after parsing
    public void FillSpaceNodes()
    {
        CodeNode prevNode = null;
        CodeNode[] nodesCopy =new CodeNode[nodes.Count];
        nodes.CopyTo(nodesCopy);
        foreach (CodeNode currNode in nodesCopy)
        {
            if (prevNode == null)
            {
                prevNode = currNode;
                continue;
            }

            if (prevNode.Position + prevNode.Lenght < currNode.Position)
            {
                CodeNode fillNode = new SpaceNode(currNode.Position - prevNode.Position - prevNode.Lenght,prevNode.Position + prevNode.Lenght);
                fillNode.BlockTreeAttached = this;

                prevNode.Child = fillNode;
                currNode.Parent = fillNode;
                fillNode.Parent = prevNode;
                fillNode.Child = currNode;
                nodes.Add(fillNode);
            }

            prevNode = currNode;
        }
    }
    //Tries to link every linked node with each other
    public void linkNodes()
    {
        for(int i = 0; i < linkedNodes.Count; i++)
        {
            LinkedNode node = linkedNodes[i];
            for(int j = 0; j < linkedNodes.Count; j++)
            {
                if (node.CanLinkTo(linkedNodes[j]))
                {
                    node.LinkNode(linkedNodes[j]);
                }
            }
        }
    }
    //Returns node which is located on given offset from the start of the text
    public CodeNode GetNodeOnPosition(int position)
    {
        //can be made faster to O(log n) using self written lowerBound 
        foreach (CodeNode currNode in nodes)
        {

            if (currNode.Position < position && currNode.Position+ currNode.Lenght >= position)
            {
                return currNode;
            }

        }
        return endNode;
    }
    //Returns lastNode which isn't white space or space node
    public CodeNode GetLastValibleNode(int offset)
    {
        CodeNode currNode = GetNodeOnPosition(offset);
        while(currNode.Parent is not null)
        {
            if(currNode is not SpaceNode && currNode is not WhiteSpaceNode)
            {
                return currNode;
            }
            currNode = currNode.Parent;
        }
        return currNode;
    }
    //Parses XML document, creating new BlockTree
    public void rebuildFromXMLDocument(XMLDocument xMLDocument)
    {
        BlockTree newBlockTree = XMLValidor.createBlockTreeFromXML(xMLDocument);
        SortedSet<CodeNode> newNodes = newBlockTree.nodes;
        nodes.Clear();
        foreach (CodeNode currNode in newNodes)
        {
            AddNodeOnTop(currNode);
        }
        Document.RebuildCodeDocument(WhiteSpaceNodes);
    }
    public IEnumerator<CodeNode> GetEnumerator()
    {
        return nodes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return nodes.GetEnumerator();
    }
    public void NotifyAboutChangesInNode()
    {
        Document.RebuildCodeDocument(WhiteSpaceNodes);
    }
    public void ShowErrorPopup(Exception e, int offset)
    {
        MainCodeDisplayAttached.ShowPopup(e, offset);
    }
    public void HideErrorPopup()
    {
        MainCodeDisplayAttached.HidePopup();
    }
    public override string ToString()
    {
        string s = string.Empty;
        foreach(CodeNode currNode in nodes)
        {
            s += currNode.ToString() + "\n";
        }
        return s;
    }
}
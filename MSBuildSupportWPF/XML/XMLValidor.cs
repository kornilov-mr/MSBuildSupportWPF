using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using MSBuildSupport.code;
using MSBuildSupport.code.codeBlocks;
using MSBuildSupportWPF.code.codeBlocks;
using MSBuildSupportWPF.UI.UIComponents;

namespace MSBuildSupport.XML;

public sealed class XMLValidor
{
    public BlockTree createBlockTreeFromXML(XMLDocument xmlDocument)
    {
        string xmlString = xmlDocument.XmlString;
        BlockTree blockTree = new BlockTree();

        NameTable nt = new NameTable();

        XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);

        XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);

        XmlTextReader reader = new XmlTextReader(xmlString, XmlNodeType.Element, context);
            while (reader.Read())
            {
                int offSetPosition = xmlDocument.GetPositionOnStartLine(reader.LineNumber - 1) + reader.LinePosition - 1;
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    blockTree.AddNodeOnTop(new BracketNode(1, offSetPosition - 1, "<"));
                    blockTree.AddNodeOnTop(new TagNodeStart(reader.Name.Length, offSetPosition, reader.Name, reader.Depth, reader.Name));
                    int endOfTagAttributes = xmlString.Substring(offSetPosition).IndexOf('>') + offSetPosition;
                    string attributeString = xmlString.Substring(offSetPosition, endOfTagAttributes - offSetPosition).Replace(reader.Name, "");

                    List<CodeNode> codeNodes = AttributeParser.parserXMLAttributeString(attributeString, offSetPosition + reader.Name.Length);

                    foreach (CodeNode codeNode in codeNodes)
                    {
                        blockTree.AddNodeOnTop(codeNode);
                    }
                    blockTree.AddNodeOnTop(new BracketNode(1, endOfTagAttributes, ">"));
                    break;
                case XmlNodeType.Text:
                    blockTree.AddNodeOnTop(new TagBodyNode(reader.Value.Length, offSetPosition, reader.Value));
                    break;
                case XmlNodeType.EndElement:
                    blockTree.AddNodeOnTop(new BracketNode(2, offSetPosition - 2, "</"));
                    if (reader.Name.Length != 0)
                    {
                        blockTree.AddNodeOnTop(new TagNodeEnd(reader.Name.Length, offSetPosition, reader.Name, reader.Depth, reader.Name));
                    }
                    blockTree.AddNodeOnTop(new BracketNode(1, offSetPosition + reader.Name.Length, ">"));

                        break;
                    case XmlNodeType.Comment:
                        blockTree.AddNodeOnTop(new CommentNode(reader.Value.Length + 7, offSetPosition - 4, "<!--" + reader.Value + "-->"));
                        break;
                    case XmlNodeType.Whitespace:
                        blockTree.AddNodeOnTop(new WhiteSpaceNode(offSetPosition));
                        break;

                }
            }

        reader.Close();
        blockTree.FillSpaceNodes();
        blockTree.linkNodes();
        return blockTree;
    }
}
using System;
using MSBuildSupport.code;
using MSBuildSupport.XML;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using MSBuildSupport.code.codeBlocks;
using MSBuildSupportWPF.code.codeBlocks;

namespace MSBuildSupport.tests.unitTests;
[TestClass]
public class XMLParserTest
{
    [TestMethod]
    public void XMLParserTestInit()
    {
        string stringExpected = "1 -1  StartBlock\n1 0 < BracketNode\n7 1 Project linked to 7 221 Project  TagNodeStart\n2 8    SpaceNode\n3 10 Sdk AttributeNode\n1 13 = SpecialNode\n19 14 \"Microsoft.NET.Sdk\" ParameterNode\n1 33 > BracketNode\n1 34 \n WhiteSpaceNode\n5 35       SpaceNode\n1 40 < BracketNode\n13 41 PropertyGroup linked to 13 203 PropertyGroup  TagNodeStart\n1 54 > BracketNode\n1 55 \n WhiteSpaceNode\n8 56          SpaceNode\n70 64 <!-- Target framework (e.g., net6.0, net7.0, netstandard2.1, etc.) --> CommentNode\n1 134 \n WhiteSpaceNode\n8 135          SpaceNode\n1 143 < BracketNode\n15 144 TargetFramework linked to 15 180 TargetFramework  TagNodeStart\n1 159   SpaceNode\n4 160 test AttributeNode\n1 164 = SpecialNode\n6 165 \"test\" ParameterNode\n1 171 > BracketNode\n6 172 net7.0 TagBodyNode\n2 178 </ BracketNode\n15 180 TargetFramework linked to  TagNodeEnd\n1 195 > BracketNode\n1 196 \n WhiteSpaceNode\n4 197      SpaceNode\n2 201 </ BracketNode\n13 203 PropertyGroup linked to  TagNodeEnd\n1 216 > BracketNode\n1 217 \n WhiteSpaceNode\n1 218   SpaceNode\n2 219 </ BracketNode\n7 221 Project linked to  TagNodeEnd\n1 228 > BracketNode\n";
        string xmlString = "<Project  Sdk=\"Microsoft.NET.Sdk\">\n" +
                           "\n" +
                           "    <PropertyGroup>\n" +
                           "        <!-- Target framework (e.g., net6.0, net7.0, netstandard2.1, etc.) -->\n" +
                           "        <TargetFramework test=\"test\">net7.0</TargetFramework>\n" +
                           "    </PropertyGroup>\n" +
                           "\n" +
                           "</Project>";
        XMLDocument xmlDocument = new XMLDocument(xmlString);
        XMLValidor xmlValidor = new XMLValidor();
        BlockTree blockTree = xmlValidor.createBlockTreeFromXML(xmlDocument);
        if (!String.Equals(stringExpected, blockTree.ToString()))
        {
            Assert.Fail();
        }
        Assert.IsTrue(true);
    }
}
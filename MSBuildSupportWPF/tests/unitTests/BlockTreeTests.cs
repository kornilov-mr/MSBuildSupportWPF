using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildSupport.code;
using MSBuildSupport.code.codeBlocks;
using MSBuildSupport.XML;
using MSBuildSupportWPF.code.codeBlocks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MSBuildSupportWPF.tests.unitTests
{
    [TestClass]
    public class BlockTreeTests
    {
        [TestMethod]
        public void GetCodeNodeOnPostionTest()
        {
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
            if(!blockTree.GetNodeOnPosition(100).Equals(new CommentNode(70, 64, "<!-- Target framework (e.g., net6.0, net7.0, netstandard2.1, etc.) -->")))
            {
                Assert.Fail();
            }
            if (!blockTree.GetNodeOnPosition(200).Equals(new SpaceNode(4, 197)))
            {
                Assert.Fail();
            }
            if (!blockTree.GetNodeOnPosition(56).Equals(new WhiteSpaceNode(55)))
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void DeleteCharBlockTreeTest()
        {
            string stringExpected = "1 -1  StartBlock\n1 0 < BracketNode\n7 1 Project linked to 7 220 Project  TagNodeStart\n2 8    SpaceNode\n3 10 Sdk AttributeNode\n1 13 = SpecialNode\n19 14 \"Microsoft.NET.Sdk\" ParameterNode\n1 33 > BracketNode\n1 34 \n WhiteSpaceNode\n5 35       SpaceNode\n1 40 < BracketNode\n13 41 PropertyGroup linked to 13 202 PropertyGroup  TagNodeStart\n1 54 > BracketNode\n1 55 \n WhiteSpaceNode\n8 56          SpaceNode\n69 64 <-- Target framework (e.g., net6.0, net7.0, netstandard2.1, etc.) --> CommentNode\n1 133 \n WhiteSpaceNode\n8 134          SpaceNode\n1 142 < BracketNode\n15 143 TargetFramework linked to 15 179 TargetFramework  TagNodeStart\n1 158   SpaceNode\n4 159 test AttributeNode\n1 163 = SpecialNode\n6 164 \"test\" ParameterNode\n1 170 > BracketNode\n6 171 net7.0 TagBodyNode\n2 177 </ BracketNode\n15 179 TargetFramework linked to  TagNodeEnd\n1 194 > BracketNode\n1 195 \n WhiteSpaceNode\n4 196      SpaceNode\n2 200 </ BracketNode\n13 202 PropertyGroup linked to  TagNodeEnd\n1 215 > BracketNode\n1 216 \n WhiteSpaceNode\n1 217   SpaceNode\n2 218 </ BracketNode\n7 220 Project linked to  TagNodeEnd\n1 227 > BracketNode\n";
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
            blockTree.GetNodeOnPosition(76).DeleteChar(2);
            if (!String.Equals(stringExpected, blockTree.ToString()))
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void DeleteStringBlockTreeTest()
        {
            string stringExpected = "1 -1  StartBlock\n1 0 < BracketNode\n7 1 Project linked to 7 187 Project  TagNodeStart\n2 8    SpaceNode\n3 10 Sdk AttributeNode\n1 13 = SpecialNode\n19 14 \"Microsoft.NET.Sdk\" ParameterNode\n1 33 > BracketNode\n1 34 \n WhiteSpaceNode\n5 35       SpaceNode\n1 40 < BracketNode\n13 41 PropertyGroup linked to 13 169 PropertyGroup  TagNodeStart\n1 54 > BracketNode\n1 55 \n WhiteSpaceNode\n8 56          SpaceNode\n36 64 <, net7.0, netstandard2.1, etc.) --> CommentNode\n1 100 \n WhiteSpaceNode\n8 101          SpaceNode\n1 109 < BracketNode\n15 110 TargetFramework linked to 15 146 TargetFramework  TagNodeStart\n1 125   SpaceNode\n4 126 test AttributeNode\n1 130 = SpecialNode\n6 131 \"test\" ParameterNode\n1 137 > BracketNode\n6 138 net7.0 TagBodyNode\n2 144 </ BracketNode\n15 146 TargetFramework linked to  TagNodeEnd\n1 161 > BracketNode\n1 162 \n WhiteSpaceNode\n4 163      SpaceNode\n2 167 </ BracketNode\n13 169 PropertyGroup linked to  TagNodeEnd\n1 182 > BracketNode\n1 183 \n WhiteSpaceNode\n1 184   SpaceNode\n2 185 </ BracketNode\n7 187 Project linked to  TagNodeEnd\n1 194 > BracketNode\n";
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
            blockTree.GetNodeOnPosition(76).DeleteString(1,35);
            if (!String.Equals(stringExpected, blockTree.ToString()))
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void InsertCharBlockTreeTest()
        {
            string stringExpected = "1 -1  StartBlock\n1 0 < BracketNode\n7 1 Project linked to 7 222 Project  TagNodeStart\n2 8    SpaceNode\n3 10 Sdk AttributeNode\n1 13 = SpecialNode\n19 14 \"Microsoft.NET.Sdk\" ParameterNode\n1 33 > BracketNode\n1 34 \n WhiteSpaceNode\n5 35       SpaceNode\n1 40 < BracketNode\n13 41 PropertyGroup linked to 13 204 PropertyGroup  TagNodeStart\n1 54 > BracketNode\n1 55 \n WhiteSpaceNode\n8 56          SpaceNode\n71 64 <!-- Target framework (e.g., net6.0d, net7.0, netstandard2.1, etc.) --> CommentNode\n1 135 \n WhiteSpaceNode\n8 136          SpaceNode\n1 144 < BracketNode\n15 145 TargetFramework linked to 15 181 TargetFramework  TagNodeStart\n1 160   SpaceNode\n4 161 test AttributeNode\n1 165 = SpecialNode\n6 166 \"test\" ParameterNode\n1 172 > BracketNode\n6 173 net7.0 TagBodyNode\n2 179 </ BracketNode\n15 181 TargetFramework linked to  TagNodeEnd\n1 196 > BracketNode\n1 197 \n WhiteSpaceNode\n4 198      SpaceNode\n2 202 </ BracketNode\n13 204 PropertyGroup linked to  TagNodeEnd\n1 217 > BracketNode\n1 218 \n WhiteSpaceNode\n1 219   SpaceNode\n2 220 </ BracketNode\n7 222 Project linked to  TagNodeEnd\n1 229 > BracketNode\n";
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
            blockTree.GetNodeOnPosition(76).InsertChar('d',35);
            if (!String.Equals(stringExpected, blockTree.ToString()))
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void InsertStringBlockTreeTest()
        {
            string stringExpected = "1 -1  StartBlock\n1 0 < BracketNode\n7 1 Project linked to 7 229 Project  TagNodeStart\n2 8    SpaceNode\n3 10 Sdk AttributeNode\n1 13 = SpecialNode\n19 14 \"Microsoft.NET.Sdk\" ParameterNode\n1 33 > BracketNode\n1 34 \n WhiteSpaceNode\n5 35       SpaceNode\n1 40 < BracketNode\n13 41 PropertyGroup linked to 13 211 PropertyGroup  TagNodeStart\n1 54 > BracketNode\n1 55 \n WhiteSpaceNode\n8 56          SpaceNode\n78 64 <!-- Target framework (e.g., net6.0testtest, net7.0, netstandard2.1, etc.) --> CommentNode\n1 142 \n WhiteSpaceNode\n8 143          SpaceNode\n1 151 < BracketNode\n15 152 TargetFramework linked to 15 188 TargetFramework  TagNodeStart\n1 167   SpaceNode\n4 168 test AttributeNode\n1 172 = SpecialNode\n6 173 \"test\" ParameterNode\n1 179 > BracketNode\n6 180 net7.0 TagBodyNode\n2 186 </ BracketNode\n15 188 TargetFramework linked to  TagNodeEnd\n1 203 > BracketNode\n1 204 \n WhiteSpaceNode\n4 205      SpaceNode\n2 209 </ BracketNode\n13 211 PropertyGroup linked to  TagNodeEnd\n1 224 > BracketNode\n1 225 \n WhiteSpaceNode\n1 226   SpaceNode\n2 227 </ BracketNode\n7 229 Project linked to  TagNodeEnd\n1 236 > BracketNode\n";
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
            blockTree.GetNodeOnPosition(76).InsertString("testtest", 35);
            if (!String.Equals(stringExpected, blockTree.ToString()))
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void EditLinkedNodeTest()
        {
            string stringExpected = "1 -1  StartBlock\n1 0 < BracketNode\n15 1 Prtesttestoject linked to 15 229 Prtesttestoject  TagNodeStart\n2 16    SpaceNode\n3 18 Sdk AttributeNode\n1 21 = SpecialNode\n19 22 \"Microsoft.NET.Sdk\" ParameterNode\n1 41 > BracketNode\n1 42 \n WhiteSpaceNode\n5 43       SpaceNode\n1 48 < BracketNode\n13 49 PropertyGroup linked to 13 211 PropertyGroup  TagNodeStart\n1 62 > BracketNode\n1 63 \n WhiteSpaceNode\n8 64          SpaceNode\n70 72 <!-- Target framework (e.g., net6.0, net7.0, netstandard2.1, etc.) --> CommentNode\n1 142 \n WhiteSpaceNode\n8 143          SpaceNode\n1 151 < BracketNode\n15 152 TargetFramework linked to 15 188 TargetFramework  TagNodeStart\n1 167   SpaceNode\n4 168 test AttributeNode\n1 172 = SpecialNode\n6 173 \"test\" ParameterNode\n1 179 > BracketNode\n6 180 net7.0 TagBodyNode\n2 186 </ BracketNode\n15 188 TargetFramework linked to  TagNodeEnd\n1 203 > BracketNode\n1 204 \n WhiteSpaceNode\n4 205      SpaceNode\n2 209 </ BracketNode\n13 211 PropertyGroup linked to  TagNodeEnd\n1 224 > BracketNode\n1 225 \n WhiteSpaceNode\n1 226   SpaceNode\n2 227 </ BracketNode\n15 229 Prtesttestoject linked to  TagNodeEnd\n1 244 > BracketNode\n";
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
            blockTree.GetNodeOnPosition(3).InsertString("testtest", 2);
            if (!String.Equals(stringExpected, blockTree.ToString()))
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void DeleteTextBlockBLockTreeTest()
        {
            string stringExpected = "1 -1  StartBlock\n1 0 < BracketNode\n7 1 Project linked to 7 211 Project  TagNodeStart\n2 8    SpaceNode\n3 10 Sdk AttributeNode\n1 13 = SpecialNode\n16 14 \"Microsoft.NET.S ParameterNode\n1 30 < BracketNode\n13 31 PropertyGroup linked to 13 193 PropertyGroup  TagNodeStart\n1 44 > BracketNode\n1 45 \n WhiteSpaceNode\n8 46          SpaceNode\n70 54 <!-- Target framework (e.g., net6.0, net7.0, netstandard2.1, etc.) --> CommentNode\n1 124 \n WhiteSpaceNode\n8 125          SpaceNode\n1 133 < BracketNode\n15 134 TargetFramework linked to 15 170 TargetFramework  TagNodeStart\n1 149   SpaceNode\n4 150 test AttributeNode\n1 154 = SpecialNode\n6 155 \"test\" ParameterNode\n1 161 > BracketNode\n6 162 net7.0 TagBodyNode\n2 168 </ BracketNode\n15 170 TargetFramework linked to  TagNodeEnd\n1 185 > BracketNode\n1 186 \n WhiteSpaceNode\n4 187      SpaceNode\n2 191 </ BracketNode\n13 193 PropertyGroup linked to  TagNodeEnd\n1 206 > BracketNode\n1 207 \n WhiteSpaceNode\n1 208   SpaceNode\n2 209 </ BracketNode\n7 211 Project linked to  TagNodeEnd\n1 218 > BracketNode\n";
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
            blockTree.DeleteBlockOfText(40, 10);
            if (!String.Equals(stringExpected, blockTree.ToString()))
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }
    }
}

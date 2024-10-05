using System.Collections.Generic;
using MSBuildSupport.code.codeBlocks;
using MSBuildSupport.XML;
using MSBuildSupport.XML.attributeParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace MSBuildSupportWPF.tests.unitTests;

[TestClass]
public class AttributeParserTests
{
    [TestMethod]
    public void LexerTest()
    {
        List<Token> tokensExpected = new List<Token>();
        tokensExpected.Add(new Token("Include",TokenEnum.attributeName));
        tokensExpected.Add(new Token("=",TokenEnum.equals));
        tokensExpected.Add(new Token("\"Newtonsoft.Json\" ",TokenEnum.attributeValue));
        tokensExpected.Add(new Token("Version",TokenEnum.attributeName));
        tokensExpected.Add(new Token("=",TokenEnum.equals));
        tokensExpected.Add(new Token("\"13.0.3\" ",TokenEnum.attributeValue));
        
        string attributeString = "Include=\"Newtonsoft.Json\" Version=\"13.0.3\" ";
        List<Token> tokens = AttributeParser.Lexer(attributeString);

        if (tokens.Count != tokensExpected.Count)
        {
            Assert.Fail();
        }

        for (int i = 0; i < tokens.Count; i++)
        {
            if (!tokens[i].Equals(tokensExpected[i]))
            {
                Assert.Fail();
            }
        }
        Assert.IsTrue(true);
    }
    [TestMethod]
    public void ParserTest()
    {
        List<CodeNode> nodesExpected = new List<CodeNode>();
        nodesExpected.Add(new SpaceNode(1,0));
        nodesExpected.Add(new AttributeNode(7,1,"test"));
        nodesExpected.Add(new SpaceNode(1,8));
        nodesExpected.Add(new ParameterNode(18,9, "test"));
        nodesExpected.Add(new AttributeNode(7,27, "test"));
        nodesExpected.Add(new SpaceNode(1,34));
        nodesExpected.Add(new ParameterNode(9,35, "test"));

        string attributeString = " Include=\"Newtonsoft.Json\" Version=\"13.0.3\" ";
        List<CodeNode> nodes = AttributeParser.parserXMLAttributeString(attributeString,0);
        if (nodes.Count != nodesExpected.Count)
        {
            Assert.Fail();
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            if (!nodes[i].Equals(nodesExpected[i]))
            {
                Assert.Fail();
            }
        }

        Assert.IsTrue(true);
    }
}
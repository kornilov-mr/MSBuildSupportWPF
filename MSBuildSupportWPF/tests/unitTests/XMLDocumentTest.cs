using System;
using System.Collections.Generic;
using MSBuildSupport.XML;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace MSBuildSupport.tests.unitTests;

[TestClass]
public class XMLDocumentTest
{
    [TestMethod]
    public void XMLDocumentCharOnLineCounterTest()
    {
        List<int> charOnLineCounterExpected = new List<int> {34, 1, 20, 79, 50, 1, 11};
        string xmlString = "<Project Sdk=\"Microsoft.NET.Sdk\">\n" +
                           "\n" +
                           "    <PropertyGroup>\n" +
                           "        <!-- Target framework (e.g., net6.0, net7.0, netstandard2.1, etc.) -->\n" +
                           "        <TargetFramework>net7.0</TargetFramework>\n" +
                           "\n" +
                           "</Project>";
        XMLDocument xmlDocument = new XMLDocument(xmlString);
        if (charOnLineCounterExpected.Count!=xmlDocument.LengthOfLine.Count)
        {
            Assert.Fail();
        }
        

        for (int i = 0; i < xmlDocument.LengthOfLine.Count; i++)
        {
            if (xmlDocument.LengthOfLine[i] != charOnLineCounterExpected[i])
            {
                Assert.Fail();
            }
        }

        Assert.IsTrue(true);
    }
    [TestMethod]
    public void XMLDocumentCharLineStartTest()
    {
        List<int> GetPositionOnStartLineExpected = new List<int> { 0, 34, 35, 55, 134, 184, 185,196 };

        string xmlString = "<Project Sdk=\"Microsoft.NET.Sdk\">\n" +
                           "\n" +
                           "    <PropertyGroup>\n" +
                           "        <!-- Target framework (e.g., net6.0, net7.0, netstandard2.1, etc.) -->\n" +
                           "        <TargetFramework>net7.0</TargetFramework>\n" +
                           "\n" +
                           "</Project>";
        XMLDocument xmlDocument = new XMLDocument(xmlString);
        if (GetPositionOnStartLineExpected.Count != xmlDocument.OffsetOnStartLine.Count)
        {
            Assert.Fail();
        }


        for (int i = 0; i < xmlDocument.OffsetOnStartLine.Count; i++)
        {
            if (xmlDocument.OffsetOnStartLine[i] != GetPositionOnStartLineExpected[i])
            {
                Assert.Fail();
            }
        }
        Assert.IsTrue(true);
    }
}
using MSBuildSupportWPF.documents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Policy;

namespace MSBuildSupport.XML;

public class XMLDocument : Document
{
    //Class for navigation and orientation in xml
    public string XmlString { get; }

    public XMLDocument(string XmlString)
    {
        this.XmlString = XmlString;
        Console.WriteLine(XmlString);
        CountLengthOfLine();
        CalculateOffsetOnStartLineFromCharOnLine();
    }

    private void CountLengthOfLine()
    {
        String[] strings = XmlString.Split("\n");
        foreach (string s in strings)
        {
            LengthOfLine.Add(s.Length+1);
        }

    }



}
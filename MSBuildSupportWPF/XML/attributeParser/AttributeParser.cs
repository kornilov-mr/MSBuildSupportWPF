using System;
using System.Collections.Generic;
using System.Text;
using MSBuildSupport.code.codeBlocks;
using MSBuildSupport.XML.attributeParser;
using MSBuildSupportWPF.code.codeBlocks;

namespace MSBuildSupport.XML;

public class AttributeParser
{
    //Small parser for Attributes, because XMLParser from Microsoft doesn't give excact position of attributes
    public static List<CodeNode> parserXMLAttributeString(string attributeString, int stringStartPosition)
    {
        List<CodeNode> nodes = new List<CodeNode>();
        string spaces = "";
        int spaceCount = 0;
        for (int i = 0; i < attributeString.Length; i++)
        {
            if (attributeString[i] != ' ')
            {
                break;
            }
            
            spaceCount += 1;
            spaces += ' ';
        }

        if (spaceCount != 0)
        {
            nodes.Add(new SpaceNode(spaceCount, stringStartPosition));
        }
        attributeString = attributeString.Substring(spaceCount);
        stringStartPosition+= spaceCount;
        
        List<Token> tokens = Lexer(attributeString);

        foreach (Token token in tokens)
        {
            switch (token.tokenType)
            {
                case TokenEnum.attributeName :
                    nodes.Add(new AttributeNode(token.tokenValue.Length, stringStartPosition, token.tokenValue));
                    stringStartPosition+= token.tokenValue.Length;
                    break;
                case TokenEnum.equals :
                    nodes.Add(new SpecialNode(token.tokenValue.Length, stringStartPosition,"="));
                    stringStartPosition+= token.tokenValue.Length;
                    break;
                case TokenEnum.attributeValue :
                    nodes.Add(new ParameterNode(token.tokenValue.Length, stringStartPosition, token.tokenValue));
                    stringStartPosition+= token.tokenValue.Length;
                    break;
                case TokenEnum.emptySpace:
                    nodes.Add(new SpaceNode(token.tokenValue.Length, stringStartPosition));
                    stringStartPosition+= token.tokenValue.Length;
                    break;
            }
        }
        return nodes;
    }

    public static List<Token> Lexer(string attributeString)
    {
        List<Token> tokens = new List<Token>();
        StringBuilder sb = new StringBuilder();

        bool lastWasEmpty = false;
        foreach (char c in attributeString)
        {
            if (c == '=')
            {
                tokens.Add(new Token(sb.ToString(),TokenEnum.attributeName));
                tokens.Add(new Token("=",TokenEnum.equals));
                sb.Clear();
                lastWasEmpty = false;

            }else if (c == ' ')
            {
                if (lastWasEmpty)
                {
                    tokens.Add(new Token(" ",TokenEnum.emptySpace));
                }
                else
                {
                    tokens.Add(new Token(sb.ToString()+' ',TokenEnum.attributeValue));
                }

                sb.Clear();
                lastWasEmpty = true;
            }
            else
            {
                sb.Append(c);
                lastWasEmpty = false;

            }
        }

        if (sb.ToString()!="")
        {
            tokens.Add(new Token(sb.ToString(),TokenEnum.attributeValue));
        }
        return tokens;
    }
}
namespace MSBuildSupport.XML.attributeParser;

public class Token
{
    public readonly TokenEnum tokenType;
    public readonly string tokenValue;

    public Token(string tokenValue, TokenEnum tokenType)
    {
        this.tokenValue = tokenValue;
        this.tokenType = tokenType;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (!(obj is Token))
        {
            return false;
        }
        Token token = (Token)obj;
        if (tokenType != token.tokenType)
        {
            return false;
        }

        if (tokenValue != token.tokenValue)
        {
            return false;
        }
        return true;
    }
}
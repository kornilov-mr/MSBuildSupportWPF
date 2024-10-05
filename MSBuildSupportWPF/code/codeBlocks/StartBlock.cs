namespace MSBuildSupport.code.codeBlocks;

public sealed class StartBlock : CodeNode
{
    //StartBlock is used to ensure that Parent in Node is always not null
    public StartBlock() : base(1, -1,"")
    {
    }

    public override void RevalidateAllPositionBefore()
    {
        
    }

    public override bool Equals(object? obj)
    {
        if (!(obj is StartBlock)) return false;
        return base.Equals(obj);
    }
    public override string ToString()
    {
        return base.ToString() + "StartBlock";
    }
}
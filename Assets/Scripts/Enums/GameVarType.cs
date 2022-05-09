public enum IGameVarType
{
    BOOL = 0,
    INT = 1,
    FLOAT = 2,

    CHARACTER = 10
}

public abstract class IGameVarWrapper
{
    public abstract IGameVarType GetGameVarType();

    public abstract string ToString();
}

public class BoolVarWrapper : IGameVarWrapper
{
    public bool BoolValue;

    public BoolVarWrapper(bool boolValue)
    {
        BoolValue = boolValue;
    }

    public override IGameVarType GetGameVarType() => IGameVarType.BOOL;

    public override string ToString() => BoolValue.ToString();
}

public class IntVarWrapper : IGameVarWrapper
{
    public int IntValue;

    public IntVarWrapper(int intValue)
    {
        IntValue = intValue;
    }

    public override IGameVarType GetGameVarType() => IGameVarType.INT;

    public override string ToString() => IntValue.ToString();
}

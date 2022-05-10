public enum EGameVarType
{
    NULL = 0,
    BOOL = 1,
    INT = 2,
    FLOAT = 3,
    STRING = 4,

    CHARACTER = 100
}

public abstract class GameVarWrapper
{
    public abstract EGameVarType GetGameVarType();

    public abstract string ToString();
}

public class NullVarWrapper : GameVarWrapper
{
    public NullVarWrapper()
    {
    }

    public override EGameVarType GetGameVarType() => EGameVarType.NULL;

    public override string ToString() => "NULL";
}

public class BoolVarWrapper : GameVarWrapper
{
    public bool BoolValue;

    public BoolVarWrapper(bool value)
    {
        BoolValue = value;
    }

    public override EGameVarType GetGameVarType() => EGameVarType.BOOL;

    public override string ToString() => BoolValue.ToString();
}

public class IntVarWrapper : GameVarWrapper
{
    public int IntValue;

    public IntVarWrapper(int value)
    {
        IntValue = value;
    }

    public override EGameVarType GetGameVarType() => EGameVarType.INT;

    public override string ToString() => IntValue.ToString();
}

public class StringVarWrapper : GameVarWrapper
{
    public string StringValue;

    public StringVarWrapper(string value)
    {
        StringValue = value;
    }

    public override EGameVarType GetGameVarType() => EGameVarType.STRING;

    public override string ToString() => StringValue;
}

public class CharacterVarWrapper : GameVarWrapper
{
    public CharacterManager Character;

    public CharacterVarWrapper(CharacterManager value)
    {
        Character = value;
    }

    public override EGameVarType GetGameVarType() => EGameVarType.CHARACTER;

    public override string ToString() => Character.ToString();
}

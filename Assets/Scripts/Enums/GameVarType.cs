using UnityEngine;
using System.Diagnostics;
using static CharacterStats;

public enum EGameVarType
{
    NULL = 0,
    BOOL = 1,
    INT = 2,
    FLOAT = 3,
    STRING = 4,

    CHARACTER = 100,
    CHARACTER_STAT = 200
}

public abstract class GameVarWrapper
{
    public abstract EGameVarType GetGameVarType();

    public abstract string ToString();

    public abstract bool IsSameAs(GameVarWrapper other);
}

public class NullVarWrapper : GameVarWrapper
{
    public NullVarWrapper()
    {
    }

    public override EGameVarType GetGameVarType() => EGameVarType.NULL;

    public override string ToString() => "NULL";

    public override bool IsSameAs(GameVarWrapper other)
    {
        return (other.GetGameVarType() == EGameVarType.NULL);
    }

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

    public override bool IsSameAs(GameVarWrapper other)
    {
        if (other.GetGameVarType() != EGameVarType.BOOL)
        {
            return false;
        }

        return ((other as BoolVarWrapper).BoolValue == BoolValue);
    }
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

    public override bool IsSameAs(GameVarWrapper other)
    {
        if (other.GetGameVarType() != EGameVarType.INT)
        {
            return false;
        }

        return ((other as IntVarWrapper).IntValue == IntValue);
    }
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

    public override bool IsSameAs(GameVarWrapper other)
    {
        if (other.GetGameVarType() != EGameVarType.STRING)
        {
            return false;
        }

        return ((other as StringVarWrapper).StringValue.Equals(StringValue));
    }
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

    public override bool IsSameAs(GameVarWrapper other)
    {
        if (other.GetGameVarType() != EGameVarType.CHARACTER)
        {
            return false;
        }

        CharacterManager otherChara = (other as CharacterVarWrapper).Character;

        if (otherChara.GetEntityID() == -1
            || Character.GetEntityID() == -1)
        {
            UnityEngine.Debug.LogWarning("One of those two character has an ID at -1\n"
                + otherChara.name + " ID : " + otherChara.GetEntityID() + "\n"
                + Character.name + " ID : " + Character.GetEntityID());
            return (otherChara.name.Equals(Character.name));
        }

        return ((other as CharacterVarWrapper).Character.GetEntityID() == Character.GetEntityID());
    }
}

public class CharacterStatVarWrapper : GameVarWrapper
{
    public ImprovableStat CharacterStat;

    public CharacterStatVarWrapper(ImprovableStat value)
    {
        CharacterStat = value;
    }

    public override EGameVarType GetGameVarType() => EGameVarType.CHARACTER;

    public override string ToString() => CharacterStat.ToString();

    public override bool IsSameAs(GameVarWrapper other)
    {
        if (other.GetGameVarType() != EGameVarType.CHARACTER_STAT)
        {
            return false;
        }

        ImprovableStat otherStat = (other as CharacterStatVarWrapper).CharacterStat;

        return CharacterStat == otherStat;
    }
}

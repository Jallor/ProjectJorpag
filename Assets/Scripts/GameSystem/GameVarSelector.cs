using System.Collections.Generic;

[System.Serializable]
public abstract class IGameVarSelector
{
    public abstract IGameVarType GetGameVarType();

    public abstract IGameVarType GetFinalGameVarType();

    public abstract IGameVarWrapper GetFinalGameVarWrapper();
}

#region Constant Values

public class ConstBoolVarSelector : IGameVarSelector
{
    public bool ConstBoolValue;

    public override IGameVarType GetGameVarType() => (IGameVarType.BOOL);

    public override IGameVarType GetFinalGameVarType() => (GetGameVarType());

    public override IGameVarWrapper GetFinalGameVarWrapper()
    {
        return (new BoolVarWrapper(ConstBoolValue));
    }
}

public class ConstIntVarSelector : IGameVarSelector
{
    public int ConstIntValue = 0;

    public override IGameVarType GetGameVarType() => (IGameVarType.INT);

    public override IGameVarType GetFinalGameVarType() => (GetGameVarType());

    public override IGameVarWrapper GetFinalGameVarWrapper()
    {
        return (new IntVarWrapper(ConstIntValue));
    }
}

#endregion
using System.Collections.Generic;

[System.Serializable]
public abstract class GameVarSelector
{
    public abstract EGameVarType GetGameVarType();

    public abstract EGameVarType GetFinalGameVarType();

    public abstract GameVarWrapper GetFinalGameVarWrapper();
}

public abstract class FirstGameVarSelector : GameVarSelector
{

}

#region Constant Values
[SelectImplementationName("Constant/Bool")]
public class ConstBoolVarSelector : FirstGameVarSelector
{
    public bool ConstBoolValue;

    public override EGameVarType GetGameVarType() => (EGameVarType.BOOL);

    public override EGameVarType GetFinalGameVarType() => (GetGameVarType());

    public override GameVarWrapper GetFinalGameVarWrapper()
    {
        return (new BoolVarWrapper(ConstBoolValue));
    }
}

[SelectImplementationName("Constant/Int")]
public class ConstIntVarSelector : FirstGameVarSelector
{
    public int ConstIntValue = 0;

    public override EGameVarType GetGameVarType() => (EGameVarType.INT);

    public override EGameVarType GetFinalGameVarType() => (GetGameVarType());

    public override GameVarWrapper GetFinalGameVarWrapper()
    {
        return (new IntVarWrapper(ConstIntValue));
    }
}

#endregion
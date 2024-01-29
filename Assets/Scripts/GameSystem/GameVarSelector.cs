using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class GameVarSelector
{
    public abstract EGameVarType GetGameVarType();

    public abstract EGameVarType GetFinalGameVarType();
}

public abstract class FirstGameVarSelector : GameVarSelector
{
    public abstract GameVarWrapper StartGetFinalVarWrapper(GameContext context);
}

public abstract class NextGameVarSelector : GameVarSelector
{
    public abstract GameVarWrapper GetFinalGameVarWrapper(GameVarWrapper varWrapper);
}

#region Constant Values
[SelectImplementationName("Constant/Bool")]
public class ConstBoolVarSelector : FirstGameVarSelector
{
    public bool ConstBoolValue;

    public override EGameVarType GetGameVarType() => (EGameVarType.BOOL);

    public override EGameVarType GetFinalGameVarType() => (GetGameVarType());

    public override GameVarWrapper StartGetFinalVarWrapper(GameContext context)
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

    public override GameVarWrapper StartGetFinalVarWrapper(GameContext context)
    {
        return (new IntVarWrapper(ConstIntValue));
    }
}

[SelectImplementationName("Constant/String")]
public class ConstStringVarSelector : FirstGameVarSelector
{
    public string ConstStringValue = "";

    public override EGameVarType GetGameVarType() => (EGameVarType.STRING);

    public override EGameVarType GetFinalGameVarType() => (GetGameVarType());

    public override GameVarWrapper StartGetFinalVarWrapper(GameContext context)
    {
        return (new StringVarWrapper(ConstStringValue));
    }
}

#endregion
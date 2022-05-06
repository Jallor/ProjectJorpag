using System.Collections.Generic;

public abstract class IGameVarSelector
{
    public abstract IGameVarType GetGameVarType();

    public abstract IGameVarType GetFinalGameVarType();

    public abstract IGameVarWrapper GetFinalGameVarWrapper();
}

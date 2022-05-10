using UnityEngine;

[SelectImplementationName("Character")]
public class VarSelectorCharacter : FirstGameVarSelector
{
    public enum EPossibleTarget
    {
        CURRENT_PLAYER,
        CASTER,
        TARGET,
    }

    public EPossibleTarget Target;

    [SerializeReference] [SelectImplementation]
    public NextVarSelectorFromCharacter NextVarSelector = new VarSelectorCharacterFromCharacter();

    public override EGameVarType GetGameVarType() => EGameVarType.CHARACTER;

    public override EGameVarType GetFinalGameVarType()
    {
        return (NextVarSelector.GetFinalGameVarType());
    }

    public override GameVarWrapper GetFinalGameVarWrapper()
    {
        return (NextVarSelector.GetFinalGameVarWrapper());
    }
}

public abstract class NextVarSelectorFromCharacter : GameVarSelector
{}

[SelectImplementationName("Int")]
public class VarSelectorIntFromCharacter : NextVarSelectorFromCharacter
{

    public override EGameVarType GetGameVarType() => EGameVarType.INT;

    public override EGameVarType GetFinalGameVarType()
    {
        throw new System.NotImplementedException();
    }

    public override GameVarWrapper GetFinalGameVarWrapper()
    {
        throw new System.NotImplementedException();
    }
}

[SelectImplementationName("String")]
public class VarSelectorStringFromCharacter : NextVarSelectorFromCharacter
{
    public override EGameVarType GetGameVarType() => EGameVarType.STRING;

    public override EGameVarType GetFinalGameVarType()
    {
        throw new System.NotImplementedException();
    }

    public override GameVarWrapper GetFinalGameVarWrapper()
    {
        throw new System.NotImplementedException();
    }
}

[SelectImplementationName("Character")]
public class VarSelectorCharacterFromCharacter : NextVarSelectorFromCharacter
{
    public override EGameVarType GetGameVarType() => EGameVarType.CHARACTER;

    public override EGameVarType GetFinalGameVarType()
    {
        throw new System.NotImplementedException();
    }

    public override GameVarWrapper GetFinalGameVarWrapper()
    {
        throw new System.NotImplementedException();
    }
}

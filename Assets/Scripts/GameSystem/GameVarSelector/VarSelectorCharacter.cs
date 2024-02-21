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

    public override GameVarWrapper StartGetFinalVarWrapper(GameContext context)
    {
        GameVarWrapper finalWrapper = new NullVarWrapper();

        switch (Target)
        {
            case EPossibleTarget.CURRENT_PLAYER:
                finalWrapper = new CharacterVarWrapper(GameManager.Inst.GetPlayer());
                break;
            case EPossibleTarget.CASTER:
                finalWrapper = context.Caster;
                break;
            case EPossibleTarget.TARGET:
                finalWrapper = context.Target;
                break;
            default:
                Debug.LogError("VarSelectorCharacter : " + Target + " not implemented");
                break;
        }

        return (finalWrapper);
    }
}

public abstract class NextVarSelectorFromCharacter : NextGameVarSelector
{ }

[SelectImplementationName("Int")]
public class VarSelectorIntFromCharacter : NextVarSelectorFromCharacter
{
    public enum EPossibleTarget
    {
        MAX_HP = 0,
        CURRENT_HP = 1,
        ENTITY_ID = 2,
    }

    public EPossibleTarget Target;

    public override EGameVarType GetGameVarType() => EGameVarType.INT;

    public override EGameVarType GetFinalGameVarType() => GetGameVarType();

    public override GameVarWrapper GetFinalGameVarWrapper(GameVarWrapper varWrapper)
    {
        Debug.Assert(varWrapper is CharacterVarWrapper);
        CharacterVarWrapper charaWrapper = varWrapper as CharacterVarWrapper;

        int finalValue = 0;
        switch (Target)
        {
            case EPossibleTarget.MAX_HP:
                finalValue = charaWrapper.Character.GetMaxHP();
                break;
            case EPossibleTarget.CURRENT_HP:
                finalValue = charaWrapper.Character.GetMaxHP();
                break;
            case EPossibleTarget.ENTITY_ID:
                finalValue = charaWrapper.Character.GetEntityID();
                break;
            default:
                Debug.LogError("VarSelectorIntFromCharacter : " + Target + " not implemented");
                break;
        }

        return (new IntVarWrapper(finalValue));
    }
}

[SelectImplementationName("String")]
public class VarSelectorStringFromCharacter : NextVarSelectorFromCharacter
{
    public enum EPossibleTarget
    {
        CHARACTER_NAME = 0,
    }

    public EPossibleTarget Target;

    public override EGameVarType GetGameVarType() => EGameVarType.STRING;

    public override EGameVarType GetFinalGameVarType() => GetGameVarType();

    public override GameVarWrapper GetFinalGameVarWrapper(GameVarWrapper varWrapper)
    {
        Debug.Assert(varWrapper is CharacterVarWrapper);
        CharacterVarWrapper charaWrapper = varWrapper as CharacterVarWrapper;

        string finalValue = "";
        switch (Target)
        {
            case EPossibleTarget.CHARACTER_NAME:
                finalValue = charaWrapper.Character.ToString();
                break;
            default:
                Debug.LogError("VarSelectorStringFromCharacter : " + Target + " not implemented");
                break;
        }

        return (new StringVarWrapper(finalValue));
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

    public override GameVarWrapper GetFinalGameVarWrapper(GameVarWrapper varWrapper)
    {
        throw new System.NotImplementedException();
    }
}

[SelectImplementationName("Character Stat")]
public class VarSelectorCharacterStatFromCharacter : NextVarSelectorFromCharacter
{
    public override EGameVarType GetGameVarType() => EGameVarType.CHARACTER;

    public override EGameVarType GetFinalGameVarType()
    {
        throw new System.NotImplementedException();
    }

    public override GameVarWrapper GetFinalGameVarWrapper(GameVarWrapper varWrapper)
    {
        throw new System.NotImplementedException();
    }
}

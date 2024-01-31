using UnityEngine;

[System.Serializable]
public abstract class Condition
{
    public bool IsNot = false;
    public bool IsConditionValid(GameContext context)
    {
        if (IsNot)
        {
            return (!_IsConditionValid(context));
        }
        else
        {
            return (_IsConditionValid(context));
        }
    }

    protected abstract bool _IsConditionValid(GameContext context);
}

[System.Serializable] [SelectImplementationName("Test Bool")]
public class BoolCondition : Condition
{
    [SerializeReference] [SelectImplementation]
    [SerializeField] private FirstGameVarSelector _VarSelector;
    [SerializeField] private bool _InvertValue;

    protected override bool _IsConditionValid(GameContext context)
    {
        Debug.Assert(_VarSelector.GetFinalGameVarType() is EGameVarType.BOOL);

        bool boolValue = (_VarSelector.StartGetFinalVarWrapper(context) as BoolVarWrapper).BoolValue;

        if (!_InvertValue)
        {
            return (boolValue);
        }
        else
        {
            return (!boolValue);
        }
    }
}

[System.Serializable] [SelectImplementationName("Test Is Same Type")]
public class CheckTypeCondition : Condition
{
    [SerializeReference] [SelectImplementation]
    [SerializeField] private FirstGameVarSelector _VarToTest;
    [SerializeField] private EGameVarType _VarType;

    protected override bool _IsConditionValid(GameContext context)
    {
        return (_VarToTest.StartGetFinalVarWrapper(context).GetGameVarType() == _VarType);
    }
}

[System.Serializable] [SelectImplementationName("Test Is Same")]
public class TestEquivalenceCondition : Condition
{
    [SerializeReference] [SelectImplementation]
    [SerializeField] private FirstGameVarSelector _FirstVar;
    [SerializeReference] [SelectImplementation]
    [SerializeField] private FirstGameVarSelector _SecondVar;

    protected override bool _IsConditionValid(GameContext context)
    {
        GameVarWrapper firstWrap = _FirstVar.StartGetFinalVarWrapper(context);
        GameVarWrapper secondWrap = _SecondVar.StartGetFinalVarWrapper(context);

        return (firstWrap.IsSameAs(secondWrap));
    }
}

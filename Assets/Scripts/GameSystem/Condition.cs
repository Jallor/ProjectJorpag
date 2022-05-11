using UnityEngine;

[System.Serializable]
public abstract class Condition
{
    public abstract bool IsConditionValid(GameContext context);
}

[System.Serializable] [SelectImplementationName("Test Bool")]
public class BoolCondition : Condition
{
    [SerializeReference] [SelectImplementation]
    [SerializeField] private FirstGameVarSelector _VarSelector;
    [SerializeField] private bool _InvertValue;

    public override bool IsConditionValid(GameContext context)
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

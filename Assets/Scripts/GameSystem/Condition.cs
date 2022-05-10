using UnityEngine;

[System.Serializable]
public abstract class Condition
{
    public abstract bool IsConditionValid();
}

[System.Serializable] [SelectImplementationName("Test Bool")]
public class BoolCondition : Condition
{
    [SerializeReference] [SelectImplementation]
    [SerializeField] private FirstGameVarSelector _VarSelector;
    [SerializeField] private bool _InvertValue;

    public override bool IsConditionValid()
    {
        Debug.Assert(_VarSelector.GetFinalGameVarType() is EGameVarType.BOOL);

        bool boolValue = (_VarSelector.GetFinalGameVarWrapper() as BoolVarWrapper).BoolValue;

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

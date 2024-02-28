using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStat_StatusEffect : StatusEffect
{
    public override EStatusEffectType _EffectType => EStatusEffectType.ChangeStat;

    public override void OnApplied()
    {
        throw new System.NotImplementedException();
    }

    public override bool TryApplyStatusOfSameType(StatusEffect otherStatusEffect)
    {
        return true;

        // TODO : Status effect : need to check if it can be added
    }

    public override void OnTick()
    {
        throw new System.NotImplementedException();
    }

    public override void OnRemoved()
    {
        throw new System.NotImplementedException();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectImplementationName("Damage Over Time")]
public class DamageOverTime_StatusEffect : StatusEffect
{
    public float _DamagePerTick = 5;

    public override EStatusEffectType _EffectType => EStatusEffectType.DamageOverTime;

    public override void OnApplied()
    {
    }

    public override bool TryApplyStatusOfSameType(StatusEffect otherStatusEffect)
    {
        return true;

        // TODO : Status effect : need to check if it can be added
    }

    public override void OnTick()
    {
        if (!_Target)
        {
            return ;
        }

        _Target.InflictDamage(_DamagePerTick);
    }

    public override void OnRemoved()
    {

    }
}

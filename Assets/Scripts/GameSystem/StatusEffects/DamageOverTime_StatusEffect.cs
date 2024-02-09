using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectImplementationName("Damage Over Time")]
public class DamageOverTime_StatusEffect : StatusEffect
{
    [SerializeField] private float _DamagePerTick = 5;

    public override EStatusEffectType _EffectType => EStatusEffectType.DamageOverTime;

    public override void OnApplied(GameContext context)
    {
    }

    public override void OnTick(GameContext context)
    {
        GameVarWrapper targetWrapper = context.Target;

        if (targetWrapper.GetGameVarType() != EGameVarType.CHARACTER)
        {
            return ;
        }

        CharacterVarWrapper characterTarget = targetWrapper as CharacterVarWrapper;
        characterTarget.Character.InflictDamage(_DamagePerTick);
    }

    public override void OnRemoved(GameContext context)
    {

    }
}

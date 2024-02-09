using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectImplementationName("Apply Status Effect")]
public class ApplyStatusEffectEffect : GameEffect
{
    [SelectImplementation] [SerializeReference]
    [SerializeField] private StatusEffect _StatusEffectToApply;
    [SerializeField] FirstGameVarSelector _Target
        = new VarSelectorCharacter() { Target = VarSelectorCharacter.EPossibleTarget.TARGET };

    public override bool PlayEffect(GameContext context)
    {
        GameVarWrapper targetWrapper = _Target.StartGetFinalVarWrapper(context);

        if (targetWrapper.GetGameVarType() != EGameVarType.CHARACTER)
        {
            return false;
        }

        CharacterVarWrapper characterTarget = targetWrapper as CharacterVarWrapper;
        characterTarget.Character.ApplyStatusEffect(_StatusEffectToApply);

        return true;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectImplementationName("Damage/Damage Effect")]
public class DamageEffect : GameEffect
{
    [SerializeField] float _DamageQuantity = 10;
    [SerializeField] FirstGameVarSelector _DamageTarget
        = new VarSelectorCharacter() { Target = VarSelectorCharacter.EPossibleTarget.TARGET };

    public override bool PlayEffect(GameContext context)
    {
        GameVarWrapper targetWrapper = _DamageTarget.StartGetFinalVarWrapper(context);

        if (targetWrapper.GetGameVarType() != EGameVarType.CHARACTER)
        {
            return false;
        }

        CharacterVarWrapper characterTarget = targetWrapper as CharacterVarWrapper;
        characterTarget.Character.ReceiveDamage(_DamageQuantity);

        return true;
    }
}

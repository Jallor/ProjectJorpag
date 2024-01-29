using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectImplementationName("Throw")]
public class ThrowEffect : GameEffect
{
    public GameObject /*need a specific instance*/ _ThrowableObject = null;
    public Vector2 ThrowSpeed = new Vector2(0, -1);

    public override bool PlayEffect(GameContext context)
    {
        if (context.GetCaster().GetGameVarType() != EGameVarType.CHARACTER)
        {
            return false;
        }
        CharacterManager caster = (context.GetCaster() as CharacterVarWrapper).Character;
        GameObject throwableObj = caster.InstantiateThrowableObject(_ThrowableObject);

        ;

        return true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectImplementationName("Throw")]
public class ThrowEffect : GameEffect
{
    public GameObject ThrowableObject = null;
    public float ThrowSpeed = 1;
    public Vector2 ThrowDirection = new Vector2(0, -1);

    public override bool PlayEffect(GameContext context)
    {
        if (context.Caster.GetGameVarType() != EGameVarType.CHARACTER)
        {
            return false;
        }
        CharacterManager caster = (context.Caster as CharacterVarWrapper).Character;
        GameObject throwableObj = caster.InstantiateThrowableObject(ThrowableObject);

        ThrowableObject throwedObject = throwableObj.GetComponent<ThrowableObject>();

        if (throwedObject != null)
        {
            throwedObject.SetSpeed(ThrowSpeed);
            throwedObject.SetDirection(CustomMathLib.RotateVector2(ThrowDirection, caster.GetCharacterOrientation()));
            throwedObject.SetOwner(caster);
        }
        else
        {
            Debug.LogError("ThrowEffect need a ThrowableObject");
        }

        return true;
    }
}
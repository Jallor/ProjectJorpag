using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public abstract class SkillAnimationPart
{
    public float AnimPartDuration = 2f;

    public SkillAnimationPart()
    {
        AnimPartDuration = 2f;
    }
}

public class SkillAnimPart_BasicWpDisplay : SkillAnimationPart
{
    public bool DisplayWeapon = false;
    public Vector2 WeaponPosition = new Vector2(0.1f, 0.24f);
    public float WeaponRotation = 0;

    public SkillAnimPart_BasicWpDisplay() : base()
    {
        DisplayWeapon = false;
        WeaponPosition = new Vector2(0.1f, 0.24f);
    }
}

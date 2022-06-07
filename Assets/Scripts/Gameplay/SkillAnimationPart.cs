using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class SkillAnimationPart
{
    public float AnimPartDuration = 2f;

    public bool DisplayWeapon = false;
    [ShowIf("DisplayWeapon")]
    public Vector2 WeaponPosition = new Vector2(0.1f, 0.24f);
    public float WeaponRotation = 0;

    public SkillAnimationPart()
    {
        AnimPartDuration = 2f;
        DisplayWeapon = false;
        WeaponPosition = new Vector2(0.1f, 0.24f);
    }
}

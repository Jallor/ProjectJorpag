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

    public abstract void OnEnterAnimPart(CharacterManager chara);
    public abstract void OnUpdateAnimPart(CharacterManager chara, float alreadyPastTime);
    public abstract void OnExitAnimPart(CharacterManager chara);
}

[SelectImplementationName("Display Weapon")]
public class SkillAnimPart_BasicWpDisplay : SkillAnimationPart
{
    [SerializeField] private Vector2 WeaponPosition = Vector2.zero;
    [SerializeField] private Vector2 EndWeaponPosition = Vector2.zero;
    [SerializeField] private float WeaponRotation = 0;
    [SerializeField] private float EndWeaponRotation = 0;

    public SkillAnimPart_BasicWpDisplay() : base()
    {
        WeaponPosition = Vector2.zero;
    }

    public override void OnEnterAnimPart(CharacterManager chara)
    {
        chara.GetSpriteAnimator().DisplayWeapon(WeaponPosition, WeaponRotation);
    }

    public override void OnUpdateAnimPart(CharacterManager chara, float alreadyPastTime)
    {
        if (WeaponPosition != EndWeaponPosition || WeaponRotation != EndWeaponRotation)
        {
            float timeProgression = alreadyPastTime / AnimPartDuration;
            Vector2 newPosition;
            float newRotation;

            newPosition = Vector2.Lerp(WeaponPosition, EndWeaponPosition, timeProgression);
            newRotation = Mathf.Lerp(WeaponRotation, EndWeaponRotation, timeProgression);

            chara.GetSpriteAnimator().DisplayWeapon(newPosition, newRotation);
        }
    }

    public override void OnExitAnimPart(CharacterManager chara)
    {
        chara.GetSpriteAnimator().HideWeapon();
    }
}

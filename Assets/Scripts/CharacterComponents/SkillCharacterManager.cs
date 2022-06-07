using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCharacterManager : MonoBehaviour
{
    private CharacterManager _CharacterManager;

    private SkillData _CurrentPlayedSkill = null;

    public void Initialize(CharacterManager characterManager)
    {
        _CharacterManager = characterManager;
    }

    public void TryPlaySkill(SkillData skillToPlay)
    {
        if (IsPlayingSkill())
        {
            return;
        }

        _CurrentPlayedSkill = skillToPlay;
        StartCoroutine(StartPlaySkill());
    }

    private IEnumerator StartPlaySkill()
    {
        foreach (SkillAnimationPart animPart in _CurrentPlayedSkill.AnimationsList)
        {
            if (animPart.DisplayWeapon)
            {
                _CharacterManager.GetSpriteAnimator().DisplayWeapon(animPart.WeaponPosition, animPart.WeaponRotation);
            }
            else
            {
                _CharacterManager.GetSpriteAnimator().HideWeapon();
            }
            yield return new WaitForSeconds(animPart.AnimPartDuration);
        }

        _CharacterManager.GetSpriteAnimator().HideWeapon();
        EndPlayingSkill();
    }

    private void EndPlayingSkill()
    {
        _CurrentPlayedSkill = null;
    }

    public bool IsPlayingSkill() => (_CurrentPlayedSkill != null);

    public bool IsMovementAllowed()
    {
        if (!IsPlayingSkill())
        {
            return (true);
        }
        return (_CurrentPlayedSkill.AllowMovement);
    }

    public bool IsRotationAllowed()
    {
        if (!IsPlayingSkill())
        {
            return (true);
        }
        return (_CurrentPlayedSkill.AllowRotation);
    }
}

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
        print("start skill play");
        yield return new WaitForSeconds(10f);
        print("end skill");
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

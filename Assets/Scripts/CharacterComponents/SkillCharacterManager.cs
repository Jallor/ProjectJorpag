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
        GameContext context = new GameContext(new CharacterVarWrapper(_CharacterManager),
            new CharacterVarWrapper(_CharacterManager), new NullVarWrapper());

        foreach (ConditionnalEffect effect in _CurrentPlayedSkill.GameEffectList)
        {
            effect.TryPlayEffects(context);
        }

        float animPartStartTime;

        foreach (SkillAnimationPart animPart in _CurrentPlayedSkill.AnimationsList)
        {
            animPartStartTime = Time.time;
            animPart.OnEnterAnimPart(_CharacterManager);

            while ((Time.time - animPartStartTime) < animPart.AnimPartDuration)
            {
                animPart.OnUpdateAnimPart(_CharacterManager, Time.time - animPartStartTime);

                yield return new WaitForSeconds(0.01f);
            }

            animPart.OnExitAnimPart(_CharacterManager);
        }

        EndPlayingSkill();
    }

    private void EndPlayingSkill()
    {
        _CurrentPlayedSkill = null;
    }

    public bool IsPlayingSkill() => (_CurrentPlayedSkill != null);

    public bool IsMovementAllowed()
    {
        return (_CurrentPlayedSkill.AllowMovement);
    }

    public bool IsRotationAllowed()
    {
        return (_CurrentPlayedSkill.AllowRotation);
    }
}

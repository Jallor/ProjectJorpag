using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsCharacterManager : MonoBehaviour
{
    private CharacterManager _CharacterManager;

    private List<StatusEffect> _ActiveStatusEffects = new List<StatusEffect>();
    private const float _TimeBetweenTick = 1f;
    private float _TimeBeforeNextTick = 0;

    public void Initialize(CharacterManager characterManager)
    {
        _CharacterManager = characterManager;

        _TimeBeforeNextTick = _TimeBetweenTick;
    }

    private void Update()
    {
        _TimeBeforeNextTick -= Time.deltaTime;

        // Tick every status effect
        // TODO : les soucis de cette m�thode c'est que tous les status tiquent en m�me temps
        // ce qui fait que le temps entre l'application et le 1er tick change d'un moment � un autre
        // deux solutions :
            // - r�duire la dur�e d'un tick pour cacher cette effet de bord
            // - g�rer le tick dans chaque statusEffect
        if (_TimeBeforeNextTick <= 0)
        {
            foreach (StatusEffect effect in _ActiveStatusEffects)
            {
                effect.Tick();
            }

            // Remove status effects that has ended
            for (int i = _ActiveStatusEffects.Count - 1; i >= 0; i--)
            {
                if (!_ActiveStatusEffects[i].TestIsStillActive())
                {
                    _ActiveStatusEffects.RemoveAt(i);
                }
            }

            _TimeBeforeNextTick += _TimeBetweenTick;
        }

    }

    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        StatusEffect newStatusEffect = statusEffect.ShallowCopy<StatusEffect>();
        bool canBeApplied = true;

        foreach (StatusEffect effect in _ActiveStatusEffects)
        {
            Type type = newStatusEffect.GetType();
            if (effect._EffectType == newStatusEffect._EffectType)
            {
                if (!effect.TryApplyStatusOfSameType(newStatusEffect))
                {
                    canBeApplied = false;
                }
            }
        }

        if (canBeApplied)
        {
            _ActiveStatusEffects.Add(newStatusEffect);

            GameContext context = new GameContext();
            context.Target = new CharacterVarWrapper(_CharacterManager);

            newStatusEffect.Applied(context);
        }
    }
}

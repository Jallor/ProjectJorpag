using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionnalEffect
{
    [SelectImplementation] [SerializeReference]
    [SerializeField] private List<Condition> _Conditions = new List<Condition>();
    [SelectImplementation] [SerializeReference]
    [SerializeField] private List<GameEffect> _GameEffects = new List<GameEffect>();

    public bool TryPlayEffects()
    {
        foreach (Condition condition in _Conditions)
        {
            if (!condition.IsConditionValid())
            {
                return (false);
            }
        }

        foreach (GameEffect effect in _GameEffects)
        {
            effect.PlayEffect();
        }

        return (true);
    }
}

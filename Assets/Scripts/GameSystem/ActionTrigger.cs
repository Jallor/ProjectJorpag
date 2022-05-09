using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ActionTrigger
{
    [SerializeField] private List<ConditionnalEffect> _ConditionnalsEffects = new List<ConditionnalEffect>();

    public void TriggerAction()
    {
        foreach (ConditionnalEffect conditionnalEffect in _ConditionnalsEffects)
        {
            conditionnalEffect.TryPlayEffects();
        }
    }
}

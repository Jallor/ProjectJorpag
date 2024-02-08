using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class StatusEffect
{
    [SerializeField] private float _TickDuration = 5;
    private float _RemainingTick;
    public abstract EStatusEffectType _EffectType { get; }

     // private CharacterManager _Caster; // TODO !
     // private CharacterManager _Target; // TODO !

    public void Applied(GameContext context)
    {
        _RemainingTick = _TickDuration;
        OnApplied(context);
    }
    public abstract void OnApplied(GameContext context);

    public void Tick(GameContext context)
    {
        OnTick(context);
        --_RemainingTick;
    }
    public abstract void OnTick(GameContext context);

    public bool TestIsStillActive(GameContext context)
    {
        if (_RemainingTick <= 0)
        {
            OnRemoved(context);
            return false;
        }
        return true;
    }
    public abstract void OnRemoved(GameContext context);
}

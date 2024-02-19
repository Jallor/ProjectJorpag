using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

[System.Serializable]
public abstract class StatusEffect
{
    [SerializeField] private float _TickDuration = 5;
    private float _RemainingTick;
    public abstract EStatusEffectType _EffectType { get; }

    // private CharacterManager _Caster; // TODO !
    // private CharacterManager _Target; // TODO !

    public bool _HasUiToDisplay = false;
    public bool _HasVfx = false;

    [ShowIf("_HasUiToDisplay")] [AllowNesting]
    public string _UiIconNameTmp = "Ha haaa ! ça sert à rien (pour l'instant)";

    [ShowIf("_HasVfx")][AllowNesting]
    public EStatusVfx _VfxToApply = EStatusVfx.NONE;
    public int _VfxId = -1;

    public virtual T ShallowCopy<T>()
    {
        return (T)this.MemberwiseClone();
    }

    public void Applied(GameContext context)
    {
        _RemainingTick = _TickDuration;
        OnApplied(context);
        ApplyVfx(context);
    }
    public abstract void OnApplied(GameContext context);
    public void ApplyVfx(GameContext context)
    {
        if (!_HasVfx)
        {
            return;
        }

        GameVarWrapper targetWrapper = context.Target;
        if (targetWrapper.GetGameVarType() != EGameVarType.CHARACTER)
        {
            return;
        }
        CharacterVarWrapper characterTarget = targetWrapper as CharacterVarWrapper;

        _VfxId = characterTarget.Character.AddStatusVfx(_VfxToApply);
    }

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
            RemoveVfx(context);
            return false;
        }
        return true;
    }
    public abstract void OnRemoved(GameContext context);
    public void RemoveVfx(GameContext context)
    {
        if (!_HasVfx)
        {
            return;
        }

        GameVarWrapper targetWrapper = context.Target;
        if (targetWrapper.GetGameVarType() != EGameVarType.CHARACTER)
        {
            return;
        }
        CharacterVarWrapper characterTarget = targetWrapper as CharacterVarWrapper;

        characterTarget.Character.RemoveVfx(_VfxId);
    }

}

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

[System.Serializable]
public abstract class StatusEffect
{
    [SerializeField] private const float _TickDuration = 5;
    private float _RemainingTick;
    public abstract EStatusEffectType _EffectType { get; }
    public List<EStatusEffectTag> Tags { get; private set; }

    protected CharacterManager _Caster = null;
    protected CharacterManager _Target = null;

    public bool _HasUiToDisplay = false;
    public bool _HasVfx = false;

    [ShowIf("_HasUiToDisplay")][AllowNesting]
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

        if (context.Caster.GetGameVarType() == EGameVarType.CHARACTER)
        {
            _Caster = (context.Caster as CharacterVarWrapper).Character;
        }
        if (context.Target.GetGameVarType() == EGameVarType.CHARACTER)
        {
            _Target = (context.Target as CharacterVarWrapper).Character;
        }

        OnApplied();
        ApplyVfx();
    }
    public abstract void OnApplied();
    public void ApplyVfx()
    {
        if (!_HasVfx || !_Target)
        {
            return;
        }

        _VfxId = _Target.AddStatusVfx(_VfxToApply);
    }

    /// <summary>
    /// To call if another statusEffect try to be applied applied on the same character
    /// </summary>
    /// <returns>FALSE if it CAN NOT be applied</returns>
    public abstract bool TryApplyStatusOfSameType(StatusEffect otherStatusEffect);

    public void Tick()
    {
        OnTick();
        --_RemainingTick;
    }
    public abstract void OnTick();

    public bool TestIsStillActive()
    {
        if (_RemainingTick <= 0)
        {
            OnRemoved();
            RemoveVfx();
            return false;
        }
        return true;
    }
    public abstract void OnRemoved();
    public void RemoveVfx()
    {
        if (!_HasVfx || !_Target)
        {
            return;
        }

        _Target.RemoveVfx(_VfxId);
    }

}

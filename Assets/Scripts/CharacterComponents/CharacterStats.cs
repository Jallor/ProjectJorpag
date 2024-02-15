using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterStats;

public class CharacterStats
{
    public class SimpleStat
    {
        protected float InitialValue;
        protected float CurrentValue;

        /// <summary> Send the value that modifie this stat </summary>
        public DelegateWithFloat OnValueUpdated;

        public SimpleStat()
        {
            Init(0);
        }

        public virtual void Init(float initialValue)
        {
            InitialValue = initialValue;
            CurrentValue = initialValue;
        }

        public float GetInitialValue()
        {
            return InitialValue;
        }

        public float GetCurrentValue()
        {
            return CurrentValue; 
        }

        public virtual void Reset()
        {
            CurrentValue = InitialValue;
        }
    }

    // Stat like life or mana that will changes during game
    public class ConsomableStat : SimpleStat
    {
        public float InitialMaxValue { get; protected set; }
        public float MaxValue { get; protected set; }

        public ConsomableStat()
        {
            Init(0);
        }

        public override void Init(float initialValue)
        {
            base.Init(initialValue);
            MaxValue = initialValue;
            InitialMaxValue = initialValue;
        }

        public override void Reset()
        {
            base.Reset();
            MaxValue = InitialMaxValue;
        }

        public void Add(float quantity, bool allowOverload = false)
        {
            CurrentValue += quantity;

            if (!allowOverload)
            {
                float quantityAdded = quantity;
                if (CurrentValue > MaxValue)
                {
                    quantityAdded = MaxValue + quantity - CurrentValue;
                }
                else if (CurrentValue < 0)
                {
                    quantityAdded = CurrentValue - quantity;
                }
                CurrentValue = Mathf.Clamp(CurrentValue, 0, MaxValue);
                OnValueUpdated?.Invoke(quantityAdded);
            }
            else
            {
                OnValueUpdated?.Invoke(quantity);
            }
        }
    }

    public class ImprovableStat : SimpleStat
    {
        public ECharacterStat StatType { get; protected set; }

        public float BaseBuff { get; protected set; }
        public float PercentBuff { get; protected set; }
        public float MultBuff { get; protected set; }

        public ImprovableStat()
        {
            Init(0);
        }

        public override void Init(float initialValue)
        {
            base.Init(initialValue);

            BaseBuff = 0;
            PercentBuff = 0;
            MultBuff = 1;
        }

    }

    public ImprovableStat MovementSpeed;
    public ConsomableStat Life;

    public List<ImprovableStat> CharacerStats = new List<ImprovableStat>();

    public ImprovableStat AddStat(ECharacterStat newStatType)
    {
        if (HasStat(newStatType))
        {
            return TryGetCharaStat(newStatType);
        }
        else
        {
            ImprovableStat newStat = new ImprovableStat();
            CharacerStats.Add(newStat);
            return newStat;
        }
    }

    public bool HasStat(ECharacterStat statType)
    {
        foreach (ImprovableStat stat in CharacerStats)
        {
            if (stat.StatType == statType)
            {
                return true;
            }
        }
        return false;
    }

    public ImprovableStat TryGetCharaStat(ECharacterStat statType)
    {
        foreach (ImprovableStat stat in CharacerStats)
        {
            if (stat.StatType == statType)
            {
                return stat;
            }
        }

        return null;
    }
}


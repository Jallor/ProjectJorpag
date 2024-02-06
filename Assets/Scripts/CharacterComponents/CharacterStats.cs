using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats
{
    public class SimpleStat
    {
        public float InitialValue { get; protected set; }
        public float CurrentValue { get; protected set; }

        /// <summary> Send the value that modifie this stat </summary>
        public DelegateWithFloat OnValueUpdated;

        public virtual void Init(float initialValue)
        {
            InitialValue = initialValue;
            CurrentValue = initialValue;
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

    public SimpleStat MovementSpeed;
    public ConsomableStat Life;
}

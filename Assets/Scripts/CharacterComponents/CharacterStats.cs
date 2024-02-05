using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CharacterStats
{
    public class SimpleStat
    {
        public float InitialValue { get; private set; }
        public float CurrentValue { get; private set; }

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
        public float InitialMaxValue { get; private set; }
        public float MaxValue { get; private set; }

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
    }

    public ConsomableStat Life;

    public SimpleStat MovementSpeed;
}

using System.Collections;
using System.Collections.Generic;

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
    }

    // Stat like life or mana that will changes during game
    public class ConsomableStat : SimpleStat
    {
        public float MaxValue { get; private set; }
        public float InitialMaxValue { get; private set; }

        public override void Init(float initialValue)
        {
            base.Init(initialValue);
            MaxValue = initialValue;
            InitialMaxValue = initialValue;
        }
    }

    public ConsomableStat Life;

    public SimpleStat MovementSpeed;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWorldBasedMovementCharacterController : CharacterController
{
    [System.Serializable]
    public class BasicMovementData
    {
        public float Duration;
        public Vector2 Direction;
    }

    public List<BasicMovementData> MovementData = new List<BasicMovementData>();

    private float _CurrentMoveDuration = 0f;
    private int _CurrentMovementIndex = 0;

    public void Update()
    {
        if (GameManager.Inst && !GameManager.Inst.CanCharactersAct())
        {
            _CharaManager.GiveMoveInput(Vector2.zero);
            return;
        }

        if (MovementData.Count == 0)
        {
            _CharaManager.GiveMoveInput(Vector2.zero);
            return;
        }
        _CharaManager.GiveMoveInput(MovementData[_CurrentMovementIndex].Direction);

        _CurrentMoveDuration += Time.deltaTime;
        if (_CurrentMoveDuration >= MovementData[_CurrentMovementIndex].Duration)
        {
            _CurrentMoveDuration = 0f;
            ++_CurrentMovementIndex;
            if (_CurrentMovementIndex >= MovementData.Count)
            {
                _CurrentMovementIndex = 0;
            }
        }
    }
}

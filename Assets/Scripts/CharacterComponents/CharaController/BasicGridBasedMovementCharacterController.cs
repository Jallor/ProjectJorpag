using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGridBasedMovementCharacterController : CharacterController
{
    [System.Serializable]
    public class GridMovementData
    {
        public Vector2Int Direction;
    }

    public List<GridMovementData> MovementData = new List<GridMovementData>();

    private Vector2Int PreviousPosition;
    private int _CurrentMovementIndex = -1;

    public void Update()
    {
        // Case when you can't move
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

        if (_CurrentMovementIndex == -1)
        {
            PreviousPosition = _CharaManager.GetGridPosition();
            _CurrentMovementIndex = 0;
        }

        _CharaManager.GiveMoveInput(MovementData[_CurrentMovementIndex].Direction);

        if (_CharaManager.GetGridPosition() == PreviousPosition + MovementData[_CurrentMovementIndex].Direction)
        {
            ++_CurrentMovementIndex;
            if (_CurrentMovementIndex >= MovementData.Count)
            {
                _CurrentMovementIndex = 0;
            }
            PreviousPosition = _CharaManager.GetGridPosition();
        }
    }
}

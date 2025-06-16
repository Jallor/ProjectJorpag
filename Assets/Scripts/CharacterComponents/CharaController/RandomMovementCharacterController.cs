using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : y a encore des trucs qui vont pas.
// Le controller penche vers en bas à gauche, surement du aux écart entre GridPos et WorldPos

public class RandomMovementCharacterController : CharacterController
{
    // TODO : les position semblent ne pas être alignés avec la grille. Décallage de 0.5

    [SerializeField] private KeyValuePair<int, int> _MoveDistanceGap
        = new KeyValuePair<int, int>(-6, 6);

    Vector2Int _PreviousPosition = Vector2Int.zero;
    Vector2Int _TargetMovement = Vector2Int.zero;

    public override ECharacterControllerType GetCharacterControllerType()
        => ECharacterControllerType.RANDOM_MOVEMENT;

    public void Start()
    {
        _PreviousPosition = _CharaManager.GetGridPosition();
        SelectNewDirection();
    }

    public void Update()
    {
        Vector2Int targetPosition = _PreviousPosition + _TargetMovement;
        if (_CharaManager.GetGridPosition() != targetPosition)
        {
            Vector2 direction = targetPosition - _CharaManager.GetGridPosition();
            direction.Normalize();
            _CharaManager.GiveMoveInput(direction);
        }
        else
        {
            // TODO : Enchainer les ordre de direction
                // A tester
            SelectNewDirection();
        }
    }

    private void SelectNewDirection()
    {
        _TargetMovement.x = Random.Range(_MoveDistanceGap.Key, _MoveDistanceGap.Value);
        _TargetMovement.y = Random.Range(_MoveDistanceGap.Key, _MoveDistanceGap.Value);
    }

    public void SetDistanceGat(KeyValuePair<int, int> distanceGap)
    {
        _MoveDistanceGap = distanceGap;
    }
}
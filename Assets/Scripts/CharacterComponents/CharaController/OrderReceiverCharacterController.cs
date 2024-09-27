using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OrderReceiverCharacterController : CharacterController
{
    [SerializeField] private Vector2Int _TargetGridPos;
    [SerializeField] private bool _TargetPosReached = false;

    private Vector2 _PreviousWorldPos;

    public override ECharacterControllerType GetCharacterControllerType()
        => ECharacterControllerType.ORDER_RECEIVER;

    private void Start()
    {
        _PreviousWorldPos = _CharaManager.GetWorldPosition();
    }

    public void Update()
    {
        if (_TargetPosReached)
        {
            return;
        }

        Vector2 targetWorldPos = GameTileGrid.Inst.GridPositionToWorldPosition(_TargetGridPos);
        Vector2 currentWorldPos = _CharaManager.GetWorldPosition();
        Vector2 direction = targetWorldPos - currentWorldPos;
        direction.Normalize();

        // Is Position Reached ?
        if ((currentWorldPos - _PreviousWorldPos).sqrMagnitude >= (targetWorldPos - _PreviousWorldPos).sqrMagnitude)
        {
            _TargetPosReached = true;
            _CharaManager.GiveMoveInput(Vector2.zero);
            return;
        }

        _PreviousWorldPos = currentWorldPos;

        _CharaManager.GiveMoveInput(direction);
    }

    private void SelectNewTargetPosition(Vector2Int newTarget)
    {
        _TargetGridPos = newTarget;
    }
}
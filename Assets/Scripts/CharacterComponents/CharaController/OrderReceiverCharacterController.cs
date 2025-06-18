using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OrderReceiverCharacterController : CharacterController
{
    [SerializeField] private Vector2Int _TargetGridPos;

    private Vector2 _PreviousWorldPos;
    private bool _TargetPosReached = false;

    private HiveMindManager _OwningHiveMind = null;

    public override ECharacterControllerType GetCharacterControllerType()
        => ECharacterControllerType.ORDER_RECEIVER;


    // TODO Later : Peut être qu'à terme il faudra externaliser ça (la class et l'enum)
    public enum EOrderType
    {
        NONE,
        MOVE,
        INTERRACT
    }
    public class OrderData
    {
        public EOrderType OrderType = EOrderType.NONE;
        public Vector2 Position;
    }

    private OrderData _CurrentOrder = null;
    private List<OrderData> _QueuedOrders = new List<OrderData>();

    private void Start()
    {
        _PreviousWorldPos = _CharaManager.GetWorldPosition();

        if (_OwningHiveMind == null && HiveMindManager.Inst != null)
        {
            HiveMindManager.Inst.RegisterToHiveMind(this);
        }
    }


    public void Update()
    {
        if (_CurrentOrder == null)
        {
            if (_QueuedOrders.Count <= 0)
            {
                return;
            }
            else
            {
                _CurrentOrder = _QueuedOrders[0];
                _QueuedOrders.RemoveAt(0);
            }
        }






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

    public void SetOwningHiveMind(HiveMindManager newHiveMind)
    {
        _OwningHiveMind = newHiveMind;
    }

    public void ForceNewTargetPosition(Vector2Int newGridTarget)
    {
        _TargetGridPos = newGridTarget;
        _TargetPosReached = false;
    }
}
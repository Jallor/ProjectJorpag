using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OrderReceiverCharacterController : CharacterController
{
    [SerializeField] private Vector2Int _TargetGridPos;

    // private Vector2 _PreviousWorldPos;
    // private bool _TargetPosReached = false;

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
        public Vector2Int Position;

        public OrderData(EOrderType orderType, Vector2Int orderPosition)
        {
            OrderType = orderType;
            Position = orderPosition;
        }
    }

    private OrderData _CurrentOrder = null;
    private List<OrderData> _QueuedOrders = new List<OrderData>();

    private void Start()
    {
        // _PreviousWorldPos = _CharaManager.GetWorldPosition();

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

        switch (_CurrentOrder.OrderType)
        {
            case EOrderType.MOVE:
                ExecuteOrderMove(_CurrentOrder.Position);
                break;
            case EOrderType.INTERRACT:
                break;
            default:
                Debug.LogError("Order of type " + _CurrentOrder.OrderType.ToString() + " not implemented !");
                break;
        }
    }

    public void SetOwningHiveMind(HiveMindManager newHiveMind)
    {
        _OwningHiveMind = newHiveMind;
    }

    public void QueueNewOrder(EOrderType orderType, Vector2Int orderPosition)
    {
        _QueuedOrders.Add(new OrderData(orderType, orderPosition));
    }

    public void CompleteCurrentOrder()
    {
        _CurrentOrder = null;
    }

    // TODO to check : il est possible que le placement se fasse mal, qu'on peut dépasser la tile si on vas trop vite
    private void ExecuteOrderMove(Vector2Int position)
    {
        Vector2 targetWorldPos = GameTileGrid.Inst.GridPositionToWorldPosition(_TargetGridPos);
        Vector2 currentWorldPos = _CharaManager.GetWorldPosition();
        Vector2 direction = targetWorldPos - currentWorldPos;
        direction.Normalize();

        if (_CharaManager.GetGridPosition() == position)
        {
            CompleteCurrentOrder();
        }

        // Old version
        // // Is Position Reached ?
        // if ((currentWorldPos - _PreviousWorldPos).sqrMagnitude >= (targetWorldPos - _PreviousWorldPos).sqrMagnitude)
        // {
        //     _TargetPosReached = true;
        //     _CharaManager.GiveMoveInput(Vector2.zero);
        //     return;
        // }
        // _PreviousWorldPos = currentWorldPos;

        _CharaManager.GiveMoveInput(direction);

    }

    private void ExecuteOrderInterract(Vector2Int position)
    {

    }
}
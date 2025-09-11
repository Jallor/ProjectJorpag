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
        NONE = 0,
        MOVE = 10,
        INTERRACT_NEAREAST = 20,
        INTERRACT_LANDMARK = 21,
    }

    [System.Serializable]
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

        if (_OwningHiveMind == null)
        {
            _OwningHiveMind = FindObjectOfType<HiveMindManager>();
            _OwningHiveMind.RegisterToHiveMind(this);
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
                ExecuteOrder_Move(_CurrentOrder.Position);
                break;
            case EOrderType.INTERRACT_LANDMARK:
                ExecuteOrder_InterractLandmark(_CurrentOrder.Position);
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

        if (_QueuedOrders.Count <= 0)
        {
            _OwningHiveMind.GiveOrderToCharacter(this);
        }
    }

    // TODO to check : il est possible que le placement se fasse mal, qu'on peut dépasser la tile si on vas trop vite (voir // Old Version)
    private void ExecuteOrder_Move(Vector2Int position)
    {
        if (_CharaManager.GetGridPosition() == position)
        {
            CompleteCurrentOrder();
            _CharaManager.GiveMoveInput(Vector2.zero);
            return;
        }

        Vector2 targetWorldPos = GameTileGrid.Inst.GridPositionToWorldPosition(position);
        Vector2 currentWorldPos = _CharaManager.GetWorldPosition();
        Vector2 direction = targetWorldPos - currentWorldPos;
        direction.Normalize();

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

    private void ExecuteOrder_InterractLandmark(Vector2Int position)
    {
        Landmark landmark = GameManager.Inst.GetLandmarkAtPosition(position);
        if (landmark == null)
        {
            print("Landmark not found at position");
            return;
        }
        // TODO : Si y en a pas, peut être trouver une manière de notifier ça au hiveMind ou permettre de récupérer le landmark s'il est à une case ?

        landmark.Interract(GetCharaManager());

        CompleteCurrentOrder();

        // TODO Later : Bind un delegate pour savoir quand le interact à finit
    }
}
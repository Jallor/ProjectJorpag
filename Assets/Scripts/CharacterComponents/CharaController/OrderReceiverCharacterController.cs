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


    // TODO Later : Peut �tre qu'� terme il faudra externaliser �a (la class et l'enum)
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
            case EOrderType.INTERRACT_LANDMARK:
                ExecuteOrderInterractLandmark(_CurrentOrder.Position);
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

    // TODO to check : il est possible que le placement se fasse mal, qu'on peut d�passer la tile si on vas trop vite (voir // Old Version)
    private void ExecuteOrderMove(Vector2Int position)
    {
        Vector2 targetWorldPos = GameTileGrid.Inst.GridPositionToWorldPosition(position);
        Vector2 currentWorldPos = _CharaManager.GetWorldPosition();
        Vector2 direction = targetWorldPos - currentWorldPos;
        direction.Normalize();

        if (_CharaManager.GetGridPosition() == position)
        {
            CompleteCurrentOrder();
            _CharaManager.GiveMoveInput(Vector2.zero);
            return;
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

    private void ExecuteOrderInterractLandmark(Vector2Int position)
    {
        // TODO : Faire une vrai impl�mentation du interract 
        // r�cup�rer le landmark courant. (permettre de r�cup�rer le landmark s'il est � une case)
        // Si y en a pas, peut �tre trouver une mani�re de notifier �a au hiveMind !
        // Appeler la fonction Interact du landmark
        //  => o� stocker �a ? avoir un gameobject landmark ?
        //      => g�rer le LandmarkData pour le LvEditor
        // Bind un delegate pour savoir quand le interact � finit


        // TODO : \/ ceci est du TMP \/
        InventoryItem itemToAdd = AllSimpleItemList.Inst.dataList[0];
        _CharaManager.GetCharaInventory().AddItem(itemToAdd);
        CompleteCurrentOrder();
    }
}
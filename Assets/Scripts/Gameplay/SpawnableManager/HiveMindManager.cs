using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IWorldEntity;

// TODO : utiliser une instance, avec comme fonction principale du style SetManagerData(ManagerSpawnData data)
public class HiveMindManager : MonoBehaviour/*todo : A remplacer par un spawnable manager*/, IWorldEntity
{
    [SerializeField] private SpawnableManagerData _Data;

    private EEntityType _EntityType = EEntityType.SPAWNABLE_MANAGER;
    private int _EntityID = -1;
    private SpawnableManagerData ManagerData = null;

    private List<OrderReceiverCharacterController> _ControlledCharacterList = new List<OrderReceiverCharacterController>();
    private Landmark_HiveSpawn _AssociatedLandmark = null;

    private InventoryComponent _InventoryComponent = null;

    private void Start()
    {
        foreach (OrderReceiverCharacterController charaController in FindObjectsOfType<OrderReceiverCharacterController>())
        {
            RegisterToHiveMind(charaController);
        }

        foreach (OrderReceiverCharacterController charaController in _ControlledCharacterList)
        {
            GiveOrderToCharacter(charaController);
        }
    }

    // Todo quand ça sera hérité en enfant de SpawnableManager, envoyer cette function et toutes les autres dont c'est necessaire là bas !
    public void InitializeManager(TileMapMetaData.ManagerSpawnData managerData)
    {
        TileMapMetaData.ManagerSpawnData managerSpawnData = managerData as TileMapMetaData.ManagerSpawnData;
        ManagerData = managerData.ManagerData;

        if (ManagerData.HasInventory && !GetComponent<InventoryComponent>())
        {
            _InventoryComponent = gameObject.AddComponent<InventoryComponent>();
            _InventoryComponent.Initialize(this, ManagerData);
        }

        LandmarkData landmarkData = new LandmarkData_HiveSpawn();
        landmarkData.Position = managerData.SpawnPoint;
        _AssociatedLandmark = GameManager.Inst.CreateNewLandmark(landmarkData, managerData.SpawnPoint) as Landmark_HiveSpawn;
        _AssociatedLandmark.HiveMindManager = this;
    }

    public void RegisterToHiveMind(OrderReceiverCharacterController charaController)
    {
        if (!_ControlledCharacterList.Contains(charaController))
        {
            _ControlledCharacterList.Add(charaController);
            charaController.SetOwningHiveMind(this);
        }
    }

    public InventoryComponent GetInventoryComponent()
    {
        return _InventoryComponent;
    }

    public void GiveOrderToCharacter(OrderReceiverCharacterController charaController)
    {
        InventoryComponent inventory = charaController.GetCharaManager().GetCharaInventory();
        // TODO : à terme, à remplacer par une fonction qui "load" les instructions à executer

        // Go search ressources
        if (inventory.GetItemCountOfType(InventoryItem.EItemType.RESSOURCES) <= 0)
        {
            charaController.QueueNewOrder(OrderReceiverCharacterController.EOrderType.MOVE, _AssociatedLandmark.Position);
            charaController.QueueNewOrder(OrderReceiverCharacterController.EOrderType.INTERRACT_LANDMARK, _AssociatedLandmark.Position);
        }
        // Come back to the hive
        else
        {
            charaController.QueueNewOrder(OrderReceiverCharacterController.EOrderType.MOVE, _AssociatedLandmark.Position);
            charaController.QueueNewOrder(OrderReceiverCharacterController.EOrderType.INTERRACT_LANDMARK, _AssociatedLandmark.Position);
        }
    }

    #region Getters
    public EEntityType GetEntityType() => (_EntityType);
    public int GetEntityID() => (_EntityID);
    public IWorldEntityData GetEntityData() => (_Data);
    public Vector2Int GetGridPosition() => (Vector2Int.zero);
    public Vector3 GetWorldPosition() => (Vector3.zero);
    #endregion

    #region Setters
    public void SetEntityType(EEntityType entityType) => _EntityType = entityType;
    public void SetEntityID(int entityID) => _EntityID = entityID;
    public void SetEntityData(IWorldEntityData entityData) { }
    public void SetWorldPosition(Vector3 worldPosition) { }
    #endregion
}

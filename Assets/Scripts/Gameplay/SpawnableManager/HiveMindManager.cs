using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IWorldEntity;

// TODO : utiliser une instance, avec comme fonction principale du style SetManagerData(ManagerSpawnData data)
public class HiveMindManager : MonobehaviourSingleton<HiveMindManager>, IWorldEntity
{
    [SerializeField] private SpawnableManagerData _Data;

    private EEntityType _EntityType = EEntityType.SPAWNABLE_MANAGER;
    private int _EntityID = -1;

    private List<OrderReceiverCharacterController> _ControlledCharacterList = new List<OrderReceiverCharacterController>();

    private void Start()
    {
        foreach (OrderReceiverCharacterController charaController in FindObjectsOfType<OrderReceiverCharacterController>())
        {
            RegisterToHiveMind(charaController);
        }

        // Give initial order to all controlledCharacter 
    }

    public void InitializeManager(TileMapMetaData.ManagerSpawnData managerData)
    {
        TileMapMetaData.ManagerSpawnData managerSpawnData = managerData as TileMapMetaData.ManagerSpawnData;


    }

    public void RegisterToHiveMind(OrderReceiverCharacterController charaController)
    {
        _ControlledCharacterList.Add(charaController);
        charaController.SetOwningHiveMind(this);
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

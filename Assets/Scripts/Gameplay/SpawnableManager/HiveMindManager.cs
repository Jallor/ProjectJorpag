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

        foreach (OrderReceiverCharacterController charaController in _ControlledCharacterList)
        {
            GiveOrderToCharacter(charaController);
        }
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

    public void GiveOrderToCharacter(OrderReceiverCharacterController charaController)
    {
        CharacterInventory inventory = charaController.GetCharaManager().GetCharaInventory();
        // TODO : à terme, à remplacer par une fonction qui "load" les instructions à executer

        // Go search ressources
        if (inventory.GetItemCountOfType(InventoryItem.EItemType.RESSOURCES) <= 0)
        {
            List<LandmarkData> allLandmark = GameManager.Inst.GetLandmarksOfType(ELandmarkType.GoldMine);
            if (allLandmark.Count > 0)
            {
                charaController.ForceNewTargetPosition(allLandmark[0].Position);
                // TODO chercher le landmark le plus proche (y a moyen que ça existe déjà, si c'est pas le cas, go !)
            }
            else
            {
                Debug.LogError("No landmark");
            }
        }
        // Come back to the hive
        else
        {
            List<LandmarkData> allLandmark = GameManager.Inst.GetLandmarksOfType(ELandmarkType.HiveSpawn);
            charaController.ForceNewTargetPosition(allLandmark[0].Position);
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

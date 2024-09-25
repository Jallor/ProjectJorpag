using System.Collections.Generic;
using UnityEngine;
using static TileMapMetaData;

public class EntityManager : MonobehaviourSingleton<EntityManager>
{
    [Header("Data")]
    [SerializeField] private int _MinAvailableEntityID = 10001;

    [Header("Prefabs")]
    [SerializeField] private GameObject _PlayerPrefab;
    [SerializeField] private GameObject _BaseCharacterPrefab;

    private Dictionary<int, IWorldEntity> _SpawnedEntity = new Dictionary<int, IWorldEntity>();

    public int SpawnEntity(IWorldEntity.EEntityType entityType, CharacterSpawnData spawnData,
        IWorldEntityData entityData, Vector3 worldPosition, int entityID = -1)
    {
        while (entityID == -1 || _SpawnedEntity.ContainsKey(entityID))
        {
            entityID = Random.Range(_MinAvailableEntityID, int.MaxValue);
        }

        IWorldEntity entity = null;
        switch (entityType)
        {
            case IWorldEntity.EEntityType.PLAYER:
                GameObject player = Instantiate(_PlayerPrefab);
                entity = player.GetComponent<CharacterManager>();
                break;
            case IWorldEntity.EEntityType.BASE_CHARACTER:
                GameObject character = Instantiate(_BaseCharacterPrefab);
                entity = character.GetComponent<CharacterManager>();

                // Set up character controller
                TileMapMetaData.CharacterSpawnData charaSpawnData = spawnData as TileMapMetaData.CharacterSpawnData;
                if (charaSpawnData != null
                    && charaSpawnData.CharaControllerType != CharacterController.ECharacterControllerType.NULL
                    && charaSpawnData.CharaControllerType != CharacterController.ECharacterControllerType.PLAYER_CONTROLLER)
                {
                    CharacterController[] previousControllerComps = character.GetComponents<CharacterController>();
                    foreach (CharacterController oldController in previousControllerComps)
                    {
                        Destroy(oldController);
                    }
                    character.GetComponent<CharacterManager>().SetNewCharacterControllerOfType(charaSpawnData.CharaControllerType);
                }
                break;
            default:
                Debug.LogError(entityType.ToString() + " not currently handle for spawn");
                return -1;
        }

        entity.SetEntityData(entityData);
        entity.SetEntityType(entityType);
        entity.SetEntityID(entityID);
        entity.SetWorldPosition(worldPosition);

        _SpawnedEntity.Add(entityID, entity);

        return entityID;
    }

    public IWorldEntity TryGetEntityByID(int ID)
    {
        if (_SpawnedEntity.ContainsKey(ID))
        {
            return _SpawnedEntity[ID];
        }
        return (null);
    }

    public void DeleteEntity(int ID)
    {
        // TODO : search the object of this entity and go delete it

        DeleteEntityFromList(ID);
    }

    public void DeleteEntityFromList(int ID)
    {
        _SpawnedEntity.Remove(ID);
    }
}

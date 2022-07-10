using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonobehaviourSingleton<EntityManager>
{
    [Header("Data")]
    [SerializeField] private int _MinAvailableEntityID = 10001;

    [Header("Prefabs")]
    [SerializeField] private GameObject _PlayerPrefab;
    [SerializeField] private GameObject _BaseCharacterPrefab;

    private Dictionary<int, IWorldEntity> _SpawnedEntity = new Dictionary<int, IWorldEntity>();

    public void SpawnEntity(IWorldEntity.EEntityType entityType, IWorldEntityData entityData,
        Vector3 worldPosition, int entityID = -1)
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
                break;
            default:
                Debug.LogError(entityType.ToString() + " not currently handle for spawn");
                return;
        }

        entity.SetEntityData(entityData);
        entity.SetEntityType(entityType);
        entity.SetEntityID(entityID);

        _SpawnedEntity.Add(entityID, entity);
    }

    public IWorldEntity TryGetEntityByID(int ID)
    {
        if (_SpawnedEntity.ContainsKey(ID))
        {
            return _SpawnedEntity[ID];
        }
        return (null);
    }
}

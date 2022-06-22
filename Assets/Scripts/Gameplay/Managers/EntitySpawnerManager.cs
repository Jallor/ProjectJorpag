using System.Collections.Generic;
using UnityEngine;

public class EntitySpawnerManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private int _MinAvailableEntityID = 1001;

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

        // Create id if needed 
        // instantiate
        // add to dico
    }
}

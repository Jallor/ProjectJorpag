using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IWorldEntity;

public class HiveMindManager : MonoBehaviour, IWorldEntity
{
    [Required][SerializeField] private SpawnableManagerData _Data;

    private EEntityType _EntityType = EEntityType.SPAWNABLE_MANAGER;
    private int _EntityID = -1;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

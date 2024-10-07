using UnityEngine;

public interface IWorldEntity
{
    public enum EEntityType
    {
        NONE = -1,
        PLAYER = 1,
        BASE_CHARACTER = 2,
        SPAWNABLE_MANAGER = 3,
    }

    public void SetEntityData(IWorldEntityData entityData);
    public IWorldEntityData GetEntityData();

    public void SetEntityType(EEntityType entityType);
    public EEntityType GetEntityType();

    public void SetEntityID(int entityID);
    public int GetEntityID();

    public void SetWorldPosition(Vector3 worldPosition);
    public Vector3 GetWorldPosition();
    public Vector2Int GetGridPosition();
}

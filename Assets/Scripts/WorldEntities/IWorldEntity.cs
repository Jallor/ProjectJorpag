using UnityEngine;

public interface IWorldEntity
{
    public enum EEntityType
    {
        NONE = -1,
        PLAYER = 1,
        BASE_CHARACTER = 2,
    }

    public void SetEntityType(EEntityType entityType);
    public EEntityType GetEntityType();

    public void SetEntityID(int entityID);
    public int GetEntityID();

    public Vector2Int GetWorldPosition();
    public Vector3 GetGridPosition();
}

using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class LandmarkData
{
    public Vector2Int Position = Vector2Int.zero;

    public abstract ELandmarkType Type { get; }
}

public class LandmarkData_SimpleLandmark : LandmarkData
{
    public override ELandmarkType Type => ELandmarkType.SimpleLandmark;
}

public class LandmarkData_HiveSpawn : LandmarkData
{
    public override ELandmarkType Type => ELandmarkType.HiveSpawn;
}

public class LandmarkData_Deposit : LandmarkData
{
    public override ELandmarkType Type => ELandmarkType.Deposit;

    public InventoryItem LootableItem;
}

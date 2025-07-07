using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LandmarkData
{
    public Vector2Int Position = Vector2Int.zero;
    [SerializeField] public ELandmarkType Type = ELandmarkType.SimpleLandmark;

}

public class LandmarkData_Deposit : LandmarkData
{
    [SerializeField] public new ELandmarkType Type = ELandmarkType.Deposit;

    public InventoryItem LootableItem;
}

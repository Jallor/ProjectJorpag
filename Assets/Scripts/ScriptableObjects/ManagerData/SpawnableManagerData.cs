using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnableManagerData", menuName = "Data/SpawnableManager")]
public class SpawnableManagerData : ScriptableObject, IWorldEntityData
{
    [Header("Inventory")]
    public bool HasInventory = false;
    [ShowIf("HasInventory")][SerializeField] private int _MaxInventorySlot = 10;
    public int MaxInventorySlot { get => _MaxInventorySlot; set => _MaxInventorySlot = value; }
    public bool DisplayFirstOwningObject { get; set; }
}

using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static TileMapMetaData;

[CreateAssetMenu(fileName = "NewHiveMindBehavior", menuName = "Data/HiveMind Behavior")]
public class HiveMindBehaviorData : ScriptableObject
{
    [System.Serializable]
    public class CraftRecipe
    {
        [SerializeField] public List<Pair<InventoryItem, int>> RequestedItems = new List<Pair<InventoryItem, int>>();
        [SelectImplementation] [SerializeReference]
        [SerializeField] public SpawnData CraftResult;
    }

    [SerializeField]
    public List<CraftRecipe> AvailableCrafts = new List<CraftRecipe>();
}

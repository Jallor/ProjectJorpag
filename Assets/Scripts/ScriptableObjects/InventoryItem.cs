using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Data/Inventory Item")]
public class InventoryItem : ScriptableObject
{
    public enum EItemType
    {
        RESSOURCES = 0,
        WEAPON = 1,

    }


    public EItemType ItemType = EItemType.RESSOURCES;
    public string ItemName = "[ITEM]";
    [ShowAssetPreview]
    public Sprite Sprite = null;
    public bool IsSimpleItem = true;
}

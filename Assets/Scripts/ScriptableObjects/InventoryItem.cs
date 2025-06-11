using System.Collections.Generic;
using UnityEngine;

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
    public Sprite Sprite;
    public bool IsSimpleItem = true;
}

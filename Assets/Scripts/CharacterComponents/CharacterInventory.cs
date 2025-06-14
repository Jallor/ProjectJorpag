﻿using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

public class CharacterInventory : MonoBehaviour
{
    private Dictionary<InventoryItem, int> _SimpleItemsMap = new Dictionary<InventoryItem, int>();
    // TODO Later : une autre map pour des items custom, qui seraient stockés sous forme de scriptable obj directement, surement
    // private Dictionary<InventoryItem, int> _ComplexeItemsMap = new Dictionary<InventoryItem, int>();

    public int GetItemCount(InventoryItem item)
    {
        if (_SimpleItemsMap.ContainsKey(item))
        {
            return (_SimpleItemsMap[item]);
        }

        return 0;
    }

    public int GetItemCountOfType(InventoryItem.EItemType itemType)
    {
        int count = 0;

        foreach (InventoryItem item in _SimpleItemsMap.Keys)
        {
            if (item.ItemType == itemType)
            {
                count += _SimpleItemsMap[item];
            }
        }

        return (count);
    }
}

using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using System;

public class CharacterInventory : MonoBehaviour
{
    private CharacterManager _Manager = null;
    private Dictionary<InventoryItem, int> _SimpleItemsMap = new Dictionary<InventoryItem, int>();
    // TODO Later : une autre map pour des items custom, qui seraient stockés sous forme de scriptable obj directement, surement
    // private Dictionary<InventoryItem, int> _ComplexeItemsMap = new Dictionary<InventoryItem, int>();
    // TODO : vérifier que quand on ajoutera ça, qu'on adapte bien tout le code

    // TODO : à distinguer à terme des arme : les armes et obj équipés doivent avoir leur propre slot

    private int _MaxInventorySlot = -1;
    private bool _ShouldDisplayFirstOwnedObject = false;
    private InventoryItem _FirstOwningItem = null;

    public void Initialize(CharacterManager manager, CharacterData data)
    {
        _Manager = manager;

        _MaxInventorySlot = data.MaxInventorySlot;
        _ShouldDisplayFirstOwnedObject = data.DisplayFirstOwningObject;
    }

    public bool AddItem(InventoryItem item, int count = 1)
    {
        // Simple Items
        if (item.IsSimpleItem)
        {
            if (_SimpleItemsMap.ContainsKey(item))
            {
                _SimpleItemsMap[item] += count;

                return true;
            }
            else if (_MaxInventorySlot == -1 || GetNbInventorySlotUsed() < _MaxInventorySlot)
            {
                _SimpleItemsMap.Add(item, count);

                _FirstOwningItem = item;
                if (_ShouldDisplayFirstOwnedObject)
                {
                    _Manager.GetSpriteAnimator().DisplayOwnedObject(item.Sprite);
                }

                return true;
            }
        }
        // Complexe Items
        else
        {
        }

        // TODO : prévoir les raison de fail (inventaire limité, restriction spécifique, trop d'item, ...)
        return false;
    }

    public void RemoveAllItems()
    {
        throw new NotImplementedException();
    }

    // -1 is used for remove all objects of type
    public int RemoveItem(InventoryItem item, int countToRemove = -1)
    {
        int itemRemoved = 0;

        if (item.IsSimpleItem)
        {
            if (_SimpleItemsMap.ContainsKey(item))
            {
                int currentCount = _SimpleItemsMap[item];
                if (countToRemove == -1)
                {
                    countToRemove = currentCount;
                }

                itemRemoved = Mathf.Min(countToRemove, currentCount);
                _SimpleItemsMap[item] -= itemRemoved;

                if (_SimpleItemsMap[item] == 0)
                {
                    _SimpleItemsMap.Remove(item);

                    // TODO devrait être géré ailleur pour ne pas allourdir la fonction
                    if (_ShouldDisplayFirstOwnedObject && item == _FirstOwningItem)
                    {
                        if (GetAllItemsCount() > 0)
                        {

                            _Manager.GetSpriteAnimator().DisplayOwnedObject(_SimpleItemsMap.Keys.ToList()[0].Sprite);
                        }
                        else
                        {
                            _Manager.GetSpriteAnimator().DisplayOwnedObject(null);
                        }
                    }
                }
            }
            else
            {
                return -1;
            }
        }
        // TODO : update de l'inventaire. Surement via delegate

        // TODO : handle complexe items

        return itemRemoved;
    }

    public int GetAllItemsCount()
    {
        int count = 0;

        foreach (InventoryItem item in _SimpleItemsMap.Keys)
        {
            count += _SimpleItemsMap[item];
        }

        return (count);
    }

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

    public int GetNbInventorySlotUsed()
    {
        int nbSlot = 0;

        foreach (int values in _SimpleItemsMap.Values)
        {
            if (values > 0)
            {
                ++nbSlot;
            }
        }

        // TODO : implement also for complexe items

        return nbSlot;
    }

    public List<InventoryItem> GetItemList()
    {
        throw new NotImplementedException();
    }
}

using UnityEngine;

public interface IWorldEntityData
{
    // Inventory
    int MaxInventorySlot { get; set; }
    bool DisplayFirstOwningObject { get; set; }
}

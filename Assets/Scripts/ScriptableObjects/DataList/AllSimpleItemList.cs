using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllTileGridMaps", menuName = "DataList/All SimpleItems")]
public class AllSimpleItemList : DataAutoList<InventoryItem, AllSimpleItemList>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void RefreshList()
    {
        if (!(Inst is null))
        {
            Inst.RefreshDataList();
        }
    }
}

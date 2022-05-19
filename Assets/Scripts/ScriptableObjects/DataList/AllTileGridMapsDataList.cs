using UnityEngine;

[CreateAssetMenu(fileName = "AllTileGridMaps", menuName = "DataList/All TileGridMaps")]
public class AllTileGridMapsDataList : DataAutoList<SerializedTileGridMapData, AllTileGridMapsDataList>
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

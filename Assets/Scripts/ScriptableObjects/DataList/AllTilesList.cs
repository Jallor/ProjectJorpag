using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "AllTiles", menuName = "DataList/All Tiles")]
public class AllTilesList : DataAutoList<TileBase, AllTilesList>
{
    public EnumEditable CorrespondingEditableEnum;

    public Dictionary<int, TileBase> TilesDictionnary = new Dictionary<int, TileBase>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void RefreshList()
    {
        if (!(Inst is null))
        {
            Inst.RefreshDataList();
        }
    }

    public override void RefreshDataList()
    {
#if UNITY_EDITOR
        base.RefreshDataList();

        bool isDirty = false;
        foreach (TileBase tile in dataList)
        {
            int tileHashCode = tile.name.GetHashCode();
            if (!Enum.IsDefined(typeof(ETileType), tileHashCode))
            {
                isDirty = true;

                EnumEditable.EnumValue enumValue = new EnumEditable.EnumValue();
                enumValue.enumValueName = tile.name;
                enumValue.resetIDToHash = true;
                CorrespondingEditableEnum.EnumValues.Add(enumValue);

                TilesDictionnary[tileHashCode] = tile;
            }

            if (isDirty)
            {
                CorrespondingEditableEnum.UpdateEnum();
            }
        }
#endif
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using static SerializedTileGridMapData;

public class GameTileGrid : MonoBehaviour
{
    public enum TilemapTag
    {
        BACKGROUND = 0,
        WALL = 1,
        FRONTGROUND = 2
    }

    [SerializeField] private List<Tilemap> _BackgroundTilemaps = new List<Tilemap>();
    [SerializeField] private List<Tilemap> _Tilemaps = new List<Tilemap>();
    [SerializeField] private List<Tilemap> _FrontgroundTilemaps = new List<Tilemap>();

    public void Start()
    {
        SerializeTileGridMap();
    }

    public SerializedTileGridMapData SerializeTileGridMap()
    {
        SerializedTileGridMapData tileGrideMap = ScriptableObject.CreateInstance<SerializedTileGridMapData>();

        for (int i = 0; i < _BackgroundTilemaps.Count; i++)
        {
            tileGrideMap._TileMapsLayers.Add(SerializeTilemap(_BackgroundTilemaps[i], TilemapTag.BACKGROUND, i));
        }
        for (int i = 0; i < _Tilemaps.Count; i++)
        {
            tileGrideMap._TileMapsLayers.Add(SerializeTilemap(_Tilemaps[i], TilemapTag.WALL, i));
        }
        for (int i = 0; i < _FrontgroundTilemaps.Count; i++)
        {
            tileGrideMap._TileMapsLayers.Add(SerializeTilemap(_FrontgroundTilemaps[i], TilemapTag.FRONTGROUND, i));
        }

        return (tileGrideMap);
    }

    private SerializedTilemapLayer SerializeTilemap(Tilemap tilemap, TilemapTag tag, int index)
    {
        SerializedTilemapLayer tilemapLayer = new SerializedTilemapLayer();

        tilemapLayer.Tag = tag;
        tilemapLayer.Index = index;
        tilemapLayer.BoundsCenter = tilemap.cellBounds.center;
        tilemapLayer.BoundsSize = tilemap.cellBounds.size;

        foreach (TileBase tile in tilemap.GetTilesBlock(tilemap.cellBounds))
        {
            if (tile != null)
            {
                SerializedTile mapTile = new SerializedTile();
                mapTile.TileType = (ETileType)tile.name.GetHashCode();
                tilemapLayer.TileList.Add(mapTile);
            }
            else
            {
                SerializedTile mapTile = new SerializedTile();
                mapTile.TileType = ETileType.NONE;
                tilemapLayer.TileList.Add(mapTile);
            }
        }

        return (tilemapLayer);
    }
}

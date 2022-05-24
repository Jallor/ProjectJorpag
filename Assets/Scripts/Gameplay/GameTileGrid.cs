using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

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
    [SerializeField] private List<Tilemap> _WallsTilemaps = new List<Tilemap>();
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
            tileGrideMap._TileMapsLayers.Add(SerializeTilemapLayer(_BackgroundTilemaps[i], TilemapTag.BACKGROUND, i));
        }
        for (int i = 0; i < _WallsTilemaps.Count; i++)
        {
            tileGrideMap._TileMapsLayers.Add(SerializeTilemapLayer(_WallsTilemaps[i], TilemapTag.WALL, i));
        }
        for (int i = 0; i < _FrontgroundTilemaps.Count; i++)
        {
            tileGrideMap._TileMapsLayers.Add(SerializeTilemapLayer(_FrontgroundTilemaps[i], TilemapTag.FRONTGROUND, i));
        }

        return (tileGrideMap);
    }

    private SerializedTilemapLayer SerializeTilemapLayer(Tilemap tilemap, TilemapTag tag, int index)
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

    public void Clear()
    {
        List<Tilemap> tilemaps = new List<Tilemap>();
        tilemaps.AddRange(_BackgroundTilemaps);
        tilemaps.AddRange(_WallsTilemaps);
        tilemaps.AddRange(_FrontgroundTilemaps);

        foreach (Tilemap tilemap in tilemaps)
        {
            tilemap.ClearAllTiles();
            tilemap.size = new Vector3Int(0, 0, 0);
            tilemap.ResizeBounds();
        }
    }

    public void LoadTileGridMap(SerializedTileGridMapData tileGridMap)
    {
        Clear();

        foreach (SerializedTilemapLayer mapLayer in tileGridMap._TileMapsLayers)
        {
            Tilemap tilemapToUse = mapLayer.Tag switch
            {
                TilemapTag.BACKGROUND => _BackgroundTilemaps[mapLayer.Index],
                TilemapTag.WALL => _WallsTilemaps[mapLayer.Index],
                TilemapTag.FRONTGROUND => _FrontgroundTilemaps[mapLayer.Index]
            };

            TileBase[] tileList = new TileBase[mapLayer.TileList.Count];

            for (int i = 0; i < mapLayer.TileList.Count; i++)
            {
                if (mapLayer.TileList[i].TileType != ETileType.NONE)
                {
                    TileBase tile;
                    string tileNameToFind = mapLayer.TileList[i].TileType.ToString();
                    tile = Instantiate(AllTilesList.Inst.dataList.Where(t => t.name == tileNameToFind).First());
                    tileList[i] = tile;
                }
            }

            BoundsInt bounds = new BoundsInt();
            bounds.position = new Vector3Int((int)mapLayer.BoundsCenter.x,
                (int)mapLayer.BoundsCenter.y, (int)mapLayer.BoundsCenter.z);
            bounds.size = new Vector3Int((int)mapLayer.BoundsSize.x,
                (int)mapLayer.BoundsSize.y, (int)mapLayer.BoundsSize.z);
            tilemapToUse.SetTilesBlock(bounds, tileList);
        }
    }
}

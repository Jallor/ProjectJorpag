using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

using static SerializedTileGridMapData;
using Unity.VisualScripting;

public class GameTileGrid : MonobehaviourSingleton<GameTileGrid>
{
    public enum TilemapTag
    {
        BACKGROUND = 0,
        WALL = 1,
        FRONTGROUND = 2
    }

    [SerializeField] private Grid _GameGrid = null;
    [SerializeField] private List<Tilemap> _BackgroundTilemaps = new List<Tilemap>();
    [SerializeField] private List<Tilemap> _WallsTilemaps = new List<Tilemap>();
    [SerializeField] private List<Tilemap> _FrontgroundTilemaps = new List<Tilemap>();

    public override void Awake()
    {
        base.Awake();
        if (_GameGrid == null)
        {
            _GameGrid = GetComponent<Grid>();
        }
    }

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
        tilemapLayer.BoundsCenter = tilemap.origin;
        tilemapLayer.BoundsSize = tilemap.size;

        foreach (TileBase tile in tilemap.GetTilesBlock(tilemap.cellBounds))
        {
            if (tile == null)
            {
                SerializedTile mapTile = new SerializedTile();
                mapTile.Tile = null;
                tilemapLayer.TileList.Add(mapTile);
            }
            // Rule Override Tile & Simple sprite
            else if (tile.GetType().IsSubclassOf(typeof(RuleOverrideTile))
                || tile.GetType() == typeof(RuleOverrideTile)
                || tile.GetType() == typeof(Tile))
            {
                SerializedTile mapTile = new SerializedTile();
                string tileName = tile.name.Replace("(Clone)", "");
                IEnumerable<TileBase> foundTiles = AllTilesList.Inst.dataList.Where(t => t.name == tileName);
                if (foundTiles.Count() > 0)
                {
                    mapTile.Tile = foundTiles.First();
                }
                else
                {
                    mapTile.Tile = null;
                }
                tilemapLayer.TileList.Add(mapTile);
            }
            else
            {
                Debug.Log("Tile not saved : " + tile.name + " of type " + tile.GetType());
                SerializedTile mapTile = new SerializedTile();
                mapTile.Tile = null;
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
                if (mapLayer.TileList[i].Tile != null)
                {
                    TileBase tile = Instantiate(mapLayer.TileList[i].Tile);
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

    public Vector3 GridPositionToWorldPosition(Vector2Int gridPosition)
    {
        Vector3 worldPosition = _BackgroundTilemaps[0].GetCellCenterWorld(new Vector3Int(gridPosition.x, gridPosition.y, 0));
        
        return (new Vector3(worldPosition.x, worldPosition.y, 0));
    }

    public Vector3Int WorldPositionToGridPosition(Vector3 worldPosition)
    {
        Vector3Int gridPosition = _BackgroundTilemaps[0].WorldToCell(worldPosition);
        
        return (gridPosition);
    }
}

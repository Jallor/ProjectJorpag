using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameGrid : MonoBehaviour
{
    [System.Serializable]
    public class SerializedGameGrid
    {

    }
    
    [System.Serializable]
    public class SerializedTilemap
    {
        public TilemapTag Tag;
        public int Index;
        public List<SerializedTile> _TileList = new List<SerializedTile>();
    }

    [System.Serializable]
    public class SerializedTile
    {

    }

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
        SerializeGrid();
    }

    public void SerializeGrid()
    {
        for (int i = 0; i < _BackgroundTilemaps.Count; i++)
        {
            SerializeTilemap(_BackgroundTilemaps[i], TilemapTag.BACKGROUND, i);
        }
        for (int i = 0; i < _Tilemaps.Count; i++)
        {
            SerializeTilemap(_Tilemaps[i], TilemapTag.WALL, i);
        }
        for (int i = 0; i < _FrontgroundTilemaps.Count; i++)
        {
            SerializeTilemap(_FrontgroundTilemaps[i], TilemapTag.FRONTGROUND, i);
        }
    }

    private void SerializeTilemap(Tilemap tilemap, TilemapTag tag, int index)
    {
        Vector3 center = tilemap.localBounds.center;
        Vector3 size = tilemap.localBounds.size;

        Bounds b = new Bounds(center, size);
    }
}

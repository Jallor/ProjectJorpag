using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using static GameTileGrid;

[CreateAssetMenu(fileName = "NewSerializedTileMap", menuName = "Data/Serialized TileMap")]
public class SerializedTileGridMapData : ScriptableObject
{
    [System.Serializable]
    public class SerializedTile
    {
        public TileBase Tile;
    }

    [System.Serializable]
    public class SerializedTilemapLayer
    {
        public TilemapTag Tag;
        public int Index;
        public Vector3 BoundsCenter;
        public Vector3 BoundsSize;
        public List<SerializedTile> TileList = new List<SerializedTile>();
    }

    public List<SerializedTilemapLayer> _TileMapsLayers = new List<SerializedTilemapLayer>();

}

using UnityEngine.Timeline;
using UnityEngine;

[CreateAssetMenu(fileName="NewScenariMapData", menuName ="Data/Scenari Map Data")]
public class ScenariMapData : ScriptableObject
{
    // Contains the tile grid map with onliy tiles
    public SerializedTileGridMapData TileMap;
    // Contains like character spawn map set up and (maybe) tile override
    public TileMapMetaData MapMetaData;
    // First cinematic
    public TimelineAsset FirstCinematic;
}

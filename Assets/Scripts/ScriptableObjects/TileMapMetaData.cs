using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewTileMapMetaData", menuName ="Data/Map MetaData")]
public class TileMapMetaData : ScriptableObject
{
    [System.Serializable]
    public class SpawnData
    {
        public CharacterData CharaData;
        public bool IsPlayer = false;
        public Vector2Int SpawnPoint;
    }

    public List<SpawnData> SpawnDataList;
}

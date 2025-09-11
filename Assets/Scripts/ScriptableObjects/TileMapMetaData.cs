using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTileMapMetaData", menuName = "Data/Map MetaData")]
public class TileMapMetaData : ScriptableObject
{
    #region SpawnData (CharacterSpawnData, ManagerSpawnData, ...)
    [SelectImplementationName("Manager Spawn Data")]
    public class ManagerSpawnData : SpawnData
    {
        public enum ESpawnableManager
        {
            HIVE_MIND = 0,
        }

        public ESpawnableManager ManagerType;
        public SpawnableManagerData ManagerData;
    }

    [SelectImplementationName("Character Spawn Data")]
    public class CharacterSpawnData : SpawnData
    {
        public CharacterData CharaData;
        public bool IsPlayer = false;

        [HideIf("IsPlayer")] [AllowNesting]
        public CharacterController.ECharacterControllerType CharaControllerType = CharacterController.ECharacterControllerType.NULL;
        public bool DisableCollider = false;
    }

    [System.Serializable]
    public abstract class SpawnData
    {
        public Vector2Int SpawnPoint;
    }
    #endregion // Spawn Data

    [SelectImplementation] [SerializeReference]
    [SerializeField] public List<SpawnData> SpawnDataList;

    [SelectImplementation] [SerializeReference]
    [SerializeField] public List<LandmarkData> LandmarkList;
}

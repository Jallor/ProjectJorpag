using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonobehaviourSingleton<GameManager>
{
    [Header("Managers")]
    [Required] [SerializeField] private GameTileGrid _TileGrid;
    [SerializeField] private EntityManager _EntityManager;

    // Other stuff ("Defautl value still at the end")

    [Header("Defaults value")]
    [SerializeField] private ScenariMapData _DefaultScenari;

    private CharacterManager _Player = null;
    private List<LandmarkData> _LandmarkList = new List<LandmarkData>();

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // Handle scenari to load
        // If not, load default one
        LoadScenariMap(_DefaultScenari);
    }

    public void LoadScenariMap(ScenariMapData scenariToLoad)
    {
        _TileGrid.LoadTileGridMap(scenariToLoad.TileMap);

        foreach (TileMapMetaData.SpawnData spawnData in scenariToLoad.MapMetaData.SpawnDataList)
        {
            Vector3 position = _TileGrid.GridPositionToWorldPosition(spawnData.SpawnPoint);
            if (spawnData is TileMapMetaData.CharacterSpawnData)
            {
                TileMapMetaData.CharacterSpawnData charaSpawnData = spawnData as TileMapMetaData.CharacterSpawnData;

                if (charaSpawnData.IsPlayer)
                {
                    _EntityManager.SpawnEntity(IWorldEntity.EEntityType.PLAYER,
                        charaSpawnData, charaSpawnData.CharaData, position, 0);
                }
                else
                {
                    _EntityManager.SpawnEntity(IWorldEntity.EEntityType.BASE_CHARACTER,
                        charaSpawnData, charaSpawnData.CharaData, position);
                }
            }
            else if (spawnData is TileMapMetaData.ManagerSpawnData)
            {
                TileMapMetaData.ManagerSpawnData managerSpawnData = spawnData as TileMapMetaData.ManagerSpawnData;
                _EntityManager.SpawnEntity(IWorldEntity.EEntityType.SPAWNABLE_MANAGER,
                    managerSpawnData, null, Vector3.zero);
            }
            else
            {
                Debug.LogError("Spawn data not implemented");
            }
        }

        _LandmarkList.AddRange(scenariToLoad.MapMetaData.LandmarkList);

        // cinematic
    }

    public CharacterManager GetPlayer()
    {
        if (_Player == null)
        {
            PlayerController playerController = FindObjectOfType<PlayerController>();
            if (playerController != null)
            {
                _Player = playerController.GetCharaManager(); ;
            }
        }
        return (_Player);
    }

    public List<LandmarkData> GetLandmarksOfType(ELandmarkType landmarkType)
    {
        List<LandmarkData> landmarkList = new List<LandmarkData>();
        foreach (LandmarkData landmark in _LandmarkList)
        {
            if (landmark.Type == landmarkType)
            {
                landmarkList.Add(landmark);
            }
        }

        return (landmarkList);
    }

    public bool CanCharactersAct()
    {
        if (UiManager.Inst && UiManager.Inst.IsUiBlockingCharaActions())
        {
            return false;
        }

        return true;
    }
}

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
    private List<Landmark> _LandmarkList = new List<Landmark>();

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

        foreach (LandmarkData landmarkData in scenariToLoad.MapMetaData.LandmarkList)
        {
            CreateNewLandmark(landmarkData, landmarkData.Position);
            _LandmarkList.Add(Landmark.CreateNewLandmark(landmarkData, landmarkData.Position));
        }


        // cinematic
    }

    // Note : function created to permits HiveMind (and other future manager) to creater their landmark. This may be a wrong design
    public Landmark CreateNewLandmark(LandmarkData landmarkData, Vector2Int position)
    {
        Landmark newLandmark = Landmark.CreateNewLandmark(landmarkData, position);
        _LandmarkList.Add(newLandmark);
        return (newLandmark);
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

    public List<Landmark> GetLandmarksOfType(ELandmarkType landmarkType)
    {
        List<Landmark> landmarkList = new List<Landmark>();
        foreach (Landmark landmark in _LandmarkList)
        {
            if (landmark.Type == landmarkType)
            {
                landmarkList.Add(landmark);
            }
        }

        return (landmarkList);
    }

    public Landmark GetLandmarkAtPosition(Vector2Int position)
    {
        foreach (Landmark landmark in _LandmarkList)
        {
            if (landmark.Position == position)
            {
                return landmark;
            }
        }

        return null;
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

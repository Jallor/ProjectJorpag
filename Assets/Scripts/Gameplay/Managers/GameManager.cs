using NaughtyAttributes;
using UnityEngine;

public class GameManager : MonobehaviourSingleton<GameManager>
{
    [Header("Managers")]
    [Required]
    [SerializeField] private GameTileGrid _TileGrid;
    [SerializeField] private EntitySpawnerManager _EntitySpawner;

    //

    [Header("Defaults value")]
    [SerializeField] private ScenariMapData _DefaultScenari;

    private CharacterManager _Player = null;

    private void Awake()
    {
        
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
            if (spawnData.IsPlayer)
            {

            }
            else
            {

            }
        }

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

    public bool CanCharactersAct()
    {
        if (UiManager.Inst && UiManager.Inst.IsUiBlockingCharaActions())
        {
            return false;
        }

        return true;
    }
}

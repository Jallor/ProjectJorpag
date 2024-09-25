using UnityEngine;
using NaughtyAttributes;

public abstract class CharacterController : MonoBehaviour
{
    public enum ECharacterControllerType
    {
        NULL = 0,
        PLAYER_CONTROLLER = 1,
        BASIC_WORLD_BASED_MOVE = 2,
        BASIC_GRID_BASED_MOVE = 3,
    }

    [Required] [SerializeField] protected CharacterManager _CharaManager;

    public CharacterManager GetCharaManager() => _CharaManager;
    public void SetCharaManager(CharacterManager charaManager) { _CharaManager = charaManager; }

    public abstract ECharacterControllerType GetCharacterControllerType();
}

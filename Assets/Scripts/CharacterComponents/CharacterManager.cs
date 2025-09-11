using UnityEngine;
using NaughtyAttributes;

using static CharacterSpriteAnimator;
using UnityEditor.SceneManagement;

[RequireComponent(typeof(CharacterMovement), typeof(CharacterSpriteAnimator))]
public class CharacterManager : MonoBehaviour, IWorldEntity
{
    [Required] [SerializeField] private CharacterController _Controller;
    [Required] [SerializeField] private CharacterMovement _Movement;
    [Required] [SerializeField] private CharacterSpriteAnimator _SpriteAnimator;
    [Required] [SerializeField] private CharacterData _Data;
    [Required] [SerializeField] private SkillCharacterManager _SkillManager;
    [Required] [SerializeField] private StatusEffectsCharacterManager _StatusManager;
    [Required] [SerializeField] private CharacterVfxManager _VfxManager;
    [Required] [SerializeField] private InventoryComponent _Inventory;

    private CharacterStats _Stats = new CharacterStats();

    private IWorldEntity.EEntityType _EntityType = IWorldEntity.EEntityType.NONE;
    private int _EntityID = -1;

    private bool _IsInitialized = false;

    private CharacterOrientation _CurrentOrientation = CharacterOrientation.DOWN;
    private bool _IsMoving = false;


    public void Start()
    {
        Initialize();
    }

    public override string ToString()
    {
        return (_Data.CharaName);
    }

    private void Initialize()
    {
        if ( _IsInitialized)
        {
            return;
        }

        // TODO : To change (je sais plus pourquoi 'x) ) (peut être normalizer ça à toutes les entitées ?)
        SetEntityData(_Data);

        _SkillManager.Initialize(this);
        _StatusManager.Initialize(this);
        _VfxManager.Initialize(this);

        _SpriteAnimator.SetWeapon(GetDefaultWeapon());
        _SpriteAnimator.PlayIdleAnimation(CharacterOrientation.DOWN);

        _IsInitialized = true;

        // Bind delegates
        _Stats.Life.OnValueUpdated += CheckLifeUpdated;
    }

    #region Movement Stuff
    public void GiveMoveInput(Vector2 newDir)
    {
        Vector2 modifiedDir = newDir;

        if (!CanCharacterMove())
        {
            modifiedDir = Vector2.zero;
        }
        _Movement.GiveInput(modifiedDir);

        if (newDir == Vector2.zero || !CanCharacterRotate())
        {
            if (_IsMoving)
            {
                _SpriteAnimator.PlayIdleAnimation(_CurrentOrientation);
                _IsMoving = false;
            }
        }
        else
        {
            CharacterOrientation orientation = _CurrentOrientation;
            if (newDir.x > 0)
            {
                orientation = CharacterOrientation.RIGHT;
            }
            else if (newDir.x < 0)
            {
                orientation = CharacterOrientation.LEFT;
            }
            else if (newDir.y > 0)
            {
                orientation = CharacterOrientation.TOP;
            }
            else if (newDir.y < 0)
            {
                orientation = CharacterOrientation.DOWN;
            }

            if (orientation != _CurrentOrientation || !_IsMoving)
            {
                _CurrentOrientation = orientation;
                _IsMoving = true;
                _SpriteAnimator.PlayWalkAnimation(orientation);
            }
        }
    }

    public bool CanCharacterMove()
    {
        if (IsPlayingSkill())
        {
            return (_SkillManager.IsMovementAllowed());
        }

        return (true);
    }

    public bool CanCharacterRotate()
    {
        if (IsPlayingSkill())
        {
            return (_SkillManager.IsRotationAllowed());
        }

        return (true);
    }
    #endregion

    public void TryPlaySkill()
    {
        _SkillManager.TryPlaySkill(_Data.TMP_SkillData);
    }

    public GameObject InstantiateThrowableObject(GameObject prefab)
    {
        GameObject throwableObj = Instantiate(prefab);
        throwableObj.transform.position = _SpriteAnimator.GetForwardWeaponWorldPosition();

        return throwableObj;
    }

    public void InflictDamage(float damageQuantity)
    {
        _Stats.Life.Add(-damageQuantity);
    }

    public void ApplyStatusEffect(StatusEffect statusEffectToApply)
    {
        _StatusManager.ApplyStatusEffect(statusEffectToApply);
    }

    public int AddStatusVfx(EStatusVfx statusVfxToAdd)
    {
        return (_VfxManager.AddStatusVfx(statusVfxToAdd));
    }

    public void RemoveVfx(int idToRemove)
    {
        _VfxManager.RemoveVfx(idToRemove);
    }

    void CheckLifeUpdated(float lifeModifier)
    {
        if (_Stats.Life.GetCurrentValue() <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        print("Oups I'm dead !");

        EntityManager.Inst.DeleteEntityFromList(_EntityID);

        Destroy(gameObject);
    }

    #region IWorldEntity
    public void SetEntityData(IWorldEntityData entityData)
    {
        CharacterData dataToApply = (CharacterData)entityData;
        Debug.Assert(dataToApply != null);
        _Data = Instantiate(dataToApply);

        _SpriteAnimator.UpdateSpriteSheet(_Data.SpriteSheet);

        _Stats.Life = new CharacterStats.ConsomableStat();
        _Stats.Life.Init(_Data.MaxHP);
        _Stats.MovementSpeed = new CharacterStats.ImprovableStat();
        _Stats.MovementSpeed.Init(_Data.MovementSpeed);

        _Inventory.Initialize(this, _Data);

        name = dataToApply.name;
    }
    public IWorldEntityData GetEntityData() => (_Data);

    public void SetEntityType(IWorldEntity.EEntityType entityType) => _EntityType = entityType;
    public IWorldEntity.EEntityType GetEntityType() => (_EntityType);

    public void SetEntityID(int entityID) => _EntityID = entityID;
    public int GetEntityID() => (_EntityID);

    public void SetWorldPosition(Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }
    public Vector3 GetWorldPosition()
    {
        return (transform.position);
    }
    public Vector2Int GetGridPosition()
    {
        Vector3Int gridPosition = GameTileGrid.Inst.WorldPositionToGridPosition(transform.position);
        return (new Vector2Int(gridPosition.x, gridPosition.y));
    }
    #endregion

    #region Getters
    public bool IsPlayingSkill() => (_SkillManager.IsPlayingSkill());
    public CharacterSpriteAnimator GetSpriteAnimator() => (_SpriteAnimator);
    public InventoryComponent GetCharaInventory() => (_Inventory);
    public CharacterOrientation GetCharacterOrientation() => (_CurrentOrientation);
    public int GetCurrentHP() => ((int)_Stats.Life.GetCurrentValue());
    public int GetMaxHP() => ((int)_Stats.Life.MaxValue);
    public float GetCurrentMovementSpeed() => (_Stats.MovementSpeed.GetCurrentValue());
    public WeaponData GetDefaultWeapon() => (_Data.DefaultWeapon);
    public Collider2D GetCollider2D() => (GetComponent<Collider2D>());
    public CharacterSpriteSheetData InstantiateCharaSpriteSheet() => (Instantiate(_Data.SpriteSheet));
    #endregion

    #region Setters
    public void SetNewCharacterControllerOfType(CharacterController.ECharacterControllerType controllerType)
    {
        CharacterController newController = null;

        switch (controllerType)
        {
            case CharacterController.ECharacterControllerType.PLAYER_CONTROLLER:
                newController = gameObject.AddComponent<PlayerController>();
                break;
            case CharacterController.ECharacterControllerType.BASIC_WORLD_BASED_MOVE:
                newController = gameObject.AddComponent<BasicWorldBasedMovementCharacterController>();
                break;
            case CharacterController.ECharacterControllerType.BASIC_GRID_BASED_MOVE:
                newController = gameObject.AddComponent<BasicGridBasedMovementCharacterController>();
                break;
            case CharacterController.ECharacterControllerType.RANDOM_MOVEMENT:
                newController = gameObject.AddComponent<RandomMovementCharacterController>();
                break;
            case CharacterController.ECharacterControllerType.ORDER_RECEIVER:
                newController = gameObject.AddComponent<OrderReceiverCharacterController>();
                break;
        }
        Debug.Assert(newController);
        newController.SetCharaManager(this);
        _Controller = newController;
    }
    #endregion
}

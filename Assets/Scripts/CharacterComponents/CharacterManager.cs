using UnityEngine;
using NaughtyAttributes;

using static CharacterSpriteAnimator;

[RequireComponent(typeof(CharacterMovement), typeof(CharacterSpriteAnimator))]
public class CharacterManager : MonoBehaviour, IWorldEntity
{
    [Required] [SerializeField] private CharacterController _Controller;
    [Required] [SerializeField] private CharacterMovement _Movement;
    [Required] [SerializeField] private CharacterSpriteAnimator _SpriteAnimator;
    [Required] [SerializeField] private CharacterData _Data;
    [Required] [SerializeField] private SkillCharacterManager _SkillManager;
    [Required] [SerializeField] private StatusEffectsCharacterManager _StatusManager;
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

        // To change
        SetEntityData(_Data);

        _SkillManager.Initialize(this);
        _StatusManager.Initialize(this);

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

    void CheckLifeUpdated(float lifeModifier)
    {
        print("DEBUG : Life " + _Stats.Life.CurrentValue + "   modified by " + lifeModifier);
        if (_Stats.Life.CurrentValue <= 0)
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
        _Stats.MovementSpeed = new CharacterStats.ConsomableStat();
        _Stats.MovementSpeed.Init(_Data.MovementSpeed);
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
        throw new System.NotImplementedException();
    }
    #endregion

    #region Getters
    public bool IsPlayingSkill() => (_SkillManager.IsPlayingSkill());

    public CharacterSpriteAnimator GetSpriteAnimator() => (_SpriteAnimator);

    public CharacterSpriteSheetData GetCharaSpriteSheet() => (Instantiate(_Data.SpriteSheet));

    public CharacterOrientation GetCharacterOrientation() => (_CurrentOrientation);

    public int GetCurrentHP() => ((int)_Stats.Life.CurrentValue);

    public int GetMaxHP() => ((int)_Stats.Life.MaxValue);

    public float GetCurrentMovementSpeed() => (_Stats.MovementSpeed.CurrentValue);

    public WeaponData GetDefaultWeapon() => (_Data.DefaultWeapon);
    #endregion
}

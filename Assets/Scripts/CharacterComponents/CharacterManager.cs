using UnityEngine;
using NaughtyAttributes;

using static CharacterSpriteAnimator;

[RequireComponent(typeof(CharacterMovement), typeof(CharacterSpriteAnimator))]
public class CharacterManager : MonoBehaviour
{
    [Required] [SerializeField] private CharacterController _Controller;
    [Required] [SerializeField] private CharacterMovement _Movement;
    [Required] [SerializeField] private CharacterSpriteAnimator _SpriteAnimator;
    [Required] [SerializeField] private CharacterData _Data;
    [Required] [SerializeField] private SkillCharacterManager _SkillManager;
    private CharacterStats _Stats = new CharacterStats();

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

        _Data = Instantiate(_Data);

        _Stats.Life = new CharacterStats.ConsomableStat();
        _Stats.Life.Init(_Data.MaxHP);
        _Stats.MovementSpeed = new CharacterStats.ConsomableStat();
        _Stats.MovementSpeed.Init(_Data.MovementSpeed);

        _SkillManager.Initialize(this);

        _SpriteAnimator.PlayIdleAnimation(CharacterOrientation.DOWN);

        _IsInitialized = true;
    }

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

    public void TryPlaySkill()
    {
        _SkillManager.TryPlaySkill(_Data.TMP_SkillData);
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

    #region Getters
    public bool IsPlayingSkill() => (_SkillManager.IsPlayingSkill());

    public CharacterSpriteAnimator GetSpriteAnimator() => (_SpriteAnimator);

    public CharacterSpriteSheetData GetCharaSpriteSheet() => (Instantiate(_Data.SpriteSheet));

    public CharacterOrientation GetCharacterOrientation() => (_CurrentOrientation);

    public int GetCurrentHP() => ((int)_Stats.Life.CurrentValue);

    public int GetMaxHP() => ((int)_Stats.Life.MaxValue);

    public float GetCurrentMovementSpeed() => (_Stats.MovementSpeed.CurrentValue);
    #endregion
}

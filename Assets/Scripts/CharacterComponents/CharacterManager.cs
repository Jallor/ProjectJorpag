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

    private CharacterOrientation _CurrentOrientation = CharacterOrientation.DOWN;
    private bool _IsMoving = false;

    public void Start()
    {
        _Data = Instantiate(_Data);

        _SpriteAnimator.PlayIdleAnimation(CharacterOrientation.DOWN);
    }

    public override string ToString()
    {
        return (_Data.CharaName);
    }

    public void GiveMoveInput(Vector2 newDir)
    {
        _Movement.GiveInput(newDir);

        if (newDir == Vector2.zero)
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

    #region Getters
    public CharacterSpriteSheetData GetCharaSpriteSheet() => (Instantiate(_Data.SpriteSheet));

    // TODO : get this var in manageer
    public int GetMaxHP() => (_Data.MaxHP);
    #endregion
}

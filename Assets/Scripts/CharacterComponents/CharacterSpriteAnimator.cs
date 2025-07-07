using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UIElements;

public class CharacterSpriteAnimator : MonoBehaviour
{
    [Required] [SerializeField] private CharacterManager _CharaManager;

    [Required] [SerializeField] private SpriteRenderer _SprRend;
    [Required] [SerializeField] private SpriteRenderer _WeaponObject;
    [Required] [SerializeField] private SpriteRenderer _OwnedObjectSlot;

    [Foldout("Key Positions")]
    [Required][SerializeField] private Transform _WeaponPositionDown;
    [Foldout("Key Positions")]
    [Required][SerializeField] private Transform _WeaponPositionLeft;
    [Foldout("Key Positions")]
    [Required][SerializeField] private Transform _WeaponPositionRight;
    [Foldout("Key Positions")]
    [Required][SerializeField] private Transform _WeaponPositionUp;

    [Header("Animation Parameters")]
    [SerializeField] private float _FrameDuration = 0.5f;

    private CharacterSpriteSheetData _SpriteSheet;
    private WeaponData _CurrentWeapon;

    // Current animation
    private List<Sprite> _CurrentSpriteList = null;
    private bool _IsLooping = true;
    // private delegate _CallbackOnEndAnim

    private int _CurrentSpriteListIndex = 0;
    private float _FrameClock = 0f;

    public enum CharacterOrientation
    {
        DOWN,
        LEFT,
        RIGHT,
        TOP
    }

    public void Awake()
    {
        _SpriteSheet = _CharaManager.InstantiateCharaSpriteSheet();
    }

    public void Start()
    {
        _SprRend.sprite = _SpriteSheet.GetDefaultSprite();
        HideWeapon();
        DisplayOwnedObject(null);
    }

    public void UpdateSpriteSheet(CharacterSpriteSheetData newSpriteSheet)
    {
        _SpriteSheet = newSpriteSheet;
        PlayIdleAnimation(_CharaManager.GetCharacterOrientation());
    }

    public void SetWeapon(WeaponData weapon)
    {
        _CurrentWeapon = weapon;
        _WeaponObject.sprite = _CurrentWeapon.Sprite;
        PolygonCollider2D collider = _WeaponObject.GetComponent<PolygonCollider2D>();
        if (collider)
        {
            Destroy(collider);
        }
        _WeaponObject.gameObject.AddComponent<PolygonCollider2D>().isTrigger = true;
    }

    public void Update()
    {
        if (_CurrentSpriteList == null)
            return ;

        _FrameClock += Time.deltaTime;

        if (_FrameClock >= _FrameDuration)
        {
            ++_CurrentSpriteListIndex;
            _FrameClock -= _FrameDuration;

            if (_CurrentSpriteListIndex >= _CurrentSpriteList.Count)
            {
                // if no loop
                // do stuff

                _CurrentSpriteListIndex = 0;
            }

            _SprRend.sprite = _CurrentSpriteList[_CurrentSpriteListIndex];
        }
    }

    public void PlayWalkAnimation(CharacterOrientation orientation)
    {
        PlayAnimation(_SpriteSheet.GetMoveSpriteListForOrientation(orientation), true);
    }

    public void PlayIdleAnimation(CharacterOrientation orientation)
    {
        List<Sprite> spriteList = new List<Sprite>();
        spriteList.Add(_SpriteSheet.GetIdleSpriteForOrientation(orientation));
        PlayAnimation(spriteList, true);
    }

    public void PlayAnimation(List<Sprite> spriteList, bool isLooping/*, callback*/)
    {
        _CurrentSpriteListIndex = 0;
        _CurrentSpriteList = spriteList;
        _IsLooping = isLooping;
        _FrameClock = 0;
        _SprRend.sprite = spriteList[0];
    }

    // Base Position is for CharacterOrientation down
    public void DisplayWeapon(Vector2 wpPosition, float wpRotation)
    {
        _WeaponObject.gameObject.SetActive(true);

        Vector2 orientedPosition = GetForwardWeaponLocalPosition();
        float orientedRotation = _CurrentWeapon.BaseRotation + wpRotation;
        wpPosition = CustomMathLib.RotateVector2(wpPosition, _CharaManager.GetCharacterOrientation());
        switch (_CharaManager.GetCharacterOrientation())
        {
            case CharacterOrientation.DOWN:
                break;
            case CharacterOrientation.LEFT:
                orientedRotation -= 90f;
                break;
            case CharacterOrientation.RIGHT:
                orientedRotation += 90f;
                break;
            case CharacterOrientation.TOP:
                orientedRotation += 180f;
                break;
        }
        orientedPosition += wpPosition;
        _WeaponObject.transform.localPosition =
            new Vector3(orientedPosition.x, orientedPosition.y, _WeaponObject.transform.localPosition.z);
        _WeaponObject.transform.localEulerAngles = new Vector3(0, 0, orientedRotation);
    }

    public void HideWeapon()
    {
        _WeaponObject.gameObject.gameObject.SetActive(false);
    }

    public void DisplayOwnedObject(Sprite spriteToDisplay)
    {
        if (spriteToDisplay)
        {
            _OwnedObjectSlot.gameObject.SetActive(true);
            _OwnedObjectSlot.sprite = spriteToDisplay;
        }
        else
        {
            _OwnedObjectSlot.gameObject.SetActive(false);
        }
    }

    public Vector2 GetForwardWeaponLocalPosition()
    {
        switch (_CharaManager.GetCharacterOrientation())
        {
            case CharacterOrientation.DOWN:
                return _WeaponPositionDown.localPosition;
            case CharacterOrientation.LEFT:
                return _WeaponPositionLeft.localPosition;
            case CharacterOrientation.RIGHT:
                return _WeaponPositionRight.localPosition;
            case CharacterOrientation.TOP:
                return _WeaponPositionUp.localPosition;
            default:
                return _WeaponPositionDown.localPosition;
        }
    }

    public Vector2 GetForwardWeaponWorldPosition()
    {
        switch (_CharaManager.GetCharacterOrientation())
        {
            case CharacterOrientation.DOWN:
                return _WeaponPositionDown.position;
            case CharacterOrientation.LEFT:
                return _WeaponPositionLeft.position;
            case CharacterOrientation.RIGHT:
                return _WeaponPositionRight.position;
            case CharacterOrientation.TOP:
                return _WeaponPositionUp.position;
            default:
                return _WeaponPositionDown.position;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CharacterSpriteAnimator : MonoBehaviour
{
    [Required] [SerializeField] private CharacterManager _CharaManager;

    [Required] [SerializeField] private SpriteRenderer _SprRend;
    [Required] [SerializeField] private SpriteRenderer _WeaponObject;

    [Foldout("Weapons Positions")]
    [Required][SerializeField] private Transform _WeaponPositionDown;
    [Foldout("Weapons Positions")]
    [Required][SerializeField] private Transform _WeaponPositionLeft;
    [Foldout("Weapons Positions")]
    [Required][SerializeField] private Transform _WeaponPositionRight;
    [Foldout("Weapons Positions")]
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
        _SpriteSheet = _CharaManager.GetCharaSpriteSheet();
    }

    public void Start()
    {
        _SprRend.sprite = _SpriteSheet.GetDefaultSprite();
        HideWeapon();
    }

    public void SetWeapon(WeaponData weapon)
    {
        _CurrentWeapon = weapon;
        _WeaponObject.sprite = _CurrentWeapon.Sprite;
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
        _WeaponObject.gameObject.gameObject.SetActive(true);

        Vector2 orientedPosition = new Vector2();
        float orientedRotation = _CurrentWeapon.BaseRotation + wpRotation;
        switch (_CharaManager.GetCharacterOrientation())
        {
            case CharacterOrientation.DOWN:
                orientedPosition = new Vector2(
                    _WeaponPositionDown.localPosition.x + wpPosition.x,
                    _WeaponPositionDown.localPosition.y + wpPosition.y);
                break;
            case CharacterOrientation.LEFT:
                orientedPosition = new Vector2(
                    _WeaponPositionLeft.localPosition.x + wpPosition.y,
                    _WeaponPositionLeft.localPosition.y + -wpPosition.x);
                orientedRotation -= 90f;
                break;
            case CharacterOrientation.RIGHT:
                orientedPosition = new Vector2(
                    _WeaponPositionRight.localPosition.x + -wpPosition.y,
                    _WeaponPositionRight.localPosition.y + wpPosition.x);
                orientedRotation += 90f;
                break;
            case CharacterOrientation.TOP:
                orientedPosition = new Vector2(
                    _WeaponPositionUp.localPosition.x + -wpPosition.x,
                    _WeaponPositionUp.localPosition.y + -wpPosition.y);
                orientedRotation += 180f;
                break;
        }
        _WeaponObject.transform.localPosition =
            new Vector3(orientedPosition.x, orientedPosition.y, _WeaponObject.transform.localPosition.z);
        _WeaponObject.transform.localEulerAngles = new Vector3(0, 0, orientedRotation);
    }

    public void HideWeapon()
    {
        _WeaponObject.gameObject.gameObject.SetActive(false);
    }
}

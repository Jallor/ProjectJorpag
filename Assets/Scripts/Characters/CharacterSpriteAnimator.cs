using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CharacterSpriteAnimator : MonoBehaviour
{
    [Required] [SerializeField] private CharacterManager _CharaManager;

    [Required] [SerializeField] private SpriteRenderer _SprRend;

    [Header("Animation Parameters")]
    [SerializeField] private float _FrameDuration = 0.5f;

    private CharacterSpriteSheetData _SpriteSheet;

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
}

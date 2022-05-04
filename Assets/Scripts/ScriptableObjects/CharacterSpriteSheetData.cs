using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

using static CharacterSpriteAnimator;

[CreateAssetMenu(fileName = "NewCharacterSpriteSheeet", menuName = "Data/Character SpriteSheet")]
public class CharacterSpriteSheetData : ScriptableObject
{
    [ShowAssetPreview]
    [SerializeField] private Sprite _DefaultSprite;

    [Header("On Map")]
    [SerializeField] private List<Sprite> _DownSpriteList = new List<Sprite>();
    [SerializeField] private List<Sprite> _LeftSpriteList = new List<Sprite>();
    [SerializeField] private List<Sprite> _RightSpriteList = new List<Sprite>();
    [SerializeField] private List<Sprite> _TopSpriteList = new List<Sprite>();

    public Sprite GetDefaultSprite() => _DefaultSprite;

    public Sprite GetIdleSpriteForOrientation(CharacterOrientation orientation)
    {
        return (orientation switch
        {
            CharacterOrientation.DOWN => _DownSpriteList[1],
            CharacterOrientation.LEFT => _LeftSpriteList[1],
            CharacterOrientation.RIGHT => _RightSpriteList[1],
            CharacterOrientation.TOP => _TopSpriteList[1],
            _ => _DefaultSprite,
        });
    }

    public List<Sprite> GetMoveSpriteListForOrientation(CharacterOrientation orientation)
    {
        List<Sprite> spriteList = new List<Sprite>(orientation switch
        {
            CharacterOrientation.DOWN => _DownSpriteList,
            CharacterOrientation.LEFT => _LeftSpriteList,
            CharacterOrientation.RIGHT => _RightSpriteList,
            CharacterOrientation.TOP => _TopSpriteList,
            _ => null,
        });

        spriteList.Add(GetIdleSpriteForOrientation(orientation));

        return (spriteList);
    }
}

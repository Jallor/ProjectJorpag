using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewCharacterSpriteSheeet", menuName = "Data/Character SpriteSheet")]
public class CharacterSpriteSheetData : ScriptableObject
{
    [ShowAssetPreview]
    [SerializeField] private Sprite _DefaultSprite;

    [Header("On Map")]
    [SerializeField] private List<Sprite> _DownSpriteList = new List<Sprite>(3);
    [SerializeField] private List<Sprite> _LeftSpriteList = new List<Sprite>(3);
    [SerializeField] private List<Sprite> _RightSpriteList = new List<Sprite>(3);
    [SerializeField] private List<Sprite> _TopSpriteList = new List<Sprite>(3);
}

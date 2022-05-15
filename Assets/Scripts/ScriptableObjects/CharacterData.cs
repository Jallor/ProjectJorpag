using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Data/Character Data")]
public class CharacterData : ScriptableObject
{
    public string CharaName = "";
    [ShowAssetPreview]
    [Required] public Sprite FaceSprite;
    [Required] public CharacterSpriteSheetData SpriteSheet;

    public int MaxHP = 100;

    [TextArea(3, 15)]
    public string CharacterQuickDescription;
}

using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Data/Character Data")]
public class CharacterData : ScriptableObject
{
    [ShowAssetPreview]
    [Required] public Sprite FaceSprite;
    [Required] public CharacterSpriteSheetData SpriteSheet;
}

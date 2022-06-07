using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Data/Character Data")]
public class CharacterData : ScriptableObject
{
    public string CharaName = "";
    [ShowAssetPreview]
    [Required] public Sprite FaceSprite;
    [Required] public CharacterSpriteSheetData SpriteSheet;

    public WeaponData DefaultWeapon;
    // TODO to replace
    public SkillData TMP_SkillData;

    [Header("Stats")]
    public int MaxHP = 100;
    public float MovementSpeed = 5000;

    [TextArea(3, 15)]
    public string CharacterQuickDescription;
}

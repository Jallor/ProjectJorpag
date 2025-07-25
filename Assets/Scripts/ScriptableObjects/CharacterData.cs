using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Data/Character")]
public class CharacterData : ScriptableObject, IWorldEntityData
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

    [Header("Inventory")]
    public int MaxInventorySlot = 10;
    public bool DisplayFirstOwningObject = false;

    [TextArea(3, 15)]
    public string CharacterQuickDescription;
}

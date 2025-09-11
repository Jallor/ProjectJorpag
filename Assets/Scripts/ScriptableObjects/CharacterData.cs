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
    [SerializeField] private int _MaxInventorySlot = 10;
    [SerializeField] private bool _DisplayFirstOwningObject = false;
    public int MaxInventorySlot { get => _MaxInventorySlot; set => _MaxInventorySlot = value; }
    public bool DisplayFirstOwningObject { get => _DisplayFirstOwningObject; set => _DisplayFirstOwningObject = value; }

    [Header("Misc")]
    [TextArea(3, 15)]
    public string CharacterQuickDescription;

}

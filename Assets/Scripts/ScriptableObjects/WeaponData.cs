using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Data/Weapon")]
public class WeaponData : ScriptableObject
{
    public string WeaponName;
    [ShowAssetPreview]
    public Sprite Sprite = null;
    public GameObject WeaponObject = null;
    [Tooltip("Rotation to fit with orientation DOWN")]
    public float BaseRotation = 135f;
}

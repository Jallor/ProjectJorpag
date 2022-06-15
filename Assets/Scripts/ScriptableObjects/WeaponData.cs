using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Data/Weapon")]
public class WeaponData : ScriptableObject
{
    [ShowAssetPreview]
    public Sprite Sprite = null;
    [Tooltip("Rotation to fit with orientation DOWN")]
    public float BaseRotation = 135f;
}

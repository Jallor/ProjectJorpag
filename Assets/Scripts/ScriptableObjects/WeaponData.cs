using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public Sprite Sprite = null;
    [Tooltip("Rotation to fit with orientation DOWN")]
    public float BaseRotation = 135f;
}

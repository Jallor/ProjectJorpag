using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement), typeof(CharacterSpriteAnimator))]
public class PlayerController : MonoBehaviour
{
    [Required] [SerializeField] private CharacterMovement _CharaMovement;

    public void Update()
    {
        // TODO : PlayerController : Use axis
        Vector2 newDir = new Vector2();
        if (Input.GetKey(KeyCode.Z))
        {
            ++newDir.y;
        }
        if (Input.GetKey(KeyCode.S))
        {
            --newDir.y;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            --newDir.x;
        }
        if (Input.GetKey(KeyCode.D))
        {
            ++newDir.x;
        }

        _CharaMovement.GiveInput(newDir);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovementCharacterController : CharacterController
{
    public override ECharacterControllerType GetCharacterControllerType()
        => ECharacterControllerType.RANDOM_MOVEMENT;

    public void Update()
    {
        
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using static CharacterSpriteAnimator;

public static class CustomMathLib
{
    public static Vector2 RotateVector2(Vector2 vec, CharacterOrientation newDir)
    {
        switch (newDir)
        {
            case CharacterOrientation.DOWN:
                return (vec);
            case CharacterOrientation.LEFT:
                return (new Vector2(vec.y, -vec.x));
            case CharacterOrientation.RIGHT:
                return (new Vector2(-vec.y, vec.x));
            case CharacterOrientation.TOP:
                return (new Vector2(-vec.x, -vec.y));
            default:
                return (vec);
        }
    }

}

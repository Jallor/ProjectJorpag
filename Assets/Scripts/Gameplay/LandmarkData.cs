using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LandmarkData
{
    public Vector2Int Position = Vector2Int.zero;
    public ELandmarkType Type = ELandmarkType.SimpleLandmark;
}

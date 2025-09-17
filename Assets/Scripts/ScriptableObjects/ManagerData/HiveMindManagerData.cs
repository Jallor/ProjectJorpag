using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHiveMindManagerData", menuName = "Data/HiveMind Manager")]
public class HiveMindManagerData : SpawnableManagerData
{
    [Header("HiveMind")]
    public HiveMindBehaviorData BehaviorData = null;
}

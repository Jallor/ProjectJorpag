using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillData", menuName = "Data/Skill Data")]
public class SkillData : ScriptableObject
{
    public bool AllowMovement = false;
    public bool AllowRotation = false;

    [SelectImplementation] [SerializeReference]
    public List<SkillAnimationPart> AnimationsList = new List<SkillAnimationPart>();
}

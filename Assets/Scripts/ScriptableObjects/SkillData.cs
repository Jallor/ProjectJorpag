using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillData", menuName = "Data/Skill Data")]
public class SkillData : ScriptableObject
{
    public string SkillName = "[Default Skill Name]";
    public bool AllowMovement = false;
    public bool AllowRotation = false;

    [SerializeReference]
    public List<ConditionnalEffect> GameEffectList = new List<ConditionnalEffect>();

    [SerializeReference]
    public List<ConditionnalEffect> EffectOnWeaponHit = new List<ConditionnalEffect>();

    [SelectImplementation] [SerializeReference]
    public List<SkillAnimationPart> AnimationsList = new List<SkillAnimationPart>();
}

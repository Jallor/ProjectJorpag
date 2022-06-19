using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Timeline.Samples;
using System.ComponentModel;

[System.Serializable]
[DisplayName("Project JORPAG/Character Controller")]
public class TimelineCharacterControllerPlayableAsset : PlayableAsset, ITimelineClipAsset
{
    [NoFoldOut]
    public TimelineCharacterControllerBehaviour template = new TimelineCharacterControllerBehaviour();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Extrapolation | ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<TimelineCharacterControllerBehaviour>.Create(graph, template);
    }
}

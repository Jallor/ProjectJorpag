using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.9f, 0.1f, 0.1f)]
[TrackBindingType(typeof(CharacterManager))]
[TrackClipType(typeof(TimelineCharacterControllerPlayableAsset))]
[DisplayName("Project JORPAG/Character Controller")]
public class TimelineCharacterControllerTrack : TrackAsset
{
}

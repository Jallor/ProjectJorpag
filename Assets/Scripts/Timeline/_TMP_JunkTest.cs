using UnityEngine;
using UnityEngine.Playables;


public class _TMP_JunkTest : MonoBehaviour
{
    public PlayableDirector Director;

    public void SetFrame(float newFrame)
    {
        Director.time = newFrame;
    }

    public void SetTimeUpdateMode(DirectorUpdateMode updateMode)
    {
        Director.timeUpdateMode = updateMode;
    }
}

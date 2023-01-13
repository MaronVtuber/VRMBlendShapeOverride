using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class VrmBlendShapeClip : PlayableAsset, ITimelineClipAsset
{
    public VrmBlendShapeBehaviour behaviour = new VrmBlendShapeBehaviour();

    public ClipCaps clipCaps
    {
        get
        {
            return ClipCaps.Blending | ClipCaps.SpeedMultiplier;
        }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<VrmBlendShapeBehaviour>.Create(graph);
    }
}
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using VRM;

[TrackColor(0.5f, 0.7f, 0.5f)]
[TrackBindingType(typeof(VRMBlendShapeProxy))]
[TrackClipType(typeof(VrmBlendShapeClip))]
public class VrmBlendShapeTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var mixer = ScriptPlayable<VrmBlendShapeMixerBehaviour>.Create(graph, inputCount);
        mixer.GetBehaviour().Clips = GetClips().ToArray();
        mixer.GetBehaviour().Director = go.GetComponent<PlayableDirector>();

        // ñºëOïœçX
        foreach (TimelineClip clip in m_Clips)
        {
            var playableAsset = clip.asset as VrmBlendShapeClip;
            clip.displayName = playableAsset.behaviour.blendShapePreset.ToString();
        }
        return mixer;
    }
}
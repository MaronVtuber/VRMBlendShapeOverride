using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;
using VRM;

public class VrmBlendShapeMixerBehaviour : PlayableBehaviour
{

    private static Dictionary<int, BlendShapeMerger> _mergers = new Dictionary<int, BlendShapeMerger>();
    private static void ResetMergerInstance()
    {
        _mergers = new Dictionary<int, BlendShapeMerger>();
    }

    private static BlendShapeMerger GetEditModeMergerInstance(VRMBlendShapeProxy proxy)
    {
        var go = proxy.gameObject;
        var id = go.GetInstanceID();
        if (!_mergers.ContainsKey(id))
        {
            _mergers.Add(id, new BlendShapeMerger(proxy.BlendShapeAvatar.Clips,go.transform));
        }

        return _mergers[id];

    }
    
    public TimelineClip[] Clips { get; set; }
    public PlayableDirector Director { get; set; }

    public override void OnGraphStart( Playable playable ) {

        ResetMergerInstance();
        base.OnGraphStart( playable );
    }
    
    
    
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var proxy = playerData as VRMBlendShapeProxy;
        if (proxy == null)
        {
            return;
        }

        var time = Director.time;

        // 「あいうえお」
        var value_A = 0f;
        var value_I = 0f;
        var value_U = 0f;
        var value_E = 0f;
        var value_O = 0f;

        // 「喜怒哀楽」
        var value_Angry = 0f;
        var value_Blink = 0f;
        var value_Blink_L = 0f;
        var value_Blink_R = 0f;
        var value_Fun = 0f;
        var value_Joy = 0f;
        var value_Sorrow = 0f;
        var value_Netural = 0f;
        var isLipSync = false;
        var isFacial = false;


        for (int i = 0; i < Clips.Length; i++)
        {
            var clip = Clips[i];
            var clipAsset = clip.asset as VrmBlendShapeClip;
            var behaviour = clipAsset.behaviour;
            var clipWeight = playable.GetInputWeight(i);
            var clipProgress = (float)((time - clip.start) / clip.duration);

            if (clipProgress >= 0.0f && clipProgress <= 1.0f)
            {
                switch (behaviour.blendShapePreset)
                {
                    // 「あいうえお」
                    case BlendShapePreset.A:
                        value_A += clipWeight * behaviour.blendShapeValue;
                        isLipSync = true;
                        break;
                    case BlendShapePreset.I:
                        value_I += clipWeight * behaviour.blendShapeValue;
                        isLipSync = true;
                        break;
                    case BlendShapePreset.U:
                        value_U += clipWeight * behaviour.blendShapeValue;
                        isLipSync = true;
                        break;
                    case BlendShapePreset.E:
                        value_E += clipWeight * behaviour.blendShapeValue;
                        isLipSync = true;
                        break;
                    case BlendShapePreset.O:
                        value_O += clipWeight * behaviour.blendShapeValue;
                        isLipSync = true;
                        break;
                    case BlendShapePreset.Unknown:
                        isLipSync = true;
                        break;

                    // 「喜怒哀楽」
                    case BlendShapePreset.Angry:
                        value_Angry += clipWeight * behaviour.blendShapeValue;
                        isFacial = true;
                        break;
                    case BlendShapePreset.Blink:
                        value_Blink += clipWeight * behaviour.blendShapeValue;
                        isFacial = true;
                        break;
                    case BlendShapePreset.Blink_L:
                        value_Blink_L += clipWeight * behaviour.blendShapeValue;
                        isFacial = true;
                        break;
                    case BlendShapePreset.Blink_R:
                        value_Blink_R += clipWeight * behaviour.blendShapeValue;
                        isFacial = true;
                        break;
                    case BlendShapePreset.Fun:
                        value_Fun += clipWeight * behaviour.blendShapeValue;
                        isFacial = true;
                        break;
                    case BlendShapePreset.Joy:
                        value_Joy += clipWeight * behaviour.blendShapeValue;
                        isFacial = true;
                        break;
                    case BlendShapePreset.Sorrow:
                        value_Sorrow += clipWeight * behaviour.blendShapeValue;
                        isFacial = true;
                        break;
                    case BlendShapePreset.Neutral:
                        value_Netural += clipWeight * behaviour.blendShapeValue;
                        isFacial = true;
                        break;
                }
            }
        }

        var isEditMode = !Application.isPlaying;
        BlendShapeMerger merger = null;

        if (isEditMode) merger = GetEditModeMergerInstance(proxy);

        // 「あいうえお」と「喜怒哀楽」どちらを操作するか判断して適用する
        if (isLipSync)
        {
            if(isEditMode){
                merger.SetValues(new Dictionary<BlendShapeKey, float>
                {
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.A), value_A},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.I), value_I},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.U), value_U},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.E), value_E},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.O), value_O},
                });
            }
            else
            {
                proxy.SetValues(new Dictionary<BlendShapeKey, float>
                {
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.A), value_A},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.I), value_I},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.U), value_U},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.E), value_E},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.O), value_O},
                });
            }
        }
        else if(isFacial)
        {
            if (isEditMode)
            {
                merger.SetValues(new Dictionary<BlendShapeKey, float>
                {
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Angry), value_Angry},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink), value_Blink},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink_L), value_Blink_L},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink_R), value_Blink_R},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Fun), value_Fun},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Joy), value_Joy},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Sorrow), value_Sorrow},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Neutral), value_Netural},
                });
            }
            else
            {
                proxy.SetValues(new Dictionary<BlendShapeKey, float>
                {
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Angry), value_Angry},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink), value_Blink},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink_L), value_Blink_L},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Blink_R), value_Blink_R},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Fun), value_Fun},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Joy), value_Joy},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Sorrow), value_Sorrow},
                    {BlendShapeKey.CreateFromPreset(BlendShapePreset.Neutral), value_Netural},
                });
            }
        }
    }
}
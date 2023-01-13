using UnityEngine;
using UnityEngine.Playables;
using VRM;

[System.Serializable]
public class VrmBlendShapeBehaviour : PlayableBehaviour
{
    public BlendShapePreset blendShapePreset = BlendShapePreset.A;
    public float blendShapeValue = 1f;
}

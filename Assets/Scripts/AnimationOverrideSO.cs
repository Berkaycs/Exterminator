using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationOverrideSO", menuName = "SO/AnimationOverride")]
public class AnimationOverrideSO : ScriptableObject
{
    public ClipTransition DefaultIdle;
    public ClipTransition DefaultAttack;
}

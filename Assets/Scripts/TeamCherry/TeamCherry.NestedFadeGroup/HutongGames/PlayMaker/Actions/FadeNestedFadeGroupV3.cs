using TeamCherry.NestedFadeGroup;
using TeamCherry.SharedUtils;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

public class FadeNestedFadeGroupV3 : FsmStateAction
{
	public FsmOwnerDefault Target;

	public FsmFloat ToAlpha;

	public FsmFloat FadeTime;

	public FsmAnimationCurve Curve;

	public FsmBool Realtime;

	public override void Reset()
	{
		Target = null;
		ToAlpha = null;
		FadeTime = null;
		Curve = new FsmAnimationCurve
		{
			curve = AnimationCurve.Linear(0f, 0f, 1f, 1f)
		};
		Realtime = null;
	}

	public override void OnEnter()
	{
		GameObject safe = Target.GetSafe(this);
		if (Object.op_Implicit((Object)(object)safe))
		{
			NestedFadeGroupBase component = safe.GetComponent<NestedFadeGroup>();
			if (!Object.op_Implicit((Object)(object)component))
			{
				component = safe.GetComponent<NestedFadeGroupBase>();
			}
			if (Object.op_Implicit((Object)(object)component))
			{
				component.FadeTo(ToAlpha.Value, FadeTime.Value, Curve.curve, Realtime.Value);
			}
		}
		Finish();
	}
}

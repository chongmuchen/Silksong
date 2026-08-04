#if PLAYMAKER
using TeamCherry.NestedFadeGroup;
using TeamCherry.SharedUtils;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

public class FadeNestedFadeGroupV2 : FsmStateAction
{
	public FsmOwnerDefault Target;

	public FsmFloat ToAlpha;

	public FsmFloat FadeTime;

	public FsmAnimationCurve Curve;

	public override void Reset()
	{
		Target = null;
		ToAlpha = null;
		FadeTime = null;
		Curve = new FsmAnimationCurve
		{
			curve = AnimationCurve.Linear(0f, 0f, 1f, 1f)
		};
	}

	public override void OnEnter()
	{
		GameObject safe = Target.GetSafe(this);
		if ((safe != null))
		{
			NestedFadeGroupBase component = safe.GetComponent<NestedFadeGroup>();
			if (!(component != null))
			{
				component = safe.GetComponent<NestedFadeGroupBase>();
			}
			if ((component != null))
			{
				component.FadeTo(ToAlpha.Value, FadeTime.Value, Curve.curve);
			}
		}
		Finish();
	}
}
}

#endif

using TeamCherry.NestedFadeGroup;
using TeamCherry.SharedUtils;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

public class FadeNestedFadeGroup : FsmStateAction
{
	public FsmOwnerDefault Target;

	public FsmFloat ToAlpha;

	public FsmFloat FadeTime;

	public override void Reset()
	{
		Target = null;
		ToAlpha = null;
		FadeTime = null;
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
				component.FadeTo(ToAlpha.Value, FadeTime.Value);
			}
		}
		Finish();
	}
}

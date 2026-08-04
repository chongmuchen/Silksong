using System;
using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup;

[ExecuteAlways]
[RequireComponent(typeof(TintRendererGroup))]
[NestedFadeGroupBridge(new Type[] { typeof(TintRendererGroup) })]
public class NestedFadeGroupTintRendererGroup : NestedFadeGroupBase
{
	private TintRendererGroup target;

	protected override void GetMissingReferences()
	{
		if (!Object.op_Implicit((Object)(object)target))
		{
			target = ((Component)this).GetComponent<TintRendererGroup>();
		}
	}

	protected override void OnComponentAdded()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)target))
		{
			base.AlphaSelf = target.Color.a;
		}
	}

	protected override void OnAlphaChanged(float alpha)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		Color color = target.Color;
		color.a = alpha;
		target.Color = color;
	}
}

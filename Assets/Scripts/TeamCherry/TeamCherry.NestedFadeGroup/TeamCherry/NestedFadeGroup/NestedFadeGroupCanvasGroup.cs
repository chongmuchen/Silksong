using System;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup
{

[ExecuteAlways]
[NestedFadeGroupBridge(new Type[]
{
	typeof(CanvasGroup),
	typeof(Canvas)
})]
[RequireComponent(typeof(CanvasGroup))]
public class NestedFadeGroupCanvasGroup : NestedFadeGroupBase
{
	private CanvasGroup canvasGroup;

	protected override void GetMissingReferences()
	{
		if (!(canvasGroup != null))
		{
			canvasGroup = ((Component)this).GetComponent<CanvasGroup>();
		}
	}

	protected override void OnAlphaChanged(float alpha)
	{
		canvasGroup.alpha = alpha;
	}
}
}

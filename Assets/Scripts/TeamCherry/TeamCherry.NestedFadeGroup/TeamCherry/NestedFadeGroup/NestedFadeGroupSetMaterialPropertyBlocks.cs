using System;
using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup;

[ExecuteAlways]
[RequireComponent(typeof(SetMaterialPropertyBlocks))]
[NestedFadeGroupBridge(new Type[] { typeof(SetMaterialPropertyBlocks) })]
public class NestedFadeGroupSetMaterialPropertyBlocks : NestedFadeGroupBase
{
	[Space]
	[SerializeField]
	private bool affectFloats;

	private SetMaterialPropertyBlocks setMatBlocks;

	protected override void GetMissingReferences()
	{
		if ((Object)(object)setMatBlocks == (Object)null)
		{
			setMatBlocks = ((Component)this).GetComponent<SetMaterialPropertyBlocks>();
		}
	}

	protected override void OnAlphaChanged(float alpha)
	{
		setMatBlocks.ColorAlpha = alpha;
		if (affectFloats)
		{
			setMatBlocks.FloatAlpha = alpha;
		}
	}
}

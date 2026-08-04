using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup;

[ExecuteAlways]
public class NestedFadeGroupRetarget : NestedFadeGroupBase
{
	[SerializeField]
	private NestedFadeGroupBase target;

	[SerializeField]
	private MinMaxFloat range = new MinMaxFloat(0f, 1f);

	public NestedFadeGroupBase Target
	{
		get
		{
			return target;
		}
		set
		{
			target = value;
			RefreshAlpha(forced: true);
		}
	}

	private void OnValidate()
	{
		if (range.Start < 0f)
		{
			range.Start = 0f;
		}
		else if (range.Start > 1f)
		{
			range.Start = 1f;
		}
		if (range.End < 0f)
		{
			range.End = 0f;
		}
		else if (range.End > 1f)
		{
			range.End = 1f;
		}
	}

	protected override void OnAlphaChanged(float alpha)
	{
		if (Object.op_Implicit((Object)(object)target))
		{
			target.AlphaSelf = range.GetLerpedValue(alpha);
		}
	}
}

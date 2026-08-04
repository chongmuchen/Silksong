using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup;

public class NestedFadeGroupRamp : RampBase
{
	[Space]
	[SerializeField]
	private float alpha;

	[SerializeField]
	private NestedFadeGroupBase group;

	private float? startAlpha;

	public float Alpha
	{
		get
		{
			return alpha;
		}
		set
		{
			alpha = value;
		}
	}

	private void Awake()
	{
		if (!Object.op_Implicit((Object)(object)group))
		{
			group = ((Component)this).GetComponent<NestedFadeGroupBase>();
		}
	}

	protected override void ResetValues()
	{
		if (Object.op_Implicit((Object)(object)group))
		{
			if (!startAlpha.HasValue)
			{
				startAlpha = group.AlphaSelf;
			}
			else
			{
				group.AlphaSelf = startAlpha.Value;
			}
		}
	}

	protected override void UpdateValues(float multiplier)
	{
		if (Object.op_Implicit((Object)(object)group))
		{
			group.AlphaSelf = alpha * multiplier;
		}
	}
}

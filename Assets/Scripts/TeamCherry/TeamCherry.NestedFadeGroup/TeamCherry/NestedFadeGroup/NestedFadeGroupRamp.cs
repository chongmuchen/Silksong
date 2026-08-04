using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup
{

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
		if (!(group != null))
		{
			group = ((Component)this).GetComponent<NestedFadeGroupBase>();
		}
	}

	protected override void ResetValues()
	{
		if ((group != null))
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
		if ((group != null))
		{
			group.AlphaSelf = alpha * multiplier;
		}
	}
}
}

using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup;

public class NestedFadeGroupCurveAnimator : FloatCurveAnimator
{
	[SerializeField]
	private NestedFadeGroupBase group;

	public NestedFadeGroupBase Group => group;

	protected override float Value
	{
		get
		{
			if (!Object.op_Implicit((Object)(object)group))
			{
				return 0f;
			}
			return group.AlphaSelf;
		}
		set
		{
			if (Object.op_Implicit((Object)(object)group))
			{
				group.AlphaSelf = value;
			}
		}
	}

	private void Reset()
	{
		group = ((Component)this).GetComponent<NestedFadeGroupBase>();
	}
}

using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup
{

public class NestedFadeGroupCurveAnimator : FloatCurveAnimator
{
	[SerializeField]
	private NestedFadeGroupBase group;

	public NestedFadeGroupBase Group => group;

	protected override float Value
	{
		get
		{
			if (!(group != null))
			{
				return 0f;
			}
			return group.AlphaSelf;
		}
		set
		{
			if ((group != null))
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
}

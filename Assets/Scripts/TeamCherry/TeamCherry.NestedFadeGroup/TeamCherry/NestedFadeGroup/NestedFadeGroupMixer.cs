using System;
using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup
{

[ExecuteAlways]
public class NestedFadeGroupMixer : MonoBehaviour
{
	private enum MixMethods
	{
		Average,
		Max
	}

	[SerializeField]
	private NestedFadeGroupBase[] readFromGroups;

	[SerializeField]
	private MixMethods mixMethod;

	[SerializeField]
	private NestedFadeGroupBase applyToGroup;

	[SerializeField]
	private OverrideFloat selfAlpha;

	private float previousAlpha = -1f;

	public float SelfAlpha
	{
		get
		{
			return selfAlpha.Value;
		}
		set
		{
			selfAlpha.Value = value;
			ValidateSelfAlpha();
			UpdateAlpha();
		}
	}

	private void OnValidate()
	{
		ValidateSelfAlpha();
	}

	private void ValidateSelfAlpha()
	{
		if (selfAlpha.Value < 0f)
		{
			selfAlpha.Value = 0f;
		}
		else if (selfAlpha.Value > 1f)
		{
			selfAlpha.Value = 1f;
		}
	}

	private void OnEnable()
	{
		if ((applyToGroup != null))
		{
			previousAlpha = applyToGroup.AlphaSelf;
		}
	}

	private void LateUpdate()
	{
		UpdateAlpha();
	}

	private void UpdateAlpha()
	{
		float num = mixMethod switch
		{
			MixMethods.Average => MixAverage(), 
			MixMethods.Max => MixMax(), 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		if (Math.Abs(num - previousAlpha) > Mathf.Epsilon && (applyToGroup != null))
		{
			applyToGroup.AlphaSelf = num;
			previousAlpha = num;
		}
	}

	private float MixAverage()
	{
		float num = 0f;
		int num2 = 0;
		NestedFadeGroupBase[] array = readFromGroups;
		foreach (NestedFadeGroupBase nestedFadeGroupBase in array)
		{
			if ((nestedFadeGroupBase != null))
			{
				num += nestedFadeGroupBase.AlphaTotal;
				num2++;
			}
		}
		if (selfAlpha.IsEnabled)
		{
			num += selfAlpha.Value;
			num2++;
		}
		return num / (float)num2;
	}

	private float MixMax()
	{
		float num = 0f;
		NestedFadeGroupBase[] array = readFromGroups;
		foreach (NestedFadeGroupBase nestedFadeGroupBase in array)
		{
			if ((nestedFadeGroupBase != null))
			{
				float alphaTotal = nestedFadeGroupBase.AlphaTotal;
				if (alphaTotal > num)
				{
					num = alphaTotal;
				}
			}
		}
		if (selfAlpha.IsEnabled && selfAlpha.Value > num)
		{
			num = selfAlpha.Value;
		}
		return num;
	}
}
}

using System;
using UnityEngine;
using UnityEngine.Events;

namespace TeamCherry.NestedFadeGroup
{

[ExecuteAlways]
public class NestedFadeGroupFloatEvent : NestedFadeGroupBase
{
	[Serializable]
	private class UnityFloatEvent : UnityEvent<float>
	{
	}

	[SerializeField]
	private UnityFloatEvent onAlphaChanged;

	protected override void OnAlphaChanged(float alpha)
	{
		if (onAlphaChanged != null)
		{
			((UnityEvent<float>)onAlphaChanged).Invoke(alpha);
		}
	}
}
}

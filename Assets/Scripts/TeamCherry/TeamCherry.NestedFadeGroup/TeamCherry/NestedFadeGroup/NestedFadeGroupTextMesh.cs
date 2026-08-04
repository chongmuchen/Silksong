using System;
using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup;

[ExecuteAlways]
[RequireComponent(typeof(TextMesh))]
[NestedFadeGroupBridge(new Type[] { typeof(TextMesh) })]
public class NestedFadeGroupTextMesh : NestedFadeGroupBase
{
	private TextMesh textMesh;

	protected override void GetMissingReferences()
	{
		if (!Object.op_Implicit((Object)(object)textMesh))
		{
			textMesh = ((Component)this).GetComponent<TextMesh>();
		}
	}

	protected override void OnComponentAdded()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)textMesh))
		{
			base.AlphaSelf = textMesh.color.a;
		}
	}

	protected override void OnAlphaChanged(float alpha)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		TextMesh obj = textMesh;
		Color color = textMesh.color;
		float? a = alpha;
		obj.color = color.Where(null, null, null, a);
	}

	protected override void OnLateUpdate()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		base.OnLateUpdate();
		float alphaTotal = base.AlphaTotal;
		Color color = textMesh.color;
		if (!(Math.Abs(color.a - alphaTotal) <= Mathf.Epsilon))
		{
			color.a = alphaTotal;
			textMesh.color = color;
		}
	}
}

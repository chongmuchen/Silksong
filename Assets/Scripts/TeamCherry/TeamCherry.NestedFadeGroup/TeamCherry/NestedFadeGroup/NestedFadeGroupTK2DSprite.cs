using System;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup
{

[ExecuteAlways]
[RequireComponent(typeof(tk2dSprite))]
[NestedFadeGroupBridge(new Type[] { typeof(tk2dSprite) })]
public class NestedFadeGroupTK2DSprite : NestedFadeGroupBase
{
	public enum DisplayType
	{
		Alpha,
		Frames
	}

	[SerializeField]
	private DisplayType displayType;

	[SerializeField]
	private string clipName;

	private tk2dSprite sprite;

	private tk2dSpriteAnimator animator;

	private MeshRenderer meshRenderer;

	public Color Color
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return sprite.color;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			base.AlphaSelf = value.a;
			if ((sprite != null) && displayType == DisplayType.Alpha)
			{
				sprite.color = new Color(value.r, value.g, value.b, base.AlphaTotal);
			}
		}
	}

	protected override void GetMissingReferences()
	{
		if (!(sprite != null))
		{
			sprite = ((Component)this).GetComponent<tk2dSprite>();
		}
		if (!(animator != null) && displayType == DisplayType.Frames)
		{
			animator = ((Component)this).GetComponent<tk2dSpriteAnimator>();
		}
		if (!(meshRenderer != null))
		{
			meshRenderer = ((Component)this).GetComponent<MeshRenderer>();
		}
	}

	protected override void OnComponentAdded()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if ((sprite != null))
		{
			base.AlphaSelf = sprite.color.a;
		}
	}

	protected override void OnAlphaChanged(float alpha)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		switch (displayType)
		{
		case DisplayType.Alpha:
		{
			Color color = sprite.color;
			color.a = alpha;
			sprite.color = color;
			break;
		}
		case DisplayType.Frames:
		{
			if (!(animator != null))
			{
				break;
			}
			((Behaviour)animator).enabled = false;
			tk2dSpriteAnimationClip clipByName = animator.GetClipByName(clipName);
			if (clipByName == null)
			{
				break;
			}
			int num = Mathf.CeilToInt(alpha * (float)clipByName.frames.Length) - 1;
			((Renderer)meshRenderer).enabled = num >= 0;
			if (num >= 0)
			{
				tk2dSpriteAnimationFrame frame = clipByName.GetFrame(num);
				if (frame != null)
				{
					sprite.SetSprite(frame.spriteCollection, frame.spriteId);
				}
			}
			break;
		}
		}
	}

	protected override void OnLateUpdate()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		base.OnLateUpdate();
		if (displayType == DisplayType.Alpha)
		{
			float alphaTotal = base.AlphaTotal;
			Color color = sprite.color;
			if (Math.Abs(color.a - alphaTotal) > Mathf.Epsilon)
			{
				color.a = alphaTotal;
				sprite.color = color;
			}
		}
	}
}
}

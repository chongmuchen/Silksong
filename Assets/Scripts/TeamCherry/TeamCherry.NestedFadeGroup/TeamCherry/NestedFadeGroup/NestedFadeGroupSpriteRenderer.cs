using System;
using TeamCherry.SharedUtils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TeamCherry.NestedFadeGroup
{

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
[NestedFadeGroupBridge(new Type[] { typeof(SpriteRenderer) })]
public class NestedFadeGroupSpriteRenderer : NestedFadeGroupBase
{
	public enum DisplayType
	{
		Alpha,
		Frames
	}

	[SerializeField]
	private DisplayType displayType;

	[SerializeField]
	private Sprite[] frames;

	private SpriteRenderer spriteRenderer;

	public Color Color
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			GetMissingReferences();
			return spriteRenderer.color;
		}
		set
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			GetMissingReferences();
			base.AlphaSelf = value.a;
			BaseColor = value;
		}
	}

	public Color BaseColor
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			GetMissingReferences();
			Color color = spriteRenderer.color;
			float? a = 1f;
			return color.Where(null, null, null, a);
		}
		set
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if ((spriteRenderer != null))
			{
				spriteRenderer.color = new Color(value.r, value.g, value.b, (displayType == DisplayType.Alpha) ? base.AlphaTotal : 1f);
			}
		}
	}

	public Sprite Sprite
	{
		get
		{
			GetMissingReferences();
			return spriteRenderer.sprite;
		}
		set
		{
			GetMissingReferences();
			spriteRenderer.sprite = value;
		}
	}

	protected override void GetMissingReferences()
	{
		if (!(spriteRenderer != null))
		{
			spriteRenderer = ((Component)this).GetComponent<SpriteRenderer>();
			if (!(spriteRenderer != null) && Application.isPlaying)
			{
				Object.Destroy((Object)(object)this);
			}
		}
	}

	protected override void OnComponentAdded()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if ((spriteRenderer != null))
		{
			base.AlphaSelf = spriteRenderer.color.a;
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
			Color color = spriteRenderer.color;
			color.a = alpha;
			spriteRenderer.color = color;
			((Renderer)spriteRenderer).enabled = alpha > Mathf.Epsilon;
			break;
		}
		case DisplayType.Frames:
		{
			int num = Mathf.CeilToInt(alpha * (float)frames.Length) - 1;
			if (num < 0)
			{
				((Renderer)spriteRenderer).enabled = false;
				break;
			}
			if (num >= frames.Length)
			{
				num = frames.Length - 1;
			}
			((Renderer)spriteRenderer).enabled = true;
			spriteRenderer.sprite = frames[num];
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
			Color color = spriteRenderer.color;
			if (Math.Abs(color.a - alphaTotal) > 0.001f)
			{
				color.a = alphaTotal;
				spriteRenderer.color = color;
			}
		}
	}
}
}

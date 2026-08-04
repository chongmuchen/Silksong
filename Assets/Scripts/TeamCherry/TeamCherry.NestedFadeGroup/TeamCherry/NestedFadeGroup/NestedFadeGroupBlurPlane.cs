using UnityEngine;

namespace TeamCherry.NestedFadeGroup
{

[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer))]
public class NestedFadeGroupBlurPlane : NestedFadeGroupBase
{
	[SerializeField]
	private float maxBlurSpacing;

	[SerializeField]
	private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	private MaterialPropertyBlock block;

	private MeshRenderer renderer;

	private static readonly int Size = Shader.PropertyToID("_Size");

	protected override void GetMissingReferences()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (block == null)
		{
			block = new MaterialPropertyBlock();
		}
		if ((Object)(object)renderer == (Object)null)
		{
			renderer = ((Component)this).GetComponent<MeshRenderer>();
		}
	}

	protected override void OnAlphaChanged(float alpha)
	{
		if (alpha <= Mathf.Epsilon)
		{
			((Renderer)renderer).enabled = false;
			return;
		}
		((Renderer)renderer).enabled = true;
		((Renderer)renderer).GetPropertyBlock(block);
		block.SetFloat(Size, Mathf.Lerp(0f, maxBlurSpacing, curve.Evaluate(alpha)));
		((Renderer)renderer).SetPropertyBlock(block);
	}
}
}

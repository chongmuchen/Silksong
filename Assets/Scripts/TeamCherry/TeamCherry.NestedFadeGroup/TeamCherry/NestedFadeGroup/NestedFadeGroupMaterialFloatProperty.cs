using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup
{

[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class NestedFadeGroupMaterialFloatProperty : NestedFadeGroupBase
{
	[SerializeField]
	private string propertyName;

	[SerializeField]
	private MinMaxFloat range = new MinMaxFloat(0f, 1f);

	private string oldPropertyName;

	private int propertyId;

	private Renderer renderer;

	private MaterialPropertyBlock block;

	protected override void GetMissingReferences()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		if (!(renderer != null))
		{
			renderer = ((Component)this).GetComponent<Renderer>();
		}
		if (block == null)
		{
			block = new MaterialPropertyBlock();
		}
		if (propertyId == 0)
		{
			propertyId = Shader.PropertyToID(propertyName);
		}
	}

	protected override void OnAlphaChanged(float alpha)
	{
		float lerpedValue = range.GetLerpedValue(alpha);
		renderer.GetPropertyBlock(block);
		block.SetFloat(propertyId, lerpedValue);
		renderer.SetPropertyBlock(block);
	}
}
}

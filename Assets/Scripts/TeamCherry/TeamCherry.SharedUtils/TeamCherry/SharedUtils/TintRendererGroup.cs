using System.Collections.Generic;
using UnityEngine;

namespace TeamCherry.SharedUtils;

[ExecuteInEditMode]
public class TintRendererGroup : MonoBehaviour
{
	[SerializeField]
	private Color color = Color.white;

	private Color oldColor;

	private readonly List<SpriteRenderer> sprites = new List<SpriteRenderer>();

	private readonly List<IVertexColor> others = new List<IVertexColor>();

	private readonly List<ParticleSystem> particles = new List<ParticleSystem>();

	private readonly List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

	private MaterialPropertyBlock block;

	private static readonly int _tintColorProp = Shader.PropertyToID("_TintColor");

	public Color Color
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return color;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			color = value;
			Update();
		}
	}

	private void OnEnable()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		sprites.Clear();
		others.Clear();
		particles.Clear();
		meshRenderers.Clear();
		GetComponentsInChildrenRecursively(((Component)this).transform);
		if (meshRenderers.Count > 0)
		{
			block = new MaterialPropertyBlock();
		}
		UpdateTint();
		if (Application.isPlaying)
		{
			((Behaviour)this).enabled = false;
		}
	}

	private void GetComponentsInChildrenRecursively(Transform root)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		sprites.AddRange(((Component)root).gameObject.GetComponents<SpriteRenderer>());
		others.AddRange(((Component)root).gameObject.GetComponents<IVertexColor>());
		particles.AddRange(((Component)root).gameObject.GetComponents<ParticleSystem>());
		meshRenderers.AddRange(((Component)root).gameObject.GetComponents<MeshRenderer>());
		foreach (Transform item in root)
		{
			Transform val = item;
			if (!Object.op_Implicit((Object)(object)((Component)val).GetComponent<TintRendererGroup>()))
			{
				GetComponentsInChildrenRecursively(val);
			}
		}
	}

	private void Update()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (!(color == oldColor))
		{
			UpdateTint();
		}
	}

	private void UpdateTint()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		oldColor = color;
		foreach (SpriteRenderer sprite in sprites)
		{
			sprite.color = color;
		}
		foreach (IVertexColor other in others)
		{
			other.VertexColor = color;
		}
		foreach (ParticleSystem particle in particles)
		{
			MainModule main = particle.main;
			((MainModule)(ref main)).startColor = MinMaxGradient.op_Implicit(color);
		}
		foreach (MeshRenderer meshRenderer in meshRenderers)
		{
			if (((Renderer)meshRenderer).sharedMaterial.HasProperty(_tintColorProp))
			{
				int tintColorProp = _tintColorProp;
				block.Clear();
				((Renderer)meshRenderer).GetPropertyBlock(block);
				block.SetColor(tintColorProp, color);
				((Renderer)meshRenderer).SetPropertyBlock(block);
			}
		}
	}
}

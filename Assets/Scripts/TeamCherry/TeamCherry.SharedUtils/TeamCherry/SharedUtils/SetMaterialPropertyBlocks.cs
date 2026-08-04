using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TeamCherry.SharedUtils
{

public class SetMaterialPropertyBlocks : MonoBehaviour, IVertexColor
{
	[Serializable]
	private class PropertyModifier<T>
	{
		public string PropertyName;

		public T Value;

		public int PropertyId { get; private set; }

		public void Init()
		{
			PropertyId = Shader.PropertyToID(PropertyName);
		}
	}

	[Serializable]
	private class FloatModifier : PropertyModifier<float>
	{
	}

	[Serializable]
	private class ColorModifier : PropertyModifier<Color>
	{
	}

	[Serializable]
	private class VectorModifier : PropertyModifier<Vector4>
	{
	}

	[SerializeField]
	private Renderer[] renderers;

	[SerializeField]
	private bool getChildren;

	[Space]
	[SerializeField]
	private List<FloatModifier> floats;

	[SerializeField]
	private List<VectorModifier> vectors;

	[SerializeField]
	private List<ColorModifier> colors;

	[Space]
	[SerializeField]
	[Range(0f, 1f)]
	private float floatAlpha = 1f;

	[SerializeField]
	[Range(0f, 1f)]
	private float colorAlpha = 1f;

	private MaterialPropertyBlock block;

	private Color tintColor = Color.white;

	public Color VertexColor
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return tintColor;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			tintColor = value;
			UpdateRenderers();
		}
	}

	public float FloatAlpha
	{
		get
		{
			return floatAlpha;
		}
		set
		{
			floatAlpha = value;
			UpdateRenderers();
		}
	}

	public float ColorAlpha
	{
		get
		{
			return colorAlpha;
		}
		set
		{
			colorAlpha = value;
			UpdateRenderers();
		}
	}

	GameObject IVertexColor.gameObject => ((Component)this).gameObject;

	private void OnValidate()
	{
		if (getChildren && !Application.isPlaying)
		{
			GetChildren();
		}
		UpdateRenderers();
	}

	private void Awake()
	{
		if (getChildren)
		{
			GetChildren();
		}
		UpdateRenderers();
	}

	private void GetChildren()
	{
		renderers = ((Component)this).GetComponentsInChildren<Renderer>(true);
	}

	private void UpdateRenderers()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		if (block == null)
		{
			block = new MaterialPropertyBlock();
		}
		foreach (FloatModifier @float in floats)
		{
			@float.Init();
		}
		foreach (VectorModifier vector in vectors)
		{
			vector.Init();
		}
		foreach (ColorModifier color in colors)
		{
			color.Init();
		}
		Renderer[] array = renderers;
		foreach (Renderer val in array)
		{
			if (val == null)
			{
				continue;
			}
			block.Clear();
			val.GetPropertyBlock(block);
			foreach (FloatModifier float2 in floats)
			{
				block.SetFloat(float2.PropertyId, float2.Value * floatAlpha);
			}
			foreach (VectorModifier vector2 in vectors)
			{
				block.SetVector(vector2.PropertyId, vector2.Value);
			}
			foreach (ColorModifier color2 in colors)
			{
				Color value = color2.Value;
				value.a *= colorAlpha;
				block.SetColor(color2.PropertyId, value * tintColor);
			}
			val.SetPropertyBlock(block);
		}
	}

	public void SetFloatModifier(string propertyName, float value)
	{
		SetPropertyModifier(floats, propertyName, value);
	}

	private void SetPropertyModifier<TModifier, TValue>(List<TModifier> propertyList, string propertyName, TValue value) where TModifier : PropertyModifier<TValue>, new()
	{
		TModifier val = propertyList.Find((TModifier modifier) => modifier.PropertyName == propertyName);
		if (val != null)
		{
			val.Value = value;
		}
		else
		{
			propertyList.Add(new TModifier
			{
				PropertyName = propertyName,
				Value = value
			});
		}
		UpdateRenderers();
	}
}
}

using System;
using UnityEngine;

[Serializable]
public class tk2dSpriteColliderDefinition
{
	public enum Type
	{
		Box,
		Circle
	}

	public Type type;

	public Vector3 origin;

	public float angle;

	public string name = "";

	public Vector3[] vectors = (Vector3[])(object)new Vector3[0];

	public float[] floats = new float[0];

	public float Radius
	{
		get
		{
			if (type != Type.Circle)
			{
				return 0f;
			}
			return floats[0];
		}
	}

	public Vector3 Size
	{
		get
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			if (type != Type.Box)
			{
				return Vector3.zero;
			}
			return vectors[0];
		}
	}

	public tk2dSpriteColliderDefinition(Type type, Vector3 origin, float angle)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		this.type = type;
		this.origin = origin;
		this.angle = angle;
	}
}

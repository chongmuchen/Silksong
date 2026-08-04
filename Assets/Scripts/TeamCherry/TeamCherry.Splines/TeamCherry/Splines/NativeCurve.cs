using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace TeamCherry.Splines
{

public struct NativeCurve : IDisposable
{
	private NativeArray<float> values;

	private WrapMode preWrapMode;

	private WrapMode postWrapMode;

	public bool IsCreated => values.IsCreated;

	public int Resolution => values.Length;

	private void InitializeValues(int count)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (values.IsCreated)
		{
			values.Dispose();
		}
		values = new NativeArray<float>(count, (Allocator)4, (NativeArrayOptions)0);
	}

	public void Update(AnimationCurve curve, int resolution)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (curve == null)
		{
			if (resolution > 0)
			{
				throw new NullReferenceException("Animation curve is null.");
			}
		}
		else
		{
			preWrapMode = curve.preWrapMode;
			postWrapMode = curve.postWrapMode;
		}
		if (!values.IsCreated || values.Length != resolution)
		{
			InitializeValues(resolution);
		}
		if (curve != null)
		{
			for (int i = 0; i < resolution; i++)
			{
				values[i] = curve.Evaluate((float)i / (float)resolution);
			}
		}
	}

	public float Evaluate(float t)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Invalid comparison between Unknown and I4
		int length = values.Length;
		if (length == 1)
		{
			return values[0];
		}
		if (t < 0f)
		{
			WrapMode val = preWrapMode;
			if ((int)val != 2)
			{
				if ((int)val != 4)
				{
					return values[0];
				}
				t = pingpong(t, 1f);
			}
			else
			{
				t = 1f - math.abs(t) % 1f;
			}
		}
		else if (t > 1f)
		{
			WrapMode val = postWrapMode;
			if ((int)val != 2)
			{
				if ((int)val != 4)
				{
					return values[length - 1];
				}
				t = pingpong(t, 1f);
			}
			else
			{
				t %= 1f;
			}
		}
		float num = t * (float)(length - 1);
		int num2 = (int)num;
		int num3 = num2 + 1;
		if (num3 >= length)
		{
			num3 = length - 1;
		}
		return math.lerp(values[num2], values[num3], num - (float)num2);
	}

	public void Dispose()
	{
		if (values.IsCreated)
		{
			values.Dispose();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private float repeat(float t, float length)
	{
		return math.clamp(t - math.floor(t / length) * length, 0f, length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private float pingpong(float t, float length)
	{
		t = repeat(t, length * 2f);
		return length - math.abs(t - length);
	}
}
}

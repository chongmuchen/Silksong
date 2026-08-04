using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TeamCherry.SharedUtils
{

[Serializable]
public struct MinMaxFloat
{
	public float Start;

	public float End;

	public MinMaxFloat(float start, float end)
	{
		Start = start;
		End = end;
	}

	[Pure]
	public float GetRandomValue()
	{
		return UnityEngine.Random.Range(Start, End);
	}

	public float GetLerpedValue(float t)
	{
		return Mathf.Lerp(Start, End, t);
	}

	public float GetLerpUnclampedValue(float t)
	{
		return Mathf.LerpUnclamped(Start, End, t);
	}

	public bool IsInRange(float value)
	{
		if (value >= Start)
		{
			return value <= End;
		}
		return false;
	}

	public float GetClampedBetween(float value)
	{
		return GetLerpedValue(GetTBetween(value));
	}

	public float GetTBetween(float value)
	{
		float num = End - Start;
		return (value - Start) / num;
	}
}
}

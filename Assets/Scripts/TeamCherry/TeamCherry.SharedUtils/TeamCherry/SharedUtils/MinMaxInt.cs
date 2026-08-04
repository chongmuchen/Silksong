using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TeamCherry.SharedUtils
{

[Serializable]
public struct MinMaxInt
{
	public int Start;

	public int End;

	public int GetRandomValue(bool isInclusive = true)
	{
		if (isInclusive)
		{
			return UnityEngine.Random.Range(Start, End + 1);
		}
		return UnityEngine.Random.Range(Start, End);
	}

	public bool IsInRange(int value)
	{
		if (value >= Start)
		{
			return value <= End;
		}
		return false;
	}

	public MinMaxInt(int start, int end)
	{
		Start = start;
		End = end;
	}
}
}

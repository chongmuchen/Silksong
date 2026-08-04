using System;
using UnityEngine;

namespace TeamCherry.SharedUtils;

[Serializable]
public struct MinMaxInt
{
	public int Start;

	public int End;

	public int GetRandomValue(bool isInclusive = true)
	{
		if (isInclusive)
		{
			return Random.Range(Start, End + 1);
		}
		return Random.Range(Start, End);
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

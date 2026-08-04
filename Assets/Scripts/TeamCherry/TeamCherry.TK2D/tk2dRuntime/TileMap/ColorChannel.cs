using System;
using UnityEngine;

namespace tk2dRuntime.TileMap;

[Serializable]
public class ColorChannel
{
	public Color clearColor = Color.white;

	public ColorChunk[] chunks;

	public int numColumns;

	public int numRows;

	public int divX;

	public int divY;

	public bool IsEmpty => chunks.Length == 0;

	public int NumActiveChunks
	{
		get
		{
			int num = 0;
			ColorChunk[] array = chunks;
			foreach (ColorChunk colorChunk in array)
			{
				if (colorChunk != null && colorChunk.colors != null && colorChunk.colors.Length != 0)
				{
					num++;
				}
			}
			return num;
		}
	}

	public ColorChannel(int width, int height, int divX, int divY)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		Init(width, height, divX, divY);
	}

	public ColorChannel()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		chunks = new ColorChunk[0];
	}

	public void Init(int width, int height, int divX, int divY)
	{
		numColumns = (width + divX - 1) / divX;
		numRows = (height + divY - 1) / divY;
		chunks = new ColorChunk[0];
		this.divX = divX;
		this.divY = divY;
	}

	public ColorChunk FindChunkAndCoordinate(int x, int y, out int offset)
	{
		int num = x / divX;
		int num2 = y / divY;
		num = Mathf.Clamp(num, 0, numColumns - 1);
		num2 = Mathf.Clamp(num2, 0, numRows - 1);
		int num3 = num2 * numColumns + num;
		ColorChunk result = chunks[num3];
		int num4 = x - num * divX;
		int num5 = y - num2 * divY;
		offset = num5 * (divX + 1) + num4;
		return result;
	}

	public Color GetColor(int x, int y)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if (IsEmpty)
		{
			return clearColor;
		}
		int offset;
		ColorChunk colorChunk = FindChunkAndCoordinate(x, y, out offset);
		if (colorChunk.colors.Length == 0)
		{
			return clearColor;
		}
		return Color32.op_Implicit(colorChunk.colors[offset]);
	}

	private void InitChunk(ColorChunk chunk)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (chunk.colors.Length == 0)
		{
			chunk.colors = (Color32[])(object)new Color32[(divX + 1) * (divY + 1)];
			for (int i = 0; i < chunk.colors.Length; i++)
			{
				chunk.colors[i] = Color32.op_Implicit(clearColor);
			}
		}
	}

	public void SetColor(int x, int y, Color color)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		if (IsEmpty)
		{
			Create();
		}
		int num = divX + 1;
		int num2 = Mathf.Max(x - 1, 0) / divX;
		int num3 = Mathf.Max(y - 1, 0) / divY;
		ColorChunk chunk = GetChunk(num2, num3, init: true);
		int num4 = x - num2 * divX;
		int num5 = y - num3 * divY;
		chunk.colors[num5 * num + num4] = Color32.op_Implicit(color);
		chunk.Dirty = true;
		bool flag = false;
		bool flag2 = false;
		if (x != 0 && x % divX == 0 && num2 + 1 < numColumns)
		{
			flag = true;
		}
		if (y != 0 && y % divY == 0 && num3 + 1 < numRows)
		{
			flag2 = true;
		}
		if (flag)
		{
			int num6 = num2 + 1;
			ColorChunk chunk2 = GetChunk(num6, num3, init: true);
			num4 = x - num6 * divX;
			num5 = y - num3 * divY;
			chunk2.colors[num5 * num + num4] = Color32.op_Implicit(color);
			chunk2.Dirty = true;
		}
		if (flag2)
		{
			int num7 = num3 + 1;
			ColorChunk chunk3 = GetChunk(num2, num7, init: true);
			num4 = x - num2 * divX;
			num5 = y - num7 * divY;
			chunk3.colors[num5 * num + num4] = Color32.op_Implicit(color);
			chunk3.Dirty = true;
		}
		if (flag && flag2)
		{
			int num8 = num2 + 1;
			int num9 = num3 + 1;
			ColorChunk chunk4 = GetChunk(num8, num9, init: true);
			num4 = x - num8 * divX;
			num5 = y - num9 * divY;
			chunk4.colors[num5 * num + num4] = Color32.op_Implicit(color);
			chunk4.Dirty = true;
		}
	}

	public ColorChunk GetChunk(int x, int y)
	{
		if (chunks == null || chunks.Length == 0)
		{
			return null;
		}
		return chunks[y * numColumns + x];
	}

	public ColorChunk GetChunk(int x, int y, bool init)
	{
		if (chunks == null || chunks.Length == 0)
		{
			return null;
		}
		ColorChunk colorChunk = chunks[y * numColumns + x];
		InitChunk(colorChunk);
		return colorChunk;
	}

	public void ClearChunk(ColorChunk chunk)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < chunk.colors.Length; i++)
		{
			chunk.colors[i] = Color32.op_Implicit(clearColor);
		}
	}

	public void ClearDirtyFlag()
	{
		ColorChunk[] array = chunks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Dirty = false;
		}
	}

	public void Clear(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		clearColor = color;
		ColorChunk[] array = chunks;
		foreach (ColorChunk chunk in array)
		{
			ClearChunk(chunk);
		}
		Optimize();
	}

	public void Delete()
	{
		chunks = new ColorChunk[0];
	}

	public void Create()
	{
		chunks = new ColorChunk[numColumns * numRows];
		for (int i = 0; i < chunks.Length; i++)
		{
			chunks[i] = new ColorChunk();
		}
	}

	private void Optimize(ColorChunk chunk)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		Color32 val = Color32.op_Implicit(clearColor);
		Color32[] colors = chunk.colors;
		foreach (Color32 val2 in colors)
		{
			if (val2.r != val.r || val2.g != val.g || val2.b != val.b || val2.a != val.a)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			chunk.colors = (Color32[])(object)new Color32[0];
		}
	}

	public void Optimize()
	{
		ColorChunk[] array = chunks;
		foreach (ColorChunk chunk in array)
		{
			Optimize(chunk);
		}
	}
}

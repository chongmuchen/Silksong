using UnityEngine;

public static class tk2dTextGeomGen
{
	public class GeomData
	{
		internal tk2dTextMeshData textMeshData;

		internal tk2dFontData fontInst;

		internal string formattedText = "";
	}

	private static GeomData tmpData = new GeomData();

	private static readonly Color32[] channelSelectColors = (Color32[])(object)new Color32[4]
	{
		new Color32((byte)0, (byte)0, byte.MaxValue, (byte)0),
		(Color32)(new Color(0f, 255f, 0f, 0f)),
		(Color32)(new Color(255f, 0f, 0f, 0f)),
		(Color32)(new Color(0f, 0f, 0f, 255f))
	};

	private static Color32 meshTopColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	private static Color32 meshBottomColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	private static float meshGradientTexU = 0f;

	private static int curGradientCount = 1;

	private static Color32 errorColor = new Color32(byte.MaxValue, (byte)0, byte.MaxValue, byte.MaxValue);

	public static GeomData Data(tk2dTextMeshData textMeshData, tk2dFontData fontData, string formattedText)
	{
		tmpData.textMeshData = textMeshData;
		tmpData.fontInst = fontData;
		tmpData.formattedText = formattedText;
		return tmpData;
	}

	public static Vector2 GetMeshDimensionsForString(string str, GeomData geomData)
	{
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		tk2dFontData fontInst = geomData.fontInst;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		bool flag = false;
		int num4 = 0;
		for (int i = 0; i < str.Length && num4 < textMeshData.maxChars; i++)
		{
			if (flag)
			{
				flag = false;
				continue;
			}
			int num5 = str[i];
			if (num5 == 10)
			{
				num = Mathf.Max(num2, num);
				num2 = 0f;
				num3 -= (fontInst.lineHeight + textMeshData.lineSpacing) * textMeshData.scale.y;
				continue;
			}
			if (textMeshData.inlineStyling && num5 == 94 && i + 1 < str.Length)
			{
				if (str[i + 1] != '^')
				{
					int num6 = 0;
					switch (str[i + 1])
					{
					case 'c':
						num6 = 5;
						break;
					case 'C':
						num6 = 9;
						break;
					case 'g':
						num6 = 9;
						break;
					case 'G':
						num6 = 17;
						break;
					}
					i += num6;
					continue;
				}
				flag = true;
			}
			bool num7 = num5 == 94;
			tk2dFontChar tk2dFontChar2;
			if (fontInst.useDictionary)
			{
				if (!fontInst.charDict.ContainsKey(num5))
				{
					num5 = 0;
				}
				tk2dFontChar2 = fontInst.charDict[num5];
			}
			else
			{
				if (num5 >= fontInst.chars.Length)
				{
					num5 = 0;
				}
				tk2dFontChar2 = fontInst.chars[num5];
			}
			if (num7)
			{
				num5 = 94;
			}
			num2 += (tk2dFontChar2.advance + textMeshData.spacing) * textMeshData.scale.x;
			if (textMeshData.kerning && i < str.Length - 1)
			{
				tk2dFontKerning[] kerning = fontInst.kerning;
				foreach (tk2dFontKerning tk2dFontKerning2 in kerning)
				{
					if (tk2dFontKerning2.c0 == str[i] && tk2dFontKerning2.c1 == str[i + 1])
					{
						num2 += tk2dFontKerning2.amount * textMeshData.scale.x;
						break;
					}
				}
			}
			num4++;
		}
		num = Mathf.Max(num2, num);
		num3 -= (fontInst.lineHeight + textMeshData.lineSpacing) * textMeshData.scale.y;
		return new Vector2(num, num3);
	}

	public static float GetYAnchorForHeight(float textHeight, GeomData geomData)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected I4, but got Unknown
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		tk2dFontData fontInst = geomData.fontInst;
		int num = (int)textMeshData.anchor / 3;
		float num2 = (fontInst.lineHeight + textMeshData.lineSpacing) * textMeshData.scale.y;
		switch (num)
		{
		case 0:
			return 0f - num2;
		case 1:
		{
			float num3 = (0f - textHeight) / 2f - num2;
			if (fontInst.version >= 2)
			{
				float num4 = fontInst.texelSize.y * textMeshData.scale.y;
				return Mathf.Floor(num3 / num4) * num4;
			}
			return num3;
		}
		case 2:
			return 0f - textHeight - num2;
		default:
			return 0f - num2;
		}
	}

	public static float GetXAnchorForWidth(float lineWidth, GeomData geomData)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected I4, but got Unknown
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		tk2dFontData fontInst = geomData.fontInst;
		switch ((int)textMeshData.anchor % 3)
		{
		case 0:
			return 0f;
		case 1:
		{
			float num = (0f - lineWidth) / 2f;
			if (fontInst.version >= 2)
			{
				float num2 = fontInst.texelSize.x * textMeshData.scale.x;
				return Mathf.Floor(num / num2) * num2;
			}
			return num;
		}
		case 2:
			return 0f - lineWidth;
		default:
			return 0f;
		}
	}

	private static void PostAlignTextData(Vector3[] pos, int offset, int targetStart, int targetEnd, float offsetX)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		for (int i = targetStart * 4; i < targetEnd * 4; i++)
		{
			Vector3 val = pos[offset + i];
			val.x += offsetX;
			pos[offset + i] = val;
		}
	}

	private static int GetFullHexColorComponent(int c1, int c2)
	{
		int num = 0;
		if (c1 >= 48 && c1 <= 57)
		{
			num += (c1 - 48) * 16;
		}
		else if (c1 >= 97 && c1 <= 102)
		{
			num += (10 + c1 - 97) * 16;
		}
		else
		{
			if (c1 < 65 || c1 > 70)
			{
				return -1;
			}
			num += (10 + c1 - 65) * 16;
		}
		if (c2 >= 48 && c2 <= 57)
		{
			return num + (c2 - 48);
		}
		if (c2 >= 97 && c2 <= 102)
		{
			return num + (10 + c2 - 97);
		}
		if (c2 >= 65 && c2 <= 70)
		{
			return num + (10 + c2 - 65);
		}
		return -1;
	}

	private static int GetCompactHexColorComponent(int c)
	{
		if (c >= 48 && c <= 57)
		{
			return (c - 48) * 17;
		}
		if (c >= 97 && c <= 102)
		{
			return (10 + c - 97) * 17;
		}
		if (c >= 65 && c <= 70)
		{
			return (10 + c - 65) * 17;
		}
		return -1;
	}

	private static int GetStyleHexColor(string str, bool fullHex, ref Color32 color)
	{
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		int num;
		int num2;
		int num3;
		int num4;
		if (fullHex)
		{
			if (str.Length < 8)
			{
				return 1;
			}
			num = GetFullHexColorComponent(str[0], str[1]);
			num2 = GetFullHexColorComponent(str[2], str[3]);
			num3 = GetFullHexColorComponent(str[4], str[5]);
			num4 = GetFullHexColorComponent(str[6], str[7]);
		}
		else
		{
			if (str.Length < 4)
			{
				return 1;
			}
			num = GetCompactHexColorComponent(str[0]);
			num2 = GetCompactHexColorComponent(str[1]);
			num3 = GetCompactHexColorComponent(str[2]);
			num4 = GetCompactHexColorComponent(str[3]);
		}
		if (num == -1 || num2 == -1 || num3 == -1 || num4 == -1)
		{
			return 1;
		}
		color = new Color32((byte)num, (byte)num2, (byte)num3, (byte)num4);
		return 0;
	}

	private static int SetColorsFromStyleCommand(string args, bool twoColors, bool fullHex)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		int num = ((!twoColors) ? 1 : 2) * (fullHex ? 8 : 4);
		bool flag = false;
		if (args.Length >= num)
		{
			if (GetStyleHexColor(args, fullHex, ref meshTopColor) != 0)
			{
				flag = true;
			}
			if (twoColors)
			{
				if (GetStyleHexColor(args.Substring(fullHex ? 8 : 4), fullHex, ref meshBottomColor) != 0)
				{
					flag = true;
				}
			}
			else
			{
				meshBottomColor = meshTopColor;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			meshTopColor = (meshBottomColor = errorColor);
		}
		return num;
	}

	private static void SetGradientTexUFromStyleCommand(int arg)
	{
		meshGradientTexU = (float)(arg - 48) / (float)((curGradientCount <= 0) ? 1 : curGradientCount);
	}

	private static int HandleStyleCommand(string cmd)
	{
		if (cmd.Length == 0)
		{
			return 0;
		}
		int num = cmd[0];
		string args = cmd.Substring(1);
		int result = 0;
		switch (num)
		{
		case 99:
			result = 1 + SetColorsFromStyleCommand(args, twoColors: false, fullHex: false);
			break;
		case 67:
			result = 1 + SetColorsFromStyleCommand(args, twoColors: false, fullHex: true);
			break;
		case 103:
			result = 1 + SetColorsFromStyleCommand(args, twoColors: true, fullHex: false);
			break;
		case 71:
			result = 1 + SetColorsFromStyleCommand(args, twoColors: true, fullHex: true);
			break;
		}
		if (num >= 48 && num <= 57)
		{
			SetGradientTexUFromStyleCommand(num);
			result = 1;
		}
		return result;
	}

	public static void GetTextMeshGeomDesc(out int numVertices, out int numIndices, GeomData geomData)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		numVertices = textMeshData.maxChars * 4;
		numIndices = textMeshData.maxChars * 6;
	}

	public static int SetTextMeshGeom(Vector3[] pos, Vector2[] uv, Vector2[] uv2, Color32[] color, int offset, GeomData geomData)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Unknown result type (might be due to invalid IL or missing references)
		//IL_078b: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0793: Unknown result type (might be due to invalid IL or missing references)
		//IL_0795: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Unknown result type (might be due to invalid IL or missing references)
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_082c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Unknown result type (might be due to invalid IL or missing references)
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Unknown result type (might be due to invalid IL or missing references)
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_0843: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Unknown result type (might be due to invalid IL or missing references)
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Unknown result type (might be due to invalid IL or missing references)
		//IL_084d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Unknown result type (might be due to invalid IL or missing references)
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		tk2dFontData fontInst = geomData.fontInst;
		string formattedText = geomData.formattedText;
		meshTopColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		meshBottomColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		meshGradientTexU = (float)textMeshData.textureGradient / (float)((fontInst.gradientCount <= 0) ? 1 : fontInst.gradientCount);
		curGradientCount = fontInst.gradientCount;
		float yAnchorForHeight = GetYAnchorForHeight(GetMeshDimensionsForString(geomData.formattedText, geomData).y, geomData);
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < formattedText.Length && num3 < textMeshData.maxChars; i++)
		{
			int num5 = formattedText[i];
			bool num6 = num5 == 94;
			tk2dFontChar tk2dFontChar2;
			if (fontInst.useDictionary)
			{
				if (!fontInst.charDict.ContainsKey(num5))
				{
					num5 = 0;
				}
				tk2dFontChar2 = fontInst.charDict[num5];
			}
			else
			{
				if (num5 >= fontInst.chars.Length)
				{
					num5 = 0;
				}
				tk2dFontChar2 = fontInst.chars[num5];
			}
			if (num6)
			{
				num5 = 94;
			}
			if (num5 == 10)
			{
				float lineWidth = num;
				int targetEnd = num3;
				if (num4 != num3)
				{
					float xAnchorForWidth = GetXAnchorForWidth(lineWidth, geomData);
					PostAlignTextData(pos, offset, num4, targetEnd, xAnchorForWidth);
				}
				num4 = num3;
				num = 0f;
				num2 -= (fontInst.lineHeight + textMeshData.lineSpacing) * textMeshData.scale.y;
				continue;
			}
			if (textMeshData.inlineStyling && num5 == 94)
			{
				if (i + 1 >= formattedText.Length || formattedText[i + 1] != '^')
				{
					i += HandleStyleCommand(formattedText.Substring(i + 1));
					continue;
				}
				i++;
			}
			pos[offset + num3 * 4] = new Vector3(num + tk2dFontChar2.p0.x * textMeshData.scale.x, yAnchorForHeight + num2 + tk2dFontChar2.p0.y * textMeshData.scale.y, 0f);
			pos[offset + num3 * 4 + 1] = new Vector3(num + tk2dFontChar2.p1.x * textMeshData.scale.x, yAnchorForHeight + num2 + tk2dFontChar2.p0.y * textMeshData.scale.y, 0f);
			pos[offset + num3 * 4 + 2] = new Vector3(num + tk2dFontChar2.p0.x * textMeshData.scale.x, yAnchorForHeight + num2 + tk2dFontChar2.p1.y * textMeshData.scale.y, 0f);
			pos[offset + num3 * 4 + 3] = new Vector3(num + tk2dFontChar2.p1.x * textMeshData.scale.x, yAnchorForHeight + num2 + tk2dFontChar2.p1.y * textMeshData.scale.y, 0f);
			if (tk2dFontChar2.flipped)
			{
				uv[offset + num3 * 4] = new Vector2(tk2dFontChar2.uv1.x, tk2dFontChar2.uv1.y);
				uv[offset + num3 * 4 + 1] = new Vector2(tk2dFontChar2.uv1.x, tk2dFontChar2.uv0.y);
				uv[offset + num3 * 4 + 2] = new Vector2(tk2dFontChar2.uv0.x, tk2dFontChar2.uv1.y);
				uv[offset + num3 * 4 + 3] = new Vector2(tk2dFontChar2.uv0.x, tk2dFontChar2.uv0.y);
			}
			else
			{
				uv[offset + num3 * 4] = new Vector2(tk2dFontChar2.uv0.x, tk2dFontChar2.uv0.y);
				uv[offset + num3 * 4 + 1] = new Vector2(tk2dFontChar2.uv1.x, tk2dFontChar2.uv0.y);
				uv[offset + num3 * 4 + 2] = new Vector2(tk2dFontChar2.uv0.x, tk2dFontChar2.uv1.y);
				uv[offset + num3 * 4 + 3] = new Vector2(tk2dFontChar2.uv1.x, tk2dFontChar2.uv1.y);
			}
			if (fontInst.textureGradients)
			{
				uv2[offset + num3 * 4] = tk2dFontChar2.gradientUv[0] + new Vector2(meshGradientTexU, 0f);
				uv2[offset + num3 * 4 + 1] = tk2dFontChar2.gradientUv[1] + new Vector2(meshGradientTexU, 0f);
				uv2[offset + num3 * 4 + 2] = tk2dFontChar2.gradientUv[2] + new Vector2(meshGradientTexU, 0f);
				uv2[offset + num3 * 4 + 3] = tk2dFontChar2.gradientUv[3] + new Vector2(meshGradientTexU, 0f);
			}
			if (fontInst.isPacked)
			{
				color[offset + num3 * 4 + 3] = (color[offset + num3 * 4 + 2] = (color[offset + num3 * 4 + 1] = (color[offset + num3 * 4] = channelSelectColors[tk2dFontChar2.channel])));
			}
			else
			{
				color[offset + num3 * 4] = meshTopColor;
				color[offset + num3 * 4 + 1] = meshTopColor;
				color[offset + num3 * 4 + 2] = meshBottomColor;
				color[offset + num3 * 4 + 3] = meshBottomColor;
			}
			num += (tk2dFontChar2.advance + textMeshData.spacing) * textMeshData.scale.x;
			if (textMeshData.kerning && i < formattedText.Length - 1)
			{
				tk2dFontKerning[] kerning = fontInst.kerning;
				foreach (tk2dFontKerning tk2dFontKerning2 in kerning)
				{
					if (tk2dFontKerning2.c0 == formattedText[i] && tk2dFontKerning2.c1 == formattedText[i + 1])
					{
						num += tk2dFontKerning2.amount * textMeshData.scale.x;
						break;
					}
				}
			}
			num3++;
		}
		if (num4 != num3)
		{
			float lineWidth2 = num;
			int targetEnd2 = num3;
			float xAnchorForWidth2 = GetXAnchorForWidth(lineWidth2, geomData);
			PostAlignTextData(pos, offset, num4, targetEnd2, xAnchorForWidth2);
		}
		for (int k = num3; k < textMeshData.maxChars; k++)
		{
			pos[offset + k * 4] = (pos[offset + k * 4 + 1] = (pos[offset + k * 4 + 2] = (pos[offset + k * 4 + 3] = Vector3.zero)));
			uv[offset + k * 4] = (uv[offset + k * 4 + 1] = (uv[offset + k * 4 + 2] = (uv[offset + k * 4 + 3] = Vector2.zero)));
			if (fontInst.textureGradients)
			{
				uv2[offset + k * 4] = (uv2[offset + k * 4 + 1] = (uv2[offset + k * 4 + 2] = (uv2[offset + k * 4 + 3] = Vector2.zero)));
			}
			if (!fontInst.isPacked)
			{
				color[offset + k * 4] = (color[offset + k * 4 + 1] = meshTopColor);
				color[offset + k * 4 + 2] = (color[offset + k * 4 + 3] = meshBottomColor);
			}
			else
			{
				color[offset + k * 4] = (color[offset + k * 4 + 1] = (color[offset + k * 4 + 2] = (color[offset + k * 4 + 3] = (Color32)(Color.clear))));
			}
		}
		return num3;
	}

	public static void SetTextMeshIndices(int[] indices, int offset, int vStart, GeomData geomData, int target)
	{
		tk2dTextMeshData textMeshData = geomData.textMeshData;
		for (int i = 0; i < textMeshData.maxChars; i++)
		{
			indices[offset + i * 6] = vStart + i * 4;
			indices[offset + i * 6 + 1] = vStart + i * 4 + 1;
			indices[offset + i * 6 + 2] = vStart + i * 4 + 3;
			indices[offset + i * 6 + 3] = vStart + i * 4 + 2;
			indices[offset + i * 6 + 4] = vStart + i * 4;
			indices[offset + i * 6 + 5] = vStart + i * 4 + 3;
		}
	}
}

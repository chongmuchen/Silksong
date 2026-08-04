using UnityEngine;

public static class tk2dSpriteGeomGen
{
	private static readonly int[] boxIndicesBack = new int[36]
	{
		0, 1, 2, 2, 1, 3, 6, 5, 4, 7,
		5, 6, 3, 7, 6, 2, 3, 6, 4, 5,
		1, 4, 1, 0, 6, 4, 0, 6, 0, 2,
		1, 7, 3, 5, 7, 1
	};

	private static readonly int[] boxIndicesFwd = new int[36]
	{
		2, 1, 0, 3, 1, 2, 4, 5, 6, 6,
		5, 7, 6, 7, 3, 6, 3, 2, 1, 5,
		4, 0, 1, 4, 0, 4, 6, 2, 0, 6,
		3, 7, 1, 1, 7, 5
	};

	private static readonly Vector3[] boxUnitVertices = (Vector3[])(object)new Vector3[8]
	{
		new Vector3(-1f, -1f, -1f),
		new Vector3(-1f, -1f, 1f),
		new Vector3(1f, -1f, -1f),
		new Vector3(1f, -1f, 1f),
		new Vector3(-1f, 1f, -1f),
		new Vector3(-1f, 1f, 1f),
		new Vector3(1f, 1f, -1f),
		new Vector3(1f, 1f, 1f)
	};

	private static Matrix4x4 boxScaleMatrix = Matrix4x4.identity;

	public static void SetSpriteColors(Color32[] dest, int offset, int numVertices, Color c, bool premulAlpha)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if (premulAlpha)
		{
			c.r *= c.a;
			c.g *= c.a;
			c.b *= c.a;
		}
		Color32 val = (Color32)(c);
		for (int i = 0; i < numVertices; i++)
		{
			dest[offset + i] = val;
		}
	}

	public static Vector2 GetAnchorOffset(tk2dBaseSprite.Anchor anchor, float width, float height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		Vector2 zero = Vector2.zero;
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.UpperCenter:
			zero.x = (int)(width / 2f);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
		case tk2dBaseSprite.Anchor.MiddleRight:
		case tk2dBaseSprite.Anchor.UpperRight:
			zero.x = (int)width;
			break;
		}
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.MiddleLeft:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.MiddleRight:
			zero.y = (int)(height / 2f);
			break;
		case tk2dBaseSprite.Anchor.LowerLeft:
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.LowerRight:
			zero.y = (int)height;
			break;
		}
		return zero;
	}

	public static void GetSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef)
	{
		numVertices = spriteDef.positions.Length;
		numIndices = spriteDef.indices.Length;
	}

	public static void SetSpriteGeom(Vector3[] pos, Vector2[] uv, Vector3[] norm, Vector4[] tang, int offset, tk2dSpriteDefinition spriteDef, Vector3 scale)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < spriteDef.positions.Length; i++)
		{
			pos[offset + i] = Vector3.Scale(spriteDef.positions[i], scale);
		}
		for (int j = 0; j < spriteDef.uvs.Length; j++)
		{
			uv[offset + j] = spriteDef.uvs[j];
		}
		if (norm != null && spriteDef.normals != null)
		{
			for (int k = 0; k < spriteDef.normals.Length; k++)
			{
				norm[offset + k] = spriteDef.normals[k];
			}
		}
		if (tang != null && spriteDef.tangents != null)
		{
			for (int l = 0; l < spriteDef.tangents.Length; l++)
			{
				tang[offset + l] = spriteDef.tangents[l];
			}
		}
	}

	public static void SetSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef)
	{
		for (int i = 0; i < spriteDef.indices.Length; i++)
		{
			indices[offset + i] = vStart + spriteDef.indices[i];
		}
	}

	public static void GetClippedSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef)
	{
		if (spriteDef.positions.Length == 4)
		{
			numVertices = 4;
			numIndices = 6;
		}
		else
		{
			numVertices = 0;
			numIndices = 0;
		}
	}

	public static void SetClippedSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 clipBottomLeft, Vector2 clipTopRight, float colliderOffsetZ, float colliderExtentZ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		boundsCenter = Vector3.zero;
		boundsExtents = Vector3.zero;
		if (spriteDef.positions.Length == 4)
		{
			Vector3 val = spriteDef.untrimmedBoundsData[0] - spriteDef.untrimmedBoundsData[1] * 0.5f;
			Vector3 val2 = spriteDef.untrimmedBoundsData[0] + spriteDef.untrimmedBoundsData[1] * 0.5f;
			float num = Mathf.Lerp(val.x, val2.x, clipBottomLeft.x);
			float num2 = Mathf.Lerp(val.x, val2.x, clipTopRight.x);
			float num3 = Mathf.Lerp(val.y, val2.y, clipBottomLeft.y);
			float num4 = Mathf.Lerp(val.y, val2.y, clipTopRight.y);
			Vector3 val3 = spriteDef.boundsData[1];
			Vector3 val4 = spriteDef.boundsData[0] - val3 * 0.5f;
			float num5 = (num - val4.x) / val3.x;
			float num6 = (num2 - val4.x) / val3.x;
			float num7 = (num3 - val4.y) / val3.y;
			float num8 = (num4 - val4.y) / val3.y;
			Vector2 val5 = default(Vector2);
			val5 = new Vector2(Mathf.Clamp01(num5), Mathf.Clamp01(num7));
			Vector2 val6 = default(Vector2);
			val6 = new Vector2(Mathf.Clamp01(num6), Mathf.Clamp01(num8));
			Vector3 val7 = spriteDef.positions[0];
			Vector3 val8 = spriteDef.positions[3];
			Vector3 val9 = default(Vector3);
			val9 = new Vector3(Mathf.Lerp(val7.x, val8.x, val5.x) * scale.x, Mathf.Lerp(val7.y, val8.y, val5.y) * scale.y, val7.z * scale.z);
			Vector3 val10 = default(Vector3);
			val10 = new Vector3(Mathf.Lerp(val7.x, val8.x, val6.x) * scale.x, Mathf.Lerp(val7.y, val8.y, val6.y) * scale.y, val7.z * scale.z);
			boundsCenter.Set(val9.x + (val10.x - val9.x) * 0.5f, val9.y + (val10.y - val9.y) * 0.5f, colliderOffsetZ);
			boundsExtents.Set((val10.x - val9.x) * 0.5f, (val10.y - val9.y) * 0.5f, colliderExtentZ);
			pos[offset] = new Vector3(val9.x, val9.y, val9.z);
			pos[offset + 1] = new Vector3(val10.x, val9.y, val9.z);
			pos[offset + 2] = new Vector3(val9.x, val10.y, val9.z);
			pos[offset + 3] = new Vector3(val10.x, val10.y, val9.z);
			if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
			{
				Vector2 val11 = default(Vector2);
				val11 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, val5.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, val5.x));
				Vector2 val12 = default(Vector2);
				val12 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, val6.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, val6.x));
				uv[offset] = new Vector2(val11.x, val11.y);
				uv[offset + 1] = new Vector2(val11.x, val12.y);
				uv[offset + 2] = new Vector2(val12.x, val11.y);
				uv[offset + 3] = new Vector2(val12.x, val12.y);
			}
			else if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
			{
				Vector2 val13 = default(Vector2);
				val13 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, val5.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, val5.x));
				Vector2 val14 = default(Vector2);
				val14 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, val6.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, val6.x));
				uv[offset] = new Vector2(val13.x, val13.y);
				uv[offset + 2] = new Vector2(val14.x, val13.y);
				uv[offset + 1] = new Vector2(val13.x, val14.y);
				uv[offset + 3] = new Vector2(val14.x, val14.y);
			}
			else
			{
				Vector2 val15 = default(Vector2);
				val15 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, val5.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, val5.y));
				Vector2 val16 = default(Vector2);
				val16 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, val6.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, val6.y));
				uv[offset] = new Vector2(val15.x, val15.y);
				uv[offset + 1] = new Vector2(val16.x, val15.y);
				uv[offset + 2] = new Vector2(val15.x, val16.y);
				uv[offset + 3] = new Vector2(val16.x, val16.y);
			}
		}
	}

	public static void SetClippedSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef)
	{
		if (spriteDef.positions.Length == 4)
		{
			indices[offset] = vStart;
			indices[offset + 1] = vStart + 3;
			indices[offset + 2] = vStart + 1;
			indices[offset + 3] = vStart + 2;
			indices[offset + 4] = vStart + 3;
			indices[offset + 5] = vStart;
		}
	}

	public static void GetSlicedSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef, bool borderOnly)
	{
		if (spriteDef.positions.Length == 4)
		{
			numVertices = 16;
			numIndices = (borderOnly ? 48 : 54);
		}
		else
		{
			numVertices = 0;
			numIndices = 0;
		}
	}

	public static void SetSlicedSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 dimensions, Vector2 borderBottomLeft, Vector2 borderTopRight, tk2dBaseSprite.Anchor anchor, float colliderOffsetZ, float colliderExtentZ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		boundsCenter = Vector3.zero;
		boundsExtents = Vector3.zero;
		if (spriteDef.positions.Length != 4)
		{
			return;
		}
		float x = spriteDef.texelSize.x;
		float y = spriteDef.texelSize.y;
		Vector3[] positions = spriteDef.positions;
		float num = positions[1].x - positions[0].x;
		float num2 = positions[2].y - positions[0].y;
		float num3 = borderTopRight.y * num2;
		float num4 = borderBottomLeft.y * num2;
		float num5 = borderTopRight.x * num;
		float num6 = borderBottomLeft.x * num;
		float num7 = dimensions.x * x;
		float num8 = dimensions.y * y;
		float num9 = 0f;
		float num10 = 0f;
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.UpperCenter:
			num9 = -(int)(dimensions.x / 2f);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
		case tk2dBaseSprite.Anchor.MiddleRight:
		case tk2dBaseSprite.Anchor.UpperRight:
			num9 = -(int)dimensions.x;
			break;
		}
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.MiddleLeft:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.MiddleRight:
			num10 = -(int)(dimensions.y / 2f);
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
		case tk2dBaseSprite.Anchor.UpperCenter:
		case tk2dBaseSprite.Anchor.UpperRight:
			num10 = -(int)dimensions.y;
			break;
		}
		num9 *= x;
		num10 *= y;
		boundsCenter.Set(scale.x * (num7 * 0.5f + num9), scale.y * (num8 * 0.5f + num10), colliderOffsetZ);
		boundsExtents.Set(scale.x * (num7 * 0.5f), scale.y * (num8 * 0.5f), colliderExtentZ);
		Vector2[] uvs = spriteDef.uvs;
		Vector2 val = uvs[1] - uvs[0];
		Vector2 val2 = uvs[2] - uvs[0];
		Vector3 val3 = default(Vector3);
		val3 = new Vector3(num9, num10, 0f);
		Vector3[] array = (Vector3[])(object)new Vector3[4]
		{
			val3,
			val3 + new Vector3(0f, num4, 0f),
			val3 + new Vector3(0f, num8 - num3, 0f),
			val3 + new Vector3(0f, num8, 0f)
		};
		Vector2[] array2 = (Vector2[])(object)new Vector2[4]
		{
			uvs[0],
			uvs[0] + val2 * borderBottomLeft.y,
			uvs[0] + val2 * (1f - borderTopRight.y),
			uvs[0] + val2
		};
		for (int i = 0; i < 4; i++)
		{
			pos[offset + i * 4] = array[i];
			pos[offset + i * 4 + 1] = array[i] + new Vector3(num6, 0f, 0f);
			pos[offset + i * 4 + 2] = array[i] + new Vector3(num7 - num5, 0f, 0f);
			pos[offset + i * 4 + 3] = array[i] + new Vector3(num7, 0f, 0f);
			for (int j = 0; j < 4; j++)
			{
				pos[offset + i * 4 + j] = Vector3.Scale(pos[offset + i * 4 + j], scale);
			}
			uv[offset + i * 4] = array2[i];
			uv[offset + i * 4 + 1] = array2[i] + val * borderBottomLeft.x;
			uv[offset + i * 4 + 2] = array2[i] + val * (1f - borderTopRight.x);
			uv[offset + i * 4 + 3] = array2[i] + val;
		}
	}

	public static void SetSlicedSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef, bool borderOnly)
	{
		if (spriteDef.positions.Length == 4)
		{
			int[] array = new int[54]
			{
				0, 4, 1, 1, 4, 5, 1, 5, 2, 2,
				5, 6, 2, 6, 3, 3, 6, 7, 4, 8,
				5, 5, 8, 9, 6, 10, 7, 7, 10, 11,
				8, 12, 9, 9, 12, 13, 9, 13, 10, 10,
				13, 14, 10, 14, 11, 11, 14, 15, 5, 9,
				6, 6, 9, 10
			};
			int num = array.Length;
			if (borderOnly)
			{
				num -= 6;
			}
			for (int i = 0; i < num; i++)
			{
				indices[offset + i] = vStart + array[i];
			}
		}
	}

	public static void GetTiledSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef, Vector2 dimensions)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)Mathf.Ceil(dimensions.x * spriteDef.texelSize.x / spriteDef.untrimmedBoundsData[1].x);
		int num2 = (int)Mathf.Ceil(dimensions.y * spriteDef.texelSize.y / spriteDef.untrimmedBoundsData[1].y);
		numVertices = num * num2 * 4;
		numIndices = num * num2 * 6;
	}

	public static void SetTiledSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 dimensions, tk2dBaseSprite.Anchor anchor, float colliderOffsetZ, float colliderExtentZ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_0909: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Unknown result type (might be due to invalid IL or missing references)
		//IL_091f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_092b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0930: Unknown result type (might be due to invalid IL or missing references)
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0946: Unknown result type (might be due to invalid IL or missing references)
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_0969: Unknown result type (might be due to invalid IL or missing references)
		//IL_096e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0982: Unknown result type (might be due to invalid IL or missing references)
		//IL_0987: Unknown result type (might be due to invalid IL or missing references)
		//IL_099b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0802: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0856: Unknown result type (might be due to invalid IL or missing references)
		//IL_085b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0867: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_0899: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_075f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cd: Unknown result type (might be due to invalid IL or missing references)
		boundsCenter = Vector3.zero;
		boundsExtents = Vector3.zero;
		int num = (int)Mathf.Ceil(dimensions.x * spriteDef.texelSize.x / spriteDef.untrimmedBoundsData[1].x);
		int num2 = (int)Mathf.Ceil(dimensions.y * spriteDef.texelSize.y / spriteDef.untrimmedBoundsData[1].y);
		Vector2 val = default(Vector2);
		val = new Vector2(dimensions.x * spriteDef.texelSize.x * scale.x, dimensions.y * spriteDef.texelSize.y * scale.y);
		Vector2 val2 = Vector2.Scale(spriteDef.texelSize, (Vector2)(scale)) * 0.1f;
		Vector3 zero = Vector3.zero;
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.LowerCenter:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.UpperCenter:
			zero.x = 0f - val.x / 2f;
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
		case tk2dBaseSprite.Anchor.MiddleRight:
		case tk2dBaseSprite.Anchor.UpperRight:
			zero.x = 0f - val.x;
			break;
		}
		switch (anchor)
		{
		case tk2dBaseSprite.Anchor.MiddleLeft:
		case tk2dBaseSprite.Anchor.MiddleCenter:
		case tk2dBaseSprite.Anchor.MiddleRight:
			zero.y = 0f - val.y / 2f;
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
		case tk2dBaseSprite.Anchor.UpperCenter:
		case tk2dBaseSprite.Anchor.UpperRight:
			zero.y = 0f - val.y;
			break;
		}
		Vector3 val3 = zero;
		zero -= Vector3.Scale(spriteDef.positions[0], scale);
		boundsCenter.Set(val.x * 0.5f + val3.x, val.y * 0.5f + val3.y, colliderOffsetZ);
		boundsExtents.Set(val.x * 0.5f, val.y * 0.5f, colliderExtentZ);
		int num3 = 0;
		Vector3 val4 = Vector3.Scale(spriteDef.untrimmedBoundsData[1], scale);
		Vector3 zero2 = Vector3.zero;
		Vector3 val5 = zero2;
		Vector2 val7 = default(Vector2);
		Vector3 val8 = default(Vector3);
		Vector3 val9 = default(Vector3);
		Vector2 val10 = default(Vector2);
		Vector2 val11 = default(Vector2);
		Vector2 val12 = default(Vector2);
		Vector2 val13 = default(Vector2);
		Vector2 val14 = default(Vector2);
		Vector2 val15 = default(Vector2);
		for (int i = 0; i < num2; i++)
		{
			val5.x = zero2.x;
			for (int j = 0; j < num; j++)
			{
				float num4 = 1f;
				float num5 = 1f;
				if (Mathf.Abs(val5.x + val4.x) > Mathf.Abs(val.x) + val2.x)
				{
					num4 = val.x % val4.x / val4.x;
				}
				if (Mathf.Abs(val5.y + val4.y) > Mathf.Abs(val.y) + val2.y)
				{
					num5 = val.y % val4.y / val4.y;
				}
				Vector3 val6 = val5 + zero;
				if (num4 != 1f || num5 != 1f)
				{
					Vector2 zero3 = Vector2.zero;
					val7 = new Vector2(num4, num5);
					val8 = new Vector3(Mathf.Lerp(spriteDef.positions[0].x, spriteDef.positions[3].x, zero3.x) * scale.x, Mathf.Lerp(spriteDef.positions[0].y, spriteDef.positions[3].y, zero3.y) * scale.y, spriteDef.positions[0].z * scale.z);
					val9 = new Vector3(Mathf.Lerp(spriteDef.positions[0].x, spriteDef.positions[3].x, val7.x) * scale.x, Mathf.Lerp(spriteDef.positions[0].y, spriteDef.positions[3].y, val7.y) * scale.y, spriteDef.positions[0].z * scale.z);
					pos[offset + num3] = val6 + new Vector3(val8.x, val8.y, val8.z);
					pos[offset + num3 + 1] = val6 + new Vector3(val9.x, val8.y, val8.z);
					pos[offset + num3 + 2] = val6 + new Vector3(val8.x, val9.y, val8.z);
					pos[offset + num3 + 3] = val6 + new Vector3(val9.x, val9.y, val8.z);
					if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
					{
						val10 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, zero3.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, zero3.x));
						val11 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, val7.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, val7.x));
						uv[offset + num3] = new Vector2(val10.x, val10.y);
						uv[offset + num3 + 1] = new Vector2(val10.x, val11.y);
						uv[offset + num3 + 2] = new Vector2(val11.x, val10.y);
						uv[offset + num3 + 3] = new Vector2(val11.x, val11.y);
					}
					else if (spriteDef.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
					{
						val12 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, zero3.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, zero3.x));
						val13 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, val7.y), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, val7.x));
						uv[offset + num3] = new Vector2(val12.x, val12.y);
						uv[offset + num3 + 2] = new Vector2(val13.x, val12.y);
						uv[offset + num3 + 1] = new Vector2(val12.x, val13.y);
						uv[offset + num3 + 3] = new Vector2(val13.x, val13.y);
					}
					else
					{
						val14 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, zero3.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, zero3.y));
						val15 = new Vector2(Mathf.Lerp(spriteDef.uvs[0].x, spriteDef.uvs[3].x, val7.x), Mathf.Lerp(spriteDef.uvs[0].y, spriteDef.uvs[3].y, val7.y));
						uv[offset + num3] = new Vector2(val14.x, val14.y);
						uv[offset + num3 + 1] = new Vector2(val15.x, val14.y);
						uv[offset + num3 + 2] = new Vector2(val14.x, val15.y);
						uv[offset + num3 + 3] = new Vector2(val15.x, val15.y);
					}
				}
				else
				{
					pos[offset + num3] = val6 + Vector3.Scale(spriteDef.positions[0], scale);
					pos[offset + num3 + 1] = val6 + Vector3.Scale(spriteDef.positions[1], scale);
					pos[offset + num3 + 2] = val6 + Vector3.Scale(spriteDef.positions[2], scale);
					pos[offset + num3 + 3] = val6 + Vector3.Scale(spriteDef.positions[3], scale);
					uv[offset + num3] = spriteDef.uvs[0];
					uv[offset + num3 + 1] = spriteDef.uvs[1];
					uv[offset + num3 + 2] = spriteDef.uvs[2];
					uv[offset + num3 + 3] = spriteDef.uvs[3];
				}
				num3 += 4;
				val5.x += val4.x;
			}
			val5.y += val4.y;
		}
	}

	public static void SetTiledSpriteIndices(int[] indices, int offset, int vStart, tk2dSpriteDefinition spriteDef, Vector2 dimensions)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		GetTiledSpriteGeomDesc(out var _, out var numIndices, spriteDef, dimensions);
		int num = 0;
		for (int i = 0; i < numIndices; i += 6)
		{
			indices[offset + i] = vStart + spriteDef.indices[0] + num;
			indices[offset + i + 1] = vStart + spriteDef.indices[1] + num;
			indices[offset + i + 2] = vStart + spriteDef.indices[2] + num;
			indices[offset + i + 3] = vStart + spriteDef.indices[3] + num;
			indices[offset + i + 4] = vStart + spriteDef.indices[4] + num;
			indices[offset + i + 5] = vStart + spriteDef.indices[5] + num;
			num += 4;
		}
	}

	public static void SetBoxMeshData(Vector3[] pos, int[] indices, int posOffset, int indicesOffset, int vStart, Vector3 origin, Vector3 extents, Matrix4x4 mat, Vector3 baseScale)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		boxScaleMatrix.m03 = origin.x * baseScale.x;
		boxScaleMatrix.m13 = origin.y * baseScale.y;
		boxScaleMatrix.m23 = origin.z * baseScale.z;
		boxScaleMatrix.m00 = extents.x * baseScale.x;
		boxScaleMatrix.m11 = extents.y * baseScale.y;
		boxScaleMatrix.m22 = extents.z * baseScale.z;
		Matrix4x4 val = mat * boxScaleMatrix;
		for (int i = 0; i < 8; i++)
		{
			pos[posOffset + i] = val.MultiplyPoint(boxUnitVertices[i]);
		}
		int[] array = ((mat.m00 * mat.m11 * mat.m22 * baseScale.x * baseScale.y * baseScale.z >= 0f) ? boxIndicesFwd : boxIndicesBack);
		for (int j = 0; j < array.Length; j++)
		{
			indices[indicesOffset + j] = vStart + array[j];
		}
	}

	public static void SetSpriteDefinitionMeshData(Vector3[] pos, int[] indices, int posOffset, int indicesOffset, int vStart, tk2dSpriteDefinition spriteDef, Matrix4x4 mat, Vector3 baseScale)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < spriteDef.colliderVertices.Length; i++)
		{
			Vector3 val = Vector3.Scale(spriteDef.colliderVertices[i], baseScale);
			val = mat.MultiplyPoint(val);
			pos[posOffset + i] = val;
		}
		int[] array = ((mat.m00 * mat.m11 * mat.m22 >= 0f) ? spriteDef.colliderIndicesFwd : spriteDef.colliderIndicesBack);
		for (int j = 0; j < array.Length; j++)
		{
			indices[indicesOffset + j] = vStart + array[j];
		}
	}

	public static void SetSpriteVertexNormals(Vector3[] pos, Vector3 pMin, Vector3 pMax, Vector3[] spriteDefNormals, Vector4[] spriteDefTangents, Vector3[] normals, Vector4[] tangents)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = pMax - pMin;
		int num = pos.Length;
		for (int i = 0; i < num; i++)
		{
			Vector3 val2 = pos[i];
			float num2 = (val2.x - pMin.x) / val.x;
			float num3 = (val2.y - pMin.y) / val.y;
			float num4 = (1f - num2) * (1f - num3);
			float num5 = num2 * (1f - num3);
			float num6 = (1f - num2) * num3;
			float num7 = num2 * num3;
			if (spriteDefNormals != null && spriteDefNormals.Length == 4 && i < normals.Length)
			{
				normals[i] = spriteDefNormals[0] * num4 + spriteDefNormals[1] * num5 + spriteDefNormals[2] * num6 + spriteDefNormals[3] * num7;
			}
			if (spriteDefTangents != null && spriteDefTangents.Length == 4 && i < tangents.Length)
			{
				tangents[i] = spriteDefTangents[0] * num4 + spriteDefTangents[1] * num5 + spriteDefTangents[2] * num6 + spriteDefTangents[3] * num7;
			}
		}
	}
}

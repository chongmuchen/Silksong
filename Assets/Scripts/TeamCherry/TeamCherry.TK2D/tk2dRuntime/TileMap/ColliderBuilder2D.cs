using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace tk2dRuntime.TileMap
{

public static class ColliderBuilder2D
{
	public static void Build(tk2dTileMap tileMap, bool forceBuild)
	{
		bool flag = !forceBuild;
		int num = tileMap.Layers.Length;
		for (int i = 0; i < num; i++)
		{
			Layer layer = tileMap.Layers[i];
			if (layer.IsEmpty || !tileMap.data.Layers[i].generateCollider)
			{
				continue;
			}
			for (int j = 0; j < layer.numRows; j++)
			{
				int baseY = j * layer.divY;
				for (int k = 0; k < layer.numColumns; k++)
				{
					int baseX = k * layer.divX;
					SpriteChunk chunk = layer.GetChunk(k, j);
					if ((flag && !chunk.Dirty) || chunk.IsEmpty)
					{
						continue;
					}
					BuildForChunk(tileMap, chunk, baseX, baseY);
					PhysicsMaterial2D physicsMaterial2D = tileMap.data.Layers[i].physicsMaterial2D;
					foreach (EdgeCollider2D edgeCollider in chunk.edgeColliders)
					{
						if ((Object)(object)edgeCollider != (Object)null)
						{
							((Collider2D)edgeCollider).sharedMaterial = physicsMaterial2D;
						}
					}
				}
			}
		}
	}

	public static void BuildForChunk(tk2dTileMap tileMap, SpriteChunk chunk, int baseX, int baseY)
	{
		Vector2[] vertices = (Vector2[])(object)new Vector2[0];
		int[] indices = new int[0];
		List<Vector2[]> list = new List<Vector2[]>();
		BuildLocalMeshForChunk(tileMap, chunk, baseX, baseY, ref vertices, ref indices);
		if (indices.Length > 4)
		{
			vertices = WeldVertices(vertices, ref indices);
			indices = RemoveDuplicateEdges(indices);
		}
		list = MergeEdges(vertices, indices);
		if ((Object)(object)chunk.meshCollider != (Object)null)
		{
			tk2dUtil.DestroyImmediate((Object)(object)chunk.meshCollider);
			chunk.meshCollider = null;
		}
		if (list.Count == 0)
		{
			for (int i = 0; i < chunk.edgeColliders.Count; i++)
			{
				if ((Object)(object)chunk.edgeColliders[i] != (Object)null)
				{
					tk2dUtil.DestroyImmediate((Object)(object)chunk.edgeColliders[i]);
				}
			}
			chunk.edgeColliders.Clear();
			return;
		}
		int count = list.Count;
		for (int j = count; j < chunk.edgeColliders.Count; j++)
		{
			if ((Object)(object)chunk.edgeColliders[j] != (Object)null)
			{
				tk2dUtil.DestroyImmediate((Object)(object)chunk.edgeColliders[j]);
			}
		}
		int num = chunk.edgeColliders.Count - count;
		if (num > 0)
		{
			chunk.edgeColliders.RemoveRange(chunk.edgeColliders.Count - num, num);
		}
		for (int k = 0; k < chunk.edgeColliders.Count; k++)
		{
			if ((Object)(object)chunk.edgeColliders[k] == (Object)null)
			{
				chunk.edgeColliders[k] = tk2dUtil.AddComponent<EdgeCollider2D>(chunk.gameObject);
			}
		}
		while (chunk.edgeColliders.Count < count)
		{
			chunk.edgeColliders.Add(tk2dUtil.AddComponent<EdgeCollider2D>(chunk.gameObject));
		}
		for (int l = 0; l < count; l++)
		{
			chunk.edgeColliders[l].points = list[l];
		}
	}

	private static void BuildLocalMeshForChunk(tk2dTileMap tileMap, SpriteChunk chunk, int baseX, int baseY, ref Vector2[] vertices, ref int[] indices)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		List<Vector2> list = new List<Vector2>();
		List<int> list2 = new List<int>();
		Vector2[] array = (Vector2[])(object)new Vector2[4];
		int[] array2 = new int[8] { 0, 1, 1, 2, 2, 3, 3, 0 };
		int[] array3 = new int[8] { 0, 3, 3, 2, 2, 1, 1, 0 };
		int num = tileMap.SpriteCollectionInst.spriteDefinitions.Length;
		Vector2 val = (Vector2)(new Vector3(tileMap.data.tileSize.x, tileMap.data.tileSize.y));
		GameObject[] tilePrefabs = tileMap.data.tilePrefabs;
		float x = 0f;
		float y = 0f;
		tileMap.data.GetTileOffset(out x, out y);
		int[] spriteIds = chunk.spriteIds;
		Vector2 val2 = default(Vector2);
		for (int i = 0; i < tileMap.partitionSizeY; i++)
		{
			float num2 = (float)((baseY + i) & 1) * x;
			for (int j = 0; j < tileMap.partitionSizeX; j++)
			{
				int rawTile = spriteIds[i * tileMap.partitionSizeX + j];
				int tileFromRawTile = BuilderUtil.GetTileFromRawTile(rawTile);
				val2 = new Vector2(val.x * ((float)j + num2), val.y * (float)i);
				if (tileFromRawTile < 0 || tileFromRawTile >= num || (tilePrefabs[tileFromRawTile] != null))
				{
					continue;
				}
				bool flag = BuilderUtil.IsRawTileFlagSet(rawTile, tk2dTileFlags.FlipX);
				bool flag2 = BuilderUtil.IsRawTileFlagSet(rawTile, tk2dTileFlags.FlipY);
				bool rot = BuilderUtil.IsRawTileFlagSet(rawTile, tk2dTileFlags.Rot90);
				bool flag3 = false;
				if (flag)
				{
					flag3 = !flag3;
				}
				if (flag2)
				{
					flag3 = !flag3;
				}
				tk2dSpriteDefinition tk2dSpriteDefinition = tileMap.SpriteCollectionInst.spriteDefinitions[tileFromRawTile];
				int count = list.Count;
				if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box)
				{
					Vector3 val3 = tk2dSpriteDefinition.colliderVertices[0];
					Vector3 val4 = tk2dSpriteDefinition.colliderVertices[1];
					Vector3 val5 = val3 - val4;
					Vector3 val6 = val3 + val4;
					array[0] = new Vector2(val5.x, val5.y);
					array[1] = new Vector2(val6.x, val5.y);
					array[2] = new Vector2(val6.x, val6.y);
					array[3] = new Vector2(val5.x, val6.y);
					for (int k = 0; k < 4; k++)
					{
						list.Add(BuilderUtil.ApplySpriteVertexTileFlags(tileMap, tk2dSpriteDefinition, array[k], flag, flag2, rot) + val2);
					}
					int[] array4 = (flag3 ? array3 : array2);
					for (int l = 0; l < 8; l++)
					{
						list2.Add(count + array4[l]);
					}
				}
				else
				{
					if (tk2dSpriteDefinition.colliderType != tk2dSpriteDefinition.ColliderType.Mesh)
					{
						continue;
					}
					tk2dCollider2DData[] edgeCollider2D = tk2dSpriteDefinition.edgeCollider2D;
					foreach (tk2dCollider2DData tk2dCollider2DData in edgeCollider2D)
					{
						count = list.Count;
						Vector2[] points = tk2dCollider2DData.points;
						foreach (Vector2 pos in points)
						{
							list.Add(BuilderUtil.ApplySpriteVertexTileFlags(tileMap, tk2dSpriteDefinition, pos, flag, flag2, rot) + val2);
						}
						int num3 = tk2dCollider2DData.points.Length;
						if (flag3)
						{
							for (int num4 = num3 - 1; num4 > 0; num4--)
							{
								list2.Add(count + num4);
								list2.Add(count + num4 - 1);
							}
						}
						else
						{
							for (int num5 = 0; num5 < num3 - 1; num5++)
							{
								list2.Add(count + num5);
								list2.Add(count + num5 + 1);
							}
						}
					}
					edgeCollider2D = tk2dSpriteDefinition.polygonCollider2D;
					foreach (tk2dCollider2DData tk2dCollider2DData2 in edgeCollider2D)
					{
						count = list.Count;
						Vector2[] points = tk2dCollider2DData2.points;
						foreach (Vector2 pos2 in points)
						{
							list.Add(BuilderUtil.ApplySpriteVertexTileFlags(tileMap, tk2dSpriteDefinition, pos2, flag, flag2, rot) + val2);
						}
						int num6 = tk2dCollider2DData2.points.Length;
						if (flag3)
						{
							for (int num7 = num6; num7 > 0; num7--)
							{
								list2.Add(count + num7 % num6);
								list2.Add(count + num7 - 1);
							}
						}
						else
						{
							for (int num8 = 0; num8 < num6; num8++)
							{
								list2.Add(count + num8);
								list2.Add(count + (num8 + 1) % num6);
							}
						}
					}
				}
			}
		}
		vertices = list.ToArray();
		indices = list2.ToArray();
	}

	private static int CompareWeldVertices(Vector2 a, Vector2 b)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		float num = 0.01f;
		float num2 = a.x - b.x;
		if (Mathf.Abs(num2) > num)
		{
			return (int)Mathf.Sign(num2);
		}
		float num3 = a.y - b.y;
		if (Mathf.Abs(num3) > num)
		{
			return (int)Mathf.Sign(num3);
		}
		return 0;
	}

	private static Vector2[] WeldVertices(Vector2[] vertices, ref int[] indices)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		int[] array = new int[vertices.Length];
		for (int i = 0; i < vertices.Length; i++)
		{
			array[i] = i;
		}
		Array.Sort(array, (int a, int b) => CompareWeldVertices(vertices[a], vertices[b]));
		List<Vector2> list = new List<Vector2>();
		int[] array2 = new int[vertices.Length];
		Vector2 val = vertices[array[0]];
		list.Add(val);
		array2[array[0]] = list.Count - 1;
		for (int num = 1; num < array.Length; num++)
		{
			Vector2 val2 = vertices[array[num]];
			if (CompareWeldVertices(val2, val) != 0)
			{
				val = val2;
				list.Add(val);
				array2[array[num]] = list.Count - 1;
			}
			array2[array[num]] = list.Count - 1;
		}
		for (int num2 = 0; num2 < indices.Length; num2++)
		{
			indices[num2] = array2[indices[num2]];
		}
		return list.ToArray();
	}

	private static int CompareDuplicateFaces(int[] indices, int face0index, int face1index)
	{
		for (int i = 0; i < 2; i++)
		{
			int num = indices[face0index + i] - indices[face1index + i];
			if (num != 0)
			{
				return num;
			}
		}
		return 0;
	}

	private static int[] RemoveDuplicateEdges(int[] indices)
	{
		int[] sortedFaceIndices = new int[indices.Length];
		for (int i = 0; i < indices.Length; i += 2)
		{
			if (indices[i] > indices[i + 1])
			{
				sortedFaceIndices[i] = indices[i + 1];
				sortedFaceIndices[i + 1] = indices[i];
			}
			else
			{
				sortedFaceIndices[i] = indices[i];
				sortedFaceIndices[i + 1] = indices[i + 1];
			}
		}
		int[] array = new int[indices.Length / 2];
		for (int j = 0; j < indices.Length; j += 2)
		{
			array[j / 2] = j;
		}
		Array.Sort(array, (int a, int b) => CompareDuplicateFaces(sortedFaceIndices, a, b));
		List<int> list = new List<int>();
		for (int num = 0; num < array.Length; num++)
		{
			if (num != array.Length - 1 && CompareDuplicateFaces(sortedFaceIndices, array[num], array[num + 1]) == 0)
			{
				num++;
				continue;
			}
			for (int num2 = 0; num2 < 2; num2++)
			{
				list.Add(indices[array[num] + num2]);
			}
		}
		return list.ToArray();
	}

	private static List<Vector2[]> MergeEdges(Vector2[] verts, int[] indices)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		List<Vector2[]> list = new List<Vector2[]>();
		List<Vector2> list2 = new List<Vector2>();
		List<int> list3 = new List<int>();
		Vector2 zero = Vector2.zero;
		_ = Vector2.zero;
		bool[] array = new bool[indices.Length / 2];
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i])
				{
					continue;
				}
				array[i] = true;
				int num = indices[i * 2];
				int num2 = indices[i * 2 + 1];
				Vector2 val = verts[num2] - verts[num];
				zero = val.normalized;
				list3.Add(num);
				list3.Add(num2);
				for (int j = i + 1; j < array.Length; j++)
				{
					if (array[j])
					{
						continue;
					}
					int num3 = indices[j * 2];
					if (num3 == num2)
					{
						int num4 = indices[j * 2 + 1];
						val = verts[num4] - verts[num3];
						Vector2 normalized = val.normalized;
						if (Vector2.Dot(normalized, zero) > 0.999f)
						{
							list3.RemoveAt(list3.Count - 1);
						}
						list3.Add(num4);
						array[j] = true;
						zero = normalized;
						j = i;
						num2 = num4;
					}
				}
				flag = true;
				break;
			}
			if (flag)
			{
				list2.Clear();
				list2.Capacity = Mathf.Max(list2.Capacity, list3.Count);
				for (int k = 0; k < list3.Count; k++)
				{
					list2.Add(verts[list3[k]]);
				}
				list.Add(list2.ToArray());
				list3.Clear();
			}
		}
		return list;
	}
}
}

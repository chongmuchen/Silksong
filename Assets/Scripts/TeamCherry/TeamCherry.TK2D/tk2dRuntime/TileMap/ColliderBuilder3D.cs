using System;
using System.Collections.Generic;
using UnityEngine;

namespace tk2dRuntime.TileMap;

public static class ColliderBuilder3D
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
					if ((!flag || chunk.Dirty) && !chunk.IsEmpty)
					{
						BuildForChunk(tileMap, chunk, baseX, baseY);
						PhysicsMaterial physicMaterial = tileMap.data.Layers[i].physicMaterial;
						if ((Object)(object)chunk.meshCollider != (Object)null)
						{
							((Collider)chunk.meshCollider).sharedMaterial = physicMaterial;
						}
					}
				}
			}
		}
	}

	public static void BuildForChunk(tk2dTileMap tileMap, SpriteChunk chunk, int baseX, int baseY)
	{
		Vector3[] vertices = (Vector3[])(object)new Vector3[0];
		int[] indices = new int[0];
		BuildLocalMeshForChunk(tileMap, chunk, baseX, baseY, ref vertices, ref indices);
		if (indices.Length > 6)
		{
			vertices = WeldVertices(vertices, ref indices);
			indices = RemoveDuplicateFaces(indices);
		}
		foreach (EdgeCollider2D edgeCollider in chunk.edgeColliders)
		{
			if ((Object)(object)edgeCollider != (Object)null)
			{
				tk2dUtil.DestroyImmediate((Object)(object)edgeCollider);
			}
		}
		chunk.edgeColliders.Clear();
		if (vertices.Length != 0)
		{
			if ((Object)(object)chunk.colliderMesh != (Object)null)
			{
				tk2dUtil.DestroyImmediate((Object)(object)chunk.colliderMesh);
				chunk.colliderMesh = null;
			}
			if ((Object)(object)chunk.meshCollider == (Object)null)
			{
				chunk.meshCollider = chunk.gameObject.GetComponent<MeshCollider>();
				if ((Object)(object)chunk.meshCollider == (Object)null)
				{
					chunk.meshCollider = tk2dUtil.AddComponent<MeshCollider>(chunk.gameObject);
				}
			}
			chunk.colliderMesh = tk2dUtil.CreateMesh();
			chunk.colliderMesh.vertices = vertices;
			chunk.colliderMesh.triangles = indices;
			chunk.colliderMesh.RecalculateBounds();
			chunk.meshCollider.sharedMesh = chunk.colliderMesh;
		}
		else
		{
			chunk.DestroyColliderData(tileMap);
		}
	}

	private static void BuildLocalMeshForChunk(tk2dTileMap tileMap, SpriteChunk chunk, int baseX, int baseY, ref Vector3[] vertices, ref int[] indices)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		int num = tileMap.SpriteCollectionInst.spriteDefinitions.Length;
		Vector3 tileSize = tileMap.data.tileSize;
		GameObject[] tilePrefabs = tileMap.data.tilePrefabs;
		float x = 0f;
		float y = 0f;
		tileMap.data.GetTileOffset(out x, out y);
		int[] spriteIds = chunk.spriteIds;
		Vector3 val = default(Vector3);
		for (int i = 0; i < tileMap.partitionSizeY; i++)
		{
			float num2 = (float)((baseY + i) & 1) * x;
			for (int j = 0; j < tileMap.partitionSizeX; j++)
			{
				int rawTile = spriteIds[i * tileMap.partitionSizeX + j];
				int tileFromRawTile = BuilderUtil.GetTileFromRawTile(rawTile);
				((Vector3)(ref val))._002Ector(tileSize.x * ((float)j + num2), tileSize.y * (float)i, 0f);
				if (tileFromRawTile < 0 || tileFromRawTile >= num || Object.op_Implicit((Object)(object)tilePrefabs[tileFromRawTile]))
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
					Vector3 val2 = tk2dSpriteDefinition.colliderVertices[0];
					Vector3 val3 = tk2dSpriteDefinition.colliderVertices[1];
					Vector3 val4 = val2 - val3;
					Vector3 val5 = val2 + val3;
					Vector3[] array = (Vector3[])(object)new Vector3[8]
					{
						new Vector3(val4.x, val4.y, val4.z),
						new Vector3(val4.x, val4.y, val5.z),
						new Vector3(val5.x, val4.y, val4.z),
						new Vector3(val5.x, val4.y, val5.z),
						new Vector3(val4.x, val5.y, val4.z),
						new Vector3(val4.x, val5.y, val5.z),
						new Vector3(val5.x, val5.y, val4.z),
						new Vector3(val5.x, val5.y, val5.z)
					};
					for (int k = 0; k < 8; k++)
					{
						Vector3 val6 = BuilderUtil.ApplySpriteVertexTileFlags(tileMap, tk2dSpriteDefinition, array[k], flag, flag2, rot);
						list.Add(val6 + val);
					}
					int[] array2 = new int[24]
					{
						2, 1, 0, 3, 1, 2, 4, 5, 6, 6,
						5, 7, 6, 7, 3, 6, 3, 2, 1, 5,
						4, 0, 1, 4
					};
					for (int l = 0; l < array2.Length; l++)
					{
						int num3 = (flag3 ? (array2.Length - 1 - l) : l);
						list2.Add(count + array2[num3]);
					}
				}
				else if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Mesh)
				{
					for (int m = 0; m < tk2dSpriteDefinition.colliderVertices.Length; m++)
					{
						Vector3 val7 = BuilderUtil.ApplySpriteVertexTileFlags(tileMap, tk2dSpriteDefinition, tk2dSpriteDefinition.colliderVertices[m], flag, flag2, rot);
						list.Add(val7 + val);
					}
					int[] colliderIndicesFwd = tk2dSpriteDefinition.colliderIndicesFwd;
					for (int n = 0; n < colliderIndicesFwd.Length; n++)
					{
						int num4 = (flag3 ? (colliderIndicesFwd.Length - 1 - n) : n);
						list2.Add(count + colliderIndicesFwd[num4]);
					}
				}
			}
		}
		vertices = list.ToArray();
		indices = list2.ToArray();
	}

	private static int CompareWeldVertices(Vector3 a, Vector3 b)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
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
		float num4 = a.z - b.z;
		if (Mathf.Abs(num4) > num)
		{
			return (int)Mathf.Sign(num4);
		}
		return 0;
	}

	private static Vector3[] WeldVertices(Vector3[] vertices, ref int[] indices)
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
		List<Vector3> list = new List<Vector3>();
		int[] array2 = new int[vertices.Length];
		Vector3 val = vertices[array[0]];
		list.Add(val);
		array2[array[0]] = list.Count - 1;
		for (int num = 1; num < array.Length; num++)
		{
			Vector3 val2 = vertices[array[num]];
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
		for (int i = 0; i < 3; i++)
		{
			int num = indices[face0index + i] - indices[face1index + i];
			if (num != 0)
			{
				return num;
			}
		}
		return 0;
	}

	private static int[] RemoveDuplicateFaces(int[] indices)
	{
		int[] sortedFaceIndices = new int[indices.Length];
		for (int i = 0; i < indices.Length; i += 3)
		{
			int[] array = new int[3]
			{
				indices[i],
				indices[i + 1],
				indices[i + 2]
			};
			Array.Sort(array);
			sortedFaceIndices[i] = array[0];
			sortedFaceIndices[i + 1] = array[1];
			sortedFaceIndices[i + 2] = array[2];
		}
		int[] array2 = new int[indices.Length / 3];
		for (int j = 0; j < indices.Length; j += 3)
		{
			array2[j / 3] = j;
		}
		Array.Sort(array2, (int a, int b) => CompareDuplicateFaces(sortedFaceIndices, a, b));
		List<int> list = new List<int>();
		for (int num = 0; num < array2.Length; num++)
		{
			if (num != array2.Length - 1 && CompareDuplicateFaces(sortedFaceIndices, array2[num], array2[num + 1]) == 0)
			{
				num++;
				continue;
			}
			for (int num2 = 0; num2 < 3; num2++)
			{
				list.Add(indices[array2[num] + num2]);
			}
		}
		return list.ToArray();
	}
}

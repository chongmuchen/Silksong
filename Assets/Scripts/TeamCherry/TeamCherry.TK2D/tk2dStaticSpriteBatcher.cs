using System.Collections.Generic;
using UnityEngine;
using tk2dRuntime;

[AddComponentMenu("2D Toolkit/Sprite/tk2dStaticSpriteBatcher")]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteAlways]
public class tk2dStaticSpriteBatcher : MonoBehaviour, ISpriteCollectionForceBuild
{
	public enum Flags
	{
		None = 0,
		GenerateCollider = 1,
		FlattenDepth = 2,
		SortToCamera = 4
	}

	public static int CURRENT_VERSION = 3;

	public int version;

	public tk2dBatchedSprite[] batchedSprites;

	public tk2dTextMeshData[] allTextMeshData;

	public tk2dSpriteCollectionData spriteCollection;

	[SerializeField]
	private Flags flags = Flags.GenerateCollider;

	private Mesh mesh;

	private Mesh colliderMesh;

	[SerializeField]
	private Vector3 _scale = new Vector3(1f, 1f, 1f);

	public bool CheckFlag(Flags mask)
	{
		return (flags & mask) != 0;
	}

	public void SetFlag(Flags mask, bool value)
	{
		if (CheckFlag(mask) != value)
		{
			if (value)
			{
				flags |= mask;
			}
			else
			{
				flags &= ~mask;
			}
			Build();
		}
	}

	private void Awake()
	{
		Build();
	}

	private bool UpgradeData()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (version == CURRENT_VERSION)
		{
			return false;
		}
		if (_scale == Vector3.zero)
		{
			_scale = Vector3.one;
		}
		if (version < 2 && batchedSprites != null)
		{
			tk2dBatchedSprite[] array = batchedSprites;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].parentId = -1;
			}
		}
		if (version < 3)
		{
			if (batchedSprites != null)
			{
				tk2dBatchedSprite[] array = batchedSprites;
				foreach (tk2dBatchedSprite tk2dBatchedSprite2 in array)
				{
					if (tk2dBatchedSprite2.spriteId == -1)
					{
						tk2dBatchedSprite2.type = tk2dBatchedSprite.Type.EmptyGameObject;
						continue;
					}
					tk2dBatchedSprite2.type = tk2dBatchedSprite.Type.Sprite;
					if ((Object)(object)tk2dBatchedSprite2.spriteCollection == (Object)null)
					{
						tk2dBatchedSprite2.spriteCollection = spriteCollection;
					}
				}
				UpdateMatrices();
			}
			spriteCollection = null;
		}
		version = CURRENT_VERSION;
		return true;
	}

	protected void OnDestroy()
	{
		if ((mesh != null))
		{
			Object.Destroy((Object)(object)mesh);
		}
		if ((colliderMesh != null))
		{
			Object.Destroy((Object)(object)colliderMesh);
		}
	}

	public void UpdateMatrices()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		tk2dBatchedSprite[] array = batchedSprites;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].parentId != -1)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			Matrix4x4 val = default(Matrix4x4);
			List<tk2dBatchedSprite> list = new List<tk2dBatchedSprite>(batchedSprites);
			list.Sort((tk2dBatchedSprite a, tk2dBatchedSprite b) => a.parentId.CompareTo(b.parentId));
			{
				foreach (tk2dBatchedSprite item in list)
				{
					val.SetTRS(item.position, item.rotation, item.localScale);
					item.relativeMatrix = ((item.parentId == -1) ? Matrix4x4.identity : batchedSprites[item.parentId].relativeMatrix) * val;
				}
				return;
			}
		}
		array = batchedSprites;
		foreach (tk2dBatchedSprite tk2dBatchedSprite2 in array)
		{
			tk2dBatchedSprite2.relativeMatrix.SetTRS(tk2dBatchedSprite2.position, tk2dBatchedSprite2.rotation, tk2dBatchedSprite2.localScale);
		}
	}

	public void Build()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		UpgradeData();
		if ((Object)(object)mesh == (Object)null)
		{
			mesh = new Mesh();
			((Object)mesh).hideFlags = (HideFlags)52;
			((Component)this).GetComponent<MeshFilter>().mesh = mesh;
		}
		else
		{
			mesh.Clear();
		}
		if ((colliderMesh != null))
		{
			Object.Destroy((Object)(object)colliderMesh);
			colliderMesh = null;
		}
		if (batchedSprites != null && batchedSprites.Length != 0)
		{
			SortBatchedSprites();
			BuildRenderMesh();
			BuildPhysicsMesh();
		}
	}

	private void SortBatchedSprites()
	{
		List<tk2dBatchedSprite> list = new List<tk2dBatchedSprite>();
		List<tk2dBatchedSprite> list2 = new List<tk2dBatchedSprite>();
		List<tk2dBatchedSprite> list3 = new List<tk2dBatchedSprite>();
		tk2dBatchedSprite[] array = batchedSprites;
		foreach (tk2dBatchedSprite tk2dBatchedSprite2 in array)
		{
			if (!tk2dBatchedSprite2.IsDrawn)
			{
				list3.Add(tk2dBatchedSprite2);
				continue;
			}
			Material material = GetMaterial(tk2dBatchedSprite2);
			if ((Object)(object)material != (Object)null)
			{
				if (material.renderQueue == 2000)
				{
					list.Add(tk2dBatchedSprite2);
				}
				else
				{
					list2.Add(tk2dBatchedSprite2);
				}
			}
			else
			{
				list.Add(tk2dBatchedSprite2);
			}
		}
		List<tk2dBatchedSprite> list4 = new List<tk2dBatchedSprite>(list.Count + list2.Count + list3.Count);
		list4.AddRange(list);
		list4.AddRange(list2);
		list4.AddRange(list3);
		Dictionary<tk2dBatchedSprite, int> dictionary = new Dictionary<tk2dBatchedSprite, int>();
		int num = 0;
		foreach (tk2dBatchedSprite item in list4)
		{
			dictionary[item] = num++;
		}
		foreach (tk2dBatchedSprite item2 in list4)
		{
			if (item2.parentId != -1)
			{
				item2.parentId = dictionary[batchedSprites[item2.parentId]];
			}
		}
		batchedSprites = list4.ToArray();
	}

	private Material GetMaterial(tk2dBatchedSprite bs)
	{
		return (Material)(bs.type switch
		{
			tk2dBatchedSprite.Type.EmptyGameObject => null, 
			tk2dBatchedSprite.Type.TextMesh => allTextMeshData[bs.xRefId].font.materialInst, 
			_ => bs.GetSpriteDefinition().materialInst, 
		});
	}

	private void BuildRenderMesh()
	{
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		List<Material> list = new List<Material>();
		List<List<int>> list2 = new List<List<int>>();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = CheckFlag(Flags.FlattenDepth);
		tk2dBatchedSprite[] array = batchedSprites;
		foreach (tk2dBatchedSprite tk2dBatchedSprite2 in array)
		{
			tk2dSpriteDefinition spriteDefinition = tk2dBatchedSprite2.GetSpriteDefinition();
			if (spriteDefinition != null)
			{
				flag |= spriteDefinition.normals != null && spriteDefinition.normals.Length != 0;
				flag2 |= spriteDefinition.tangents != null && spriteDefinition.tangents.Length != 0;
			}
			if (tk2dBatchedSprite2.type == tk2dBatchedSprite.Type.TextMesh)
			{
				tk2dTextMeshData tk2dTextMeshData2 = allTextMeshData[tk2dBatchedSprite2.xRefId];
				if ((Object)(object)tk2dTextMeshData2.font != (Object)null && tk2dTextMeshData2.font.inst.textureGradients)
				{
					flag3 = true;
				}
			}
		}
		List<int> list3 = new List<int>();
		List<int> list4 = new List<int>();
		int num = 0;
		array = batchedSprites;
		foreach (tk2dBatchedSprite tk2dBatchedSprite3 in array)
		{
			if (!tk2dBatchedSprite3.IsDrawn)
			{
				break;
			}
			tk2dSpriteDefinition spriteDefinition2 = tk2dBatchedSprite3.GetSpriteDefinition();
			int numVertices = 0;
			int numIndices = 0;
			switch (tk2dBatchedSprite3.type)
			{
			case tk2dBatchedSprite.Type.Sprite:
				if (spriteDefinition2 != null)
				{
					tk2dSpriteGeomGen.GetSpriteGeomDesc(out numVertices, out numIndices, spriteDefinition2);
				}
				break;
			case tk2dBatchedSprite.Type.TiledSprite:
				if (spriteDefinition2 != null)
				{
					tk2dSpriteGeomGen.GetTiledSpriteGeomDesc(out numVertices, out numIndices, spriteDefinition2, tk2dBatchedSprite3.Dimensions);
				}
				break;
			case tk2dBatchedSprite.Type.SlicedSprite:
				if (spriteDefinition2 != null)
				{
					tk2dSpriteGeomGen.GetSlicedSpriteGeomDesc(out numVertices, out numIndices, spriteDefinition2, tk2dBatchedSprite3.CheckFlag(tk2dBatchedSprite.Flags.SlicedSprite_BorderOnly));
				}
				break;
			case tk2dBatchedSprite.Type.ClippedSprite:
				if (spriteDefinition2 != null)
				{
					tk2dSpriteGeomGen.GetClippedSpriteGeomDesc(out numVertices, out numIndices, spriteDefinition2);
				}
				break;
			case tk2dBatchedSprite.Type.TextMesh:
			{
				tk2dTextMeshData tk2dTextMeshData3 = allTextMeshData[tk2dBatchedSprite3.xRefId];
				tk2dTextGeomGen.GetTextMeshGeomDesc(out numVertices, out numIndices, tk2dTextGeomGen.Data(tk2dTextMeshData3, tk2dTextMeshData3.font.inst, tk2dBatchedSprite3.FormattedText));
				break;
			}
			}
			num += numVertices;
			list3.Add(numVertices);
			list4.Add(numIndices);
		}
		Vector3[] array2 = (Vector3[])(object)(flag ? new Vector3[num] : null);
		Vector4[] array3 = (Vector4[])(object)(flag2 ? new Vector4[num] : null);
		Vector3[] array4 = (Vector3[])(object)new Vector3[num];
		Color32[] array5 = (Color32[])(object)new Color32[num];
		Vector2[] array6 = (Vector2[])(object)new Vector2[num];
		Vector2[] array7 = (Vector2[])(object)(flag3 ? new Vector2[num] : null);
		int num2 = 0;
		Material val = null;
		List<int> list5 = null;
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = _scale.x;
		identity.m11 = _scale.y;
		identity.m22 = _scale.z;
		int num3 = 0;
		array = batchedSprites;
		Vector3 val7 = default(Vector3);
		foreach (tk2dBatchedSprite tk2dBatchedSprite4 in array)
		{
			if (!tk2dBatchedSprite4.IsDrawn)
			{
				break;
			}
			if (tk2dBatchedSprite4.type == tk2dBatchedSprite.Type.EmptyGameObject)
			{
				num3++;
				continue;
			}
			tk2dSpriteDefinition spriteDefinition3 = tk2dBatchedSprite4.GetSpriteDefinition();
			int num4 = list3[num3];
			int num5 = list4[num3];
			Material material = GetMaterial(tk2dBatchedSprite4);
			if ((Object)(object)material != (Object)(object)val)
			{
				if ((Object)(object)val != (Object)null)
				{
					list.Add(val);
					list2.Add(list5);
				}
				val = material;
				list5 = new List<int>();
			}
			Vector3[] array8 = (Vector3[])(object)new Vector3[num4];
			Vector2[] array9 = (Vector2[])(object)new Vector2[num4];
			Vector2[] array10 = (Vector2[])(object)(flag3 ? new Vector2[num4] : null);
			Color32[] array11 = (Color32[])(object)new Color32[num4];
			Vector3[] array12 = (Vector3[])(object)(flag ? new Vector3[num4] : null);
			Vector4[] array13 = (Vector4[])(object)(flag2 ? new Vector4[num4] : null);
			int[] array14 = new int[num5];
			Vector3 boundsCenter = Vector3.zero;
			Vector3 boundsExtents = Vector3.zero;
			switch (tk2dBatchedSprite4.type)
			{
			case tk2dBatchedSprite.Type.Sprite:
				if (spriteDefinition3 != null)
				{
					tk2dSpriteGeomGen.SetSpriteGeom(array8, array9, array12, array13, 0, spriteDefinition3, Vector3.one);
					tk2dSpriteGeomGen.SetSpriteIndices(array14, 0, num2, spriteDefinition3);
				}
				break;
			case tk2dBatchedSprite.Type.TiledSprite:
				if (spriteDefinition3 != null)
				{
					tk2dSpriteGeomGen.SetTiledSpriteGeom(array8, array9, 0, out boundsCenter, out boundsExtents, spriteDefinition3, Vector3.one, tk2dBatchedSprite4.Dimensions, tk2dBatchedSprite4.anchor, tk2dBatchedSprite4.BoxColliderOffsetZ, tk2dBatchedSprite4.BoxColliderExtentZ);
					tk2dSpriteGeomGen.SetTiledSpriteIndices(array14, 0, num2, spriteDefinition3, tk2dBatchedSprite4.Dimensions);
				}
				break;
			case tk2dBatchedSprite.Type.SlicedSprite:
				if (spriteDefinition3 != null)
				{
					tk2dSpriteGeomGen.SetSlicedSpriteGeom(array8, array9, 0, out boundsCenter, out boundsExtents, spriteDefinition3, Vector3.one, tk2dBatchedSprite4.Dimensions, tk2dBatchedSprite4.SlicedSpriteBorderBottomLeft, tk2dBatchedSprite4.SlicedSpriteBorderTopRight, tk2dBatchedSprite4.anchor, tk2dBatchedSprite4.BoxColliderOffsetZ, tk2dBatchedSprite4.BoxColliderExtentZ);
					tk2dSpriteGeomGen.SetSlicedSpriteIndices(array14, 0, num2, spriteDefinition3, tk2dBatchedSprite4.CheckFlag(tk2dBatchedSprite.Flags.SlicedSprite_BorderOnly));
				}
				break;
			case tk2dBatchedSprite.Type.ClippedSprite:
				if (spriteDefinition3 != null)
				{
					tk2dSpriteGeomGen.SetClippedSpriteGeom(array8, array9, 0, out boundsCenter, out boundsExtents, spriteDefinition3, Vector3.one, tk2dBatchedSprite4.ClippedSpriteRegionBottomLeft, tk2dBatchedSprite4.ClippedSpriteRegionTopRight, tk2dBatchedSprite4.BoxColliderOffsetZ, tk2dBatchedSprite4.BoxColliderExtentZ);
					tk2dSpriteGeomGen.SetClippedSpriteIndices(array14, 0, num2, spriteDefinition3);
				}
				break;
			case tk2dBatchedSprite.Type.TextMesh:
			{
				tk2dTextMeshData tk2dTextMeshData4 = allTextMeshData[tk2dBatchedSprite4.xRefId];
				tk2dTextGeomGen.GeomData geomData = tk2dTextGeomGen.Data(tk2dTextMeshData4, tk2dTextMeshData4.font.inst, tk2dBatchedSprite4.FormattedText);
				int target = tk2dTextGeomGen.SetTextMeshGeom(array8, array9, array10, array11, 0, geomData);
				if (!geomData.fontInst.isPacked)
				{
					Color32 val2 = (Color32)(tk2dTextMeshData4.color);
					Color32 val3 = (Color32)(tk2dTextMeshData4.useGradient ? tk2dTextMeshData4.color2 : tk2dTextMeshData4.color);
					for (int j = 0; j < array11.Length; j++)
					{
						Color32 val4 = ((j % 4 < 2) ? val2 : val3);
						byte b = (byte)(array11[j].r * val4.r / 255);
						byte b2 = (byte)(array11[j].g * val4.g / 255);
						byte b3 = (byte)(array11[j].b * val4.b / 255);
						byte b4 = (byte)(array11[j].a * val4.a / 255);
						if (geomData.fontInst.premultipliedAlpha)
						{
							b = (byte)(b * b4 / 255);
							b2 = (byte)(b2 * b4 / 255);
							b3 = (byte)(b3 * b4 / 255);
						}
						array11[j] = new Color32(b, b2, b3, b4);
					}
				}
				tk2dTextGeomGen.SetTextMeshIndices(array14, 0, num2, geomData, target);
				break;
			}
			}
			tk2dBatchedSprite4.CachedBoundsCenter = boundsCenter;
			tk2dBatchedSprite4.CachedBoundsExtents = boundsExtents;
			if (num4 > 0 && tk2dBatchedSprite4.type != tk2dBatchedSprite.Type.TextMesh)
			{
				bool premulAlpha = (Object)(object)tk2dBatchedSprite4.spriteCollection != (Object)null && tk2dBatchedSprite4.spriteCollection.premultipliedAlpha;
				tk2dSpriteGeomGen.SetSpriteColors(array11, 0, num4, tk2dBatchedSprite4.color, premulAlpha);
			}
			Matrix4x4 val5 = identity * tk2dBatchedSprite4.relativeMatrix;
			for (int k = 0; k < num4; k++)
			{
				Vector3 val6 = Vector3.Scale(array8[k], tk2dBatchedSprite4.baseScale);
				val6 = val5.MultiplyPoint(val6);
				if (flag4)
				{
					val6.z = 0f;
				}
				array4[num2 + k] = val6;
				array6[num2 + k] = array9[k];
				if (flag3)
				{
					array7[num2 + k] = array10[k];
				}
				array5[num2 + k] = array11[k];
				if (flag)
				{
					array2[num2 + k] = tk2dBatchedSprite4.rotation * array12[k];
				}
				if (flag2)
				{
					val7 = new Vector3(array13[k].x, array13[k].y, array13[k].z);
					val7 = tk2dBatchedSprite4.rotation * val7;
					array3[num2 + k] = new Vector4(val7.x, val7.y, val7.z, array13[k].w);
				}
			}
			list5.AddRange(array14);
			num2 += num4;
			num3++;
		}
		if (list5 != null)
		{
			list.Add(val);
			list2.Add(list5);
		}
		if ((mesh != null))
		{
			mesh.vertices = array4;
			mesh.uv = array6;
			if (flag3)
			{
				mesh.uv2 = array7;
			}
			mesh.colors32 = array5;
			if (flag)
			{
				mesh.normals = array2;
			}
			if (flag2)
			{
				mesh.tangents = array3;
			}
			mesh.subMeshCount = list2.Count;
			for (int l = 0; l < list2.Count; l++)
			{
				mesh.SetTriangles(list2[l].ToArray(), l);
			}
			mesh.RecalculateBounds();
		}
		((Component)this).GetComponent<Renderer>().sharedMaterials = list.ToArray();
	}

	private void BuildPhysicsMesh()
	{
		MeshCollider component = ((Component)this).GetComponent<MeshCollider>();
		if ((Object)(object)component != (Object)null)
		{
			if ((Object)(object)((Component)this).GetComponent<Collider>() != (Object)(object)component)
			{
				return;
			}
			if (!CheckFlag(Flags.GenerateCollider))
			{
				Object.Destroy((Object)(object)component);
			}
		}
		EdgeCollider2D[] components = ((Component)this).GetComponents<EdgeCollider2D>();
		if (!CheckFlag(Flags.GenerateCollider))
		{
			EdgeCollider2D[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy((Object)(object)array[i]);
			}
		}
		if (!CheckFlag(Flags.GenerateCollider))
		{
			return;
		}
		bool flattenDepth = CheckFlag(Flags.FlattenDepth);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		bool flag = true;
		tk2dBatchedSprite[] array2 = batchedSprites;
		foreach (tk2dBatchedSprite tk2dBatchedSprite2 in array2)
		{
			if (!tk2dBatchedSprite2.IsDrawn)
			{
				break;
			}
			tk2dSpriteDefinition spriteDefinition = tk2dBatchedSprite2.GetSpriteDefinition();
			bool flag2 = false;
			bool flag3 = false;
			switch (tk2dBatchedSprite2.type)
			{
			case tk2dBatchedSprite.Type.Sprite:
				if (spriteDefinition != null && spriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Mesh)
				{
					flag2 = true;
				}
				if (spriteDefinition != null && spriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box)
				{
					flag3 = true;
				}
				break;
			case tk2dBatchedSprite.Type.TiledSprite:
			case tk2dBatchedSprite.Type.SlicedSprite:
			case tk2dBatchedSprite.Type.ClippedSprite:
				flag3 = tk2dBatchedSprite2.CheckFlag(tk2dBatchedSprite.Flags.Sprite_CreateBoxCollider);
				break;
			}
			if (flag2)
			{
				num += spriteDefinition.colliderIndicesFwd.Length;
				num2 += spriteDefinition.colliderVertices.Length;
				num3 += spriteDefinition.edgeCollider2D.Length;
				num3 += spriteDefinition.polygonCollider2D.Length;
			}
			else if (flag3)
			{
				num += 36;
				num2 += 8;
				num3++;
			}
			if (spriteDefinition.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics2D)
			{
				flag = false;
			}
		}
		if ((flag && num == 0) || (!flag && num3 == 0))
		{
			if ((Object)(object)colliderMesh != (Object)null)
			{
				Object.Destroy((Object)(object)colliderMesh);
				colliderMesh = null;
			}
			if ((Object)(object)component != (Object)null)
			{
				Object.Destroy((Object)(object)component);
			}
			EdgeCollider2D[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy((Object)(object)array[i]);
			}
			return;
		}
		if (flag)
		{
			EdgeCollider2D[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy((Object)(object)array[i]);
			}
		}
		else
		{
			if ((Object)(object)colliderMesh != (Object)null)
			{
				Object.Destroy((Object)(object)colliderMesh);
			}
			if ((Object)(object)component != (Object)null)
			{
				Object.Destroy((Object)(object)component);
			}
		}
		if (flag)
		{
			BuildPhysicsMesh3D(component, flattenDepth, num2, num);
		}
		else
		{
			BuildPhysicsMesh2D(components, num3);
		}
	}

	private void BuildPhysicsMesh2D(EdgeCollider2D[] edgeColliders, int numEdgeColliders)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		for (int i = numEdgeColliders; i < edgeColliders.Length; i++)
		{
			Object.Destroy((Object)(object)edgeColliders[i]);
		}
		Vector2[] array = (Vector2[])(object)new Vector2[5];
		if (numEdgeColliders > edgeColliders.Length)
		{
			EdgeCollider2D[] array2 = (EdgeCollider2D[])(object)new EdgeCollider2D[numEdgeColliders];
			int num = Mathf.Min(numEdgeColliders, edgeColliders.Length);
			for (int j = 0; j < num; j++)
			{
				array2[j] = edgeColliders[j];
			}
			for (int k = num; k < numEdgeColliders; k++)
			{
				array2[k] = ((Component)this).gameObject.AddComponent<EdgeCollider2D>();
			}
			edgeColliders = array2;
		}
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = _scale.x;
		identity.m11 = _scale.y;
		identity.m22 = _scale.z;
		int num2 = 0;
		tk2dBatchedSprite[] array3 = batchedSprites;
		foreach (tk2dBatchedSprite tk2dBatchedSprite2 in array3)
		{
			if (!tk2dBatchedSprite2.IsDrawn)
			{
				break;
			}
			tk2dSpriteDefinition spriteDefinition = tk2dBatchedSprite2.GetSpriteDefinition();
			bool flag = false;
			bool flag2 = false;
			Vector3 val = Vector3.zero;
			Vector3 val2 = Vector3.zero;
			switch (tk2dBatchedSprite2.type)
			{
			case tk2dBatchedSprite.Type.Sprite:
				if (spriteDefinition != null && spriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Mesh)
				{
					flag = true;
				}
				if (spriteDefinition != null && spriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box)
				{
					flag2 = true;
					val = spriteDefinition.colliderVertices[0];
					val2 = spriteDefinition.colliderVertices[1];
				}
				break;
			case tk2dBatchedSprite.Type.TiledSprite:
			case tk2dBatchedSprite.Type.SlicedSprite:
			case tk2dBatchedSprite.Type.ClippedSprite:
				flag2 = tk2dBatchedSprite2.CheckFlag(tk2dBatchedSprite.Flags.Sprite_CreateBoxCollider);
				if (flag2)
				{
					val = tk2dBatchedSprite2.CachedBoundsCenter;
					val2 = tk2dBatchedSprite2.CachedBoundsExtents;
				}
				break;
			}
			Matrix4x4 val3 = identity * tk2dBatchedSprite2.relativeMatrix;
			if (flag)
			{
				tk2dCollider2DData[] edgeCollider2D = spriteDefinition.edgeCollider2D;
				foreach (tk2dCollider2DData tk2dCollider2DData2 in edgeCollider2D)
				{
					Vector2[] array4 = (Vector2[])(object)new Vector2[tk2dCollider2DData2.points.Length];
					for (int n = 0; n < tk2dCollider2DData2.points.Length; n++)
					{
						array4[n] = (Vector2)(val3.MultiplyPoint((Vector2)(tk2dCollider2DData2.points[n])));
					}
					edgeColliders[num2].points = array4;
				}
				edgeCollider2D = spriteDefinition.polygonCollider2D;
				foreach (tk2dCollider2DData tk2dCollider2DData3 in edgeCollider2D)
				{
					Vector2[] array5 = (Vector2[])(object)new Vector2[tk2dCollider2DData3.points.Length + 1];
					for (int num3 = 0; num3 < tk2dCollider2DData3.points.Length; num3++)
					{
						array5[num3] = (Vector2)(val3.MultiplyPoint((Vector2)(tk2dCollider2DData3.points[num3])));
					}
					array5[tk2dCollider2DData3.points.Length] = array5[0];
					edgeColliders[num2].points = array5;
				}
				num2++;
			}
			else if (flag2)
			{
				Vector3 val4 = val - val2;
				Vector3 val5 = val + val2;
				array[0] = (Vector2)(val3.MultiplyPoint((Vector2)(new Vector2(val4.x, val4.y))));
				array[1] = (Vector2)(val3.MultiplyPoint((Vector2)(new Vector2(val5.x, val4.y))));
				array[2] = (Vector2)(val3.MultiplyPoint((Vector2)(new Vector2(val5.x, val5.y))));
				array[3] = (Vector2)(val3.MultiplyPoint((Vector2)(new Vector2(val4.x, val5.y))));
				array[4] = array[0];
				edgeColliders[num2].points = array;
				num2++;
			}
		}
	}

	private void BuildPhysicsMesh3D(MeshCollider meshCollider, bool flattenDepth, int numVertices, int numIndices)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)meshCollider == (Object)null)
		{
			meshCollider = ((Component)this).gameObject.AddComponent<MeshCollider>();
		}
		if ((Object)(object)colliderMesh == (Object)null)
		{
			colliderMesh = new Mesh();
			((Object)colliderMesh).hideFlags = (HideFlags)52;
		}
		else
		{
			colliderMesh.Clear();
		}
		int num = 0;
		Vector3[] array = (Vector3[])(object)new Vector3[numVertices];
		int num2 = 0;
		int[] array2 = new int[numIndices];
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = _scale.x;
		identity.m11 = _scale.y;
		identity.m22 = _scale.z;
		tk2dBatchedSprite[] array3 = batchedSprites;
		foreach (tk2dBatchedSprite tk2dBatchedSprite2 in array3)
		{
			if (!tk2dBatchedSprite2.IsDrawn)
			{
				break;
			}
			tk2dSpriteDefinition spriteDefinition = tk2dBatchedSprite2.GetSpriteDefinition();
			bool flag = false;
			bool flag2 = false;
			Vector3 origin = Vector3.zero;
			Vector3 extents = Vector3.zero;
			switch (tk2dBatchedSprite2.type)
			{
			case tk2dBatchedSprite.Type.Sprite:
				if (spriteDefinition != null && spriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Mesh)
				{
					flag = true;
				}
				if (spriteDefinition != null && spriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box)
				{
					flag2 = true;
					origin = spriteDefinition.colliderVertices[0];
					extents = spriteDefinition.colliderVertices[1];
				}
				break;
			case tk2dBatchedSprite.Type.TiledSprite:
			case tk2dBatchedSprite.Type.SlicedSprite:
			case tk2dBatchedSprite.Type.ClippedSprite:
				flag2 = tk2dBatchedSprite2.CheckFlag(tk2dBatchedSprite.Flags.Sprite_CreateBoxCollider);
				if (flag2)
				{
					origin = tk2dBatchedSprite2.CachedBoundsCenter;
					extents = tk2dBatchedSprite2.CachedBoundsExtents;
				}
				break;
			}
			Matrix4x4 mat = identity * tk2dBatchedSprite2.relativeMatrix;
			if (flattenDepth)
			{
				mat.m23 = 0f;
			}
			if (flag)
			{
				tk2dSpriteGeomGen.SetSpriteDefinitionMeshData(array, array2, num, num2, num, spriteDefinition, mat, tk2dBatchedSprite2.baseScale);
				num += spriteDefinition.colliderVertices.Length;
				num2 += spriteDefinition.colliderIndicesFwd.Length;
			}
			else if (flag2)
			{
				tk2dSpriteGeomGen.SetBoxMeshData(array, array2, num, num2, num, origin, extents, mat, tk2dBatchedSprite2.baseScale);
				num += 8;
				num2 += 36;
			}
		}
		colliderMesh.vertices = array;
		colliderMesh.triangles = array2;
		meshCollider.sharedMesh = colliderMesh;
	}

	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		return (Object)(object)this.spriteCollection == (Object)(object)spriteCollection;
	}

	public void ForceBuild()
	{
		Build();
	}
}

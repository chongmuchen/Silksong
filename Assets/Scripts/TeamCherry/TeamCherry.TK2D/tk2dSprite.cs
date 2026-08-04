using System.Collections.Generic;
using TeamCherry.SharedUtils;
using UnityEngine;

[AddComponentMenu("2D Toolkit/Sprite/tk2dSprite")]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteAlways]
public class tk2dSprite : tk2dBaseSprite
{
	private Mesh mesh;

	private Vector3[] meshVertices;

	private Vector3[] meshNormals;

	private Vector4[] meshTangents;

	private Color32[] meshColors;

	private List<string> materialKeywords;

	private Dictionary<int, float> materialFloats;

	private Dictionary<Material, Material> instancedMaterials;

	public bool HasKeywordsDefined
	{
		get
		{
			if (materialKeywords != null)
			{
				return materialKeywords.Count > 0;
			}
			return false;
		}
	}

	private new void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		base.Awake();
		mesh = new Mesh();
		mesh.MarkDynamic();
		((Object)mesh).hideFlags = (HideFlags)52;
		((Component)this).GetComponent<MeshFilter>().sharedMesh = mesh;
		if (Object.op_Implicit((Object)(object)base.Collection))
		{
			if (_spriteId < 0 || _spriteId >= base.Collection.Count)
			{
				_spriteId = 0;
			}
			Build();
		}
	}

	protected void OnDestroy()
	{
		if (Object.op_Implicit((Object)(object)mesh))
		{
			Object.Destroy((Object)(object)mesh);
		}
		if (Object.op_Implicit((Object)(object)meshColliderMesh))
		{
			Object.Destroy((Object)(object)meshColliderMesh);
		}
	}

	public void EnableKeyword(string keyword)
	{
		if (materialKeywords == null)
		{
			materialKeywords = new List<string>();
		}
		materialKeywords.AddIfNotPresent(keyword);
		UpdateMaterial();
	}

	public void DisableKeyword(string keyword)
	{
		if (materialKeywords == null)
		{
			return;
		}
		materialKeywords.Remove(keyword);
		if (instancedMaterials == null)
		{
			return;
		}
		foreach (KeyValuePair<Material, Material> instancedMaterial in instancedMaterials)
		{
			if (instancedMaterial.Value.IsKeywordEnabled(keyword))
			{
				instancedMaterial.Value.DisableKeyword(keyword);
			}
		}
	}

	public bool HasKeyword(string keyword)
	{
		if (materialKeywords == null)
		{
			return false;
		}
		return materialKeywords.Contains(keyword);
	}

	public void MoveKeywords(tk2dSprite otherSprite)
	{
		otherSprite.materialKeywords = materialKeywords;
		materialKeywords = null;
		otherSprite.instancedMaterials = instancedMaterials;
		instancedMaterials = null;
		UpdateMaterial();
	}

	public void SetFloat(int propId, float value)
	{
		if (materialFloats == null)
		{
			materialFloats = new Dictionary<int, float>();
		}
		materialFloats[propId] = value;
		UpdateMaterial();
	}

	public override void Build()
	{
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		tk2dSpriteDefinition tk2dSpriteDefinition2 = collectionInst.spriteDefinitions[base.spriteId];
		meshVertices = (Vector3[])(object)new Vector3[tk2dSpriteDefinition2.positions.Length];
		meshColors = (Color32[])(object)new Color32[tk2dSpriteDefinition2.positions.Length];
		meshNormals = (Vector3[])(object)new Vector3[0];
		meshTangents = (Vector4[])(object)new Vector4[0];
		if (tk2dSpriteDefinition2.normals != null && tk2dSpriteDefinition2.normals.Length != 0)
		{
			meshNormals = (Vector3[])(object)new Vector3[tk2dSpriteDefinition2.normals.Length];
		}
		if (tk2dSpriteDefinition2.tangents != null && tk2dSpriteDefinition2.tangents.Length != 0)
		{
			meshTangents = (Vector4[])(object)new Vector4[tk2dSpriteDefinition2.tangents.Length];
		}
		SetPositions(meshVertices, meshNormals, meshTangents);
		SetColors(meshColors);
		if ((Object)(object)mesh == (Object)null)
		{
			mesh = new Mesh();
			mesh.MarkDynamic();
			((Object)mesh).hideFlags = (HideFlags)52;
			((Component)this).GetComponent<MeshFilter>().sharedMesh = mesh;
		}
		mesh.Clear();
		mesh.vertices = meshVertices;
		mesh.normals = meshNormals;
		mesh.tangents = meshTangents;
		mesh.colors32 = meshColors;
		mesh.uv = tk2dSpriteDefinition2.uvs;
		mesh.triangles = tk2dSpriteDefinition2.indices;
		mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(GetBounds(), renderLayer);
		UpdateMaterial();
		CreateCollider();
	}

	public static tk2dSprite AddComponent(GameObject go, tk2dSpriteCollectionData spriteCollection, int spriteId)
	{
		return tk2dBaseSprite.AddComponent<tk2dSprite>(go, spriteCollection, spriteId);
	}

	public static tk2dSprite AddComponent(GameObject go, tk2dSpriteCollectionData spriteCollection, string spriteName)
	{
		return tk2dBaseSprite.AddComponent<tk2dSprite>(go, spriteCollection, spriteName);
	}

	public static GameObject CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, Rect region, Vector2 anchor)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return tk2dBaseSprite.CreateFromTexture<tk2dSprite>(texture, size, region, anchor);
	}

	protected override void UpdateGeometry()
	{
		UpdateGeometryImpl();
	}

	protected override void UpdateColors()
	{
		UpdateColorsImpl();
	}

	protected override void UpdateVertices()
	{
		UpdateVerticesImpl();
	}

	protected void UpdateColorsImpl()
	{
		if (!((Object)(object)mesh == (Object)null) && meshColors != null && meshColors.Length != 0)
		{
			SetColors(meshColors);
			mesh.colors32 = meshColors;
		}
	}

	protected void UpdateVerticesImpl()
	{
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		tk2dSpriteDefinition tk2dSpriteDefinition2 = collectionInst.spriteDefinitions[base.spriteId];
		if (!((Object)(object)mesh == (Object)null) && meshVertices != null && meshVertices.Length != 0)
		{
			if (tk2dSpriteDefinition2.normals.Length != meshNormals.Length)
			{
				meshNormals = (Vector3[])(object)((tk2dSpriteDefinition2.normals != null && tk2dSpriteDefinition2.normals.Length != 0) ? new Vector3[tk2dSpriteDefinition2.normals.Length] : new Vector3[0]);
			}
			if (tk2dSpriteDefinition2.tangents.Length != meshTangents.Length)
			{
				meshTangents = (Vector4[])(object)((tk2dSpriteDefinition2.tangents != null && tk2dSpriteDefinition2.tangents.Length != 0) ? new Vector4[tk2dSpriteDefinition2.tangents.Length] : new Vector4[0]);
			}
			SetPositions(meshVertices, meshNormals, meshTangents);
			mesh.vertices = meshVertices;
			mesh.normals = meshNormals;
			mesh.tangents = meshTangents;
			mesh.uv = tk2dSpriteDefinition2.uvs;
			mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(GetBounds(), renderLayer);
		}
	}

	protected void UpdateGeometryImpl()
	{
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)mesh == (Object)null))
		{
			tk2dSpriteDefinition tk2dSpriteDefinition2 = collectionInst.spriteDefinitions[base.spriteId];
			if (meshVertices == null || meshVertices.Length != tk2dSpriteDefinition2.positions.Length)
			{
				meshVertices = (Vector3[])(object)new Vector3[tk2dSpriteDefinition2.positions.Length];
				meshColors = (Color32[])(object)new Color32[tk2dSpriteDefinition2.positions.Length];
			}
			if (meshNormals == null || (tk2dSpriteDefinition2.normals != null && meshNormals.Length != tk2dSpriteDefinition2.normals.Length))
			{
				meshNormals = (Vector3[])(object)new Vector3[tk2dSpriteDefinition2.normals.Length];
			}
			else if (tk2dSpriteDefinition2.normals == null)
			{
				meshNormals = (Vector3[])(object)new Vector3[0];
			}
			if (meshTangents == null || (tk2dSpriteDefinition2.tangents != null && meshTangents.Length != tk2dSpriteDefinition2.tangents.Length))
			{
				meshTangents = (Vector4[])(object)new Vector4[tk2dSpriteDefinition2.tangents.Length];
			}
			else if (tk2dSpriteDefinition2.tangents == null)
			{
				meshTangents = (Vector4[])(object)new Vector4[0];
			}
			SetPositions(meshVertices, meshNormals, meshTangents);
			SetColors(meshColors);
			mesh.Clear();
			mesh.vertices = meshVertices;
			mesh.normals = meshNormals;
			mesh.tangents = meshTangents;
			mesh.colors32 = meshColors;
			mesh.uv = tk2dSpriteDefinition2.uvs;
			mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(GetBounds(), renderLayer);
			mesh.triangles = tk2dSpriteDefinition2.indices;
		}
	}

	protected override void UpdateMaterial()
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		Material val = collectionInst.spriteDefinitions[base.spriteId].materialInst;
		bool num = materialKeywords != null && materialKeywords.Count > 0;
		bool flag = materialFloats != null && materialFloats.Count > 0;
		if (num || flag)
		{
			if (instancedMaterials == null)
			{
				instancedMaterials = new Dictionary<Material, Material>();
			}
			if (instancedMaterials.ContainsKey(val))
			{
				val = instancedMaterials[val];
			}
			else
			{
				Material key = val;
				val = new Material(val);
				Material obj = val;
				((Object)obj).name = ((Object)obj).name + " (using keywords)";
				instancedMaterials[key] = val;
			}
			if (materialKeywords != null)
			{
				foreach (string materialKeyword in materialKeywords)
				{
					if (!val.IsKeywordEnabled(materialKeyword))
					{
						val.EnableKeyword(materialKeyword);
					}
				}
			}
			if (materialFloats != null)
			{
				foreach (KeyValuePair<int, float> materialFloat in materialFloats)
				{
					val.SetFloat(materialFloat.Key, materialFloat.Value);
				}
			}
		}
		Renderer component = ((Component)this).GetComponent<Renderer>();
		if ((Object)(object)component.sharedMaterial != (Object)(object)val)
		{
			component.material = val;
		}
	}

	protected override int GetCurrentVertexCount()
	{
		if (meshVertices == null)
		{
			return 0;
		}
		return meshVertices.Length;
	}

	public override void ForceBuild()
	{
		base.ForceBuild();
		((Component)this).GetComponent<MeshFilter>().sharedMesh = mesh;
	}

	public override void ReshapeBounds(Vector3 dMin, Vector3 dMax)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		float num = 0.1f;
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		Vector3 val = default(Vector3);
		((Vector3)(ref val))._002Ector(Mathf.Abs(_scale.x), Mathf.Abs(_scale.y), Mathf.Abs(_scale.z));
		Vector3 val2 = Vector3.Scale(currentSprite.untrimmedBoundsData[0], _scale) - 0.5f * Vector3.Scale(currentSprite.untrimmedBoundsData[1], val);
		Vector3 val3 = Vector3.Scale(currentSprite.untrimmedBoundsData[1], val) + dMax - dMin;
		val3.x /= currentSprite.untrimmedBoundsData[1].x;
		val3.y /= currentSprite.untrimmedBoundsData[1].y;
		if (currentSprite.untrimmedBoundsData[1].x * val3.x < currentSprite.texelSize.x * num && val3.x < val.x)
		{
			dMin.x = 0f;
			val3.x = val.x;
		}
		if (currentSprite.untrimmedBoundsData[1].y * val3.y < currentSprite.texelSize.y * num && val3.y < val.y)
		{
			dMin.y = 0f;
			val3.y = val.y;
		}
		Vector2 val4 = Vector2.op_Implicit(new Vector3(Mathf.Approximately(val.x, 0f) ? 0f : (val3.x / val.x), Mathf.Approximately(val.y, 0f) ? 0f : (val3.y / val.y)));
		Vector3 val5 = default(Vector3);
		((Vector3)(ref val5))._002Ector(val2.x * val4.x, val2.y * val4.y);
		Vector3 val6 = dMin + val2 - val5;
		val6.z = 0f;
		((Component)this).transform.position = ((Component)this).transform.TransformPoint(val6);
		base.scale = new Vector3(_scale.x * val4.x, _scale.y * val4.y, _scale.z);
	}
}

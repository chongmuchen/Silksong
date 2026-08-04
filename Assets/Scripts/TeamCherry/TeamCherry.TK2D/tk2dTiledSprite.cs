using UnityEngine;

[AddComponentMenu("2D Toolkit/Sprite/tk2dTiledSprite")]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteAlways]
public class tk2dTiledSprite : tk2dBaseSprite
{
	private Mesh mesh;

	private Vector2[] meshUvs;

	private Vector3[] meshVertices;

	private Color32[] meshColors;

	private Vector3[] meshNormals;

	private Vector4[] meshTangents;

	private int[] meshIndices;

	[SerializeField]
	private Vector2 _dimensions = new Vector2(50f, 50f);

	[SerializeField]
	private Anchor _anchor;

	[SerializeField]
	protected bool _createBoxCollider;

	private Vector3 boundsCenter = Vector3.zero;

	private Vector3 boundsExtents = Vector3.zero;

	public Vector2 dimensions
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _dimensions;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (value != _dimensions)
			{
				_dimensions = value;
				UpdateVertices();
				UpdateCollider();
			}
		}
	}

	public Anchor anchor
	{
		get
		{
			return _anchor;
		}
		set
		{
			if (value != _anchor)
			{
				_anchor = value;
				UpdateVertices();
				UpdateCollider();
			}
		}
	}

	public bool CreateBoxCollider
	{
		get
		{
			return _createBoxCollider;
		}
		set
		{
			if (_createBoxCollider != value)
			{
				_createBoxCollider = value;
				UpdateCollider();
			}
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
		((Component)this).GetComponent<MeshFilter>().mesh = mesh;
		if ((base.Collection != null))
		{
			if (_spriteId < 0 || _spriteId >= base.Collection.Count)
			{
				_spriteId = 0;
			}
			Build();
			if ((Object)(object)boxCollider == (Object)null)
			{
				boxCollider = ((Component)this).GetComponent<BoxCollider>();
			}
			if ((Object)(object)boxCollider2D == (Object)null)
			{
				boxCollider2D = ((Component)this).GetComponent<BoxCollider2D>();
			}
		}
	}

	protected void OnDestroy()
	{
		if ((mesh != null))
		{
			Object.Destroy((Object)(object)mesh);
		}
	}

	protected new void SetColors(Color32[] dest)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		tk2dSpriteGeomGen.GetTiledSpriteGeomDesc(out var numVertices, out var _, base.CurrentSprite, dimensions);
		tk2dSpriteGeomGen.SetSpriteColors(dest, 0, numVertices, _color, collectionInst.premultipliedAlpha);
	}

	public override void Build()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Expected O, but got Unknown
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		tk2dSpriteGeomGen.GetTiledSpriteGeomDesc(out var numVertices, out var numIndices, currentSprite, dimensions);
		if (meshUvs == null || meshUvs.Length != numVertices)
		{
			meshUvs = (Vector2[])(object)new Vector2[numVertices];
			meshVertices = (Vector3[])(object)new Vector3[numVertices];
			meshColors = (Color32[])(object)new Color32[numVertices];
		}
		if (meshIndices == null || meshIndices.Length != numIndices)
		{
			meshIndices = new int[numIndices];
		}
		meshNormals = (Vector3[])(object)new Vector3[0];
		meshTangents = (Vector4[])(object)new Vector4[0];
		if (currentSprite.normals != null && currentSprite.normals.Length != 0)
		{
			meshNormals = (Vector3[])(object)new Vector3[numVertices];
		}
		if (currentSprite.tangents != null && currentSprite.tangents.Length != 0)
		{
			meshTangents = (Vector4[])(object)new Vector4[numVertices];
		}
		float colliderOffsetZ = (((Object)(object)boxCollider != (Object)null) ? boxCollider.center.z : 0f);
		float colliderExtentZ = (((Object)(object)boxCollider != (Object)null) ? (boxCollider.size.z * 0.5f) : 0.5f);
		tk2dSpriteGeomGen.SetTiledSpriteGeom(meshVertices, meshUvs, 0, out boundsCenter, out boundsExtents, currentSprite, _scale, dimensions, anchor, colliderOffsetZ, colliderExtentZ);
		tk2dSpriteGeomGen.SetTiledSpriteIndices(meshIndices, 0, 0, currentSprite, dimensions);
		if (meshNormals.Length != 0 || meshTangents.Length != 0)
		{
			Vector3 pMin = default(Vector3);
			pMin = new Vector3(currentSprite.positions[0].x * dimensions.x * currentSprite.texelSize.x * base.scale.x, currentSprite.positions[0].y * dimensions.y * currentSprite.texelSize.y * base.scale.y);
			Vector3 pMax = default(Vector3);
			pMax = new Vector3(currentSprite.positions[3].x * dimensions.x * currentSprite.texelSize.x * base.scale.x, currentSprite.positions[3].y * dimensions.y * currentSprite.texelSize.y * base.scale.y);
			tk2dSpriteGeomGen.SetSpriteVertexNormals(meshVertices, pMin, pMax, currentSprite.normals, currentSprite.tangents, meshNormals, meshTangents);
		}
		SetColors(meshColors);
		if ((Object)(object)mesh == (Object)null)
		{
			mesh = new Mesh();
			mesh.MarkDynamic();
			((Object)mesh).hideFlags = (HideFlags)52;
		}
		else
		{
			mesh.Clear();
		}
		mesh.vertices = meshVertices;
		mesh.colors32 = meshColors;
		mesh.uv = meshUvs;
		mesh.normals = meshNormals;
		mesh.tangents = meshTangents;
		mesh.triangles = meshIndices;
		mesh.RecalculateBounds();
		mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(mesh.bounds, renderLayer);
		((Component)this).GetComponent<MeshFilter>().mesh = mesh;
		UpdateCollider();
		UpdateMaterial();
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
		UpdateGeometryImpl();
	}

	protected void UpdateColorsImpl()
	{
		if (meshColors == null || meshColors.Length == 0)
		{
			Build();
			return;
		}
		SetColors(meshColors);
		mesh.colors32 = meshColors;
	}

	protected void UpdateGeometryImpl()
	{
		Build();
	}

	protected override void UpdateCollider()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		if (!CreateBoxCollider)
		{
			return;
		}
		if (base.CurrentSprite.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics3D)
		{
			if ((Object)(object)boxCollider != (Object)null)
			{
				boxCollider.size = 2f * boundsExtents;
				boxCollider.center = boundsCenter;
			}
		}
		else if (base.CurrentSprite.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics2D && (Object)(object)boxCollider2D != (Object)null)
		{
			boxCollider2D.size = (Vector2)(2f * boundsExtents);
			((Collider2D)boxCollider2D).offset = (Vector2)(boundsCenter);
		}
	}

	protected override void CreateCollider()
	{
		UpdateCollider();
	}

	protected override void UpdateMaterial()
	{
		Renderer component = ((Component)this).GetComponent<Renderer>();
		if ((Object)(object)component.sharedMaterial != (Object)(object)collectionInst.spriteDefinitions[base.spriteId].materialInst)
		{
			component.material = collectionInst.spriteDefinitions[base.spriteId].materialInst;
		}
	}

	protected override int GetCurrentVertexCount()
	{
		return 16;
	}

	public override void ReshapeBounds(Vector3 dMin, Vector3 dMax)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		float num = 0.1f;
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		Vector2 val = default(Vector2);
		val = new Vector2(_dimensions.x * currentSprite.texelSize.x, _dimensions.y * currentSprite.texelSize.y);
		Vector3 val2 = default(Vector3);
		val2 = new Vector3(val.x * _scale.x, val.y * _scale.y);
		Vector3 val3 = Vector3.zero;
		switch (_anchor)
		{
		case Anchor.LowerLeft:
			val3.Set(0f, 0f, 0f);
			break;
		case Anchor.LowerCenter:
			val3.Set(0.5f, 0f, 0f);
			break;
		case Anchor.LowerRight:
			val3.Set(1f, 0f, 0f);
			break;
		case Anchor.MiddleLeft:
			val3.Set(0f, 0.5f, 0f);
			break;
		case Anchor.MiddleCenter:
			val3.Set(0.5f, 0.5f, 0f);
			break;
		case Anchor.MiddleRight:
			val3.Set(1f, 0.5f, 0f);
			break;
		case Anchor.UpperLeft:
			val3.Set(0f, 1f, 0f);
			break;
		case Anchor.UpperCenter:
			val3.Set(0.5f, 1f, 0f);
			break;
		case Anchor.UpperRight:
			val3.Set(1f, 1f, 0f);
			break;
		}
		val3 = Vector3.Scale(val3, val2) * -1f;
		Vector3 val4 = val2 + dMax - dMin;
		val4.x /= val.x;
		val4.y /= val.y;
		if (Mathf.Abs(val.x * val4.x) < currentSprite.texelSize.x * num && Mathf.Abs(val4.x) < Mathf.Abs(_scale.x))
		{
			dMin.x = 0f;
			val4.x = _scale.x;
		}
		if (Mathf.Abs(val.y * val4.y) < currentSprite.texelSize.y * num && Mathf.Abs(val4.y) < Mathf.Abs(_scale.y))
		{
			dMin.y = 0f;
			val4.y = _scale.y;
		}
		Vector2 val5 = (Vector2)(new Vector3(Mathf.Approximately(_scale.x, 0f) ? 0f : (val4.x / _scale.x), Mathf.Approximately(_scale.y, 0f) ? 0f : (val4.y / _scale.y)));
		Vector3 val6 = default(Vector3);
		val6 = new Vector3(val3.x * val5.x, val3.y * val5.y);
		Vector3 val7 = dMin + val3 - val6;
		val7.z = 0f;
		((Component)this).transform.position = ((Component)this).transform.TransformPoint(val7);
		dimensions = new Vector2(_dimensions.x * val5.x, _dimensions.y * val5.y);
	}
}

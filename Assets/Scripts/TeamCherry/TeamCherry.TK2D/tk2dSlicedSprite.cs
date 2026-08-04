using UnityEngine;

[AddComponentMenu("2D Toolkit/Sprite/tk2dSlicedSprite")]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteAlways]
public class tk2dSlicedSprite : tk2dBaseSprite
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
	private bool _borderOnly;

	[SerializeField]
	private bool legacyMode;

	public float borderTop = 0.2f;

	public float borderBottom = 0.2f;

	public float borderLeft = 0.2f;

	public float borderRight = 0.2f;

	[SerializeField]
	protected bool _createBoxCollider;

	private Vector3 boundsCenter = Vector3.zero;

	private Vector3 boundsExtents = Vector3.zero;

	public bool BorderOnly
	{
		get
		{
			return _borderOnly;
		}
		set
		{
			if (value != _borderOnly)
			{
				_borderOnly = value;
				UpdateIndices();
			}
		}
	}

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

	public void SetBorder(float left, float bottom, float right, float top)
	{
		if (borderLeft != left || borderBottom != bottom || borderRight != right || borderTop != top)
		{
			borderLeft = left;
			borderBottom = bottom;
			borderRight = right;
			borderTop = top;
			UpdateVertices();
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
		if ((Object)(object)boxCollider == (Object)null)
		{
			boxCollider = ((Component)this).GetComponent<BoxCollider>();
		}
		if ((Object)(object)boxCollider2D == (Object)null)
		{
			boxCollider2D = ((Component)this).GetComponent<BoxCollider2D>();
		}
		if ((base.Collection != null))
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
		if ((mesh != null))
		{
			Object.Destroy((Object)(object)mesh);
		}
	}

	protected new void SetColors(Color32[] dest)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		tk2dSpriteGeomGen.SetSpriteColors(dest, 0, 16, _color, collectionInst.premultipliedAlpha);
	}

	protected void SetGeometry(Vector3[] vertices, Vector2[] uvs)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		float colliderOffsetZ = (((Object)(object)boxCollider != (Object)null) ? boxCollider.center.z : 0f);
		float colliderExtentZ = (((Object)(object)boxCollider != (Object)null) ? (boxCollider.size.z * 0.5f) : 0.5f);
		tk2dSpriteGeomGen.SetSlicedSpriteGeom(meshVertices, meshUvs, 0, out boundsCenter, out boundsExtents, currentSprite, _scale, dimensions, new Vector2(borderLeft, borderBottom), new Vector2(borderRight, borderTop), anchor, colliderOffsetZ, colliderExtentZ);
		if (meshNormals.Length != 0 || meshTangents.Length != 0)
		{
			tk2dSpriteGeomGen.SetSpriteVertexNormals(meshVertices, meshVertices[0], meshVertices[15], currentSprite.normals, currentSprite.tangents, meshNormals, meshTangents);
		}
		if (currentSprite.positions.Length != 4 || currentSprite.complexGeometry)
		{
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = Vector3.zero;
			}
		}
	}

	private void SetIndices()
	{
		int num = (_borderOnly ? 48 : 54);
		meshIndices = new int[num];
		tk2dSpriteGeomGen.SetSlicedSpriteIndices(meshIndices, 0, 0, base.CurrentSprite, _borderOnly);
	}

	private bool NearEnough(float value, float compValue, float scale)
	{
		return Mathf.Abs(Mathf.Abs(value - compValue) / scale) < 0.01f;
	}

	private void PermanentUpgradeLegacyMode()
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		float x = currentSprite.untrimmedBoundsData[0].x;
		float y = currentSprite.untrimmedBoundsData[0].y;
		float x2 = currentSprite.untrimmedBoundsData[1].x;
		float y2 = currentSprite.untrimmedBoundsData[1].y;
		if (NearEnough(x, 0f, x2) && NearEnough(y, (0f - y2) / 2f, y2))
		{
			_anchor = Anchor.UpperCenter;
		}
		else if (NearEnough(x, 0f, x2) && NearEnough(y, 0f, y2))
		{
			_anchor = Anchor.MiddleCenter;
		}
		else if (NearEnough(x, 0f, x2) && NearEnough(y, y2 / 2f, y2))
		{
			_anchor = Anchor.LowerCenter;
		}
		else if (NearEnough(x, (0f - x2) / 2f, x2) && NearEnough(y, (0f - y2) / 2f, y2))
		{
			_anchor = Anchor.UpperRight;
		}
		else if (NearEnough(x, (0f - x2) / 2f, x2) && NearEnough(y, 0f, y2))
		{
			_anchor = Anchor.MiddleRight;
		}
		else if (NearEnough(x, (0f - x2) / 2f, x2) && NearEnough(y, y2 / 2f, y2))
		{
			_anchor = Anchor.LowerRight;
		}
		else if (NearEnough(x, x2 / 2f, x2) && NearEnough(y, (0f - y2) / 2f, y2))
		{
			_anchor = Anchor.UpperLeft;
		}
		else if (NearEnough(x, x2 / 2f, x2) && NearEnough(y, 0f, y2))
		{
			_anchor = Anchor.MiddleLeft;
		}
		else if (NearEnough(x, x2 / 2f, x2) && NearEnough(y, y2 / 2f, y2))
		{
			_anchor = Anchor.LowerLeft;
		}
		else
		{
			Debug.LogError((object)("tk2dSlicedSprite (" + ((Object)this).name + ") error - Unable to determine anchor upgrading from legacy mode. Please fix this manually."));
			_anchor = Anchor.MiddleCenter;
		}
		float num = x2 / currentSprite.texelSize.x;
		float num2 = y2 / currentSprite.texelSize.y;
		_dimensions.x = _scale.x * num;
		_dimensions.y = _scale.y * num2;
		_scale.Set(1f, 1f, 1f);
		legacyMode = false;
	}

	public override void Build()
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		if (legacyMode)
		{
			PermanentUpgradeLegacyMode();
		}
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		meshUvs = (Vector2[])(object)new Vector2[16];
		meshVertices = (Vector3[])(object)new Vector3[16];
		meshColors = (Color32[])(object)new Color32[16];
		meshNormals = (Vector3[])(object)new Vector3[0];
		meshTangents = (Vector4[])(object)new Vector4[0];
		if (currentSprite.normals != null && currentSprite.normals.Length != 0)
		{
			meshNormals = (Vector3[])(object)new Vector3[16];
		}
		if (currentSprite.tangents != null && currentSprite.tangents.Length != 0)
		{
			meshTangents = (Vector4[])(object)new Vector4[16];
		}
		SetIndices();
		SetGeometry(meshVertices, meshUvs);
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

	private void UpdateIndices()
	{
		if ((Object)(object)mesh != (Object)null)
		{
			SetIndices();
			mesh.triangles = meshIndices;
		}
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
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		if (meshVertices == null || meshVertices.Length == 0)
		{
			Build();
			return;
		}
		SetGeometry(meshVertices, meshUvs);
		mesh.vertices = meshVertices;
		mesh.uv = meshUvs;
		mesh.normals = meshNormals;
		mesh.tangents = meshTangents;
		mesh.RecalculateBounds();
		mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(mesh.bounds, renderLayer);
		UpdateCollider();
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

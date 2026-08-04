using System;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using tk2dRuntime;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[AddComponentMenu("2D Toolkit/Text/tk2dTextMesh")]
public class tk2dTextMesh : MonoBehaviour, ISpriteCollectionForceBuild
{
	[Flags]
	private enum UpdateFlags
	{
		UpdateNone = 0,
		UpdateText = 1,
		UpdateColors = 2,
		UpdateBuffers = 4
	}

	private tk2dFontData _fontInst;

	private string _formattedText = "";

	[SerializeField]
	private tk2dFontData _font;

	[SerializeField]
	private string _text = "";

	[SerializeField]
	private Color _color = Color.white;

	[SerializeField]
	private Color _color2 = Color.white;

	[SerializeField]
	private bool _useGradient;

	[SerializeField]
	private int _textureGradient;

	[SerializeField]
	private TextAnchor _anchor = (TextAnchor)6;

	[SerializeField]
	private Vector3 _scale = new Vector3(1f, 1f, 1f);

	[SerializeField]
	private bool _kerning;

	[SerializeField]
	private int _maxChars = 16;

	[SerializeField]
	private bool _inlineStyling;

	[SerializeField]
	private bool _formatting;

	[SerializeField]
	private int _wordWrapWidth;

	[SerializeField]
	private float spacing;

	[SerializeField]
	private float lineSpacing;

	[SerializeField]
	private tk2dTextMeshData data = new tk2dTextMeshData();

	private Vector3[] vertices;

	private Vector2[] uvs;

	private Vector2[] uv2;

	private Color32[] colors;

	private Color32[] untintedColors;

	private UpdateFlags updateFlags = UpdateFlags.UpdateBuffers;

	private Mesh mesh;

	private MeshFilter meshFilter;

	private Renderer _cachedRenderer;

	public string FormattedText => _formattedText;

	public tk2dFontData font
	{
		get
		{
			UpgradeData();
			return data.font;
		}
		set
		{
			UpgradeData();
			data.font = value;
			_fontInst = data.font.inst;
			SetNeedUpdate(UpdateFlags.UpdateText);
			UpdateMaterial();
		}
	}

	public bool formatting
	{
		get
		{
			UpgradeData();
			return data.formatting;
		}
		set
		{
			UpgradeData();
			if (data.formatting != value)
			{
				data.formatting = value;
				SetNeedUpdate(UpdateFlags.UpdateText);
			}
		}
	}

	public int wordWrapWidth
	{
		get
		{
			UpgradeData();
			return data.wordWrapWidth;
		}
		set
		{
			UpgradeData();
			if (data.wordWrapWidth != value)
			{
				data.wordWrapWidth = value;
				SetNeedUpdate(UpdateFlags.UpdateText);
			}
		}
	}

	public string text
	{
		get
		{
			UpgradeData();
			return data.text;
		}
		set
		{
			UpgradeData();
			data.text = value;
			SetNeedUpdate(UpdateFlags.UpdateText);
		}
	}

	public Color color
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			UpgradeData();
			return data.color;
		}
		set
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			UpgradeData();
			data.color = value;
			SetNeedUpdate(UpdateFlags.UpdateColors);
		}
	}

	public Color color2
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			UpgradeData();
			return data.color2;
		}
		set
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			UpgradeData();
			data.color2 = value;
			SetNeedUpdate(UpdateFlags.UpdateColors);
		}
	}

	public bool useGradient
	{
		get
		{
			UpgradeData();
			return data.useGradient;
		}
		set
		{
			UpgradeData();
			data.useGradient = value;
			SetNeedUpdate(UpdateFlags.UpdateColors);
		}
	}

	public TextAnchor anchor
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			UpgradeData();
			return data.anchor;
		}
		set
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			UpgradeData();
			data.anchor = value;
			SetNeedUpdate(UpdateFlags.UpdateText);
		}
	}

	public Vector3 scale
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			UpgradeData();
			return data.scale;
		}
		set
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			UpgradeData();
			data.scale = value;
			SetNeedUpdate(UpdateFlags.UpdateText);
		}
	}

	public bool kerning
	{
		get
		{
			UpgradeData();
			return data.kerning;
		}
		set
		{
			UpgradeData();
			data.kerning = value;
			SetNeedUpdate(UpdateFlags.UpdateText);
		}
	}

	public int maxChars
	{
		get
		{
			UpgradeData();
			return data.maxChars;
		}
		set
		{
			UpgradeData();
			data.maxChars = value;
			SetNeedUpdate(UpdateFlags.UpdateBuffers);
		}
	}

	public int textureGradient
	{
		get
		{
			UpgradeData();
			return data.textureGradient;
		}
		set
		{
			UpgradeData();
			data.textureGradient = value % font.gradientCount;
			SetNeedUpdate(UpdateFlags.UpdateText);
		}
	}

	public bool inlineStyling
	{
		get
		{
			UpgradeData();
			return data.inlineStyling;
		}
		set
		{
			UpgradeData();
			data.inlineStyling = value;
			SetNeedUpdate(UpdateFlags.UpdateText);
		}
	}

	public float Spacing
	{
		get
		{
			UpgradeData();
			return data.spacing;
		}
		set
		{
			UpgradeData();
			if (data.spacing != value)
			{
				data.spacing = value;
				SetNeedUpdate(UpdateFlags.UpdateText);
			}
		}
	}

	public float LineSpacing
	{
		get
		{
			UpgradeData();
			return data.lineSpacing;
		}
		set
		{
			UpgradeData();
			if (data.lineSpacing != value)
			{
				data.lineSpacing = value;
				SetNeedUpdate(UpdateFlags.UpdateText);
			}
		}
	}

	public int SortingOrder
	{
		get
		{
			return CachedRenderer.sortingOrder;
		}
		set
		{
			if (CachedRenderer.sortingOrder != value)
			{
				data.renderLayer = value;
				CachedRenderer.sortingOrder = value;
			}
		}
	}

	private Renderer CachedRenderer
	{
		get
		{
			if ((Object)(object)_cachedRenderer == (Object)null)
			{
				_cachedRenderer = ((Component)this).GetComponent<Renderer>();
			}
			return _cachedRenderer;
		}
	}

	private bool useInlineStyling
	{
		get
		{
			if (inlineStyling)
			{
				return _fontInst.textureGradients;
			}
			return false;
		}
	}

	private void UpgradeData()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		if (data.version != 1)
		{
			data.font = _font;
			data.text = _text;
			data.color = _color;
			data.color2 = _color2;
			data.useGradient = _useGradient;
			data.textureGradient = _textureGradient;
			data.anchor = _anchor;
			data.scale = _scale;
			data.kerning = _kerning;
			data.maxChars = _maxChars;
			data.inlineStyling = _inlineStyling;
			data.formatting = _formatting;
			data.wordWrapWidth = _wordWrapWidth;
			data.spacing = spacing;
			data.lineSpacing = lineSpacing;
		}
		data.version = 1;
	}

	private static int GetInlineStyleCommandLength(int cmdSymbol)
	{
		int result = 0;
		switch (cmdSymbol)
		{
		case 99:
			result = 5;
			break;
		case 67:
			result = 9;
			break;
		case 103:
			result = 9;
			break;
		case 71:
			result = 17;
			break;
		}
		return result;
	}

	public string FormatText(string unformattedString)
	{
		string _targetString = "";
		FormatText(ref _targetString, unformattedString);
		return _targetString;
	}

	private void FormatText()
	{
		FormatText(ref _formattedText, data.text);
	}

	private void FormatText(ref string _targetString, string _source)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		InitInstance();
		if (!formatting || wordWrapWidth == 0 || _fontInst.texelSize == Vector2.zero)
		{
			_targetString = _source;
			return;
		}
		float num = _fontInst.texelSize.x * (float)wordWrapWidth;
		StringBuilder stringBuilder = new StringBuilder(_source.Length);
		float num2 = 0f;
		float num3 = 0f;
		int num4 = -1;
		int num5 = -1;
		bool flag = false;
		for (int i = 0; i < _source.Length; i++)
		{
			char c = _source[i];
			bool num6 = c == '^';
			tk2dFontChar tk2dFontChar2;
			if (_fontInst.useDictionary)
			{
				if (!_fontInst.charDict.ContainsKey(c))
				{
					c = '\0';
				}
				tk2dFontChar2 = _fontInst.charDict[c];
			}
			else
			{
				if (c >= _fontInst.chars.Length)
				{
					c = '\0';
				}
				tk2dFontChar2 = _fontInst.chars[(uint)c];
			}
			if (num6)
			{
				c = '^';
			}
			if (flag)
			{
				flag = false;
				continue;
			}
			if (data.inlineStyling && c == '^' && i + 1 < _source.Length)
			{
				if (_source[i + 1] != '^')
				{
					int inlineStyleCommandLength = GetInlineStyleCommandLength(_source[i + 1]);
					int num7 = 1 + inlineStyleCommandLength;
					for (int j = 0; j < num7; j++)
					{
						if (i + j < _source.Length)
						{
							stringBuilder.Append(_source[i + j]);
						}
					}
					i += num7 - 1;
					continue;
				}
				flag = true;
				stringBuilder.Append('^');
			}
			switch (c)
			{
			case '\n':
				num2 = 0f;
				num3 = 0f;
				num4 = stringBuilder.Length;
				num5 = i;
				break;
			case ' ':
				num2 += (tk2dFontChar2.advance + data.spacing) * data.scale.x;
				num3 = num2;
				num4 = stringBuilder.Length;
				num5 = i;
				break;
			default:
				if (num2 + tk2dFontChar2.p1.x * data.scale.x > num)
				{
					if (num3 > 0f)
					{
						num3 = 0f;
						num2 = 0f;
						stringBuilder.Remove(num4 + 1, stringBuilder.Length - num4 - 1);
						stringBuilder.Append('\n');
						i = num5;
						continue;
					}
					stringBuilder.Append('\n');
					num2 = (tk2dFontChar2.advance + data.spacing) * data.scale.x;
				}
				else
				{
					num2 += (tk2dFontChar2.advance + data.spacing) * data.scale.x;
				}
				break;
			}
			stringBuilder.Append(c);
		}
		_targetString = stringBuilder.ToString();
	}

	private void SetNeedUpdate(UpdateFlags uf)
	{
		if (updateFlags == UpdateFlags.UpdateNone)
		{
			updateFlags |= uf;
			tk2dUpdateManager.QueueCommit(this);
		}
		else
		{
			updateFlags |= uf;
		}
	}

	private void InitInstance()
	{
		if (data != null && (Object)(object)data.font != (Object)null)
		{
			_fontInst = data.font.inst;
			_fontInst.InitDictionary();
		}
	}

	private void Awake()
	{
		UpgradeData();
		if ((Object)(object)data.font != (Object)null)
		{
			_fontInst = data.font.inst;
		}
		updateFlags = UpdateFlags.UpdateBuffers;
		if ((Object)(object)data.font != (Object)null)
		{
			Init();
			UpdateMaterial();
		}
		updateFlags = UpdateFlags.UpdateNone;
	}

	protected void OnDestroy()
	{
		if ((Object)(object)meshFilter == (Object)null)
		{
			meshFilter = ((Component)this).GetComponent<MeshFilter>();
		}
		if ((Object)(object)meshFilter != (Object)null)
		{
			mesh = meshFilter.sharedMesh;
		}
		if ((mesh != null))
		{
			Object.DestroyImmediate((Object)(object)mesh, true);
			meshFilter.mesh = null;
		}
	}

	public int NumDrawnCharacters()
	{
		int num = NumTotalCharacters();
		if (num > data.maxChars)
		{
			num = data.maxChars;
		}
		return num;
	}

	public int NumTotalCharacters()
	{
		InitInstance();
		if ((updateFlags & (UpdateFlags.UpdateText | UpdateFlags.UpdateBuffers)) != UpdateFlags.UpdateNone)
		{
			FormatText();
		}
		int num = 0;
		for (int i = 0; i < _formattedText.Length; i++)
		{
			int num2 = _formattedText[i];
			bool num3 = num2 == 94;
			if (_fontInst.useDictionary)
			{
				if (!_fontInst.charDict.ContainsKey(num2))
				{
					num2 = 0;
				}
			}
			else if (num2 >= _fontInst.chars.Length)
			{
				num2 = 0;
			}
			if (num3)
			{
				num2 = 94;
			}
			if (num2 == 10)
			{
				continue;
			}
			if (data.inlineStyling && num2 == 94 && i + 1 < _formattedText.Length)
			{
				if (_formattedText[i + 1] != '^')
				{
					i += GetInlineStyleCommandLength(_formattedText[i + 1]);
					continue;
				}
				i++;
			}
			num++;
		}
		return num;
	}

	[Obsolete("Use GetEstimatedMeshBoundsForString().size instead")]
	public Vector2 GetMeshDimensionsForString(string str)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		return tk2dTextGeomGen.GetMeshDimensionsForString(str, tk2dTextGeomGen.Data(data, _fontInst, _formattedText));
	}

	public Bounds GetEstimatedMeshBoundsForString(string str)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		InitInstance();
		tk2dTextGeomGen.GeomData geomData = tk2dTextGeomGen.Data(data, _fontInst, _formattedText);
		Vector2 meshDimensionsForString = tk2dTextGeomGen.GetMeshDimensionsForString(FormatText(str), geomData);
		float yAnchorForHeight = tk2dTextGeomGen.GetYAnchorForHeight(meshDimensionsForString.y, geomData);
		float xAnchorForWidth = tk2dTextGeomGen.GetXAnchorForWidth(meshDimensionsForString.x, geomData);
		float num = (_fontInst.lineHeight + data.lineSpacing) * data.scale.y;
		return new Bounds(new Vector3(xAnchorForWidth + meshDimensionsForString.x * 0.5f, yAnchorForHeight + meshDimensionsForString.y * 0.5f + num, 0f), Vector3.Scale((Vector2)(meshDimensionsForString), new Vector3(1f, -1f, 1f)));
	}

	public void Init(bool force)
	{
		if (force)
		{
			SetNeedUpdate(UpdateFlags.UpdateBuffers);
		}
		Init();
	}

	public void Init()
	{
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		if (!(_fontInst != null) || ((updateFlags & UpdateFlags.UpdateBuffers) == 0 && !((Object)(object)mesh == (Object)null)))
		{
			return;
		}
		_fontInst.InitDictionary();
		FormatText();
		tk2dTextGeomGen.GeomData geomData = tk2dTextGeomGen.Data(data, _fontInst, _formattedText);
		tk2dTextGeomGen.GetTextMeshGeomDesc(out var numVertices, out var numIndices, geomData);
		vertices = (Vector3[])(object)new Vector3[numVertices];
		uvs = (Vector2[])(object)new Vector2[numVertices];
		colors = (Color32[])(object)new Color32[numVertices];
		untintedColors = (Color32[])(object)new Color32[numVertices];
		if (_fontInst.textureGradients)
		{
			uv2 = (Vector2[])(object)new Vector2[numVertices];
		}
		int[] array = new int[numIndices];
		int target = tk2dTextGeomGen.SetTextMeshGeom(vertices, uvs, uv2, untintedColors, 0, geomData);
		if (!_fontInst.isPacked)
		{
			Color32 val = (Color32)(data.color);
			Color32 val2 = (Color32)(data.useGradient ? data.color2 : data.color);
			for (int i = 0; i < numVertices; i++)
			{
				Color32 val3 = ((i % 4 < 2) ? val : val2);
				byte b = (byte)(untintedColors[i].r * val3.r / 255);
				byte b2 = (byte)(untintedColors[i].g * val3.g / 255);
				byte b3 = (byte)(untintedColors[i].b * val3.b / 255);
				byte b4 = (byte)(untintedColors[i].a * val3.a / 255);
				if (_fontInst.premultipliedAlpha)
				{
					b = (byte)(b * b4 / 255);
					b2 = (byte)(b2 * b4 / 255);
					b3 = (byte)(b3 * b4 / 255);
				}
				colors[i] = new Color32(b, b2, b3, b4);
			}
		}
		else
		{
			colors = untintedColors;
		}
		tk2dTextGeomGen.SetTextMeshIndices(array, 0, 0, geomData, target);
		if ((Object)(object)mesh == (Object)null)
		{
			if ((Object)(object)meshFilter == (Object)null)
			{
				meshFilter = ((Component)this).GetComponent<MeshFilter>();
			}
			mesh = new Mesh();
			mesh.MarkDynamic();
			((Object)mesh).hideFlags = (HideFlags)52;
			meshFilter.mesh = mesh;
		}
		else
		{
			mesh.Clear();
		}
		mesh.vertices = vertices;
		mesh.uv = uvs;
		if (font.textureGradients)
		{
			mesh.uv2 = uv2;
		}
		mesh.triangles = array;
		mesh.colors32 = colors;
		mesh.RecalculateBounds();
		mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(mesh.bounds, data.renderLayer);
		updateFlags = UpdateFlags.UpdateNone;
	}

	public void Commit()
	{
		tk2dUpdateManager.FlushQueues();
	}

	public void DoNotUse__CommitInternal()
	{
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		InitInstance();
		if ((Object)(object)_fontInst == (Object)null)
		{
			return;
		}
		_fontInst.InitDictionary();
		if ((updateFlags & UpdateFlags.UpdateBuffers) != UpdateFlags.UpdateNone || (Object)(object)mesh == (Object)null)
		{
			Init();
		}
		else
		{
			if ((updateFlags & UpdateFlags.UpdateText) != UpdateFlags.UpdateNone)
			{
				FormatText();
				tk2dTextGeomGen.GeomData geomData = tk2dTextGeomGen.Data(data, _fontInst, _formattedText);
				for (int i = tk2dTextGeomGen.SetTextMeshGeom(vertices, uvs, uv2, untintedColors, 0, geomData); i < data.maxChars; i++)
				{
					vertices[i * 4] = (vertices[i * 4 + 1] = (vertices[i * 4 + 2] = (vertices[i * 4 + 3] = Vector3.zero)));
				}
				mesh.vertices = vertices;
				mesh.uv = uvs;
				if (_fontInst.textureGradients)
				{
					mesh.uv2 = uv2;
				}
				if (_fontInst.isPacked)
				{
					colors = untintedColors;
					mesh.colors32 = colors;
				}
				if (data.inlineStyling)
				{
					SetNeedUpdate(UpdateFlags.UpdateColors);
				}
				mesh.RecalculateBounds();
				mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(mesh.bounds, data.renderLayer);
			}
			if (!font.isPacked && (updateFlags & UpdateFlags.UpdateColors) != UpdateFlags.UpdateNone)
			{
				Color32 val = (Color32)(data.color);
				Color32 val2 = (Color32)(data.useGradient ? data.color2 : data.color);
				for (int j = 0; j < colors.Length; j++)
				{
					Color32 val3 = ((j % 4 < 2) ? val : val2);
					byte b = (byte)(untintedColors[j].r * val3.r / 255);
					byte b2 = (byte)(untintedColors[j].g * val3.g / 255);
					byte b3 = (byte)(untintedColors[j].b * val3.b / 255);
					byte b4 = (byte)(untintedColors[j].a * val3.a / 255);
					if (_fontInst.premultipliedAlpha)
					{
						b = (byte)(b * b4 / 255);
						b2 = (byte)(b2 * b4 / 255);
						b3 = (byte)(b3 * b4 / 255);
					}
					colors[j] = new Color32(b, b2, b3, b4);
				}
				mesh.colors32 = colors;
			}
		}
		updateFlags = UpdateFlags.UpdateNone;
	}

	public void MakePixelPerfect()
	{
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f;
		tk2dCamera tk2dCamera2 = tk2dCamera.CameraForLayer(((Component)this).gameObject.layer);
		if ((Object)(object)tk2dCamera2 != (Object)null)
		{
			if (_fontInst.version < 1)
			{
				Debug.LogError((object)"Need to rebuild font.");
			}
			float distance = ((Component)this).transform.position.z - ((Component)tk2dCamera2).transform.position.z;
			float num2 = _fontInst.invOrthoSize * _fontInst.halfTargetHeight;
			num = tk2dCamera2.GetSizeAtDistance(distance) * num2;
		}
		else if ((Camera.main != null))
		{
			if (Camera.main.orthographic)
			{
				num = Camera.main.orthographicSize;
			}
			else
			{
				float zdist = ((Component)this).transform.position.z - ((Component)Camera.main).transform.position.z;
				num = tk2dPixelPerfectHelper.CalculateScaleForPerspectiveCamera(Camera.main.fieldOfView, zdist);
			}
			num *= _fontInst.invOrthoSize;
		}
		scale = new Vector3(Mathf.Sign(scale.x) * num, Mathf.Sign(scale.y) * num, Mathf.Sign(scale.z) * num);
	}

	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		if ((Object)(object)data.font != (Object)null && (Object)(object)data.font.spriteCollection != (Object)null)
		{
			return (Object)(object)data.font.spriteCollection == (Object)(object)spriteCollection;
		}
		return true;
	}

	private void UpdateMaterial()
	{
		if ((Object)(object)((Component)this).GetComponent<Renderer>().sharedMaterial != (Object)(object)_fontInst.materialInst)
		{
			((Component)this).GetComponent<Renderer>().material = _fontInst.materialInst;
		}
	}

	public void ForceBuild()
	{
		if ((Object)(object)data.font != (Object)null)
		{
			_fontInst = data.font.inst;
			UpdateMaterial();
		}
		Init(force: true);
	}
}

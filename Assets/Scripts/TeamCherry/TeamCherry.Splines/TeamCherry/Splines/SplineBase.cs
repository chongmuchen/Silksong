using System;
using System.Collections.Generic;
using TeamCherry.SharedUtils;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace TeamCherry.Splines;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public abstract class SplineBase : MonoBehaviour, IVertexColor
{
	public struct Point
	{
		public Vector3 Position;

		public Vector3 Tangent;

		public Color Color;

		public Vector3 Normal => Vector3.Cross(Vector3.forward, Tangent);
	}

	public enum TangentSources
	{
		ControlPoints,
		ForceDown,
		Linear
	}

	public enum UpdateConditions
	{
		None,
		Rigidbodies,
		Manual
	}

	public enum TextureTilingMethods
	{
		Explicit,
		Auto,
		Relative,
		RelativeWithMax
	}

	public enum UvModes
	{
		Vertical,
		Horizontal
	}

	private enum UpdateOrders
	{
		UpdateScheduleLateUpdateComplete,
		LateUpdateScheduleUpdateComplete,
		AllUpdate,
		AllLateUpdate
	}

	[BurstCompile]
	private struct MeshJob : IJob
	{
		public struct JobPoint
		{
			public float3 Position;

			public float3 Tangent;

			public float3 Normal => math.cross(math.float3(0f, 0f, 1f), Tangent);
		}

		[ReadOnly]
		public NativeArray<JobPoint> Points;

		public NativeArray<float3> Vertices;

		public NativeArray<float2> Uvs;

		public NativeArray<int> Indices;

		public TextureTilingMethods TextureTilingMethod;

		public UvModes UvMode;

		public float TextureOffset;

		public float TextureTiling;

		public bool FlipTextureU;

		public bool FlipTextureV;

		public NativeArray<float> PointDistances;

		public NativeArray<float> TotalPointsDistance;

		public float Width;

		public NativeCurve WidthCurve;

		public void Execute()
		{
			float num = 0f;
			TextureTilingMethods textureTilingMethod = TextureTilingMethod;
			if (textureTilingMethod == TextureTilingMethods.Relative || textureTilingMethod == TextureTilingMethods.RelativeWithMax)
			{
				for (int i = 1; i < Points.Length; i++)
				{
					JobPoint jobPoint = Points[i - 1];
					JobPoint jobPoint2 = Points[i];
					float num2 = math.distance(jobPoint.Position, jobPoint2.Position);
					PointDistances[i - 1] = num2;
					num += num2;
				}
				TotalPointsDistance[0] = num;
			}
			float num3 = 0f;
			int num4 = 0;
			int num5 = 0;
			for (int j = 0; j < Vertices.Length; j++)
			{
				if (num5 >= 2)
				{
					num5 = 0;
					num4++;
					textureTilingMethod = TextureTilingMethod;
					if (textureTilingMethod == TextureTilingMethods.Relative || textureTilingMethod == TextureTilingMethods.RelativeWithMax)
					{
						int num6 = num4 - 1;
						float num7 = PointDistances[num6];
						if (TextureTilingMethod == TextureTilingMethods.RelativeWithMax)
						{
							num7 /= num;
						}
						float num8 = num7 * TextureTiling * -1f;
						num3 += num8;
					}
					else
					{
						float textureTiling = TextureTiling;
						num3 = (float)num4 / (float)Points.Length * textureTiling * -1f;
					}
				}
				JobPoint jobPoint3 = Points[num4];
				float num9 = Width;
				if (WidthCurve.Resolution > 0 && Points.Length > 1)
				{
					num9 = WidthCurve.Evaluate((float)num4 / (float)(Points.Length - 1));
				}
				Vertices[j] = jobPoint3.Position + jobPoint3.Normal * ((j % 2 == 1) ? num9 : (0f - num9));
				float num10 = j % 2;
				if (FlipTextureU)
				{
					num10 = 1f - num10;
				}
				float num11 = num3;
				if (FlipTextureV)
				{
					num11 = 1f - num11;
				}
				int num12 = j;
				float2 @float = UvMode switch
				{
					UvModes.Vertical => new float2(num10, num11 + TextureOffset), 
					UvModes.Horizontal => new float2(num11 + TextureOffset, num10), 
					_ => throw new ArgumentOutOfRangeException(), 
				};
				Uvs[num12] = @float;
				num5++;
			}
			int num13 = (Points.Length - 1) * 2;
			int num14 = 0;
			for (int k = 0; k < num13 * 6; k += 6)
			{
				Indices[k] = num14;
				if (num14 % 2 == 0)
				{
					Indices[k + 1] = num14 + 1;
					Indices[k + 2] = num14 + 2;
				}
				else
				{
					Indices[k + 1] = num14 + 2;
					Indices[k + 2] = num14 + 1;
				}
				num14++;
			}
		}
	}

	[Header("Spline Base")]
	[SerializeField]
	private float width = 0.2f;

	[SerializeField]
	private float textureTiling = 1f;

	[SerializeField]
	private UvModes uvMode;

	[SerializeField]
	[HideInInspector]
	[Obsolete]
	private bool autoTextureTiling;

	[SerializeField]
	private TextureTilingMethods textureTilingMethod;

	[SerializeField]
	private SplineBase textureContinueFrom;

	[SerializeField]
	private float textureOffset;

	[SerializeField]
	private bool flipTextureU;

	[SerializeField]
	private bool flipTextureV;

	[SerializeField]
	private bool reverseDirection;

	[Space]
	[SerializeField]
	private bool useColorGradient;

	[SerializeField]
	private Gradient colorGradient;

	[SerializeField]
	private bool useWidthCurve;

	[SerializeField]
	private AnimationCurve widthCurve = AnimationCurve.Constant(0f, 1f, 1f);

	[Space]
	[SerializeField]
	private bool isStatic;

	[SerializeField]
	private float fpsLimit;

	[SerializeField]
	private SplineBase updateAfter;

	[SerializeField]
	private bool preventCulling;

	[SerializeField]
	private TangentSources tangentSource = TangentSources.Linear;

	[SerializeField]
	private UpdateConditions updateCondition;

	[SerializeField]
	private UpdateOrders updateOrder;

	private bool isVisible = true;

	protected bool isDirty;

	private NativeArray<MeshJob.JobPoint> jobPoints;

	private NativeArray<float3> jobVertices;

	private NativeArray<float2> jobUvs;

	private NativeArray<int> jobIndices;

	private NativeArray<float> pointDistances;

	private NativeArray<float> totalPointsDistance;

	private NativeCurve jobWidthCurve;

	private bool scheduledMeshJob;

	private JobHandle meshJobHandle;

	private Color[] colors;

	private float updateTimeOffset;

	private double nextUpdateTime;

	protected bool started;

	private MeshFilter meshFilter;

	private Mesh mesh;

	private MeshRenderer meshRenderer;

	private Rigidbody2D[] childBodies;

	protected Point[] InternalPoints;

	private Color vertexColor = Color.white;

	private bool hasColorChanged;

	private bool hasStarted;

	private bool doUpdateAfter;

	private static bool usePositionUpdateJobs = false;

	private static bool injectedEarlyUpdate;

	private static readonly HashSet<SplineBase> splines = new HashSet<SplineBase>();

	private static readonly List<SplineBase> splinesList = new List<SplineBase>();

	private static bool splinesDirty;

	protected JobHandle positionJobHandle;

	protected bool positionJobScheduled;

	public float Width
	{
		get
		{
			return width;
		}
		set
		{
			width = value;
		}
	}

	public float TextureTiling
	{
		get
		{
			return textureTiling;
		}
		set
		{
			textureTiling = value;
		}
	}

	public TextureTilingMethods TextureTilingMethod
	{
		get
		{
			return textureTilingMethod;
		}
		set
		{
			textureTilingMethod = value;
		}
	}

	public float TextureOffset
	{
		get
		{
			return textureOffset;
		}
		set
		{
			textureOffset = value;
		}
	}

	public bool FlipTextureU
	{
		get
		{
			return flipTextureU;
		}
		set
		{
			flipTextureU = value;
		}
	}

	public bool FlipTextureV
	{
		get
		{
			return flipTextureV;
		}
		set
		{
			flipTextureV = value;
		}
	}

	public bool ReverseDirection
	{
		get
		{
			return reverseDirection;
		}
		set
		{
			reverseDirection = value;
		}
	}

	public float FpsLimit
	{
		get
		{
			return fpsLimit;
		}
		set
		{
			fpsLimit = value;
		}
	}

	public bool PreventCulling
	{
		get
		{
			return preventCulling;
		}
		set
		{
			preventCulling = value;
		}
	}

	public TangentSources TangentSource
	{
		get
		{
			return tangentSource;
		}
		set
		{
			tangentSource = value;
		}
	}

	public UpdateConditions UpdateCondition
	{
		get
		{
			return updateCondition;
		}
		set
		{
			updateCondition = value;
			childBodies = ((updateCondition == UpdateConditions.Rigidbodies) ? ((Component)this).GetComponentsInChildren<Rigidbody2D>(true) : null);
		}
	}

	public Color VertexColor
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return vertexColor;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			vertexColor = value;
			hasColorChanged = true;
		}
	}

	protected bool CalculateSplineTangent => tangentSource == TangentSources.ControlPoints;

	public float Length
	{
		get
		{
			if (!totalPointsDistance.IsCreated)
			{
				return 0f;
			}
			return totalPointsDistance[0];
		}
	}

	public static bool UsePositionUpdateJobs
	{
		get
		{
			return usePositionUpdateJobs;
		}
		set
		{
			if (usePositionUpdateJobs == value)
			{
				return;
			}
			usePositionUpdateJobs = value;
			if (value)
			{
				InjectCustomPlayerLoop();
				SplineBase[] array = Object.FindObjectsOfType<SplineBase>();
				for (int i = 0; i < array.Length; i++)
				{
					AddSpline(array[i]);
				}
				return;
			}
			foreach (SplineBase spline in splines)
			{
				spline.CompletePositionUpdate();
			}
			splines.Clear();
			splinesList.Clear();
		}
	}

	GameObject IVertexColor.gameObject => ((Component)this).gameObject;

	public event Action UpdatedSpline;

	protected virtual void OnValidate()
	{
		if (autoTextureTiling)
		{
			textureTilingMethod = TextureTilingMethods.Auto;
			autoTextureTiling = false;
		}
		if (!Application.isPlaying)
		{
			hasColorChanged = true;
		}
		doUpdateAfter = (Object)(object)updateAfter != (Object)null;
	}

	protected virtual void Awake()
	{
		OnValidate();
		meshFilter = ((Component)this).GetComponent<MeshFilter>();
		if (Application.isPlaying)
		{
			UpdateCondition = updateCondition;
		}
	}

	protected virtual void OnEnable()
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		if (started)
		{
			ComponentSingleton<SplineBaseCallbackHooks>.Instance.OnUpdate += OnUpdate;
			ComponentSingleton<SplineBaseCallbackHooks>.Instance.OnLateUpdate += OnLateUpdate;
		}
		if (Application.isPlaying)
		{
			if (hasStarted)
			{
				UpdateVisibility();
			}
			if (UsePositionUpdateJobs)
			{
				InjectCustomPlayerLoop();
				AddSpline(this);
			}
			if (fpsLimit > 0f)
			{
				float num = 1f / fpsLimit;
				Resolution currentResolution = Screen.currentResolution;
				RefreshRate refreshRateRatio = ((Resolution)(ref currentResolution)).refreshRateRatio;
				float num2 = (float)((RefreshRate)(ref refreshRateRatio)).value;
				int num3 = Mathf.RoundToInt(num * num2);
				int num4 = Random.Range(0, num3);
				updateTimeOffset = num * (float)num4;
			}
			if (useColorGradient)
			{
				hasColorChanged = true;
			}
			if (hasStarted)
			{
				UpdateSpline(forceNewMesh: true);
			}
			if (doUpdateAfter)
			{
				updateAfter.UpdatedSpline += OnOtherUpdated;
			}
		}
	}

	private void OnDisable()
	{
		ComponentSingleton<SplineBaseCallbackHooks>.Instance.OnUpdate -= OnUpdate;
		ComponentSingleton<SplineBaseCallbackHooks>.Instance.OnLateUpdate -= OnLateUpdate;
		if (UsePositionUpdateJobs)
		{
			RemoveSpline(this);
			CompletePositionUpdate();
		}
		if (scheduledMeshJob)
		{
			((JobHandle)(ref meshJobHandle)).Complete();
			scheduledMeshJob = false;
		}
		if (jobPoints.IsCreated)
		{
			jobPoints.Dispose();
		}
		if (jobVertices.IsCreated)
		{
			jobVertices.Dispose();
		}
		if (jobUvs.IsCreated)
		{
			jobUvs.Dispose();
		}
		if (jobIndices.IsCreated)
		{
			jobIndices.Dispose();
		}
		if (pointDistances.IsCreated)
		{
			pointDistances.Dispose();
		}
		if (totalPointsDistance.IsCreated)
		{
			totalPointsDistance.Dispose();
		}
		if (jobWidthCurve.IsCreated)
		{
			jobWidthCurve.Dispose();
		}
		if ((Object)(object)mesh != (Object)null)
		{
			Object.DestroyImmediate((Object)(object)mesh);
			mesh = null;
		}
		if (Application.isPlaying)
		{
			if (doUpdateAfter)
			{
				updateAfter.UpdatedSpline -= OnOtherUpdated;
			}
			OnDisabled();
		}
	}

	protected virtual void OnDisabled()
	{
	}

	protected virtual void Start()
	{
		InitialiseSpline();
		started = true;
		ComponentSingleton<SplineBaseCallbackHooks>.Instance.OnUpdate += OnUpdate;
		ComponentSingleton<SplineBaseCallbackHooks>.Instance.OnLateUpdate += OnLateUpdate;
	}

	protected void InitialiseSpline()
	{
		if (!hasStarted)
		{
			hasStarted = true;
			UpdateSpline(forceNewMesh: true);
			UpdateVisibility();
		}
	}

	private void UpdateVisibility()
	{
		if ((Object)(object)meshRenderer == (Object)null)
		{
			meshRenderer = ((Component)this).GetComponent<MeshRenderer>();
			if ((Object)(object)meshRenderer == (Object)null)
			{
				return;
			}
		}
		isVisible = ((Renderer)meshRenderer).isVisible;
	}

	private void OnBecameVisible()
	{
		isVisible = true;
	}

	private void OnBecameInvisible()
	{
		isVisible = false;
	}

	private bool IsNotUpdating()
	{
		if (!isStatic && WantsToUpdate())
		{
			if (!isVisible)
			{
				return !preventCulling;
			}
			return false;
		}
		return true;
	}

	private void OnUpdate()
	{
		switch (updateOrder)
		{
		case UpdateOrders.UpdateScheduleLateUpdateComplete:
			DoSchedule();
			break;
		case UpdateOrders.LateUpdateScheduleUpdateComplete:
			DoComplete();
			break;
		case UpdateOrders.AllUpdate:
			DoSchedule();
			DoComplete();
			break;
		}
	}

	private void OnLateUpdate()
	{
		switch (updateOrder)
		{
		case UpdateOrders.UpdateScheduleLateUpdateComplete:
			DoComplete();
			break;
		case UpdateOrders.LateUpdateScheduleUpdateComplete:
			DoSchedule();
			break;
		case UpdateOrders.AllLateUpdate:
			DoSchedule();
			DoComplete();
			break;
		case UpdateOrders.AllUpdate:
			break;
		}
	}

	public void SetDirty()
	{
		isDirty = true;
	}

	private void DoSchedule()
	{
		if (Application.isPlaying)
		{
			if (doUpdateAfter || IsNotUpdating())
			{
				return;
			}
			if (fpsLimit > 0f)
			{
				double num = Time.timeAsDouble + (double)updateTimeOffset;
				if (num <= nextUpdateTime)
				{
					return;
				}
				nextUpdateTime = num + (double)(1f / fpsLimit);
			}
		}
		UpdateSpline(forceNewMesh: false);
	}

	private void DoComplete()
	{
		if (scheduledMeshJob)
		{
			CompleteMeshJob(forceNewMesh: false);
		}
	}

	private void CompleteMeshJob(bool forceNewMesh)
	{
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e8: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		((JobHandle)(ref meshJobHandle)).Complete();
		scheduledMeshJob = false;
		if (hasColorChanged)
		{
			EnsureArraySize(ref colors, jobVertices.Length);
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < jobVertices.Length; i++)
			{
				if (num2 >= 2)
				{
					num2 = 0;
					num++;
				}
				Color val = GetPoint(num).Color * VertexColor;
				if (useColorGradient)
				{
					val *= colorGradient.Evaluate((float)num / (float)(jobPoints.Length - 1));
				}
				colors[i] = val;
				num2++;
			}
		}
		if ((Object)(object)mesh == (Object)null || forceNewMesh)
		{
			if ((Object)(object)mesh != (Object)null)
			{
				Object.DestroyImmediate((Object)(object)mesh);
			}
			MeshFilter obj = meshFilter;
			Mesh val2 = new Mesh();
			Mesh val3 = val2;
			obj.sharedMesh = val2;
			mesh = val3;
			((Object)mesh).hideFlags = (HideFlags)61;
			((Object)mesh).name = "Spline_" + ((Object)((Component)this).gameObject).name;
			mesh.MarkDynamic();
		}
		mesh.Clear();
		mesh.SetVertices<float3>(jobVertices);
		mesh.SetUVs<float2>(0, jobUvs);
		mesh.SetIndices<int>(jobIndices, (MeshTopology)0, 0, true, 0);
		if (colors != null)
		{
			mesh.SetColors(colors);
			hasColorChanged = false;
		}
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}

	private void OnOtherUpdated()
	{
		if (!IsNotUpdating())
		{
			UpdateSpline(forceNewMesh: false);
		}
	}

	public void UpdateSpline()
	{
		UpdateSpline(forceNewMesh: false);
	}

	public void UpdateSpline(bool forceNewMesh)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		isDirty = false;
		UpdatePositions();
		switch (tangentSource)
		{
		case TangentSources.ForceDown:
		{
			Vector3 down = Vector3.down;
			for (int j = 0; j < InternalPoints.Length; j++)
			{
				Point point2 = InternalPoints[j];
				point2.Tangent = down;
				InternalPoints[j] = point2;
			}
			break;
		}
		case TangentSources.Linear:
		{
			for (int i = 0; i < InternalPoints.Length; i++)
			{
				Point point = InternalPoints[i];
				Vector3 tangent;
				if (i < InternalPoints.Length - 1)
				{
					Vector3 val = InternalPoints[i + 1].Position - point.Position;
					tangent = ((Vector3)(ref val)).normalized;
				}
				else
				{
					tangent = InternalPoints[i - 1].Tangent;
				}
				point.Tangent = tangent;
				InternalPoints[i] = point;
			}
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		case TangentSources.ControlPoints:
			break;
		}
		UpdateMeshInternal(forceNewMesh);
		if (this.UpdatedSpline != null)
		{
			this.UpdatedSpline();
		}
	}

	public abstract void UpdatePositions();

	protected static void EnsureArraySize<T>(ref T[] array, int size)
	{
		if (array == null)
		{
			array = new T[size];
		}
		else if (array.Length != size)
		{
			array = new T[size];
		}
	}

	protected static void EnsureNativeArraySize<T>(ref NativeArray<T> array, int size) where T : struct
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (!array.IsCreated)
		{
			array = new NativeArray<T>(size, (Allocator)4, (NativeArrayOptions)1);
		}
		else if (array.Length != size)
		{
			array.Dispose();
			array = new NativeArray<T>(size, (Allocator)4, (NativeArrayOptions)1);
		}
	}

	public void UpdateMesh(bool forceNewMesh = true)
	{
		InitialiseSpline();
		UpdateMeshInternal(forceNewMesh);
	}

	protected virtual void UpdateMeshInternal(bool forceNewMesh = false)
	{
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		if (scheduledMeshJob)
		{
			CompleteMeshJob(forceNewMesh: false);
		}
		int pointCount = GetPointCount();
		if (pointCount < 2)
		{
			return;
		}
		EnsureNativeArraySize(ref jobPoints, pointCount);
		int num = pointCount * 2;
		EnsureNativeArraySize(ref jobVertices, num);
		int size = pointCount * 2;
		EnsureNativeArraySize(ref jobUvs, size);
		int size2 = ((pointCount - 2) * 6 + 6) * 2;
		EnsureNativeArraySize(ref jobIndices, size2);
		TextureTilingMethods textureTilingMethods = textureTilingMethod;
		bool flag = textureTilingMethods == TextureTilingMethods.Relative || textureTilingMethods == TextureTilingMethods.RelativeWithMax;
		EnsureNativeArraySize(ref pointDistances, flag ? (pointCount - 1) : 0);
		EnsureNativeArraySize(ref totalPointsDistance, flag ? 1 : 0);
		if (!hasColorChanged && colors != null && colors.Length != num)
		{
			hasColorChanged = true;
		}
		if (hasColorChanged)
		{
			EnsureArraySize(ref colors, num);
		}
		float num2 = textureOffset;
		if (Object.op_Implicit((Object)(object)textureContinueFrom))
		{
			if (textureContinueFrom.scheduledMeshJob)
			{
				textureContinueFrom.CompleteMeshJob(forceNewMesh: false);
			}
			NativeArray<float2> val = textureContinueFrom.jobUvs;
			if (val.IsCreated && val.Length > 0)
			{
				num2 += val[val.Length - 1].y;
			}
		}
		for (int i = 0; i < pointCount; i++)
		{
			Point point = GetPoint(i);
			jobPoints[i] = new MeshJob.JobPoint
			{
				Position = point.Position,
				Tangent = point.Tangent
			};
		}
		if (useWidthCurve)
		{
			if (!Application.isPlaying || !jobWidthCurve.IsCreated || jobWidthCurve.Resolution != jobPoints.Length)
			{
				jobWidthCurve.Update(widthCurve, jobPoints.Length);
			}
		}
		else if (!jobWidthCurve.IsCreated || jobWidthCurve.Resolution > 0)
		{
			jobWidthCurve.Update(null, 0);
		}
		MeshJob meshJob = new MeshJob
		{
			Points = jobPoints,
			Vertices = jobVertices,
			Uvs = jobUvs,
			Indices = jobIndices,
			Width = width,
			WidthCurve = jobWidthCurve,
			TextureTilingMethod = textureTilingMethod,
			UvMode = uvMode,
			TextureOffset = num2,
			TextureTiling = GetTextureTiling(),
			PointDistances = pointDistances,
			TotalPointsDistance = totalPointsDistance,
			FlipTextureU = flipTextureU,
			FlipTextureV = flipTextureV
		};
		scheduledMeshJob = true;
		meshJobHandle = IJobExtensions.Schedule<MeshJob>(meshJob, default(JobHandle));
		if (forceNewMesh)
		{
			CompleteMeshJob(forceNewMesh: true);
		}
	}

	private bool WantsToUpdate()
	{
		if (isDirty)
		{
			return true;
		}
		isDirty = ShouldUpdate();
		return isDirty;
	}

	private bool ShouldUpdate()
	{
		switch (updateCondition)
		{
		case UpdateConditions.None:
			return true;
		case UpdateConditions.Rigidbodies:
		{
			Rigidbody2D[] array = childBodies;
			foreach (Rigidbody2D val in array)
			{
				if (((Component)val).gameObject.activeInHierarchy && val.IsAwake())
				{
					return true;
				}
			}
			return false;
		}
		case UpdateConditions.Manual:
			return false;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public void SetStatic()
	{
		isStatic = true;
	}

	public void SetDynamic()
	{
		if (isStatic)
		{
			isStatic = false;
		}
	}

	public Point GetPoint(int index)
	{
		if (reverseDirection)
		{
			index = GetPointCount() - 1 - index;
		}
		return InternalPoints[index];
	}

	public int GetPointCount()
	{
		if (InternalPoints == null)
		{
			return 0;
		}
		return InternalPoints.Length;
	}

	private IEnumerable<Point> EnumeratePoints()
	{
		for (int i = 0; i < GetPointCount(); i++)
		{
			yield return GetPoint(i);
		}
	}

	public void SetPointColor(int index, Color color)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		Point point = InternalPoints[index];
		point.Color = color;
		InternalPoints[index] = point;
		hasColorChanged = true;
	}

	private float GetTextureTiling()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		switch (textureTilingMethod)
		{
		case TextureTilingMethods.Explicit:
			return textureTiling;
		case TextureTilingMethods.Auto:
		{
			int pointCount = GetPointCount();
			if (Math.Abs(textureTiling) < Mathf.Epsilon || pointCount == 0)
			{
				return textureTiling;
			}
			Vector3 position = GetPoint(0).Position;
			Vector3 position2 = GetPoint(pointCount - 1).Position;
			return Vector3.Distance(position, position2) / textureTiling * (1f / (float)pointCount);
		}
		default:
			return textureTiling;
		}
	}

	private static void InjectCustomPlayerLoop()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		if (!injectedEarlyUpdate)
		{
			injectedEarlyUpdate = true;
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			int num = Array.FindIndex(currentPlayerLoop.subSystemList, (PlayerLoopSystem l) => l.type == typeof(EarlyUpdate));
			if (num >= 0)
			{
				PlayerLoopSystem val = currentPlayerLoop.subSystemList[num];
				List<PlayerLoopSystem> list = new List<PlayerLoopSystem>(val.subSystemList);
				list.Add(new PlayerLoopSystem
				{
					type = typeof(SplineBase),
					updateDelegate = new UpdateFunction(EarlySplineUpdate)
				});
				val.subSystemList = list.ToArray();
				currentPlayerLoop.subSystemList[num] = val;
			}
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
		}
	}

	protected static void AddSpline(SplineBase splineBase)
	{
		if (splines.Add(splineBase))
		{
			splinesDirty = true;
		}
	}

	protected static void RemoveSpline(SplineBase splineBase)
	{
		if (splines.Remove(splineBase))
		{
			splinesDirty = true;
		}
	}

	private static void EarlySplineUpdate()
	{
		if (UsePositionUpdateJobs)
		{
			if (splinesDirty)
			{
				splinesList.Clear();
				splinesList.AddRange(splines);
				splinesDirty = false;
			}
			for (int i = 0; i < splinesList.Count; i++)
			{
				splinesList[i].SchedulePositionUpdate();
			}
		}
	}

	protected virtual void SchedulePositionUpdate()
	{
	}

	protected virtual bool CompletePositionUpdate()
	{
		if (!positionJobScheduled)
		{
			return false;
		}
		((JobHandle)(ref positionJobHandle)).Complete();
		positionJobScheduled = false;
		return true;
	}
}

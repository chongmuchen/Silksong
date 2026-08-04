using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace TeamCherry.Splines
{

public class HermiteSplinePath : MonoBehaviour, IHermiteSplinePath
{
	public delegate Vector3 GetPointPosFunc(int index);

	[BurstCompile]
	private struct UpdatePositionsJob : IJobParallelFor
	{
		public int controlPointsCount;

		public int subdivisions;

		public bool calculateSplineTangent;

		[ReadOnly]
		public NativeArray<Vector3> controlPoints;

		[WriteOnly]
		public NativeArray<SplineBase.Point> internalPoints;

		public void Execute(int index)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			int num = index / subdivisions;
			int num2 = index % subdivisions;
			Vector3 val = controlPoints[num];
			Vector3 val2 = controlPoints[num + 1];
			Vector3 val3 = ((num <= 0) ? (val2 - val) : (0.5f * (val2 - controlPoints[num - 1])));
			Vector3 val4 = ((num >= controlPointsCount - 2) ? (val2 - val) : (0.5f * (controlPoints[num + 2] - val)));
			float num3 = 1f / (float)subdivisions;
			float num4 = (float)num2 * num3;
			Vector3 position = (2f * num4 * num4 * num4 - 3f * num4 * num4 + 1f) * val + (num4 * num4 * num4 - 2f * num4 * num4 + num4) * val3 + (-2f * num4 * num4 * num4 + 3f * num4 * num4) * val2 + (num4 * num4 * num4 - num4 * num4) * val4;
			Vector3 val5 = Vector3.zero;
			if (calculateSplineTangent)
			{
				val5 = (6f * num4 * num4 - 6f * num4) * val + (3f * num4 * num4 - 4f * num4 + 1f) * val3 + (-6f * num4 * num4 + 6f * num4) * val2 + (3f * num4 * num4 - 2f * num4) * val4;
			}
			internalPoints[index] = new SplineBase.Point
			{
				Position = position,
				Tangent = val5.normalized,
				Color = Color.white
			};
		}
	}

	public const int REQUIRED_POINT_COUNT = 3;

	[SerializeField]
	private List<Vector3> controlPoints;

	[SerializeField]
	private int subdivisions;

	private SplineBase.Point[] points;

	private Vector3[] worldPositions;

	private float[] distances;

	public float TotalDistance { get; private set; }

	public int ControlPointCount => controlPoints?.Count ?? 0;

	private void OnDrawGizmos()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (!Application.isPlaying)
		{
			UpdateValues();
		}
		if (worldPositions != null)
		{
			for (int i = 1; i < worldPositions.Length; i++)
			{
				Vector3 val = worldPositions[i];
				Gizmos.DrawLine(worldPositions[i - 1], val);
			}
		}
	}

	private void OnValidate()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (controlPoints == null)
		{
			controlPoints = new List<Vector3>();
		}
		while (controlPoints.Count < 3)
		{
			controlPoints.Add((Vector2)(Vector2.zero));
		}
		if (subdivisions < 2)
		{
			subdivisions = 2;
		}
	}

	private void Awake()
	{
		OnValidate();
		UpdateValues();
	}

	public void UpdateValues()
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		OnValidate();
		UpdatePositions(controlPoints.Count, subdivisions, (int index) => controlPoints[index], ref points, calculateSplineTangent: true);
		int num = points.Length;
		if (num <= 0)
		{
			return;
		}
		if (worldPositions == null || worldPositions.Length != num)
		{
			worldPositions = (Vector3[])(object)new Vector3[num];
		}
		int num2 = num - 1;
		if (distances == null || distances.Length != num2)
		{
			distances = new float[num2];
		}
		int num3 = 0;
		for (int num4 = 0; num4 < num; num4++)
		{
			SplineBase.Point point = points[num4];
			Vector3 val = ((Component)this).transform.TransformPoint(point.Position);
			worldPositions[num4] = val;
			if (num4 != 0)
			{
				float num5 = Vector2.Distance((Vector2)(worldPositions[num4 - 1]), (Vector2)(val));
				distances[num3] = num5;
				num3++;
			}
		}
		TotalDistance = 0f;
		float[] array = distances;
		foreach (float num7 in array)
		{
			TotalDistance += num7;
		}
	}

	public Vector3 GetPositionAlongSpline(float currentDistance)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		currentDistance = Mathf.Clamp(currentDistance, 0f, TotalDistance);
		float num = 0f;
		Vector3 result = Vector3.zero;
		for (int i = 1; i < worldPositions.Length; i++)
		{
			int num2 = i - 1;
			float num3 = distances[num2];
			float num4 = num;
			num += num3;
			if (!(currentDistance > num))
			{
				float num5 = (currentDistance - num4) / num3;
				Vector3 val = worldPositions[i - 1];
				Vector3 val2 = worldPositions[i];
				result = Vector3.Lerp(val, val2, num5);
				break;
			}
		}
		return result;
	}

	public float GetDistanceAlongSpline(Vector3 position, bool getNext)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		int num = -1;
		float num2 = float.MaxValue;
		for (int i = 0; i < worldPositions.Length; i++)
		{
			float num3 = Vector3.Distance(position, worldPositions[i]);
			if (!(num3 >= num2))
			{
				num = i;
				num2 = num3;
			}
		}
		if (getNext && num < worldPositions.Length - 1)
		{
			num++;
		}
		float num4 = 0f;
		for (int j = 0; j < num; j++)
		{
			num4 += distances[j];
		}
		return num4;
	}

	public Vector3 GetControlPoint(int index)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (controlPoints == null || index >= controlPoints.Count)
		{
			return Vector3.zero;
		}
		return ((Component)this).transform.TransformPoint(controlPoints[index]);
	}

	public void SetControlPoint(int index, Vector3 pos)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (controlPoints != null && index < controlPoints.Count)
		{
			controlPoints[index] = ((Component)this).transform.InverseTransformPoint(pos);
			UpdateValues();
		}
	}

	public void InsertControlPoint(int activePointIndex, Vector3 pos, out int newPointIndex)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		Vector3 newControlPointPos = GetNewControlPointPos(((Component)this).transform, controlPoints.Count, (int i) => controlPoints[i], activePointIndex, pos, out newPointIndex);
		controlPoints.Insert(newPointIndex, newControlPointPos);
		UpdateValues();
	}

	public static Vector3 GetNewControlPointPos(Transform transform, int controlPointCount, Func<int, Vector3> getControlPointFunc, int activePointIndex, Vector3 pos, out int newPointIndex)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = transform.InverseTransformPoint(pos);
		if (controlPointCount <= 1)
		{
			newPointIndex = controlPointCount;
		}
		else if (activePointIndex >= controlPointCount - 1)
		{
			Vector3 val2 = getControlPointFunc(controlPointCount - 1);
			Vector3 val3 = getControlPointFunc(controlPointCount - 2);
			newPointIndex = ((Vector2.Distance((Vector2)(val), (Vector2)(val3)) > Vector2.Distance((Vector2)(val2), (Vector2)(val3))) ? controlPointCount : (controlPointCount - 1));
		}
		else if (activePointIndex <= 0)
		{
			Vector3 val4 = getControlPointFunc(0);
			Vector3 val5 = getControlPointFunc(1);
			newPointIndex = ((!(Vector2.Distance((Vector2)(val), (Vector2)(val5)) > Vector2.Distance((Vector2)(val4), (Vector2)(val5)))) ? 1 : 0);
		}
		else
		{
			Vector3 val6 = getControlPointFunc(activePointIndex);
			Vector3 val7 = getControlPointFunc(activePointIndex - 1);
			Vector3 val8 = val - val6;
			Vector3 normalized = val8.normalized;
			val8 = val7 - val;
			Vector3 normalized2 = val8.normalized;
			if (Vector2.Dot((Vector2)(normalized), (Vector2)(normalized2)) < 0f)
			{
				newPointIndex = activePointIndex + 1;
			}
			else
			{
				newPointIndex = activePointIndex;
			}
		}
		return val;
	}

	public bool CanDeleteControlPoint()
	{
		return controlPoints.Count > 3;
	}

	public void DeleteControlPoint(int index)
	{
		if (CanDeleteControlPoint())
		{
			controlPoints.RemoveAt(index);
			UpdateValues();
		}
	}

	public void Reverse()
	{
		controlPoints.Reverse();
		UpdateValues();
	}

	public static JobHandle ScheduleUpdatePositionsJob(int controlPointsCount, int subdivisions, GetPointPosFunc getControlPointPos, ref SplineBase.Point[] internalPoints, bool calculateSplineTangent, NativeArray<Vector3> controlPoints, NativeArray<SplineBase.Point> jobInternalPoints)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		controlPointsCount--;
		int num = subdivisions * controlPointsCount + 1;
		if (internalPoints == null || internalPoints.Length != num)
		{
			internalPoints = new SplineBase.Point[num];
		}
		for (int i = 0; i <= controlPointsCount; i++)
		{
			controlPoints[i] = getControlPointPos(i);
		}
		return IJobParallelForExtensions.Schedule<UpdatePositionsJob>(new UpdatePositionsJob
		{
			controlPointsCount = controlPointsCount,
			subdivisions = subdivisions,
			calculateSplineTangent = calculateSplineTangent,
			controlPoints = controlPoints,
			internalPoints = jobInternalPoints
		}, num - 1, 64, default(JobHandle));
	}

	public static void UpdatePositions(int controlPointsCount, int subdivisions, GetPointPosFunc getControlPointPos, ref SplineBase.Point[] internalPoints, bool calculateSplineTangent)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		controlPointsCount--;
		int num = controlPointsCount - 1;
		int num2 = subdivisions * controlPointsCount + 1;
		if (internalPoints == null || internalPoints.Length != num2)
		{
			internalPoints = new SplineBase.Point[num2];
		}
		for (int i = 0; i < controlPointsCount; i++)
		{
			Vector3 val = getControlPointPos(i);
			Vector3 val2 = getControlPointPos(i + 1);
			Vector3 val3 = ((i <= 0) ? (val2 - val) : (0.5f * (val2 - getControlPointPos(i - 1))));
			Vector3 val4 = ((i >= num) ? (val2 - val) : (0.5f * (getControlPointPos(i + 2) - val)));
			float num3 = 1f / (float)subdivisions;
			for (int j = 0; j < subdivisions; j++)
			{
				float num4 = (float)j * num3;
				Vector3 position = (2f * num4 * num4 * num4 - 3f * num4 * num4 + 1f) * val + (num4 * num4 * num4 - 2f * num4 * num4 + num4) * val3 + (-2f * num4 * num4 * num4 + 3f * num4 * num4) * val2 + (num4 * num4 * num4 - num4 * num4) * val4;
				Vector3 val5 = ((!calculateSplineTangent) ? Vector3.zero : ((6f * num4 * num4 - 6f * num4) * val + (3f * num4 * num4 - 4f * num4 + 1f) * val3 + (-6f * num4 * num4 + 6f * num4) * val2 + (3f * num4 * num4 - 2f * num4) * val4));
				int num5 = j + i * subdivisions;
				internalPoints[num5] = new SplineBase.Point
				{
					Position = position,
					Tangent = val5.normalized,
					Color = Color.white
				};
			}
		}
		internalPoints[num2 - 1] = new SplineBase.Point
		{
			Position = getControlPointPos(controlPointsCount),
			Tangent = internalPoints[num2 - 2].Tangent,
			Color = Color.white
		};
	}
}
}

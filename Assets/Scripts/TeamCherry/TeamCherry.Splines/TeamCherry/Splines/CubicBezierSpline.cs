using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace TeamCherry.Splines;

[ExecuteAlways]
public class CubicBezierSpline : SplineBase
{
	[BurstCompile]
	private struct UpdatePositionsJob : IJobParallelFor
	{
		public Vector3 p0;

		public Vector3 p1;

		public Vector3 p2;

		public Vector3 p3;

		public bool calculateSplineTangent;

		[WriteOnly]
		public NativeArray<Point> internalPoints;

		public void Execute(int index)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			float num = (float)index / (float)(internalPoints.Length - 1);
			float num2 = 1f - num;
			Vector3 position = num2 * num2 * num2 * p0 + 3f * num2 * num2 * num * p1 + 3f * num2 * num * num * p2 + num * num * num * p3;
			Vector3 tangent = Vector3.zero;
			if (calculateSplineTangent)
			{
				Vector3 val = 2f * num2 * (p1 - p0) + 2f * num * (p2 - p1);
				tangent = ((Vector3)(ref val)).normalized;
			}
			internalPoints[index] = new Point
			{
				Position = position,
				Tangent = tangent,
				Color = Color.white
			};
		}
	}

	[Header("Cubic Bezier Spline")]
	[SerializeField]
	private Transform startPoint;

	[SerializeField]
	private Transform midPoint1;

	[SerializeField]
	private Transform midPoint2;

	[SerializeField]
	private Transform endPoint;

	[SerializeField]
	private int subDivisions;

	private NativeArray<Point> jobInternalPoints;

	public Transform StartPoint => startPoint;

	public Transform MidPoint1 => midPoint1;

	public Transform MidPoint2 => midPoint2;

	public Transform EndPoint => endPoint;

	protected override void OnDisabled()
	{
		if (jobInternalPoints.IsCreated)
		{
			jobInternalPoints.Dispose();
		}
	}

	protected override void SchedulePositionUpdate()
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		if (positionJobScheduled)
		{
			return;
		}
		positionJobScheduled = true;
		if (Object.op_Implicit((Object)(object)startPoint) && Object.op_Implicit((Object)(object)endPoint) && Object.op_Implicit((Object)(object)midPoint1) && Object.op_Implicit((Object)(object)midPoint2))
		{
			int num = Mathf.Max(0, subDivisions) + 1;
			int num2 = 4 * num;
			if (GetPointCount() != num2)
			{
				InternalPoints = new Point[num2];
			}
			Vector3 localPosition = startPoint.localPosition;
			Vector3 localPosition2 = midPoint1.localPosition;
			Vector3 localPosition3 = midPoint2.localPosition;
			Vector3 localPosition4 = endPoint.localPosition;
			SplineBase.EnsureNativeArraySize(ref jobInternalPoints, num2);
			UpdatePositionsJob updatePositionsJob = new UpdatePositionsJob
			{
				p0 = localPosition,
				p1 = localPosition2,
				p2 = localPosition3,
				p3 = localPosition4,
				calculateSplineTangent = SplineBase.UsePositionUpdateJobs,
				internalPoints = jobInternalPoints
			};
			positionJobHandle = IJobParallelForExtensions.Schedule<UpdatePositionsJob>(updatePositionsJob, num2, 64, default(JobHandle));
		}
	}

	protected override bool CompletePositionUpdate()
	{
		if (base.CompletePositionUpdate())
		{
			SplineBase.EnsureArraySize(ref InternalPoints, jobInternalPoints.Length);
			jobInternalPoints.CopyTo(InternalPoints);
			return true;
		}
		return base.CompletePositionUpdate();
	}

	public override void UpdatePositions()
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		if (CompletePositionUpdate() || !Object.op_Implicit((Object)(object)startPoint) || !Object.op_Implicit((Object)(object)endPoint) || !Object.op_Implicit((Object)(object)midPoint1) || !Object.op_Implicit((Object)(object)midPoint2))
		{
			return;
		}
		int num = Mathf.Max(0, subDivisions) + 1;
		int num2 = 4 * num;
		if (GetPointCount() != num2)
		{
			InternalPoints = new Point[num2];
		}
		Vector3 localPosition = startPoint.localPosition;
		Vector3 localPosition2 = midPoint1.localPosition;
		Vector3 localPosition3 = midPoint2.localPosition;
		Vector3 localPosition4 = endPoint.localPosition;
		Color white = Color.white;
		bool calculateSplineTangent = base.CalculateSplineTangent;
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < num2; i++)
		{
			float num3 = (float)i / (float)(num2 - 1);
			float num4 = 1f - num3;
			Vector3 position = num4 * num4 * num4 * localPosition + 3f * num4 * num4 * num3 * localPosition2 + 3f * num4 * num3 * num3 * localPosition3 + num3 * num3 * num3 * localPosition4;
			Vector3 tangent;
			if (calculateSplineTangent)
			{
				Vector3 val = 2f * num4 * (localPosition2 - localPosition) + 2f * num3 * (localPosition3 - localPosition2);
				tangent = ((Vector3)(ref val)).normalized;
			}
			else
			{
				tangent = zero;
			}
			InternalPoints[i] = new Point
			{
				Position = position,
				Tangent = tangent,
				Color = white
			};
		}
	}
}

using System.Collections.Generic;
using TeamCherry.SharedUtils;
using Unity.Collections;
using UnityEngine;

namespace TeamCherry.Splines;

public abstract class HermiteSplineBase : SplineBase
{
	[Header("Hermite Spline")]
	[SerializeField]
	private List<Transform> controlPoints = new List<Transform> { null, null, null };

	[SerializeField]
	private int subdivisions = 1;

	private List<Vector3> points = new List<Vector3>();

	private NativeArray<Vector3> jobControlPoints;

	private NativeArray<Point> jobInternalPoints;

	public List<Transform> ControlPoints
	{
		get
		{
			return controlPoints;
		}
		set
		{
			controlPoints = value;
		}
	}

	public int Subdivisions
	{
		get
		{
			return subdivisions;
		}
		set
		{
			subdivisions = value;
		}
	}

	protected override void OnValidate()
	{
		base.OnValidate();
		if (subdivisions < 2)
		{
			subdivisions = 2;
		}
	}

	protected override void OnDisabled()
	{
		if (jobControlPoints.IsCreated)
		{
			jobInternalPoints.Dispose();
		}
		if (jobInternalPoints.IsCreated)
		{
			jobInternalPoints.Dispose();
		}
	}

	protected override void SchedulePositionUpdate()
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		if (positionJobScheduled)
		{
			return;
		}
		positionJobScheduled = true;
		int count = controlPoints.Count;
		points.Clear();
		if (points.Capacity < controlPoints.Count)
		{
			points.Capacity = controlPoints.Count;
		}
		foreach (Transform controlPoint in controlPoints)
		{
			if ((Object)(object)controlPoint == (Object)null)
			{
				points.Add(Vector3.zero);
			}
			else
			{
				points.Add(controlPoint.localPosition);
			}
		}
		SplineBase.EnsureNativeArraySize<Vector3>(ref jobControlPoints, count);
		SplineBase.EnsureNativeArraySize(ref jobInternalPoints, subdivisions * (count - 1) + 1);
		positionJobHandle = HermiteSplinePath.ScheduleUpdatePositionsJob(count, subdivisions, (int index) => points[index], ref InternalPoints, base.CalculateSplineTangent, jobControlPoints, jobInternalPoints);
	}

	protected override bool CompletePositionUpdate()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		if (base.CompletePositionUpdate())
		{
			SplineBase.EnsureArraySize(ref InternalPoints, jobInternalPoints.Length);
			jobInternalPoints.CopyTo(InternalPoints);
			int num = InternalPoints.Length;
			Point[] internalPoints = InternalPoints;
			int num2 = num - 1;
			Point point = default(Point);
			List<Vector3> list = points;
			point.Position = list[list.Count - 1];
			point.Tangent = InternalPoints[num - 2].Tangent;
			point.Color = Color.white;
			internalPoints[num2] = point;
			return true;
		}
		return false;
	}

	public override void UpdatePositions()
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (CompletePositionUpdate())
		{
			return;
		}
		int count = controlPoints.Count;
		points.Clear();
		if (points.Capacity < controlPoints.Count)
		{
			points.Capacity = controlPoints.Count;
		}
		foreach (Transform controlPoint in controlPoints)
		{
			if ((Object)(object)controlPoint == (Object)null)
			{
				points.Add(Vector3.zero);
			}
			else
			{
				points.Add(controlPoint.localPosition);
			}
		}
		HermiteSplinePath.UpdatePositions(count, subdivisions, (int index) => points[index], ref InternalPoints, base.CalculateSplineTangent);
	}

	public void InitDefaults()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		Transform transform = new GameObject("pt1").transform;
		transform.SetParentReset(((Component)this).transform);
		Transform transform2 = new GameObject("pt2").transform;
		transform2.SetParentReset(((Component)this).transform);
		transform2.localPosition += Vector3.right;
		Transform transform3 = new GameObject("pt3").transform;
		transform3.SetParentReset(((Component)this).transform);
		transform3.localPosition += Vector3.one;
		controlPoints = new List<Transform> { transform, transform2, transform3 };
	}
}

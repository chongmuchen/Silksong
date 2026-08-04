using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.Splines;

[ExecuteAlways]
public class QuadraticBezierSpline : SplineBase
{
	[Header("Quadratic Bezier Spline")]
	[SerializeField]
	private Transform startPoint;

	[SerializeField]
	private Transform controlPoint;

	[SerializeField]
	private Transform endPoint;

	[SerializeField]
	private int subDivisions;

	public Transform StartPoint => startPoint;

	public Transform ControlPoint => controlPoint;

	public Transform EndPoint => endPoint;

	public override void UpdatePositions()
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)startPoint) && Object.op_Implicit((Object)(object)endPoint) && Object.op_Implicit((Object)(object)controlPoint))
		{
			int num = Mathf.Max(0, subDivisions) + 1;
			int num2 = 3 * num;
			if (GetPointCount() != num2)
			{
				InternalPoints = new Point[num2];
			}
			Vector3 localPosition = startPoint.localPosition;
			Vector3 localPosition2 = controlPoint.localPosition;
			Vector3 localPosition3 = endPoint.localPosition;
			for (int i = 0; i < num2; i++)
			{
				float num3 = (float)i / (float)(num2 - 1);
				float num4 = 1f - num3;
				Vector3 position = localPosition2 + num4 * num4 * (localPosition - localPosition2) + num3 * num3 * (localPosition3 - localPosition2);
				Vector3 val = ((!base.CalculateSplineTangent) ? Vector3.zero : (2f * num4 * (localPosition2 - localPosition) + 2f * num3 * (localPosition3 - localPosition2)));
				InternalPoints[i] = new Point
				{
					Position = position,
					Tangent = ((Vector3)(ref val)).normalized,
					Color = Color.white
				};
			}
		}
	}

	public void InitDefaults()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		startPoint = new GameObject("start").transform;
		startPoint.SetParentReset(((Component)this).transform);
		controlPoint = new GameObject("control").transform;
		controlPoint.SetParentReset(((Component)this).transform);
		Transform obj = controlPoint;
		obj.localPosition += Vector3.right;
		endPoint = new GameObject("end").transform;
		endPoint.SetParentReset(((Component)this).transform);
		Transform obj2 = endPoint;
		obj2.localPosition += Vector3.one;
	}
}
